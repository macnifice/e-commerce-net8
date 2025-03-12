import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDividerModule } from '@angular/material/divider';
import { ArticleService } from '../../../../services/article/article.service';
import { ArticleStore } from '../../../../interfaces/article/article-store.interface';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { Article } from '../../../../interfaces/article/article.interface';
import { map, Observable, startWith } from 'rxjs';

@Component({
  selector: 'app-form-article',
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
    MatDividerModule,
    MatAutocompleteModule
  ],
  templateUrl: './form-article.component.html',
  styleUrl: './form-article.component.css'
})
export class FormArticleComponent implements OnInit {
  articleForm!: FormGroup;
  isEditing = false;
  allProducts: Article[] = [];
  filteredProducts: Observable<Article[]> = new Observable<Article[]>();
  productCtrl = new FormControl<Article | string>('');

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<FormArticleComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ArticleStore,
    private articleService: ArticleService,
    private snackBar: MatSnackBar
  ) {

  }

  ngOnInit(): void {
    this.isEditing = this.data.isEditing || false;
    this.loadProducts();
    this.createForm();
    this.filteredProducts = this.productCtrl.valueChanges.pipe(
      startWith(''),
      map(value => {
        const name = typeof value === 'string' ? value : value?.name;
        return name ? this._filter(name as string) : this.allProducts.slice();
      })
    );
  }

  createForm(): void {
    this.articleForm = this.fb.group({
      articleId: [this.data.articleId, Validators.required],
      price: [this.data.price, [Validators.required, Validators.min(0)]],
      stock: [this.data.stock, [Validators.required, Validators.min(0)]],
      date: [this.data.date ? new Date(this.data.date) : new Date(), Validators.required]
    });
  }

  loadProducts() {
    this.articleService.getArticles().subscribe({
      next: (products) => {
        this.allProducts = products;

        // Si estamos en modo edición, buscar y seleccionar el artículo
        if (this.isEditing && this.data.articleId) {
          const selectedArticle = this.allProducts.find(article => article.id === this.data.articleId);
          if (selectedArticle) {
            this.productCtrl.setValue(selectedArticle);
          }
        }
      },
      error: (error) => console.error('Error cargando productos:', error)
    });
  }

  private _filter(name: string): Article[] {
    const filterValue = name.toLowerCase();
    return this.allProducts.filter(option => option.name.toLowerCase().includes(filterValue));
  }

  displayFn(article: Article): string {
    return article && article.name ? article.name : '';
  }

  onArticleSelected(event: any): void {
    const article = event.option.value as Article;
    if (article && article.id) {
      this.articleForm.patchValue({
        articleId: article.id
      });
    }
  }

  onSubmit(): void {
    if (this.articleForm.valid) {
      const articleData: ArticleStore = {
        price: this.articleForm.value.price,
        stock: this.articleForm.value.stock,
        date: this.articleForm.value.date,
        articleId: this.articleForm.value.articleId,
        storeId: this.data.storeId
      };

      if (this.isEditing) {
        this.articleService.updateArticleStore(this.data.id!, articleData).subscribe({
          next: () => {
            this.snackBar.open('Artículo actualizado correctamente', 'Cerrar', { duration: 3000 });
            this.dialogRef.close(articleData);
          },
          error: (error) => {
            console.error('Error al actualizar el artículo:', error);
            this.snackBar.open('Error al actualizar el artículo', 'Cerrar', { duration: 3000 });
          }
        });
      } else {
        this.articleService.createArticleStore(articleData).subscribe({
          next: () => {
            this.snackBar.open('Artículo creado correctamente', 'Cerrar', { duration: 3000 });
            this.dialogRef.close(articleData);
          },
          error: (error) => {
            console.error('Error al crear el artículo:', error);
            this.snackBar.open('Error al crear el artículo', 'Cerrar', { duration: 3000 });
          }
        });
      }
    }
  }
}
