import { AsyncPipe } from '@angular/common';
import { Component } from '@angular/core';
import { MatListModule } from '@angular/material/list';
import { UserClaimsService } from './user-claims.service';
import { Observable } from 'rxjs';
import { PageTitleService } from '../core/page-title.service';
import { Claim } from '../core/authentication.service';

@Component({
  selector: 'app-user-claims',
  imports: [AsyncPipe, MatListModule],
  standalone: true,
  templateUrl: './user-claims.component.html'
})
export class UserClaimsComponent {
  readonly claims$ : Observable<Claim[]>;

  constructor(
    private pageTitleService : PageTitleService,
    private userClaimsService: UserClaimsService) {
    this.claims$ = this.userClaimsService.getClaims();
  }

  ngOnInit(): void {
    this.pageTitleService.setPageTitle({
      pageName: 'User Claims',
    });
  }
}
