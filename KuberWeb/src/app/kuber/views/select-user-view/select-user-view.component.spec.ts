import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectUserViewComponent } from './select-user-view.component';

describe('SelectUserViewComponent', () => {
  let component: SelectUserViewComponent;
  let fixture: ComponentFixture<SelectUserViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelectUserViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectUserViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
