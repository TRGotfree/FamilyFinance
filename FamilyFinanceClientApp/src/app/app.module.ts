import { EditPlanComponent } from './plan/dialogs/editPlan.component';
import { PlanService } from './plan/plan.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule, DEFAULT_CURRENCY_CODE } from '@angular/core';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSortModule } from '@angular/material/sort';
import { MatSelectModule } from '@angular/material/select';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCardModule } from '@angular/material/card';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import {
  MomentDateAdapter,
  MAT_MOMENT_DATE_ADAPTER_OPTIONS,
  MatMomentDateModule
} from '@angular/material-moment-adapter';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ToolbarComponent } from './shared/toolbar/toolbar.component';
import { CostsComponent } from './costs/costs.component';
import { CostsService } from './costs/costs.service';
import { CustomLogger } from './shared/services/logger.service';
import { NewCostComponent } from './costs/dialogs/newCost.component';
import { PayTypeService } from './dictionaries/paytype/paytType.service';
import { StoreService } from './dictionaries/store/store.service';
import { DateTimeBuilder } from './shared/services/dateTimeBuilder.service';
import { ConfirmationComponent } from './shared/dialogs/confirmation.component';
import { PlanComponent } from './plan/plan.component';
import { StatisticsComponent } from './statistics/statistics.component';
import { IncomesComponent } from './incomes/incomes.component';

@NgModule({
  declarations: [
    AppComponent,
    ToolbarComponent,
    CostsComponent,
    NewCostComponent,
    ConfirmationComponent,
    PlanComponent,
    EditPlanComponent,
    StatisticsComponent,
    IncomesComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    ReactiveFormsModule,
    MatToolbarModule,
    MatIconModule,
    MatDialogModule,
    MatSnackBarModule,
    MatTableModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatMomentDateModule,
    MatInputModule,
    MatButtonModule,
    MatSortModule,
    MatSelectModule,
    MatExpansionModule,
    MatAutocompleteModule,
    MatDividerModule,
    MatProgressSpinnerModule,
    MatCardModule
  ],
  providers: [
    CostsService,
    CustomLogger,
    PayTypeService,
    StoreService,
    PlanService,
    DateTimeBuilder,
    { provide: MAT_DATE_LOCALE, useValue: 'ru-RU' },
    { provide: DEFAULT_CURRENCY_CODE, useValue: '' },
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS]
    },
    {
      provide: MAT_DATE_FORMATS, useValue: {
        parse: {
          dateInput: 'LL',
        },
        display: {
          dateInput: 'LL',
          monthYearLabel: 'MMM YYYY',
          dateA11yLabel: 'LL',
          monthYearA11yLabel: 'MMMM YYYY',
        },
      }
    }
  ],
  bootstrap: [AppComponent],
  entryComponents: [NewCostComponent, ConfirmationComponent, EditPlanComponent]
})
export class AppModule { }
