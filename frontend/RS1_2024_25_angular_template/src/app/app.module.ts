import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MyAuthInterceptor } from './services/auth-services/my-auth-interceptor.service';
import { NavbarComponent } from '../../src/app/navbar/navbar.component';
import { SharedModule } from './modules/shared/shared.module';
import {NgOptimizedImage} from "@angular/common";
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { HeroSectionComponent } from './hero-section/hero-section.component';
import { ProductSectionComponent } from './product-section/product-section.component';
import { FooterComponent } from './footer/footer.component';
import {ReactiveFormsModule} from '@angular/forms';
import { PartsComponent } from './parts/parts.component';
import { PartDetailComponent } from './part-detail/part-detail.component';
import { ViewPartsComponent } from './view-parts/view-parts.component';
import {RouterModule} from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { CartComponent } from './cart/cart.component';
import {SocialLoginModule , SocialAuthServiceConfig} from '@abacritt/angularx-social-login';
import {GoogleLoginProvider} from '@abacritt/angularx-social-login';
import { CheckoutComponent } from './checkout/checkout.component';
import { OrderSuccessComponent } from './order-success/order-success.component';
import {EditProfileComponent} from './edit-profile/edit-profile.component';
import { SecurityComponent } from './security/security.component';
import { AngularFireModule } from '@angular/fire/compat';
import { AngularFireAuthModule } from '@angular/fire/compat/auth';
import { environment } from '../environments/environment';
import { FaqComponent } from './faq/faq.component';
import { AboutUsComponent } from './about-us/about-us.component';



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
    PartDetailComponent,
    ViewPartsComponent,
    CartComponent,
    CheckoutComponent,
    OrderSuccessComponent,
    EditProfileComponent,
    SecurityComponent,
    FaqComponent,
    AboutUsComponent,

  ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
        SharedModule,
        NgOptimizedImage,
        ReactiveFormsModule,
        BrowserAnimationsModule,
        SocialLoginModule,
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
      provide: 'SocialAuthServiceConfig',
      useValue: {
        autoLogin: false,
        providers: [
          {
            id: GoogleLoginProvider.PROVIDER_ID,
            provider: new GoogleLoginProvider('609510374900-m9pfd5u7gvek04q0kr3f8g02spsn45ir.apps.googleusercontent.com'),
          },
        ],
      } as SocialAuthServiceConfig
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MyAuthInterceptor,
      multi: true,
    }
  ],

  bootstrap: [AppComponent]
})
export class AppModule { }
