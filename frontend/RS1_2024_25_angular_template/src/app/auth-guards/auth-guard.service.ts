import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router} from '@angular/router';
import {MyAuthService} from '../services/auth-services/my-auth.service';

export class AuthGuardData {
  isAdmin?: boolean;
  isManager?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: MyAuthService, private router: Router) {
  }

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const guardData = route.data as AuthGuardData;  // Cast to AuthGuardData

    console.log('🔐 AuthGuard aktivan');
    console.log('Token:', this.authService.getLoginToken());
    console.log('isLoggedIn:', this.authService.getLoginToken()?.myAuthInfo?.isLoggedIn);


    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return false;
    }

    // Provjera prava pristupa za administratora
    if (guardData.isAdmin && !this.authService.isAdmin()) {
      this.router.navigate(['/dashboard']);
      return false;
    }

    // Provjera prava pristupa za menadžera
    if (guardData.isManager && !this.authService.isManager()) {
      this.router.navigate(['/unauthorized']);
      return false;
    }

    return true; // Dozvoljen pristup
  }

}
