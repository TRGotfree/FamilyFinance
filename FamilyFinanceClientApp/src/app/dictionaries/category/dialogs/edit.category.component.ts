import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Category } from '../category.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CategoryService } from '../category.service';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';

@Component({
    templateUrl: './edit.category.component.html',
    styleUrls: ['./edit.category.component.css']
})

export class EditCategoryComponent implements OnInit {

    caption: string;
    isPanelWithNewCategoryOpened = false;
    categoryFormGroup: FormGroup;
    categoryControl: FormControl;
    categoryNameControl: FormControl;
    subCategoryNameControl: FormControl;

    categories: string[] = [];
    filteredCategories: Observable<string[]>;

    constructor(public dialogRef: MatDialogRef<EditCategoryComponent>,
        @Inject(MAT_DIALOG_DATA) public category: Category,
        private snackBar: MatSnackBar,
        private categoryService: CategoryService) {

        this.caption = 'Новая категория расходов';
        if (category && category.id) {
            this.caption = 'Редактирование категории';
        }

        this.categoryControl = new FormControl(category?.categoryName, [Validators.required, Validators.minLength(3), Validators.maxLength(500)]);
        this.categoryNameControl = new FormControl('', [Validators.maxLength(500), Validators.minLength(3)]);
        this.subCategoryNameControl = new FormControl(category?.subCategoryName, [Validators.required, Validators.maxLength(500), Validators.minLength(3)]);

        this.categoryFormGroup = new FormGroup({
            category: this.categoryControl,
            categoryName: this.categoryNameControl,
            subCategoryName: this.subCategoryNameControl
        });
    }

    ngOnInit() {
        this.categoryService.getCategories().subscribe(categoriesData => {
            if (!categoriesData) {
                this.snackBar.open('Не удалось загрузить категории расходов!', 'OK', { duration: 3000 });
                return;
            }

            this.categories = Array.from(new Set(categoriesData.map(v => v.categoryName)));

            this.filteredCategories = this.categoryControl.valueChanges.pipe(startWith(''), map(searchedStoreName => {
                const categoryName = searchedStoreName.toLowerCase();
                return this.categories.filter(s => s.toLowerCase().includes(categoryName));
            }));
        });
    }

    openNewCategoryPanel() {
        if (this.isPanelWithNewCategoryOpened) {
            this.dialogRef.updateSize('520px', '300px');
        }else{
            this.dialogRef.updateSize('520px', '400px');
        }
        this.isPanelWithNewCategoryOpened = !this.isPanelWithNewCategoryOpened;
    }

    addNewCategory() {
        if (!this.categoryNameControl.value) {
            return;
        }

        this.category.id = 0;
        this.category.categoryName = this.categoryNameControl.value;
        this.categoryControl.setValue(this.categoryNameControl.value);
    }

    save() {
        if (this.categoryFormGroup.invalid) {
            return;
        }

        if (this.category && this.category.id) {
            this.category.categoryName = this.categoryControl.value;
            this.category.subCategoryName = this.subCategoryNameControl.value;

            this.categoryService.editCategory(this.category).subscribe(editedCategory => {
                if (!editedCategory) {
                    this.snackBar.open('Не удалось обновить категорию расходов!', 'OK', { duration: 3000 });
                    return;
                }

                this.dialogRef.close(editedCategory);

            }, error => {
                this.snackBar.open('Произошла ошибка! Не удалось обновить категорию расходов!', 'OK', { duration: 3000 });
            });
        } else {
            const category = new Category();
            category.categoryName = this.categoryControl.value;
            category.subCategoryName = this.subCategoryNameControl.value;

            this.categoryService.addCategory(category).subscribe(addedCategory => {

                if (!addedCategory) {
                    this.snackBar.open('Произошла ошибка! Не удалось сохранить категорию расходов!', 'OK', { duration: 3000 });
                    return;
                }

                this.dialogRef.close(addedCategory);

            }, error => {
                this.snackBar.open('Произошла ошибка! Не удалось сохранить категорию расходов!', 'OK', { duration: 3000 });
            });
        }
    }

    cancel() {
        this.dialogRef.close();
    }
}