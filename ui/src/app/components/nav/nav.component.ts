import { Component, OnInit } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatBadgeModule } from '@angular/material/badge';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Router, RouterModule, NavigationEnd } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth/auth.service';
import { CookieServ } from '../../services/cookie/cookie.service';
import { User } from '../../interfaces/user/user.interface';
import { filter } from 'rxjs/operators';

interface CartItem {
  quantity: number;
}

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [
    CommonModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatBadgeModule,
    MatTooltipModule,
    RouterModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule
  ],
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  cartItemCount: number = 0;
  isLoggedIn: boolean = false;
  userData: User = {} as User;


  constructor(
    private authService: AuthService,
    private cookieService: CookieServ,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.checkAuthStatus();

    this.updateCartItemCount();

    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      this.updateCartItemCount();
    });
  }

  checkAuthStatus(): void {
    const token = this.cookieService.getCookie();
    if (token) {
      this.isLoggedIn = true;
      this.userData = this.User;
    } else {
      this.isLoggedIn = false;
      this.userData = {} as User;
    }
  }

  updateCartItemCount(): void {
    const cartJson = localStorage.getItem('cart');
    if (cartJson) {
      const cartItems = JSON.parse(cartJson) as CartItem[];
      this.cartItemCount = cartItems.reduce((total, item) => total + item.quantity, 0);
    } else {
      this.cartItemCount = 0;
    }
  }

  logout(): void {
    this.cookieService.removeCookie();
    this.authService.removeLocalStorage();
    this.isLoggedIn = false;
    this.router.navigate(['/']);
  }

  get User(): User {
    return JSON.parse(this.authService.getLocalStorage() || '{}');
  }
}
