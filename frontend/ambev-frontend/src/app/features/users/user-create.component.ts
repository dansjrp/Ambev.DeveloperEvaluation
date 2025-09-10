import { Component } from '@angular/core';
import { UserService, UserStatus, UserRole, User } from '../../core/services/user.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-user-create',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './user-create.component.html',
  styleUrls: ['./user-create.component.scss']
})
export class UserCreateComponent {
  user: Partial<User> = {
    username: '',
    password: '',
    name: { firstname: '', lastname: '' },
    email: '',
    phone: '',
    status: 'Active',
    role: 'Customer',
    address: {
      city: '',
      street: '',
      number: 0,
      zipcode: '',
      geolocation: { lat: '', long: '' }
    }
  };

  errors: string[] = [];
  isEdit = false;

  constructor(private userService: UserService, private router: Router) {}
  ngOnInit() {
    // Verifica se há id na rota para edição
    const url = this.router.url;
    const match = url.match(/\/users\/edit\/(.+)$/);
    if (match && match[1]) {
      this.isEdit = true;
      this.userService.getUser(match[1]).subscribe(u => {
        // Garante que address e geolocation existam
        if (!u.address) {
          u.address = { city: '', street: '', number: 0, zipcode: '', geolocation: { lat: '', long: '' } };
        }
        if (!u.address.geolocation) {
          u.address.geolocation = { lat: '', long: '' };
        }
        this.user = u;
      });
    } else {
      // Garante que address e geolocation existam na criação
      if (!this.user.address) {
        this.user.address = { city: '', street: '', number: 0, zipcode: '', geolocation: { lat: '', long: '' } };
      }
      if (!this.user.address.geolocation) {
        this.user.address.geolocation = { lat: '', long: '' };
      }
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
    if (this.isEdit && this.user.id) {
      this.userService.updateUser(this.user.id, this.user as User).subscribe({
        next: () => this.router.navigate(['/users']),
        error: err => {
          this.errors = parseError(err);
        }
      });
    } else {
      this.userService.createUser(this.user).subscribe({
        next: () => this.router.navigate(['/users']),
        error: err => {
          this.errors = parseError(err);
        }
      });
    }
  }
}
