import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth-guard';
import { Layout } from './shared/components/layout/layout';
import { Login} from './features/auth/pages/login/login';
import { PurchaseBill } from './features/purchase-bill/pages/purchase-bill/purchase-bill';

export const routes: Routes = [
  { 
    path: 'login',
    component: Login
  },
  {
    path: '',
    component: Layout,
    canActivate: [authGuard],
    children: [

      {
        path: 'purchase-bill',
        component: PurchaseBill
      }

    ]
  },

 {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  }
];
