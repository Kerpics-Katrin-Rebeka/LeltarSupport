import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MovementLogComponent } from './movement-log-component';

describe('MovementLogComponent', () => {
  let component: MovementLogComponent;
  let fixture: ComponentFixture<MovementLogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MovementLogComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(MovementLogComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
