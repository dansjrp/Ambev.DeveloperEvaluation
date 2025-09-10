import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { SaleService, Sale } from '../../core/services/sale.service';

@Component({
  selector: 'app-sale-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule],
  templateUrl: './sale-detail.component.html',
  styleUrls: ['./sale-detail.component.scss']
})
export class SaleDetailComponent implements OnInit {
  sale?: Sale;
  loading = false;
  error: string | null = null;

  constructor(private route: ActivatedRoute, private saleService: SaleService) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loading = true;
      this.saleService.getSale(id).subscribe({
        next: sale => {
          this.sale = sale;
          this.loading = false;
        },
        error: err => {
          this.error = err?.error?.message || 'Erro ao carregar venda.';
          this.loading = false;
        }
      });
    }
  }
}
