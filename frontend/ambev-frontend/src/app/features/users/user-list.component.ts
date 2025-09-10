import { Component, OnInit } from '@angular/core';
import { UserService, User } from '../../core/services/user.service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule],
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {
  users: User[] = [];
  currentPage = 1;
  totalPages = 1;
  pageSize = 10;
  totalCount = 0;
  loading = false;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(page: number = 1): void {
    this.loading = true;
    this.userService.getUsers(page, this.pageSize).subscribe(res => {
      this.users = res.data;
      this.currentPage = res.currentPage;
      this.totalPages = res.totalPages;
      this.totalCount = res.totalCount;
      this.loading = false;
    });
  }

  confirmDelete(user: User): void {
    const nomeCompleto = `${user.name.firstname} ${user.name.lastname}`;
    if (confirm(`Deseja realmente excluir o usuário ${nomeCompleto}?`)) {
      if (user.id) {
        this.userService.deleteUser(user.id).subscribe(() => {
          this.loadUsers(this.currentPage);
        });
      } else {
        alert('ID do usuário não encontrado.');
      }
    }
  }
}
