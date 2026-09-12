export interface PurchaseBillItem {
  item: string;
  batch: string;
  standardCost: number;
  standardPrice: number;
  quantity: number;
  discount: number;
  totalCost: number;
  totalSelling: number;
}