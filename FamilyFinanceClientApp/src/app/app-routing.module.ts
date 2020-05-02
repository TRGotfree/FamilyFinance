import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CostsComponent } from './costs/costs.component';
import { PlanComponent } from './plan/plan.component';

const routes: Routes = [
  {
    path: '**', component: CostsComponent,
  },
  {
    path: 'costs', component: CostsComponent
  },
  {
    path: 'plan', component: PlanComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
