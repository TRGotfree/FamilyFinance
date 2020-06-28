import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CostsComponent } from './costs/costs.component';
import { PlanComponent } from './plan/plan.component';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  {
    path: '', component: HomeComponent
  },
  {
    path: 'home', component: HomeComponent
  },
  {
    path: 'costs', component: CostsComponent
  },
  {
    path: 'plans', component: PlanComponent
  },
  {
    path: '**', component: CostsComponent,
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
