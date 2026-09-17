import { ComponentFixture, TestBed } from '@angular/core/testing';
import { OldestPurchaseItems } from './oldest-purchase-items';

describe('OldestPurchaseItems', () => {
  let component: OldestPurchaseItems;
  let fixture: ComponentFixture<OldestPurchaseItems>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OldestPurchaseItems],
    }).compileComponents();

    fixture = TestBed.createComponent(OldestPurchaseItems);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
