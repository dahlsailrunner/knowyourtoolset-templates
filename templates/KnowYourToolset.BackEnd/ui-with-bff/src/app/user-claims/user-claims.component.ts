import { AsyncPipe } from '@angular/common';
import { Component } from '@angular/core';
import { MatListModule } from '@angular/material/list';
import { UserClaimsService } from './user-claims.service';
import { Claim } from '../core/types/user';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-user-claims',
  imports: [AsyncPipe, MatListModule],
  standalone: true,
  templateUrl: './user-claims.component.html'
})
export class UserClaimsComponent {
  readonly claims$ : Observable<Claim[]>;

  constructor(private userClaimsService: UserClaimsService) {
    this.claims$ = this.userClaimsService.claims$;
  }
}
