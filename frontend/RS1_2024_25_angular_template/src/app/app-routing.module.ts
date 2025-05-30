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
import {CartComponent} from './cart/cart.component';
import {CheckoutComponent} from './checkout/checkout.component';
import {OrderSuccessComponent} from './order-success/order-success.component';
import {EditProfileComponent} from './edit-profile/edit-profile.component';
import {SecurityComponent} from './security/security.component';
import {FaqComponent} from './faq/faq.component';
import {AboutUsComponent} from './about-us/about-us.component';

import { DashboardComponent } from './dashboard/dashboard.component';
import { AdminPartsComponent } from './admin-parts/admin-parts.component';
import { AdminPartFormComponent } from './admin-part-form/admin-part-form.component';
import { OrdersComponent } from './admin-orders/admin-orders.component';
import { ChatComponent } from './ai-chat/ai-chat.component';

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
    canActivate: [AuthGuard],
    loadChildren: () => import('./modules/client/client.module').then(m => m.ClientModule)  // Lazy load  modula
  },
  {
    path: 'auth',
    loadChildren: () => import('./modules/auth/auth.module').then(m => m.AuthModule)  // Lazy load  modula
  },

  {path: '', component: HeroSectionComponent},
  {path: 'parts', component: PartsComponent},
  {path: 'part-detail/:id' , component: PartDetailComponent },
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'checkout', component: CheckoutComponent},
  {path: 'product-section', component: ProductSectionComponent},
  {path: 'navbar', component: NavbarComponent},
  {path: 'view-parts', component: ViewPartsComponent,},
  {path: 'cart', component: CartComponent,},
  {path: 'edit-profile', component: EditProfileComponent, canActivate: [AuthGuard] },
  {path: 'security', component: SecurityComponent,},
  {path: 'order-success', component: OrderSuccessComponent,},
  {path: 'faq', component: FaqComponent,},
  {path: 'about-us', component: AboutUsComponent,},
  {path:'dashboard',component:DashboardComponent, canActivate: [AuthGuard], data: {isAdmin : true}},
  {path:'edit',component:AdminPartsComponent, canActivate: [AuthGuard], data: {isAdmin : true}},
  {path:'add',component:AdminPartFormComponent, canActivate: [AuthGuard], data: {isAdmin : true}},
  {path:'orders',component:OrdersComponent, canActivate: [AuthGuard], data: {isAdmin : true}},
  {path:'put/:id',component:AdminPartFormComponent, canActivate: [AuthGuard], data: {isAdmin : true}},
  {path:'ai',component:ChatComponent},
 {path: '**', redirectTo: 'public', pathMatch: 'full'},
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


