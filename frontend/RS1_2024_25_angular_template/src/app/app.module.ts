import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MyAuthInterceptor } from './services/auth-services/my-auth-interceptor.service';
import { NavbarComponent } from '../../src/app/navbar/navbar.component';
import { SharedModule } from './modules/shared/shared.module';
import { NgOptimizedImage } from "@angular/common";
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { HeroSectionComponent } from './hero-section/hero-section.component';
import { ProductSectionComponent } from './product-section/product-section.component';
import { FooterComponent } from './footer/footer.component';
import { ReactiveFormsModule } from '@angular/forms';
import { PartsComponent } from './parts/parts.component';
import { PartDetailComponent } from './part-detail/part-detail.component';
import { ViewPartsComponent } from './view-parts/view-parts.component';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { CartComponent } from './cart/cart.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { OrderSuccessComponent } from './order-success/order-success.component';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { SecurityComponent } from './security/security.component';
import { AngularFireModule } from '@angular/fire/compat';
import { AngularFireAuthModule } from '@angular/fire/compat/auth';
import { environment } from '../environments/environment';
import { FaqComponent } from './faq/faq.component';
import { AboutUsComponent } from './about-us/about-us.component';
import { DashboardComponent } from './dashboard/dashboard.component';
//import { NgChartsModule } from 'ng2-charts';
import { CommonModule } from '@angular/common';
import { AdminPartFormComponent } from './admin-part-form/admin-part-form.component';
import { AdminPartsComponent } from './admin-parts/admin-parts.component';
import { OrdersComponent } from './admin-orders/admin-orders.component';
import { MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ChatComponent } from './ai-chat/ai-chat.component';
import { FormsModule } from '@angular/forms';
import { OrdersModalComponent } from './orders-modal/orders-modal.component';
import { CustomerListComponent } from './customer-list/customer-list.component';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatDialogModule } from '@angular/material/dialog';
import { MyOrdersComponent } from './my-orders/my-orders.component';
import { ConfirmationDialogComponent } from './confirmation-dialog/confirmation-dialog.component';
import { FloatingAiButtonComponent } from './floating-ai-button/floating-ai-button.component';
import { UserSupportChatComponent } from './user-support-chat/user-support-chat.component';
import { AdminChatInboxComponent } from './admin-chat-inbox/admin-chat-inbox.component';
import { DragDropDirective } from './directives/drag-drop.directive';
import { MyFavoritesComponent } from './my-favorites/my-favorites.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    LoginComponent,
    RegisterComponent,
    HeroSectionComponent,
    ProductSectionComponent,
    FooterComponent,
    PartsComponent,
    MyFavoritesComponent, // Add here
    AdminPartFormComponent,
    AdminPartsComponent,
    PartDetailComponent,
    ViewPartsComponent,
    CartComponent,
    CheckoutComponent,
    OrderSuccessComponent,
    EditProfileComponent,
    SecurityComponent,
    FaqComponent,
    AboutUsComponent,
    DashboardComponent,
    OrdersComponent,
    ChatComponent,
    OrdersModalComponent,
    CustomerListComponent,
    MyOrdersComponent,
    ConfirmationDialogComponent,
    FloatingAiButtonComponent,
    UserSupportChatComponent,
    AdminChatInboxComponent,
    DragDropDirective,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    SharedModule,
    MatTableModule,
    MatSelectModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    // NgChartsModule,
    NgOptimizedImage,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    CommonModule,
    FormsModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    BrowserModule,
    HttpClientModule,
    MatInputModule,
    MatButtonModule,
    MatPaginatorModule,
    MatDialogModule,
    MatIconModule,
    ToastrModule.forRoot({
      positionClass: 'toast-top-center',
      timeOut: 3000,
      progressBar: true
    }),
    RouterModule.forRoot([], {
      anchorScrolling: 'enabled',
      scrollPositionRestoration: 'enabled'
    }),
    AngularFireModule.initializeApp(environment.firebase),
    AngularFireAuthModule,



  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MyAuthInterceptor,
      multi: true,
    }
  ],

  bootstrap: [AppComponent]
})
export class AppModule { }
