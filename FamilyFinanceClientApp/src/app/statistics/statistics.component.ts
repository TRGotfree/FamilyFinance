import { Component, OnInit } from "@angular/core";
import { StatisticsService } from './statistics.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ChartType, ChartOptions } from 'chart.js';
import { Label } from 'ng2-charts';
// import * as pluginDataLables from 'chartjs-plugin-datalabels'

@Component({
    templateUrl: './statistics.component.html',
    styleUrls: ['./statistics.component.css']
})
export class StatisticsComponent implements OnInit {

    month: string;
    year: number;
    planAmount: number;
    costsAmount: number;
    different: number;
    profitAmount: number;
    
    payTypeChartLabels: Label[] = ['Наличность', 'Карты', 'Прочее'];
    payTypeChartData: number[] = [];
    payTypeChartType: ChartType = 'pie';
    payTypeChartLegend = true;
    payTypeChartOptions: ChartOptions = {
        responsive: true,
        legend: {
            position: 'top'
        },
        plugins: {
            datalabels: {
                formatter: (value, ctx) => {
                    const label = ctx.chart.data.labels[ctx.dataindex];
                    return label;
                }
            }
        }
    };

    payTypeChartColors: string[] = ['#81C784', '#673AB7', '#FF8A65'];

    private months = [
        'Январь',
        'Февраль',
        'Март',
        'Апрель',
        'Май',
        'Июнь',
        'Июль',
        'Август',
        'Сентябрь',
        'Октябрь',
        'Ноябрь',
        'Деккабрь'
    ];;

    constructor(private statisticsService: StatisticsService, private snackBar: MatSnackBar) { 

    }

    ngOnInit(): void {
        const currentDate = new Date();
        this.month = this.months[currentDate.getMonth()];
        this.year = currentDate.getFullYear();
        this.statisticsService.getStatistics(currentDate.getMonth() + 1, this.year).subscribe(stat => {
            if (!stat) {
                this.snackBar.open('Не удалось загрузить статистические данные!', 'OK', { duration: 3000 });
                return;
            }

            this.costsAmount = stat.costTotal;
            this.profitAmount = stat.incomeTotal;
            this.planAmount = stat.planTotal;
            this.different = this.planAmount - this.costsAmount;
        });
    }

    private loadCostsByPayTypes(month: number, year: number): void {

    }
}
