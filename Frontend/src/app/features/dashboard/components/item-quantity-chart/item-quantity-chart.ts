import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PurchaseOrderService } from '../../../../core/services/purchase-order';

import {
  ItemQuantity
} from '../../../../core/models/purchase-order';

@Component({
  selector: 'app-item-quantity-chart',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './item-quantity-chart.html',
  styleUrl: './item-quantity-chart.css'
})
export class ItemQuantityChartComponent
  implements OnInit {

  items: ItemQuantity[] = [];

  totalQuantity = 0;

  chartBackground = '';

  isLoading = false;

  errorMessage = '';

  constructor(
    private purchaseOrderService: PurchaseOrderService
  ) {}

  ngOnInit(): void {
    this.loadChart();
  }

  loadChart(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.purchaseOrderService
      .getItemQuantities()
      .subscribe({
        next: response => {

          this.items = response;

          this.totalQuantity =
            this.items.reduce(
              (total, item) =>
                total + item.quantity,
              0
            );

          this.createChart();

          this.isLoading = false;
        },

        error: error => {

          console.error(error);

          this.errorMessage =
            'Unable to load item quantities.';

          this.isLoading = false;
        }
      });
  }

  createChart(): void {

    if (this.totalQuantity === 0) {
      this.chartBackground =
        'conic-gradient(#eeeeee 0deg 360deg)';

      return;
    }

    let currentDegree = 0;

    const segments: string[] = [];

    const colors = [
      '#079bd3',
      '#13b9c8',
      '#0c7198',
      '#18c978',
      '#5b7cfa',
      '#f3a712',
      '#8b5cf6'
    ];

    this.items.forEach((item, index) => {

      const percentage =
        item.quantity / this.totalQuantity;

      const degree =
        percentage * 360;

      const start =
        currentDegree;

      const end =
        currentDegree + degree;

      const color =
        colors[index % colors.length];

      segments.push(
        `${color} ${start}deg ${end}deg`
      );

      currentDegree = end;
    });

    this.chartBackground =
      `conic-gradient(${segments.join(', ')})`;
  }

  getPercentage(quantity: number): number {

    if (this.totalQuantity === 0) {
      return 0;
    }

    return Math.round(
      (quantity / this.totalQuantity) * 100
    );
  }
}