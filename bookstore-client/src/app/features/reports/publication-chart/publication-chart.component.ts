import { Component, OnInit } from '@angular/core';
import { ReportService } from '../../../core/services/report.service';
import { PublicationData } from '../../../core/models/publication-data.model';
import { Chart, registerables } from 'chart.js';

@Component({
  selector: 'app-publication-chart',
  templateUrl: './publication-chart.component.html',
  styleUrls: ['./publication-chart.component.css']
})
export class PublicationChartComponent implements OnInit {
  publicationData: PublicationData[] = [];

  constructor(private reportService: ReportService) {
    Chart.register(...registerables);
  }

  ngOnInit(): void {
    this.loadPublicationData();
  }

  loadPublicationData(): void {
    this.reportService.getBooksPublicationData().subscribe(data => {
      console.log('Data fetched from service:', data);
      this.publicationData = Object.entries(data).map(([year, count]) => ({
        year: parseInt(year, 10),
        count
      }));
      this.createChart();
    }, error => {
      console.error('Error fetching publication data:', error);
    });
  }

  createChart(): void {
    const ctx = document.getElementById('publicationChart') as HTMLCanvasElement;
    if (ctx) {
      const data = {
        labels: this.publicationData.map(pd => pd.year.toString()),
        datasets: [{
          label: 'Number of Books',
          data: this.publicationData.map(pd => pd.count),
          backgroundColor: 'rgba(75, 192, 192, 0.2)',
          borderColor: 'rgba(75, 192, 192, 1)',
          borderWidth: 1
        }]
      };

      new Chart(ctx, {
        type: 'bar',
        data: data,
        options: {
          scales: {
            y: {
              beginAtZero: true
            }
          }
        }
      });
    }
  }
}
