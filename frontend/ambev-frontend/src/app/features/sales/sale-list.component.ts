import { Component, OnInit } from '@angular/core';
import { SaleService, Sale } from '../../core/services/sale.service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-sale-list',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule],
  templateUrl: './sale-list.component.html',
  styleUrls: ['./sale-list.component.scss']
})
export class SaleListComponent implements OnInit {
  sales: Sale[] = [];
  currentPage = 1;
  totalPages = 1;
  pageSize = 10;
  totalCount = 0;
  loading = false;

  constructor(private saleService: SaleService) {}

  ngOnInit(): void {
    this.loadSales();
  }

  loadSales(page: number = 1): void {
    this.loading = true;
    this.saleService.getSales(page, this.pageSize).subscribe(res => {
      this.sales = res.data;
      this.currentPage = res.currentPage;
      this.totalPages = res.totalPages;
      this.totalCount = res.totalCount;
      this.loading = false;
    });
  }
}
