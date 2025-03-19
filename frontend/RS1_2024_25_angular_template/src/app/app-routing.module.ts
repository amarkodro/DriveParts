import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {UnauthorizedComponent} from './modules/shared/unauthorized/unauthorized.component';
import {AuthGuard} from './auth-guards/auth-guard.service';
import {HeroSectionComponent} from './hero-section/hero-section.component';
import {ProductSectionComponent} from './product-section/product-section.component';
import {NavbarComponent} from './navbar/navbar.component';
import {LoginComponent} from './login/login.component';
import {RegisterComponent} from './register/register.component';
import {PartsComponent} from './parts/parts.component';
import { PartDetailComponent } from './part-detail/part-detail.component';
import {ViewPartsComponent} from './view-parts/view-parts.component';


const routes: Routes = [
  {path: 'unauthorized', component: UnauthorizedComponent},
  {
    path: 'admin',
    canActivate: [AuthGuard],
    data: {isAdmin: true}, // Proslijeđivanje potrebnih prava pristupa, ako je potrebno
    loadChildren: () => import('./modules/admin/admin.module').then(m => m.AdminModule)  // Lazy load  modula
  },
  {
    path: 'public',
    loadChildren: () => import('./modules/public/public.module').then(m => m.PublicModule)  // Lazy load  modula
  },
  {
    path: 'client',
    loadChildren: () => import('./modules/client/client.module').then(m => m.ClientModule)  // Lazy load  modula
  },
  {
    path: 'auth',
    loadChildren: () => import('./modules/auth/auth.module').then(m => m.AuthModule)  // Lazy load  modula
  },

  {path: '', component: HeroSectionComponent},
  {path: 'parts', component: PartsComponent},
  {path: 'part-detail/:id', component: PartDetailComponent},
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'product-section', component: ProductSectionComponent},
  {path: 'navbar', component: NavbarComponent},
  {path: 'view-parts', component: ViewPartsComponent},
  {path: '**', redirectTo: 'public', pathMatch: 'full'},


];

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    anchorScrolling: 'enabled',
    scrollPositionRestoration: 'enabled',
    scrollOffset: [0, 64]
  })],
  exports: [RouterModule]
})
export class AppRoutingModule {
}


