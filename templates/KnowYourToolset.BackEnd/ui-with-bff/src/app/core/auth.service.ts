import { Injectable } from '@angular/core';
import { Claim } from './types/user';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

// -------------------------------------------------
// use the function below (referenced in app.config.ts) to force users to log in before they can access the app
// -------------------------------------------------
// export function initializeAuth(authService: AuthService) {
//   return (): Observable<Claim[]|string> => 
//     authService.userClaims$.pipe(
//       catchError(() => {
//         // user is not logged in - redirect them
//         const redirectPath = window.location.pathname + window.location.search;
//         window.location.href = '/account/login?returnUrl=' + encodeURI(redirectPath);
//         return of("redirecting");
//       }),
//       tap(() => { console.log('in app initializer - logged in') })
//     );
// }

@Injectable({
  providedIn: 'root'
})
export class AuthService {  
  
  constructor(private http: HttpClient) { }

  simpleGetClaims(): Observable<Claim[]> {
    return this.http.get<Claim[]>('/account/user');
  }
}