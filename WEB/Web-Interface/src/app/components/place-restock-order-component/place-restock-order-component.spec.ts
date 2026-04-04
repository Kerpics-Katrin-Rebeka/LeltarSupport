import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaceRestockOrderComponent } from './place-restock-order-component';

describe('PlaceRestockOrderComponent', () => {
  let component: PlaceRestockOrderComponent;
  let fixture: ComponentFixture<PlaceRestockOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PlaceRestockOrderComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(PlaceRestockOrderComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
