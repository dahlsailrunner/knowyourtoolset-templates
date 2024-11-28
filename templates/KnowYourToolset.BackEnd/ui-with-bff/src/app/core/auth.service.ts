import { HttpClient } from '@angular/common/http';
import { Injectable, Signal } from '@angular/core';
import { Observable, catchError, of, shareReplay, tap } from 'rxjs';
import { Claim } from './types/user';
import { toSignal } from '@angular/core/rxjs-interop';

export function initializeAuth(authService: AuthService) {
  return (): Observable<Claim[]|string> => 
    authService.userClaims$.pipe(
      catchError(() => {
        // user is not logged in - redirect them
        const redirectPath = window.location.pathname + window.location.search;
        window.location.href = '/account/login?returnUrl=' + encodeURI(redirectPath);
        return of("redirecting");
      }),
      tap(() => { console.log('in app initializer - logged in') })
    );
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {  
  readonly userClaims$: Observable<Claim[]>;
  claimsSignal: Signal<Claim[]>;
  
  constructor(private http: HttpClient) { 
    this.userClaims$ = this.http.get<Claim[]>('/account/user').pipe(
      tap(() => console.log('retrieved user claims')),
      shareReplay(1, 10_000) // cache for 10 seconds
    );   
    this.claimsSignal = toSignal(this.userClaims$, { initialValue: [] as Claim[] });
  }
}