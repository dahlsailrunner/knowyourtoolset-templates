import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from './auth.service';
import { catchError, map, of } from 'rxjs';

export const authGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {

  var authService = inject(AuthService);

  return authService.simpleGetClaims().pipe(
    map(claims => {
      console.log('claims:', claims);      
      // could check individual claims here      
      return true; // all routes are allowed for authenticated users
    }), 
    catchError(() => {
      console.log('trying to redirect based on error');
      const redirectPath = window.location.pathname + window.location.search;
      window.location.href = '/account/login?returnUrl=' + encodeURI(redirectPath);
      return of(false);
    })
  );
};



