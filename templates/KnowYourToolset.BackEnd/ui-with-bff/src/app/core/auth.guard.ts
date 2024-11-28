import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from './auth.service';

export const authGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {

  var authService = inject(AuthService);
  var claims = authService.claimsSignal();
  console.log('signal has ' + claims.length + ' claims in it.')

  if (claims.length <= 0) {
    const redirectPath = window.location.pathname + window.location.search;
    window.location.href = '/account/login?returnUrl=' + encodeURI(redirectPath)    
  }
  return true; // all routes are allowed for authenticated users
};