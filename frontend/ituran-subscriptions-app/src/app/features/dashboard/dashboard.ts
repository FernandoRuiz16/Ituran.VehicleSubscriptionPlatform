import { Component, ChangeDetectorRef, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardService } from '../../core/services/dashboard';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.scss']
})
export class Dashboard implements OnInit {
  private dashboardService = inject(DashboardService);
  private cdr = inject(ChangeDetectorRef);

  summary: any = {
    totalItems: 0,
    completed: 0,
    pending: 0,
    processing: 0,
    failed: 0,
    totalBatches: 0
  };

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.dashboardService.getSummary().subscribe({
      next: response => {
        this.summary = response;
        this.cdr.detectChanges();
      },
      error: error => {
        console.error('DASHBOARD ERROR:', error);
      }
    });
  }
}