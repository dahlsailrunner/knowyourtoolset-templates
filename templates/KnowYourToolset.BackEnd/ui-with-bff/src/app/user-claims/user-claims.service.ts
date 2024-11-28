import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Claim } from '../core/types/user';

@Injectable({
  providedIn: 'root'
})
export class UserClaimsService {

  readonly claims$: Observable<Claim[]>;

  constructor(private http: HttpClient) {
    this.claims$ = this.http.get<Claim[]>('/account/user');
  }
}