import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PurchaseOrderService } from '../../../../core/services/purchase-order';

import { LatestPurchaseOrder } from '../../../../core/models/purchase-order';

@Component({
  selector: 'app-latest-purchase-orders',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './latest-purchase-orders.html',
  styleUrl: './latest-purchase-orders.css'
})
export class LatestPurchaseOrdersComponent
  implements OnInit {

  orders: LatestPurchaseOrder[] = [];

  isLoading = false;

  errorMessage = '';

  constructor(
    private purchaseOrderService: PurchaseOrderService
  ) {}

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.purchaseOrderService
      .getLatest()
      .subscribe({
        next: response => {

          this.orders = response;

          this.isLoading = false;
        },

        error: error => {

          console.error(error);

          this.errorMessage =
            'Unable to load purchase orders.';

          this.isLoading = false;
        }
      });
  }
}