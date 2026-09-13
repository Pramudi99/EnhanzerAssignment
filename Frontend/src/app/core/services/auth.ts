import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface Location {
  location_Code: string;
  location_Name: string;
}

export interface LoginResponse {
  message: string;
  token: string;
  locations: Location[];
}

@Injectable({
  providedIn: 'root'
})
export class Auth {
    private apiUrl = `${environment.apiUrl}/Auth`;
  constructor(private http: HttpClient) {}

  login(data: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(
      `${this.apiUrl}/login`,
      data
    );
  }

  getToken(): string | null {
  return localStorage.getItem('token');
}

isLoggedIn(): boolean {

  const token = this.getToken();

  if (!token) {
    return false;
  }

  try {

    const payload = JSON.parse(
      atob(token.split('.')[1])
    );

    const expiration = payload.exp * 1000;

    if (Date.now() >= expiration) {
      this.logout();
      return false;
    }

    return true;

  } catch {
    this.logout();
    return false;
  }
}

  logout(): void {
    localStorage.removeItem('token');
  }
}
