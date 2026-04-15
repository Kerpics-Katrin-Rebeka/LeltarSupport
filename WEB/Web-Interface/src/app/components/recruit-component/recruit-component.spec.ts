import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecruitComponent } from './recruit-component';

describe('RecruitComponent', () => {
  let component: RecruitComponent;
  let fixture: ComponentFixture<RecruitComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecruitComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(RecruitComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
