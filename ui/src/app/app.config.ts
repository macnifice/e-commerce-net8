import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { authInterceptorInterceptor } from './services/auth/authInterceptor.interceptor';
import { provideNativeDateAdapter } from '@angular/material/core';

export const appConfig: ApplicationConfig = {
  providers: [provideZoneChangeDetection({ eventCoalescing: true }), provideRouter(routes), provideAnimationsAsync(), provideHttpClient(
    withInterceptors([authInterceptorInterceptor])
  ),
  provideNativeDateAdapter(),
    { provide: MAT_DIALOG_DATA, useValue: {} }
  ]
};
