import { Component, ChangeDetectorRef, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AnalyticsService } from '../../core/services/analytics';

@Component({
  selector: 'app-analytics',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './analytics.html',
  styleUrls: ['./analytics.scss']
})
export class Analytics implements OnInit {
  private analyticsService = inject(AnalyticsService);
  private cdr = inject(ChangeDetectorRef);

  summary: any = {
    totalBatches: 0,
    totalItems: 0,
    completedItems: 0,
    failedItems: 0,
    logsCount: 0,
    erpSteps: 0,
    crmSteps: 0
  };

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.analyticsService.getSummary().subscribe({
      next: response => {
        this.summary = response;
        this.cdr.detectChanges();
      },
      error: error => {
        console.error('ANALYTICS ERROR:', error);
      }
    });
  }
}