import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Article } from '../../../../interfaces/article/article.interface';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ArticleService } from '../../../../services/article/article.service';

@Component({
  selector: 'app-form-article-catalog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatDividerModule
  ],
  templateUrl: './form-article-catalog.component.html',
  styleUrl: './form-article-catalog.component.css'
})
export class FormArticleCatalogComponent implements OnInit {

  articleCatalogForm!: FormGroup;
  isEditing = false;

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<FormArticleCatalogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Article,
    private articleService: ArticleService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.isEditing = this.data.isEditing || false;
    this.createForm();
  }

  createForm(): void {
    this.articleCatalogForm = this.fb.group({
      name: [this.data?.name || '', Validators.required],
      code: [this.data?.code || '', Validators.required],
      description: [this.data?.description || ''],
      imagePath: [this.data?.imagePath || '', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.articleCatalogForm.valid) {
      const articleCatalogData: Article = {
        name: this.articleCatalogForm.value.name,
        code: this.articleCatalogForm.value.code,
        description: this.articleCatalogForm.value.description,
        imagePath: this.articleCatalogForm.value.imagePath
      };

      if (this.isEditing) {
        this.articleService.updateArticle(this.data.id!, articleCatalogData).subscribe({
          next: () => {
            this.snackBar.open('Artículo actualizado correctamente', 'Cerrar', { duration: 3000 });
            this.dialogRef.close(articleCatalogData);
          },
          error: (error) => {
            this.snackBar.open('Error al actualizar el artículo', 'Cerrar', { duration: 3000 });
            console.error('Error al actualizar el artículo:', error);
          }
        });
      } else {
        this.articleService.createArticle(articleCatalogData).subscribe({
          next: () => {
            this.snackBar.open('Artículo creado correctamente', 'Cerrar', { duration: 3000 });
            this.dialogRef.close(articleCatalogData);
          },
          error: (error) => {
            this.snackBar.open('Error al crear el artículo', 'Cerrar', { duration: 3000 });
            console.error('Error al crear el artículo:', error);
          }
        });
      }
    }
  }
}
