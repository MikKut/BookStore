import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PublicationChartComponent } from './publication-chart.component';

describe('PublicationChartComponent', () => {
  let component: PublicationChartComponent;
  let fixture: ComponentFixture<PublicationChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PublicationChartComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PublicationChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
