import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authAdminGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);

  const userString = localStorage.getItem('user');

  if (!userString) {
    router.navigate(['/login']);
    return false;
  }

  try {
    const user = JSON.parse(userString);
    if (user && user.role === 'Admin') {
      return true;
    } else {
      router.navigate(['/home']);
      return false;
    }
  } catch (error) {
    localStorage.removeItem('user');
    router.navigate(['/login']);
    return false;
  }
};
