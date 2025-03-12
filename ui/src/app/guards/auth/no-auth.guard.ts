import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { CookieServ } from '../../services/cookie/cookie.service';

export const noAuthGuard: CanActivateFn = (route, state) => {
  const cookieService = inject(CookieServ);
  const router = inject(Router);

  const token = cookieService.getCookie();

  if (!token) {
    return true;
  } else {
    router.navigate(['/home']);
    return false;
  }
};
