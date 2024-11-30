import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { csrfInterceptor } from './core/csrf-interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes), 
    provideAnimationsAsync(),
    provideHttpClient(
      withInterceptors([csrfInterceptor])
    ),
    // ----- this code can be used to force a user to log in before they can access the app -----
    // it also requires some code in the AuthService class to be uncommented
    // {
    //   provide: APP_INITIALIZER,
    //   useFactory: initializeAuth,
    //   deps: [AuthService],
    //   multi: true,
    // },   
  ]
};
