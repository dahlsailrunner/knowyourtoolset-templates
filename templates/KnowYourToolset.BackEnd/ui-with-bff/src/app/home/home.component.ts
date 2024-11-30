import { Component } from '@angular/core';
import { PageTitleService } from '../core/page-title.service';

@Component({
  selector: 'app-home',
  standalone:true,
  imports: [],
  templateUrl: './home.component.html'
})
export class HomeComponent {
  constructor(private pageTitleService : PageTitleService) {}

  ngOnInit(): void {
    this.pageTitleService.setPageTitle({
      pageName: 'Home',
    });
  }
}
