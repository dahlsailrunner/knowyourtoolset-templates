import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { map } from 'rxjs';
import { AuthenticationService } from './authentication.service';

export const loggedInGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {

  var authService = inject(AuthenticationService);

  return authService.getIsAuthenticated().pipe(
    map(isAuthenticated => {
      if (isAuthenticated) {        
        return true; // all routes are allowed for authenticated users      
      }
      else {
        window.location.href = '/account/login?returnUrl=' + encodeURI(state.url);
        return false;
      }
    })
  );
};

export function requiredClaimValueGuard(claimName: string, claimValue: string) : CanActivateFn {
  return (_route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {

    var authService = inject(AuthenticationService);
    var router = inject(Router);

    return authService.getSession().pipe(
      map(session => {
        { 
          console.log('user claims:', session);       
          if (!session) {  // user is not even logged in
            window.location.href = '/account/login?returnUrl=' + encodeURI(state.url);
            return false;
          }
          var claim = session.find(c => c.type === claimName)?.value;
          if (claim === claimValue) {
            return true;
          }
          return router.createUrlTree(['/access-denied']);
        }
      })
    );
  };
}
