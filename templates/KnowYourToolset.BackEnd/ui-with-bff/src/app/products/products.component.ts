import { Component, OnInit } from '@angular/core';
import { Product } from './types';
import { Observable, Subscription } from 'rxjs';
import { ProductService } from './product-service';
import { SubscriptionService } from '../core/subscription.service';
import { PageTitleService } from '../core/page-title.service';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [],
  templateUrl: './products.component.html',
  styleUrl: './products.component.css'
})
export class ProductsComponent implements OnInit {
  products$: Observable<Product[]>;
  products: Product[] | undefined;
  subscription: Subscription | null = null;
  error: any;

  constructor(
    private pageTitleService : PageTitleService,
    private productService: ProductService,
    private subscriptionService: SubscriptionService
  ) {
    this.products$ = this.productService.products$;
  }

  ngOnInit() {
    this.pageTitleService.setPageTitle({
      pageName: 'Products',
    });
    this.subscription = this.getProducts();
  }

  getProducts() {
    this.subscriptionService.resetSubscription(this.subscription);
    return this.products$.subscribe({
      next: (products) => (this.products = products),
      error: (error) => (this.error = error),
    });
  }
}
