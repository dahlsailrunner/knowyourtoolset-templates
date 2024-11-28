import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product, ProductById } from './types';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  products$: Observable<Product[]>;

  productRoute: string = '/api/v1/products';

  constructor(
    private http: HttpClient
  ) {
    this.products$ = this.http.get<Product[]>(this.productRoute);
  }

  getProductById(id: number) {
    return this.http.get<ProductById>(`${this.productRoute}/${id}`);
  }

  deleteProductById(id: number) {
    return this.http.delete(this.productRoute, { body: id });
  }

  updateProduct(product: ProductById) {
    return this.http.put(this.productRoute, product);
  }

  addProduct(product: ProductById) {
    return this.http.post(this.productRoute, product);
  }
}