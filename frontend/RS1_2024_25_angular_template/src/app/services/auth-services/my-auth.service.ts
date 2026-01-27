import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MyAuthInfo } from "./dto/my-auth-info";
import { LoginTokenDto } from './dto/login-token-dto';

@Injectable({ providedIn: 'root' })
export class MyAuthService {
  constructor(private httpClient: HttpClient) {
    // CRITICAL: Restore my-auth-token synchronously on service creation
    // This runs BEFORE any routing/guards, ensuring tokens are ready
    this.restoreMyAuthTokenIfNeeded();
  }

  private restoreMyAuthTokenIfNeeded(): void {
  const existing = this.getLoginToken();
  const jwtToken = localStorage.getItem('jwtToken') || sessionStorage.getItem('jwtToken');

  // If JWT exists but my-auth-token is missing or incomplete, restore it
  if (jwtToken && (!existing || !existing.myAuthInfo)) {
    try {
      const payload = JSON.parse(atob(jwtToken.split('.')[1]));
      const exp = payload.exp;
      const now = Math.floor(Date.now() / 1000);

      if (now < exp) { // Token still valid
        const authInfo = {
          userId: Number(payload.sub || payload.id || payload.userId),
          username: payload.username,
          firstName: payload.name,
          lastName: payload.surname,
          isAdmin: payload.role === 'Admin' || payload.IsAdmin === true,
          isManager: payload.role === 'Manager' || false,
          isLoggedIn: true
        };

        console.log('✅ Restoring token for:', payload.username, 'Admin:', authInfo.isAdmin);

        this.setLoggedInUser({
          token: jwtToken,
          myAuthInfo: authInfo
        });
      } else {
        // Token expired, clean up
        console.log('⚠️ Token expired, cleaning up');
        localStorage.removeItem('jwtToken');
        sessionStorage.removeItem('jwtToken');
        localStorage.removeItem('my-auth-token');
        localStorage.removeItem('refreshToken'); // Also remove refresh token
      }
    } catch (e) {
      console.error('❌ Error restoring my-auth-token:', e);
      // Clean up on error
      localStorage.removeItem('jwtToken');
      sessionStorage.removeItem('jwtToken');
      localStorage.removeItem('my-auth-token');
    }
  }
}

  getMyAuthInfo(): MyAuthInfo | null {
    return this.getLoginToken()?.myAuthInfo ?? null;
  }

  isLoggedIn(): boolean {
    const token = localStorage.getItem('jwtToken') || sessionStorage.getItem('jwtToken');
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
