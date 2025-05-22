import { TestBed } from '@angular/core/testing';

import { ContentItemService } from './content-item.service';

describe('ContentItemService', () => {
  let service: ContentItemService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ContentItemService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
