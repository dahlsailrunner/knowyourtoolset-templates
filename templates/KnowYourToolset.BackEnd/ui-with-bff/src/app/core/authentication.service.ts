import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, filter, map, Observable, of, shareReplay } from 'rxjs';

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

const ANONYMOUS: Session = null;
const CACHE_SIZE = 1;

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private session$: Observable<Session> | null = null
  constructor(private http: HttpClient) {
  }

  public getSession(ignoreCache: boolean = false) {
    if (!this.session$ || ignoreCache) {
      this.session$ = this.http.get<Session>('account/user').pipe(
        catchError(() => {
          return of(ANONYMOUS);
        }),
        shareReplay(CACHE_SIZE)
      );
    }
    return this.session$;
  }

  public getIsAuthenticated(ignoreCache: boolean = false) {
    return this.getSession(ignoreCache).pipe(
      map(UserIsAuthenticated)
    );
  }

  public getIsAnonymous(ignoreCache: boolean = false) {
    return this.getSession(ignoreCache).pipe(
      map(UserIsAnonymous)
    );
  }

  public getUsername(ignoreCache: boolean = false) {
    return this.getSession(ignoreCache).pipe(
      filter(UserIsAuthenticated),
      map(s => s.find(c => c.type === 'name')?.value)
    );
  }

  public getLogoutUrl(ignoreCache: boolean = false) {
    return this.getSession(ignoreCache).pipe(
      filter(UserIsAuthenticated),
      map(s => s.find(c => c.type === 'bff:logout_url')?.value)
    );
  }
}

export interface Claim {
  type: string;
  value: string;
}
export type Session = Claim[] | null;

function UserIsAuthenticated(s: Session): s is Claim[] {
  return s !== null;
}

function UserIsAnonymous(s: Session): s is null {
  return s === null;
}