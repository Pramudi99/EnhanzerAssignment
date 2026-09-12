import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { Location } from '../../../../core/services/location';
import { LocationsModel } from '../../../../core/models/location';
import { PurchaseBillItem } from '../../models/purchase-bill-item';

@Component({
  selector: 'app-purchase-bill',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './purchase-bill.html',
  styleUrl: './purchase-bill.css'
})
export class PurchaseBill implements OnInit {

  // Locations from backend
  locations: LocationsModel[] = [];

  // Purchase bill form
  purchaseBillForm: FormGroup;

  // Items added to table
  purchaseBillItems: PurchaseBillItem[] = [];

  // Required item values
  items: string[] = [
    'Mango',
    'Apple',
    'Banana',
    'Orange',
    'Grapes',
    'Kiwi',
    'Strawberry'
  ];

  // Autocomplete filtered items
  filteredItems: string[] = [];

  // Controls whether autocomplete list is displayed
  showItemSuggestions = false;

  constructor(
    private locationService: Location,
    private fb: FormBuilder
  ) {

    this.purchaseBillForm = this.fb.group({

      item: [
        '',
        Validators.required
      ],

      batch: [
        '',
        Validators.required
      ],

      standardCost: [
        null,
        [
          Validators.required,
          Validators.min(0)
        ]
      ],

      standardPrice: [
        null,
        [
          Validators.required,
          Validators.min(0)
        ]
      ],

      quantity: [
        null,
        [
          Validators.required,
          Validators.min(1)
        ]
      ],

      discount: [
        0,
        [
          Validators.required,
          Validators.min(0),
          Validators.max(100)
        ]
      ]

    });

    this.filteredItems = this.items;
  }

  ngOnInit(): void {
    this.loadLocations();
  }

  // ------------------------------------------------
  // Load locations from backend
  // ------------------------------------------------

  loadLocations(): void {

    this.locationService.getLocations().subscribe({

      next: (data) => {

        this.locations = data;

        console.log('Locations:', this.locations);

      },

      error: (error) => {

        console.error(
          'Failed to load locations:',
          error
        );

      }

    });

  }

  // ------------------------------------------------
  // Item autocomplete
  // ------------------------------------------------

  onItemInput(): void {

    const value =
      this.purchaseBillForm
        .get('item')
        ?.value || '';

    const searchText =
      value.toLowerCase().trim();

    if (!searchText) {

      this.filteredItems = this.items;
      this.showItemSuggestions = true;

      return;
    }

    this.filteredItems =
      this.items.filter(item =>
        item.toLowerCase().includes(searchText)
      );

    this.showItemSuggestions = true;
  }

  selectItem(item: string): void {

    this.purchaseBillForm
      .get('item')
      ?.setValue(item);

    this.showItemSuggestions = false;
  }

  hideItemSuggestions(): void {

    // Small delay allows click on suggestion
    // before the list disappears.
    setTimeout(() => {
      this.showItemSuggestions = false;
    }, 150);

  }

  // ------------------------------------------------
  // Add item
  // ------------------------------------------------

  addItem(): void {

    if (this.purchaseBillForm.invalid) {

      this.purchaseBillForm.markAllAsTouched();

      return;
    }

    const formValue =
      this.purchaseBillForm.value;

    const standardCost =
      Number(formValue.standardCost);

    const standardPrice =
      Number(formValue.standardPrice);

    const quantity =
      Number(formValue.quantity);

    const discount =
      Number(formValue.discount);

    // Total Cost
    //
    // Standard Cost × Quantity
    // minus discount percentage

    const totalCost =
      standardCost *
      quantity *
      (1 - discount / 100);

    // Total Selling
    //
    // Standard Price × Quantity

    const totalSelling =
      standardPrice *
      quantity;

    const purchaseBillItem: PurchaseBillItem = {

      item: formValue.item,

      batch: formValue.batch,

      standardCost: standardCost,

      standardPrice: standardPrice,

      quantity: quantity,

      discount: discount,

      totalCost: totalCost,

      totalSelling: totalSelling

    };

    // Add row to table
    this.purchaseBillItems.push(
      purchaseBillItem
    );

    // Clear form after adding
    this.purchaseBillForm.reset({

      item: '',

      batch: '',

      standardCost: null,

      standardPrice: null,

      quantity: null,

      discount: 0

    });

    this.filteredItems = this.items;

  }

  // ------------------------------------------------
  // Summary
  // ------------------------------------------------

  get totalItems(): number {

    return this.purchaseBillItems.length;

  }

  get totalQuantity(): number {

    return this.purchaseBillItems.reduce(
      (total, item) =>
        total + item.quantity,
      0
    );

  }

  get totalCost(): number {

    return this.purchaseBillItems.reduce(
      (total, item) =>
        total + item.totalCost,
      0
    );

  }

  get totalSelling(): number {

    return this.purchaseBillItems.reduce(
      (total, item) =>
        total + item.totalSelling,
      0
    );

  }

}