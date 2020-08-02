import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { CustomLogger } from 'src/app/shared/services/logger.service';
import { Category } from '../category.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CategoryService } from '../category.service';

@Component({
    templateUrl: './edit.category.component.html',
    styleUrls: ['./edit.category.component.css']
})

export class EditCategoryComponent implements OnInit {
    
    caption: string;
    categoryFormGroup: FormGroup;
    categoryControl: FormControl;
    categoryNameControl: FormControl;
    subCategoryNameControl: FormControl;

    constructor(public dialogRef: MatDialogRef<EditCategoryComponent>,
        @Inject(MAT_DIALOG_DATA) public category: Category,
        private logger: CustomLogger,
        private snackBar: MatSnackBar,
        private categoryService: CategoryService) {

        this.caption = 'Новая категория расходов';
        if (category && category.id) {
            this.caption = 'Редактирование категории';
        }

        this.categoryControl = new FormControl('', [ Validators.required, Validators.maxLength(500) ]);
        this.categoryNameControl = new FormControl('', [ Validators.maxLength(500) ]);
        this.subCategoryNameControl = new FormControl('', [ Validators.required, Validators.maxLength(500) ]);

        this.categoryFormGroup = new FormGroup({
            category: this.categoryControl,
            categoryName: this.categoryNameControl,
            subCategoryName: this.subCategoryNameControl
        });

    }

    ngOnInit() { }

    save() {

    }

    cancel() {

    }
}