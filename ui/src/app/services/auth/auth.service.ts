import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthLoginRequest } from '../../interfaces/auth/auth-login.request.interface';
import { AuthRegisterRequest } from '../../interfaces/auth/auth-register.request.interface';
import { Observable } from 'rxjs';
import { AuthLoginResponse  } from '../../interfaces/auth/auth-login.response.interface';
import { AuthRegisterResponse } from '../../interfaces/auth/auth-register.response.interface';
import { AuthRefreshTokenRequest } from '../../interfaces/auth/auth-refreshToken.interface';
import { User, UserInfo } from '../../interfaces/user/user.interface';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseUrl = 'https://localhost:7211/api/v1/Auth';

  constructor(private http: HttpClient) {
    this.getLocalStorage();
  }

  login(request: AuthLoginRequest) : Observable<AuthLoginResponse> {
    return this.http.post<AuthLoginResponse>(`${this.baseUrl}/login`, request);
  }

  register(request: AuthRegisterRequest) : Observable<AuthRegisterResponse> {
    return this.http.post<AuthRegisterResponse>(`${this.baseUrl}/register`, request);
  }

  refreshToken(request: AuthRefreshTokenRequest) : Observable<AuthLoginResponse> {
    return this.http.post<AuthLoginResponse>(`${this.baseUrl}/refresh-token`, request);
  }

  getUser(id:number) : Observable<UserInfo> {
    return this.http.get<UserInfo>(`${this.baseUrl}/user-info/${id}`);
  }

  setLocalStorage(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
  }

  getLocalStorage() {
    return localStorage.getItem('user');
  }

  removeLocalStorage() {
    localStorage.removeItem('user');
  }

}
