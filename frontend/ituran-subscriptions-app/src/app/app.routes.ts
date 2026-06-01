import { Routes } from '@angular/router';

import { Dashboard } from './features/dashboard/dashboard';
import { Subscriptions } from './features/subscriptions/subscriptions';
import { Analytics } from './features/analytics/analytics';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  {
    path: 'dashboard',
    component: Dashboard
  },
  {
    path: 'subscriptions',
    component: Subscriptions
  },
  {
    path: 'analytics',
    component: Analytics
  },
  {
    path: '**',
    redirectTo: 'dashboard'
  }
];