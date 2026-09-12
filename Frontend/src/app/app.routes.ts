import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth-guard';
import { Login} from './features/auth/pages/login/login';
import { PurchaseBill } from './features/purchase-bill/pages/purchase-bill/purchase-bill';

export const routes: Routes = [
  { 
    path: 'login',
    component: Login
  },
  { 
    path: 'purchase-bill', 
    component: PurchaseBill,
    canActivate: [authGuard]
},
 {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  }
];
