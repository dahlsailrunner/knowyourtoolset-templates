import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { RouterLink, RouterOutlet } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { AsyncPipe } from '@angular/common';
import { map, Observable, shareReplay, Subject, takeUntil } from 'rxjs';
import { PageTitleService } from './core/page-title.service';
import { PageTitleInfo } from './core/types/page-title-info';
import { AuthenticationService } from './core/authentication.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, 
    RouterLink,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatListModule,
    MatIconModule,
    AsyncPipe,
  ],
  templateUrl:'./app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit, OnDestroy {

  
  constructor(private pageTitleService: PageTitleService,
    private authService: AuthenticationService,
  ) { 
    this.userName$ = this.authService.getUsername();
    this.logoutUrl$ = this.authService.getLogoutUrl();
  }

  pageTitle?: string;
  userName$: Observable<string | undefined>;
  logoutUrl$: Observable<string | undefined>;

  private breakpointObserver = inject(BreakpointObserver);

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  private onDestroyed: Subject<void> = new Subject<void>();
  
  ngOnInit(): void {
    this.pageTitleService.pageTitleSet$.pipe(
      takeUntil(this.onDestroyed)
    ).subscribe((pageName) => this.onPageSet(pageName)); 
  }

  onPageSet(newPageInfo: PageTitleInfo): void {
    this.pageTitle = newPageInfo.pageName;
  }

  logout() {
    throw new Error('Method not implemented.');
  }

  ngOnDestroy(): void {
    throw new Error('Method not implemented.');
  }
}
