import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable, BehaviorSubject} from 'rxjs';
import {jwtDecode} from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
 private apiUrl = 'http://localhost:7000/api/Auth';

  constructor(private http: HttpClient) { }

  private loginStatus = new BehaviorSubject<boolean>(this.isLoggedIn());
  loginStatus$ = this.loginStatus.asObservable();


  loginUser(credentials: {username: string, password: string}): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, credentials);
  }

  registerUser(data:any) : Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, data);
  }

  saveToken(token: string, rememberMe: boolean): void {
    if (rememberMe) {
      localStorage.setItem('jwtToken', token);
    } else {
      sessionStorage.setItem('jwtToken', token);
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

  checkPhone(phone: string) {
    return this.http.get(`${this.apiUrl}/check-phone?phoneNumber=${phone}`);
  }



}
