import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router } from '@angular/router';
import { Auth } from '../../../../core/services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {

  loginForm: FormGroup;
  errorMessage = '';
  isLoading = false;
  showPassword = false;

  constructor(
    private fb: FormBuilder,
    private authService: Auth,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['', [
        Validators.required,
        Validators.email
      ]],

      password: ['', [
        Validators.required
      ]]
    });
  }

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  onSubmit(): void {

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.authService
      .login(this.loginForm.value)
      .subscribe({

        next: (response) => {

          localStorage.setItem(
            'token',
            response.token
          );

          this.router.navigate([
            '/purchase-bill'
          ]);
        },

        error: (error) => {

          this.isLoading = false;

          if (error.status === 401) {

            this.errorMessage =
              error.error?.message ||
              'Invalid email or password.';

          } else {

            this.errorMessage =
              'Unable to connect to the server.';
          }
        },

        complete: () => {
          this.isLoading = false;
        }
      });
  }
}
