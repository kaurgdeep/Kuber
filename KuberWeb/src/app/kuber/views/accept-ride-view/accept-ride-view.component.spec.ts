import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AcceptRideViewComponent } from './accept-ride-view.component';

describe('AcceptRideViewComponent', () => {
  let component: AcceptRideViewComponent;
  let fixture: ComponentFixture<AcceptRideViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AcceptRideViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AcceptRideViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
