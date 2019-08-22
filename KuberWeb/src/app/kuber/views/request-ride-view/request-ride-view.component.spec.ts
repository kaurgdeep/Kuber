import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RequestRideViewComponent } from './request-ride-view.component';

describe('RequestRideViewComponent', () => {
  let component: RequestRideViewComponent;
  let fixture: ComponentFixture<RequestRideViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RequestRideViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RequestRideViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
