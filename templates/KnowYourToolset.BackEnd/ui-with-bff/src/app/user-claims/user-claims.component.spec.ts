import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserClaimsComponent } from './user-claims.component';

describe('UserClaimsComponent', () => {
  let component: UserClaimsComponent;
  let fixture: ComponentFixture<UserClaimsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserClaimsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserClaimsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
