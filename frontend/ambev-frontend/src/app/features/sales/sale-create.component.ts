import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { Sale } from '../../core/services/sale.service';
import { SaleCreateService } from '../../core/services/sale-create.service';
import { ActivatedRoute } from '@angular/router';
import { SaleService } from '../../core/services/sale.service';
import { UserService, User } from '../../core/services/user.service';
import { ProductService, Product } from '../../core/services/product.service';

@Component({
  selector: 'app-sale-create',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule, FormsModule],
  templateUrl: './sale-create.component.html',
  styleUrls: ['./sale-create.component.scss']
})
export class SaleCreateComponent {

    errors: string[] = [];
    
  editItem(saleId: string, productId: string): void {
    this.loading = true;
    this.error = null;
    this.errors = [];

    const item = this.sale.items?.find(i => i.productId === productId);
    if (!item) {
      this.errors = ['Item não encontrado.'];
      this.loading = false;
      return;
    }
    const model = {
      quantity: item.quantity,
      cancelled: item.cancelled
    };

    const parseError = (err: any): string[] => {
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

    this.saleCreateService.editSaleItem(saleId, productId, model).subscribe({
      next: () => {
        window.location.href = '/sales';
      },
      error: (err: any) => {
        this.errors = parseError(err);
        this.loading = false;
      }
    });
  }
  sale: Partial<Sale> = { items: [] };
  error: string | null = null;
  loading = false;
  users: User[] = [];
  products: Product[] = [];
  isEdit = false;

  constructor(
    private saleCreateService: SaleCreateService,
    private saleService: SaleService,
    private userService: UserService,
    private productService: ProductService,
    private route: ActivatedRoute
  ) {
    this.userService.getUsers().subscribe(res => {
      this.users = (res as any).data ?? res;
    });
    this.productService.getProducts(1, 100).subscribe(res => {
      this.products = res.data;
    });

    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.isEdit = true;
        this.saleService.getSale(id).subscribe(sale => {
          this.sale = {
            ...sale,
            userId: sale.userId?.toString(),
            items: sale.items?.map(item => ({
              ...item,
              productId: item.productId?.toString()
            })) ?? []
          };
        });
      }
    });
  }

  addItem(): void {
    if (!this.sale.items) this.sale.items = [];
    this.sale.items.push({
      productId: '',
      productName: '',
      quantity: 1,
      price: 0,
      discounts: 0,
      totalPrice: 0,
      cancelled: false
    });
  }

  removeItem(i: number): void {
    if (this.sale.items) this.sale.items.splice(i, 1);
  }

  saveSale(): void {
    this.loading = true;
    this.error = null;
    if (!this.sale.items) this.sale.items = [];
    const request$ = this.isEdit && this.sale.id
      ? this.saleCreateService.updateSale(this.sale.id, this.sale)
      : this.saleCreateService.createSale(this.sale);
    request$.subscribe({
      next: () => {
        window.location.href = '/sales';
      },
      error: (err: any) => {
        this.error = err?.error?.message || 'Erro ao salvar venda.';
        this.loading = false;
      }
    });
  }
}
