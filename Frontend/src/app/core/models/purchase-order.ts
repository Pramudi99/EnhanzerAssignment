export interface CreatePurchaseOrder {
  items: CreatePurchaseOrderItem[];
}

export interface CreatePurchaseOrderItem {
  itemName: string;
  quantity: number;
  standardCost: number;
  standardPrice: number;
  discount: number;
}

export interface LatestPurchaseOrder {
  id: number;
  netAmount: number;
  noOfItems: number;
}

export interface OldestPurchaseOrderItem {
  purchaseOrderId: number;
  itemName: string;
  quantity: number;
}

export interface ItemQuantity {
  itemName: string;
  quantity: number;
}