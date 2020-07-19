import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CostsComponent } from './costs/costs.component';
import { PlanComponent } from './plan/plan.component';
import { StatisticsComponent } from './statistics/statistics.component';
import { PageNotFoundComponent } from './page_not_found/page_not_found.component';
import { IncomesComponent } from './incomes/incomes.component';

const routes: Routes = [
  {
    path: '', component: StatisticsComponent
  },
  {
    path: 'statistics', component: StatisticsComponent
  },
  {
    path: 'costs', component: CostsComponent
  },
  {
    path: 'plans', component: PlanComponent
  },
  {
    path: 'incomes', component: IncomesComponent
  },
  {
    path: '**', component: PageNotFoundComponent,
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
