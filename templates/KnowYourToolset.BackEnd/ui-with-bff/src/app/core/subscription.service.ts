import { Injectable } from '@angular/core';
import { Subscription } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SubscriptionService {
  constructor() {}

  resetSubscription(subscription: Subscription | null): void {
    subscription?.unsubscribe();
    subscription = null;
  }
}