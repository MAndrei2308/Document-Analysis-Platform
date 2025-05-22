import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TextUploadComponent } from './text-upload.component';

describe('TextUploadComponent', () => {
  let component: TextUploadComponent;
  let fixture: ComponentFixture<TextUploadComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TextUploadComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TextUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
