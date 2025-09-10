import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CartService, Cart } from '../../core/services/cart.service';
import { UserService, User } from '../../core/services/user.service';
import { ProductService, Product } from '../../core/services/product.service';

@Component({
  selector: 'app-cart-create',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule, FormsModule],
  templateUrl: './cart-create.component.html',
  styleUrls: ['./cart-create.component.scss']
})
export class CartCreateComponent implements OnInit {
  cart: Partial<Cart> = { products: [] };
  error: string | null = null;
  loading = false;
  isEdit = false;
  users: User[] = [];
  products: Product[] = [];

  constructor(
    private cartService: CartService,
    private route: ActivatedRoute,
    private userService: UserService,
    private productService: ProductService
  ) {}

  ngOnInit(): void {
    this.userService.getUsers().subscribe(res => {
      this.users = (res as any).data ?? res;
    });
    this.productService.getProducts(1, 100).subscribe(res => {
      this.products = res.data;
    });
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.loading = true;
      this.cartService.getCart(id).subscribe({
        next: (cart: Cart) => {
          this.cart = cart;
          if (!this.cart.products) this.cart.products = [];
          this.loading = false;
        },
        error: (err: any) => {
          this.error = err?.error?.message || 'Erro ao carregar carrinho.';
          this.loading = false;
        }
      });
    } else {
      this.cart = { products: [] };
    }
  }

  saveCart(): void {
    this.loading = true;
    this.error = null;
    if (!this.cart.products) this.cart.products = [];
    if (this.isEdit && this.cart.id) {
      this.cartService.updateCart(this.cart.id, this.cart as Cart).subscribe({
        next: () => {
          window.location.href = '/carts';
        },
        error: (err: any) => {
          this.error = err?.error?.message || 'Erro ao salvar carrinho.';
          this.loading = false;
        }
      });
    } else {
      this.cartService.createCart(this.cart).subscribe({
        next: () => {
          window.location.href = '/carts';
        },
        error: (err: any) => {
          this.error = err?.error?.message || 'Erro ao salvar carrinho.';
          this.loading = false;
        }
      });
    }
  }
}
