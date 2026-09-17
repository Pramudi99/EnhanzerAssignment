import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  CreatePurchaseOrder,
  LatestPurchaseOrder,
  OldestPurchaseOrderItem,
  ItemQuantity
} from '../models/purchase-order';

@Injectable({
  providedIn: 'root'
})
export class PurchaseOrderService {

  private apiUrl =
    'http://localhost:5135/api/PurchaseOrder';

  constructor(private http: HttpClient) {}

  create(
    data: CreatePurchaseOrder
  ): Observable<any> {
    return this.http.post<any>(
      this.apiUrl,
      data
    );
  }

  getLatest(): Observable<LatestPurchaseOrder[]> {
    return this.http.get<LatestPurchaseOrder[]>(
      `${this.apiUrl}/latest`
    );
  }

  getOldestItems(): Observable<OldestPurchaseOrderItem[]> {
    return this.http.get<OldestPurchaseOrderItem[]>(
      `${this.apiUrl}/oldest-items`
    );
  }

  getItemQuantities(): Observable<ItemQuantity[]> {
    return this.http.get<ItemQuantity[]>(
      `${this.apiUrl}/item-quantities`
    );
  }
}