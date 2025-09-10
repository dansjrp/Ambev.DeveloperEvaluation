import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Name {
  firstname: string;
  lastname: string;
}

export interface Geolocation {
  lat: string;
  long: string;
}

export interface Address {
  city: string;
  street: string;
  number: number;
  zipcode: string;
  geolocation: Geolocation;
}

export type UserStatus = 'Unknown' | 'Active' | 'Inactive' | 'Suspended';
export type UserRole = 'None' | 'Customer' | 'Manager' | 'Admin';

export interface User {
  id?: string;
  username: string;
  password: string;
  phone: string;
  email: string;
  status: UserStatus;
  role: UserRole;
  name: Name;
  address: Address;
  firstName: string;
  lastName: string;
}

export interface PaginatedResponse<T> {
  data: T[];
  currentPage: number;
  totalPages: number;
  totalCount: number;
}

@Injectable({ providedIn: 'root' })
export class UserService {
  private apiUrl = 'http://localhost:5119/api/users';

  constructor(private http: HttpClient) {}

  getUsers(page = 1, size = 10, order = 'name asc'): Observable<PaginatedResponse<User>> {
    return this.http.get<PaginatedResponse<User>>(`${this.apiUrl}?_page=${page}&_size=${size}&_order=${order}`);
  }

  getUser(id: string): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`);
  }

  createUser(user: Partial<User>): Observable<User> {
    return this.http.post<User>(this.apiUrl, user);
  }

  deleteUser(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  updateUser(id: string, user: User): Observable<User> {
    return this.http.put<User>(`${this.apiUrl}/${id}`, user);
  }
}
