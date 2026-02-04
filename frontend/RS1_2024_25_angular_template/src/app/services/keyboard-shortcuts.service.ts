import { Injectable, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './auth-services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
    providedIn: 'root'
})
export class KeyboardShortcutsService {

    constructor(
        private router: Router,
        private authService: AuthService,
        private toastr: ToastrService,
        private ngZone: NgZone
    ) { }

    init() {
        this.ngZone.runOutsideAngular(() => {
            document.addEventListener('keydown', (event) => {
                this.handleKeyboardEvent(event);
            });
        });
    }

    private handleKeyboardEvent(event: KeyboardEvent) {
        // Check if Alt + Shift are pressed
        if (event.altKey && event.shiftKey) {

            this.ngZone.run(() => {
                switch (event.key.toUpperCase()) {
                    case 'D': // Dashboard (Admin Only)
                        this.handleDashboardShortcut();
                        break;
                    case 'A': // AI Chat
                        this.navigateTo('/ai', 'AI Chat');
                        break;
                    case 'C': // AI Chat (Alternative)
                        this.navigateTo('/ai', 'AI Chat');
                        break;
                    case 'S': // Settings (Edit Profile)
                        this.navigateTo('/edit-profile', 'Settings');
                        break;
                }
            });
        }
    }

    private handleDashboardShortcut() {
        const isAdmin = this.authService.getUserInfoFromToken()?.role === 'Admin';

        if (isAdmin) {
            this.navigateTo('/dashboard', 'Dashboard');
        } else {
            this.toastr.warning('Access restricted to Admins only.', 'Dashboard Shortcut');
        }
    }

    private navigateTo(path: string, name: string) {
        this.router.navigate([path]).then(success => {
            if (success) {
                // Optional: Show a small toast or log
                // this.toastr.info(`Navigated to ${name}`, 'Shortcut Activated');
            }
        });
    }
}
