import type { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { CookieServ } from '../cookie/cookie.service';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from './auth.service';
import { AuthLoginResponse } from '../../interfaces/auth/auth-login.response.interface';
import { AuthRefreshTokenRequest } from '../../interfaces/auth/auth-refreshToken.interface';
import { User } from '../../interfaces/user/user.interface';
import { Router } from '@angular/router';

export const authInterceptorInterceptor: HttpInterceptorFn = (req, next) => {
  const cookieService = inject(CookieServ);
  const authService = inject(AuthService);
  const router = inject(Router);
  const token = cookieService.getCookie();

  if (!token) {
    return next(req);
  }

  const authReq = req.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`
    }
  });

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status !== 401) {
        return throwError(() => error);
      }
      try {
        const userJson = localStorage.getItem('user');
        if (!userJson) {
          cookieService.removeCookie();
          router.navigate(['/login']);
          return throwError(() => error);
        }
        const user: User = JSON.parse(userJson);
        const refreshRequest: AuthRefreshTokenRequest = {
          userId: user.id,
          refreshToken: user.refreshToken
        };
        cookieService.removeCookie();
        return authService.refreshToken(refreshRequest).pipe(
          switchMap((response: AuthLoginResponse) => {
            cookieService.setCookie(response.accessToken);
            response.user.refreshToken = response.refreshToken;
            localStorage.setItem('user', JSON.stringify(response.user));
            const newAuthReq = req.clone({
              setHeaders: {
                Authorization: `Bearer ${response.accessToken}`
              }
            });
            return next(newAuthReq);
          }),
          catchError(refreshError => {
            console.error('Error al refrescar el token:', refreshError);
            cookieService.removeCookie();
            localStorage.removeItem('user');
            router.navigate(['/login']);
            return throwError(() => refreshError);
          })
        );
      } catch (e) {
        console.error('Error en el proceso de refresh token:', e);
        cookieService.removeCookie();
        localStorage.removeItem('user');
        router.navigate(['/login']);
        return throwError(() => error);
      }
    })
  );
};
