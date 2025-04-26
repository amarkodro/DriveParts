import { Injectable } from '@angular/core';
import {loadStripe} from '@stripe/stripe-js';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class StripeService {

  private stripePromise = loadStripe('pk_test_51RBGYXR0PL10ni1FgCXziUZSMbm2cXaNqml2aTnyaiEITm8OJ6iHPASauz17hVX24GlxgvHkXrrlvBERgLdOEvoE00uwMvM4kt');

  constructor(private http : HttpClient) { }

  async redirectToCheckout(items: any[]) {
    const stripe = await this.stripePromise;

    return this.http.post<any>('http://localhost:7000/api/stripe/create-checkout-session', { items: items })
      .toPromise()
      .then(async (res) => {
        return stripe?.redirectToCheckout({ sessionId: res.sessionId });
      })
      .catch(err => {
        console.error('Stripe error:', err);
        throw err;
      });
  }
}
