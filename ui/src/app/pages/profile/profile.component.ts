import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { AuthService } from '../../services/auth/auth.service';
import { UserInfo, User } from '../../interfaces/user/user.interface';
import { NavComponent } from "../../components/nav/nav.component";

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule,
    MatDividerModule,
    NavComponent
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  user: User = {} as User;
  userData: UserInfo = {} as UserInfo;

  constructor(
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.loadUserData();
  }

  loadUserData(): void {
    const userJson = this.authService.getLocalStorage();
    if (userJson) {
      this.user = JSON.parse(userJson);
    }

    this.authService.getUser(this.user.id).subscribe((userData: UserInfo) => {
      this.userData = userData;
    });
  }
}
