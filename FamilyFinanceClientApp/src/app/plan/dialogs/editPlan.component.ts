import { PlanService } from './../plan.service';
// tslint:disable:align
import { Observable } from 'rxjs';
import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CustomLogger } from './../../shared/services/logger.service';
import { startWith, map } from 'rxjs/operators';
import { Plan } from '../plan.model';

@Component({
  templateUrl: './editPlan.component.html',
  styleUrls: ['./editPlan.component.css']
})
export class EditPlanComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<EditPlanComponent>,
    @Inject(MAT_DIALOG_DATA) public plan: Plan,
    private logger: CustomLogger,
    private snackBar: MatSnackBar,
    private planService: PlanService) { }

  planFormGroup: FormGroup;
  caption = (this.plan && this.plan.subCategoryName) ? this.plan.subCategoryName : 'Новое плановое значение';
  amountControl = new FormControl(this.plan.amount, [Validators.required, Validators.min(50)]);

  ngOnInit(): void {
    this.planFormGroup = new FormGroup({
      amount: this.amountControl
    });
  }

  cancel() {
    this.dialogRef.close();
  }

  save() {
    if (this.planFormGroup.invalid) {
      return;
    }

    this.plan.amount = this.amountControl.value;

    this.dialogRef.close(this.plan);
  }

}
