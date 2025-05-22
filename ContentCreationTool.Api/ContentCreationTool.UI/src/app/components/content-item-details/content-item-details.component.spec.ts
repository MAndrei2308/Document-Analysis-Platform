import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContentItemDetailsComponent } from './content-item-details.component';

describe('ContentItemDetailsComponent', () => {
  let component: ContentItemDetailsComponent;
  let fixture: ComponentFixture<ContentItemDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ContentItemDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ContentItemDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
