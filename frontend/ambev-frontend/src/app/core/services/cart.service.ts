import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Cart {
  id?: string;
  userId: string;
  userName?: string;
  products: Array<{ productId: string; productName: string; quantity: number }>;
  items?: Array<{ productId: string; productName: string; quantity: number }>;
  createdAt?: string;
}

export interface PaginatedResponse<T> {
  data: T[];
  currentPage: number;
  totalPages: number;
  totalCount: number;
}

@Injectable({ providedIn: 'root' })
export class CartService {
  finalizeCart(id: string): Observable<any> {
    // Pode ser necessário passar dados adicionais, ajuste conforme o backend
    return this.http.post(`${this.apiUrl}/${id}/close`, {});
  }
  private apiUrl = 'http://localhost:5119/api/carts';

  constructor(private http: HttpClient) {}

  getCarts(page = 1, size = 10, order = 'userId asc'): Observable<PaginatedResponse<Cart>> {
    return this.http.get<PaginatedResponse<Cart>>(`${this.apiUrl}?page=${page}&pageSize=${size}&order=${order}`);
  }

  getCart(id: string): Observable<Cart> {
    return this.http.get<Cart>(`${this.apiUrl}/${id}`);
  }

  createCart(cart: Partial<Cart>): Observable<Cart> {
    // Remover 'createdAt' antes de criar o payload
    const { createdAt, ...cartWithoutCreatedAt } = cart;
    const payload: any = { ...cartWithoutCreatedAt, items: cart.products };
    delete payload.products;
    return this.http.post<Cart>(this.apiUrl, payload);
  }

  updateCart(id: string, cart: Cart): Observable<Cart> {
    const { createdAt, ...cartWithoutCreatedAt } = cart;
    const payload: any = { ...cartWithoutCreatedAt, items: cart.products };
    delete payload.products;
    return this.http.put<Cart>(`${this.apiUrl}/${id}`, payload);
  }

  deleteCart(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
