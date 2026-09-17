import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LatestPurchaseOrders } from './latest-purchase-orders';

describe('LatestPurchaseOrders', () => {
  let component: LatestPurchaseOrders;
  let fixture: ComponentFixture<LatestPurchaseOrders>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LatestPurchaseOrders],
    }).compileComponents();

    fixture = TestBed.createComponent(LatestPurchaseOrders);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
