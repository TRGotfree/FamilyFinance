import { Component, OnInit } from "@angular/core";
import { StatisticsService } from './statistics.service';
import { MatSnackBar } from '@angular/material/snack-bar';

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
        this.statisticsService.getStatistics(currentDate.getMonth(), this.year).subscribe(stat => {
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

}
