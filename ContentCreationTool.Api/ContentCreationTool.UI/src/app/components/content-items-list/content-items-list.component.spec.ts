import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContentItemsListComponent } from './content-items-list.component';

describe('ContentItemsListComponent', () => {
  let component: ContentItemsListComponent;
  let fixture: ComponentFixture<ContentItemsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ContentItemsListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ContentItemsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
