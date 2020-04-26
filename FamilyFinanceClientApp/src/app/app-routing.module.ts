import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CostsComponent } from './costs/costs.component';
import { from } from 'rxjs';

const routes: Routes = [
  {
    path: '', component: CostsComponent,
  },
  {
    path: '/costs', component: CostsComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
