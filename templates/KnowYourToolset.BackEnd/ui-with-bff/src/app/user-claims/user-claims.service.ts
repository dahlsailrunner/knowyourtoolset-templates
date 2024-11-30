import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Claim } from '../core/types/user';

@Injectable({
  providedIn: 'root'
})
export class UserClaimsService {

  constructor(private http: HttpClient) { }

  getClaims(): Observable<Claim[]> {
    return this.http.get<Claim[]>('/account/user');
  }
}