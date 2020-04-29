// tslint:disable:align
import { Store } from './../../dictionaries/store/store.model';
import { PayType } from './../../dictionaries/paytype/paytype.model';
import { Cost } from './../cost.model';
import { Component, OnInit, Inject } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { PayTypeService } from '../../dictionaries/paytype/paytType.service';
import { StoreService } from '../../dictionaries/store/store.service';
import { CustomLogger } from './../../common/logger.service';

@Component({
  templateUrl: './newcost.component.html',
  styleUrls: ['./newcost.component.css']
})
export class NewCostComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<NewCostComponent>,
    @Inject(MAT_DIALOG_DATA) public cost: Cost,
    public userTaskTypeDialog: MatDialog,
    private logger: CustomLogger,
    private snackBar: MatSnackBar,
    private payTypeService: PayTypeService,
    private storeService: StoreService) { }

  costsFormGroup: FormGroup;
  payTypes: PayType[] = [];
  stores: Store[] = [];
  caption = (this.cost && this.cost.costSubCategory) ? this.cost.costSubCategory : 'Новая расход';

  amountControl = new FormControl(this.cost.amount, [Validators.required]);
  payTypeControl = new FormControl(this.cost.payType, [Validators.required]);
  storeControl = new FormControl(this.cost.store, [Validators.required]);
  countControl = new FormControl(this.cost.count);
  commentControl = new FormControl(this.cost.comment);

  ngOnInit(): void { }

  cancel() {
    this.dialogRef.close();
  }

  save() {

  }

  addNewStore() {

  }

  loadLists() {
    this.payTypeService.getPayTypes().subscribe(data => {}, error => {});
    this.storeService.getStores().subscribe(data => {}, error => {});
  }
}