import { Component, OnInit } from '@angular/core';
import { CartService, Cart } from '../../core/services/cart.service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-cart-list',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule, FormsModule],
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

  finalizeBranch: string = '';
  finalizeCartId: string | null = null;

  openFinalizeModal(cart: Cart): void {
    this.finalizeCartId = cart.id || null;
    this.finalizeBranch = '';
    this.errors = [];
    const modal = document.getElementById('finalizeCartModal');
    if (modal) {
      const bootstrap = (window as any)['bootstrap'];
      if (bootstrap && bootstrap.Modal) {
        const bsModal = new bootstrap.Modal(modal);
        bsModal.show();
      } else {
        alert('Bootstrap JS não está carregado.');
      }
    }
  }

  confirmFinalizeCart(): void {
    if (!this.finalizeCartId || !this.finalizeBranch) {
      alert('Preencha a filial para finalizar.');
      return;
    }
    this.cartService.finalizeCart(this.finalizeCartId, this.finalizeBranch).subscribe({
      next: result => {
        alert('Foi gerado uma venda, anote o numero: ' + result.number);
        this.loadCarts(this.currentPage);
        // Fechar modal
        const modal = document.getElementById('finalizeCartModal');
        const bootstrap = (window as any)['bootstrap'];
        if (modal && bootstrap && bootstrap.Modal) {
          const bsModal = bootstrap.Modal.getInstance(modal);
          if (bsModal) bsModal.hide();
        }
      },
      error: err => {
        this.errors = this.parseError(err);
        alert('Erro ao finalizar carrinho: ' + (this.errors.join(', ') || 'Erro desconhecido'));
      }
    });
  }

    errors: string[] = [];
    parseError = (err: any): string[] => {
      if (err.error && err.error.errors) {
        return err.error.errors.map((e: any) => typeof e === 'string' ? e : e.detail || e.message );
      } else if (err.error && err.error.detail) {
        return [typeof err.error.detail === 'string' ? err.error.detail : JSON.stringify(err.error.detail)];
      } else if (err.error && err.error.message) {
        return [typeof err.error.message === 'string' ? err.error.message : JSON.stringify(err.error.message)];
      } else {
        return ['Erro desconhecido ao salvar usuário.'];
      }
    };
}
