import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable, BehaviorSubject} from 'rxjs';
import {jwtDecode} from 'jwt-decode';
import { MyAuthService } from './my-auth.service';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
 private apiUrl = 'http://localhost:7000/api/Auth';

  constructor(private http: HttpClient, private myAuthService: MyAuthService) { }

  private loginStatus = new BehaviorSubject<boolean>(this.isLoggedIn());
  loginStatus$ = this.loginStatus.asObservable();


  loginUser(credentials: {username: string, password: string}): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, credentials);
  }

  registerUser(data:any) : Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, data);
  }

  saveToken(token: string, rememberMe: boolean): void {
    if (!token || typeof token !== 'string' || token.split('.').length !== 3) {
      console.error('❌ Invalid token received. Token skipped.');
      return;
    }

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const isAdmin = payload.role === 'Admin' || payload.IsAdmin === true;
      const isManager = payload.role === 'Manager' || false;


      const loginToken = {
        token: token,
        myAuthInfo: {
          userId: Number(payload.sub || payload.id || payload.userId),
          username: payload.username,
          firstName: payload.name,
          lastName: payload.surname,
          isAdmin,
          isManager,
          isLoggedIn: true
        }
      };

      if (rememberMe) {
        localStorage.setItem('jwtToken', token);
      } else {
        sessionStorage.setItem('jwtToken', token);
      }

      this.myAuthService.setLoggedInUser(loginToken);
    } catch (error) {
      console.error('❌ Failed to decode or process token:', error);
    }
  }


  getTokenUser() : string | null {
    return localStorage.getItem('jwtToken') || sessionStorage.getItem('jwtToken');
  }

  isLoggedIn() : boolean {
    return !! this.getTokenUser();
  }

  getUserInfoFromToken(): any {
    const token = this.getTokenUser();
    if (!token) return null;

    const payload = JSON.parse(atob(token.split('.')[1]));
    return {
      username: payload['username'],
      name: payload['name'],
      surname: payload['surname'],
      email: payload['email'],
      phone: payload['phone'],
      cityId: payload['cityId'],
      address: payload['address'],
      role: payload['role']
    };
  }

  setLoginStatus(loggedIn: boolean) {
    this.loginStatus.next(loggedIn);
  }

  getUserProfile(): Observable<any> {
    return this.http.get<any>('http://localhost:7000/api/UserAccount/profile');
  }

  private userInfoSubject = new BehaviorSubject<any>(null);
  userInfo$ = this.userInfoSubject.asObservable();

  setUserInfo(user: any) {
    this.userInfoSubject.next(user);
  }

  checkUsername(username: string) {
    return this.http.get(`${this.apiUrl}/check-username?username=${username}`);
  }

  checkEmail(email: string) {
    return this.http.get(`${this.apiUrl}/check-email?email=${email}`);
  }

  checkPhone(phone: string, userId?: number) {
    const encodedPhone = encodeURIComponent(phone);
    let url = `${this.apiUrl}/check-phone?phoneNumber=${encodedPhone}`;
    if (userId !== undefined) {
      url += `&userId=${userId}`;
    }
    return this.http.get(url);
  }

  googleLogin(token: string) {
    return this.http.post(`${this.apiUrl}/google-login`, { IdToken: token });
  }

  resetPassword(data: { email: string, newPassword: string }) {
    return this.http.post(`${this.apiUrl}/reset-password`, data);
  }

  getUserId(): number {
    const token = this.getTokenUser();
    if (!token) return 0;
    const payload = JSON.parse(atob(token.split('.')[1]));
    return Number(payload['sub'] || payload['userId'] || payload['id'] || 0);
  }

  changePassword(currentPassword: string, newPassword: string) {
    return this.http.post(`http://localhost:7000/api/Auth/change-password`, {
      currentPassword,
      newPassword
    });
  }

  enableTwoFactor(phoneNumber: string) {
    return this.http.post(`http://localhost:7000/api/Auth/enable-2fa`, { phoneNumber });
  }

  deactivate(username: string) {
    return this.http.post(`${this.apiUrl}/deactivate`, {username} );
  }

  reactivateProfile(email: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/reactivate`, { email }, { responseType: 'text' });
  }

}
