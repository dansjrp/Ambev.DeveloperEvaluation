import { Component, OnInit } from '@angular/core';
import { CartService, Cart } from '../../core/services/cart.service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-cart-list',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule],
  templateUrl: './cart-list.component.html',
  styleUrls: ['./cart-list.component.scss']
})
export class CartListComponent implements OnInit {
  carts: Cart[] = [];
  currentPage = 1;
  totalPages = 1;
  pageSize = 10;
  totalCount = 0;
  loading = false;

  constructor(private cartService: CartService) {}

  ngOnInit(): void {
    this.loadCarts();
  }

  loadCarts(page: number = 1): void {
    this.loading = true;
    this.cartService.getCarts(page, this.pageSize).subscribe(res => {
      this.carts = res.data;
      this.currentPage = res.currentPage;
      this.totalPages = res.totalPages;
      this.totalCount = res.totalCount;
      this.loading = false;
    });
  }

  confirmDelete(cart: Cart): void {
    if (confirm(`Deseja realmente excluir o carrinho ${cart.id}?`)) {
      if (cart.id) {
        this.cartService.deleteCart(cart.id).subscribe(() => {
          this.loadCarts(this.currentPage);
        });
      } else {
        alert('ID do carrinho não encontrado.');
      }
    }
  }
}
