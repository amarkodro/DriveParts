import { Injectable } from "@angular/core";
import {
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpEvent,
  HttpErrorResponse
} from "@angular/common/http";
import { Observable, throwError, BehaviorSubject, switchMap, catchError } from "rxjs";
import { AuthService } from "./auth.service";
import {Router} from '@angular/router';

@Injectable()
export class MyAuthInterceptor implements HttpInterceptor {
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);

  constructor(private authService: AuthService, private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const jwtToken = this.authService.getTokenUser();
    let request = req;

    if (jwtToken) {
      request = this.addTokenHeader(req, jwtToken);
    }

    return next.handle(request).pipe(
      catchError(error => {
        if (error instanceof HttpErrorResponse && error.status === 401 && !request.url.includes('/login')) {
          return this.handle401Error(request, next);
        }

        return throwError(() => error);
      })
    );
  }

  private addTokenHeader(request: HttpRequest<any>, token: string): HttpRequest<any> {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  private handle401Error(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      const refreshToken = this.authService.getRefreshToken();
      if (!refreshToken) {
        this.isRefreshing = false;
        this.authService.removeRefreshToken();
        return throwError(() => new Error('No refresh token'));
      }

      return this.authService.refreshToken(refreshToken).pipe(
        switchMap(res => {
          this.isRefreshing = false;
          this.authService.saveToken(res.token, true);
          this.authService.saveRefreshToken(res.refreshToken);
          this.refreshTokenSubject.next(res.token);
          return next.handle(this.addTokenHeader(request, res.token));
        }),
        catchError(err => {
          this.isRefreshing = false;
          this.authService.removeRefreshToken();
          this.authService.setLoginStatus(false);
          localStorage.removeItem('jwtToken');
          sessionStorage.removeItem('jwtToken');
          this.authService?.setUserInfo(null);
          this.router.navigate(['/login']);
          return throwError(() => err);
        })
      );
    } else {
      return this.refreshTokenSubject.pipe(
        switchMap(token => {
          if (token) {
            return next.handle(this.addTokenHeader(request, token));
          } else {
            return throwError(() => new Error('Failed to refresh token'));
          }
        })
      );
    }
  }
}
