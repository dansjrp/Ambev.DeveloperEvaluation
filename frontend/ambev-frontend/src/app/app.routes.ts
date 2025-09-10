
import { Routes } from '@angular/router';
import { UserListComponent } from './features/users/user-list.component';
import { UserCreateComponent } from './features/users/user-create.component';
import { UserDetailComponent } from './features/users/user-detail.component';

export const routes: Routes = [
	{ path: 'users', component: UserListComponent },
	{ path: 'users/create', component: UserCreateComponent },
	{ path: 'users/edit/:id', component: UserCreateComponent },
	{ path: 'products', loadComponent: () => import('./features/products/product-list.component').then(m => m.ProductListComponent) },
	{ path: 'products/create', loadComponent: () => import('./features/products/product-create.component').then(m => m.ProductCreateComponent) },
	{ path: 'products/edit/:id', loadComponent: () => import('./features/products/product-create.component').then(m => m.ProductCreateComponent) },
	{ path: 'carts', loadComponent: () => import('./features/carts/cart-list.component').then(m => m.CartListComponent) },
	{ path: 'carts/create', loadComponent: () => import('./features/carts/cart-create.component').then(m => m.CartCreateComponent) },
	{ path: 'carts/edit/:id', loadComponent: () => import('./features/carts/cart-create.component').then(m => m.CartCreateComponent) },
	{ path: '', redirectTo: 'users', pathMatch: 'full' }
];
