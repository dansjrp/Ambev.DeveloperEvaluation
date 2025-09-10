import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService, User, UserStatus, UserRole } from '../../core/services/user.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-user-edit',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.scss']
})
export class UserEditComponent implements OnInit {
  user: User | null = null;
  errors: string[] = [];

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.userService.getUser(id).subscribe(user => {
        this.user = user;
      });
    }
  }

  onSubmit() {
    if (!this.user) return;
    this.errors = [];
    this.userService.updateUser(this.user.id!, this.user).subscribe({
      next: () => this.router.navigate(['/users']),
      error: err => {
        if (err.error && err.error.errors) {
          this.errors = err.error.errors;
        } else if (err.error && err.error.message) {
          this.errors = [err.error.message];
        } else {
          this.errors = ['Erro desconhecido ao salvar usuário.'];
        }
      }
    });
  }
}
