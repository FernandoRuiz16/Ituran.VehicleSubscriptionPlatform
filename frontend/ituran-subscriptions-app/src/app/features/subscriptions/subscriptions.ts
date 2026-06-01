import { Component, ChangeDetectorRef, NgZone, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SubscriptionsService } from '../../core/services/subscriptions';

@Component({
  selector: 'app-subscriptions',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './subscriptions.html',
  styleUrls: ['./subscriptions.scss']
})
export class Subscriptions implements OnInit {
  private subscriptionsService = inject(SubscriptionsService);
  private cdr = inject(ChangeDetectorRef);
  private zone = inject(NgZone);

  subscriptions: any[] = [];

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.subscriptionsService.getSubscriptions().subscribe({
      next: response => {
        this.zone.run(() => {
          this.subscriptions = response || [];
          this.cdr.detectChanges();
        });
      },
      error: error => {
        console.error('SUBSCRIPTIONS ERROR:', error);
      }
    });
  }
}