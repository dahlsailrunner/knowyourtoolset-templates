import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { UserClaimsComponent } from './user-claims/user-claims.component';
import { authGuard, requiredClaimValueGuard } from './core/auth.guard';
import { ProductsComponent } from './products/products.component';
import { AccessDeniedComponent } from './access-denied/access-denied.component';

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
        canActivate: [requiredClaimValueGuard('given_name', 'Bob')] // probably role and value is better
      },
      {
        path: 'access-denied',
        component: AccessDeniedComponent,
        canActivate: [authGuard],
      }
];
