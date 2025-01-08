import { TestBed } from '@angular/core/testing';

import { PartService } from './navbar-search.service';
import {PartsService} from './parts.service';

describe('NavbarSearchService', () => {
  let service: PartsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PartsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
