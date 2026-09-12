import { Component, ChangeDetectorRef } from '@angular/core';
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
  successMessage = '';

  isLoading = false;
  showPassword = false;


  constructor(
    private fb: FormBuilder,
    private authService: Auth,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {

    this.loginForm = this.fb.group({

      email: [
        '',
        [
          Validators.required,
          Validators.email
        ]
      ],

      password: [
        '',
        [
          Validators.required
        ]
      ]

    });
  }


  // =========================================
  // Show / Hide Password
  // =========================================

  togglePassword(): void {

    this.showPassword =
      !this.showPassword;

  }


  // =========================================
  // Login
  // =========================================

  onSubmit(): void {

    // Clear previous messages
    this.errorMessage = '';
    this.successMessage = '';


    // =========================================
    // Validate Form
    // =========================================

    if (this.loginForm.invalid) {

      this.loginForm.markAllAsTouched();

      this.cdr.detectChanges();

      return;
    }


    // =========================================
    // Start Loading
    // =========================================

    this.isLoading = true;

    this.cdr.detectChanges();


    // =========================================
    // Call Backend
    // =========================================

    this.authService
      .login(this.loginForm.value)
      .subscribe({

        // =====================================
        // SUCCESS
        // =====================================

        next: (response) => {

          console.log(
            'Login successful:',
            response
          );


          // Store JWT
          localStorage.setItem(
            'token',
            response.token
          );


          // Stop loading
          this.isLoading = false;


          // Show success message
          this.successMessage =
            'Login successful.';


          // Force Angular to update UI
          this.cdr.detectChanges();


          // Navigate
          setTimeout(() => {

            this.router.navigate([
              '/purchase-bill'
            ]);

          }, 1000);

        },


        // =====================================
        // ERROR
        // =====================================

        error: (error) => {

          console.log(
            'Login failed:',
            error
          );

          console.log(
            'Status:',
            error.status
          );


          // IMPORTANT
          // Stop loading
          this.isLoading = false;


          // Clear success
          this.successMessage = '';


          // ===================================
          // 401 - INVALID LOGIN
          // ===================================

          if (error.status === 401) {

            this.errorMessage =
              'Login failed. Invalid email or password.';

          }


          // ===================================
          // SERVER NOT AVAILABLE
          // ===================================

          else if (error.status === 0) {

            this.errorMessage =
              'Unable to connect to the server. Please try again.';

          }


          // ===================================
          // OTHER ERROR
          // ===================================

          else {

            this.errorMessage =
              'Login failed. Please try again.';

          }


          // ===================================
          // DEBUG
          // ===================================

          console.log(
            'Error message:',
            this.errorMessage
          );


          // ===================================
          // IMPORTANT
          // Force UI update
          // ===================================

          this.cdr.detectChanges();

        },


        // =====================================
        // COMPLETE
        // =====================================

        complete: () => {

          this.isLoading = false;

          this.cdr.detectChanges();

        }

      });

  }

}