import { EventEmitter, Injectable } from '@angular/core';
import { PageTitleInfo } from './types/page-title-info';

@Injectable({
  providedIn: 'root',
})
export class PageTitleService {
  public pageTitleSet$ = new EventEmitter<PageTitleInfo>(true);

  public setPageTitle(newPageTitle: PageTitleInfo): void {
    this.pageTitleSet$.emit(newPageTitle);
  }
}