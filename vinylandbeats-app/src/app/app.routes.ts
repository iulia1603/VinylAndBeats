import { Routes } from '@angular/router';
import { ProductList } from './features/products/product-list/product-list';
import { ProductDetail } from './features/products/product-detail/product-detail';
import { ProductForm } from './features/products/product-form/product-form';
import { Login } from './features/auth/login/login';
import { Register } from './features/auth/register/register';
import { authGuard } from './core/guards/auth-guard';

export const routes: Routes = [
  { path: '', redirectTo: '/products', pathMatch: 'full' },
  { path: 'login', component: Login },
  { path: 'register', component: Register },
  { path: 'products', component: ProductList },
  { path: 'products/new', component: ProductForm, canActivate: [authGuard] },
  { path: 'products/:id/edit', component: ProductForm, canActivate: [authGuard] },
  { path: 'products/:id', component: ProductDetail },
  { path: '**', redirectTo: '/products' }
];