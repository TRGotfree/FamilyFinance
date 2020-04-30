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
import { CostsService } from '../costs.service';

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
    private costsService: CostsService,
    private payTypeService: PayTypeService,
    private storeService: StoreService) { }

  costsFormGroup: FormGroup;
  payTypes: PayType[] = [];
  stores: Store[] = [];
  caption = (this.cost && this.cost.costSubCategory) ? this.cost.costSubCategory : 'Новый расход';

  amountControl = new FormControl(this.cost.amount, [Validators.required, Validators.min(50)]);
  payTypeControl = new FormControl(this.cost.payType, [Validators.required]);
  storeControl = new FormControl(this.cost.store, [Validators.required]);
  countControl = new FormControl(this.cost.count);
  commentControl = new FormControl(this.cost.comment);
  storeNameControl = new FormControl('', [Validators.required]);

  isPanelWithNewStoreOpened = false;

  ngOnInit(): void {

    this.costsFormGroup = new FormGroup({
      amount: this.amountControl,
      payType: this.payTypeControl,
      store: this.storeControl,
      count: this.countControl,
      comment: this.commentControl
    });

    this.loadPayTypes();
    this.loadStores();
  }

  cancel() {
    this.dialogRef.close();
  }

  save() {
    if (this.costsFormGroup.invalid) {
      return;
    }

    this.cost.amount = this.amountControl.value;
    this.cost.count = this.countControl.value;
    this.cost.payTypeId = this.payTypeControl.value;
    this.cost.storeId = this.storeControl.value;
    this.cost.comment = this.commentControl.value;

    if (this.cost && this.cost.id > 0) {
      this.costsService.updateCost(this.cost).subscribe(updatedCost => {
        if (!updatedCost) {
          this.snackBar.open('Не удалось сохранить данные по расходу!', 'OK', { duration: 3000, verticalPosition: 'top' });
          return;
        }

        this.dialogRef.close(updatedCost);

      }, error => {
        this.logger.logError(error);
        this.snackBar.open('Не удалось сохранить данные по расходу!', 'OK', { duration: 3000, verticalPosition: 'top' });
      });
    } else {
      this.costsService.addCost(this.cost).subscribe(newCost => {
        if (!newCost) {
          this.snackBar.open('Не удалось сохранить данные по расходу!', 'OK', { duration: 3000, verticalPosition: 'top' });
          return;
        }

        this.dialogRef.close(newCost);

      }, error => {
        this.logger.logError(error);
        this.snackBar.open('Не удалось сохранить данные по расходу!', 'OK', { duration: 3000, verticalPosition: 'top' });
      });
    }
  }

  addNewStore() {
    const store = new Store();
    store.isremoved = false;
    store.name = this.storeNameControl.value;
    this.storeService.addStore(store).subscribe(storeData => {
      if (!storeData) {
        this.snackBar.open('Не удалось сохранить новый магазин!', 'OK', { duration: 3000, verticalPosition: 'top' });
        return;
      }
      this.stores.push(storeData);
      this.isPanelWithNewStoreOpened = false;
      this.storeControl.setValue(storeData.id);
      this.storeNameControl.setValue('');
    }, error => {
      this.logger.logError(error);
      this.snackBar.open('Не удалось сохранить новый магазин!', 'OK', { duration: 3000, verticalPosition: 'top' });
    });
  }

  loadPayTypes() {
    this.payTypeService.getPayTypes().subscribe(data => {
      this.payTypes = data;
    }, error => {
      this.logger.logError(error);
    });
  }

  loadStores() {
    this.storeService.getStores().subscribe(data => {
      this.stores = data;
    }, error => {
      this.logger.logError(error);
    });
  }

}
