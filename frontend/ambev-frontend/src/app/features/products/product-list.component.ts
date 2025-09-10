import { Component, OnInit } from '@angular/core';
import { ProductService, Product } from '../../core/services/product.service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss']
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  currentPage = 1;
  totalPages = 1;
  pageSize = 10;
  totalCount = 0;
  loading = false;

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(page: number = 1): void {
    this.loading = true;
    this.productService.getProducts(page, this.pageSize).subscribe(res => {
      this.products = res.data;
      this.currentPage = res.currentPage;
      this.totalPages = res.totalPages;
      this.totalCount = res.totalCount;
      this.loading = false;
    });
  }

  confirmDelete(product: Product): void {
    if (confirm(`Deseja realmente excluir o produto ${product.title}?`)) {
      if (product.id) {
        this.productService.deleteProduct(product.id).subscribe(() => {
          this.loadProducts(this.currentPage);
        });
      } else {
        alert('ID do produto não encontrado.');
      }
    }
  }
}
