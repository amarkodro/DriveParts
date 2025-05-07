import {HttpClient} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {MyAuthInfo} from "./dto/my-auth-info";
import {LoginTokenDto} from './dto/login-token-dto';

@Injectable({providedIn: 'root'})
export class MyAuthService {
  constructor(private httpClient: HttpClient) {
  }

  getMyAuthInfo(): MyAuthInfo | null {
    return this.getLoginToken()?.myAuthInfo ?? null;
  }

  isLoggedIn(): boolean {
    const token = localStorage.getItem('jwtToken');
    if (!token) return false;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const exp = payload.exp;
      const now = Math.floor(Date.now() / 1000);
      return now < exp;
    } catch {
      return false;
    }
  }


  isAdmin(): boolean {
    return this.getMyAuthInfo()?.isAdmin ?? false;
  }

  isManager(): boolean {
    return this.getMyAuthInfo()?.isManager ?? false;
  }

  setLoggedInUser(x: LoginTokenDto | null) {
    if (x == null) {
      window.localStorage.setItem("my-auth-token", '');
    } else {
      window.localStorage.setItem("my-auth-token", JSON.stringify(x));
    }
  }

  getLoginToken(): LoginTokenDto | null {
    let tokenString = window.localStorage.getItem("my-auth-token") ?? "";
    try {
      return JSON.parse(tokenString);
    } catch (e) {
      return null;
    }
  }
}
