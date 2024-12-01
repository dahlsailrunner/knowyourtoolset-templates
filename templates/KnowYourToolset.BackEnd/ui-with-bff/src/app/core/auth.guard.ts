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
      window.location.href = '/account/login?returnUrl=' + encodeURI(state.url);
      return of(false);
    })
  );
};



