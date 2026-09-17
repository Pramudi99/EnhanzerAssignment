import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

import { LatestPurchaseOrdersComponent } from '../../components/latest-purchase-orders/latest-purchase-orders';
import { OldestPurchaseItemsComponent } from '../../components/oldest-purchase-items/oldest-purchase-items';
import { ItemQuantityChartComponent } from '../../components/item-quantity-chart/item-quantity-chart';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    RouterLink,
    LatestPurchaseOrdersComponent,
    OldestPurchaseItemsComponent,
    ItemQuantityChartComponent
  ],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard {

}