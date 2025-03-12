import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { CookieServ } from '../../services/cookie/cookie.service';

export const authGuard: CanActivateFn = (route, state) => {
  const cookieService = inject(CookieServ);
  const router = inject(Router);

  const token = cookieService.checkCookie();

  if (!token) {
    router.navigate(['/login']);
    return false;
  } else {
    return true;
  }
};
