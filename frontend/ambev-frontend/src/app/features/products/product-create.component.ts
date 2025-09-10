import { Component } from '@angular/core';
import { ProductService, Product } from '../../core/services/product.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-product-create',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './product-create.component.html',
  styleUrls: ['./product-create.component.scss']
})
export class ProductCreateComponent {
  product: Partial<Product> = {
    title: '',
    price: 0,
    description: '',
    category: '',
    image: '',
    rating: {
      rate: 0,
      count: 0
    }
  };

  errors: string[] = [];
  isEdit = false;

  constructor(private productService: ProductService, private router: Router) {}

  ngOnInit() {
    const url = this.router.url;
    const match = url.match(/\/products\/edit\/(.+)$/);
    if (match && match[1]) {
      this.isEdit = true;
      this.productService.getProduct(match[1]).subscribe(p => {
        this.product = p;
      });
    }
  }

  onSubmit() {
    this.errors = [];
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
    
    if (this.isEdit && this.product.id) {
      this.productService.updateProduct(this.product.id, this.product as Product).subscribe({
        next: () => this.router.navigate(['/products']),
        error: err => {
          this.errors = parseError(err);
        }
      });
    } else {
      this.productService.createProduct(this.product).subscribe({
        next: () => this.router.navigate(['/products']),
        error: err => {
          this.errors = parseError(err);
        }
      });
    }
  }
}
