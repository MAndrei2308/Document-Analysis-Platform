import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TextDocumentDetailsComponent } from './text-document-details.component';

describe('TextDocumentDetailsComponent', () => {
  let component: TextDocumentDetailsComponent;
  let fixture: ComponentFixture<TextDocumentDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TextDocumentDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TextDocumentDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
