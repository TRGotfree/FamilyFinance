import { Component, OnInit } from "@angular/core";

@Component({
    templateUrl: './home.component.html', 
    styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

    month: string;
    year: number;
    plan_amount: number;

    private months: Set<{}>;

    constructor(){} 
    ngOnInit(): void {
        this.months
    }

}
