import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { UserClaimsComponent } from './user-claims/user-claims.component';
import { authGuard } from './core/auth.guard';
import { ProductsComponent } from './products/products.component';

export const routes: Routes = [
    {
        path: '',
        component: HomeComponent,
      },
      {
        path: 'home',
        component: HomeComponent,
      },
      {
        path: 'user-claims',
        component: UserClaimsComponent,
        canActivate: [authGuard],
      },
      {
        path: 'products',
        component: ProductsComponent,
        canActivate: [authGuard],
      }
];
