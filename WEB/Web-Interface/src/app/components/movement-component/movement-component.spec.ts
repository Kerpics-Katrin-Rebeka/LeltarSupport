import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MovementComponent } from './movement-component';

describe('MovementComponent', () => {
  let component: MovementComponent;
  let fixture: ComponentFixture<MovementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MovementComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(MovementComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
