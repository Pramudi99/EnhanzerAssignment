import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PurchaseOrderService } from '../../../../core/services/purchase-order';

import {
  OldestPurchaseOrderItem
} from '../../../../core/models/purchase-order';

@Component({
  selector: 'app-oldest-purchase-items',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './oldest-purchase-items.html',
  styleUrl: './oldest-purchase-items.css'
})
export class OldestPurchaseItemsComponent
  implements OnInit {

  items: OldestPurchaseOrderItem[] = [];

  isLoading = false;

  errorMessage = '';

  constructor(
    private purchaseOrderService: PurchaseOrderService
  ) {}

  ngOnInit(): void {
    this.loadItems();
  }

  loadItems(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.purchaseOrderService
      .getOldestItems()
      .subscribe({
        next: response => {

          this.items = response;

          this.isLoading = false;
        },

        error: error => {

          console.error(error);

          this.errorMessage =
            'Unable to load purchase items.';

          this.isLoading = false;
        }
      });
  }
}