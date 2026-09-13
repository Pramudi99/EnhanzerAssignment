import { Component } from '@angular/core';
import { Router} from '@angular/router';
import { Auth } from '../../../core/services/auth';


@Component({
  imports: [],
  selector: 'app-header',
  styleUrl: './header.css',
  templateUrl: './header.html',
})
export class Header {
   constructor(
    private authService: Auth,
    private router: Router
  ) {}

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
