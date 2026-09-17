import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ItemQuantityChart } from './item-quantity-chart';

describe('ItemQuantityChart', () => {
  let component: ItemQuantityChart;
  let fixture: ComponentFixture<ItemQuantityChart>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ItemQuantityChart],
    }).compileComponents();

    fixture = TestBed.createComponent(ItemQuantityChart);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
