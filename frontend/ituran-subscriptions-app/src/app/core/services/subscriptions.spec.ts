import { TestBed } from '@angular/core/testing';

import { Subscriptions } from './subscriptions';

describe('Subscriptions', () => {
  let service: Subscriptions;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Subscriptions);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
