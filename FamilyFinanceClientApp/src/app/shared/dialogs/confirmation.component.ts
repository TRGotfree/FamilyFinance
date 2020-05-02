import { Component, ViewChild, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  templateUrl: './confirmation.component.html',
  styleUrls: ['./confirmation.component.css']
})
export class ConfirmationComponent {
  caption = 'Внимание';
  message = 'Подтвердите действие!';

  constructor(public dialogRef: MatDialogRef<ConfirmationComponent>, @Inject(MAT_DIALOG_DATA) data: any) {
    this.caption = data.caption ? data.caption : this.caption;
    this.message = data.message ? data.message : this.message;
  }

  cancel() {
    this.dialogRef.close(false);
  }

  confirm() {
    this.dialogRef.close(true);
  }
}
