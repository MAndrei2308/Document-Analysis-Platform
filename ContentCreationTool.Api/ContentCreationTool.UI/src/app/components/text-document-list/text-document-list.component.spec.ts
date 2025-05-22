import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TextDocumentListComponent } from './text-document-list.component';

describe('TextDocumentListComponent', () => {
  let component: TextDocumentListComponent;
  let fixture: ComponentFixture<TextDocumentListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TextDocumentListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TextDocumentListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
