import { Component, Input, OnChanges, SimpleChanges, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDialog } from '@angular/material/dialog';
import { FormArticleComponent } from './form-article/form-article.component';
import { FormDialogService } from '../../../services/dialog/form-dialog.service';
import { ArticleService } from '../../../services/article/article.service';
import { ArticleStore } from '../../../interfaces/article/article-store.interface';
import { ConfirmDialogComponent } from '../../../components/confirm-dialog/confirm-dialog.component';
import { MatSnackBar } from '@angular/material/snack-bar';


@Component({
  selector: 'app-article',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule
  ],
  templateUrl: './article.component.html',
  styleUrl: './article.component.css'
})
export class ArticleComponent implements OnChanges {
  @Input() selectedStore: any;

  displayedColumns: string[] = ['id', 'name', 'description', 'price', 'stock', 'actions'];
  dataSource = new MatTableDataSource<ArticleStore>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private dialog: MatDialog,
    private modalService: FormDialogService,
    private articleService: ArticleService,
    private snackBar: MatSnackBar
  ) { }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['selectedStore'] && this.selectedStore) {
      this.loadArticles();
    }
  }

  loadArticles() {
    this.articleService.getArticlesStore(this.selectedStore.id).subscribe((articles) => {
      this.dataSource.data = articles;
    });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  openAssignArticleDialog(): void {
    const dialogRef = this.modalService.openModal<FormArticleComponent, any>(FormArticleComponent, { storeId: this.selectedStore.id });
    dialogRef.afterClosed().subscribe({
      next: (article: ArticleStore) => {
        if (article) {
          this.refreshArticles();
        }
      }
    });
  }

  editArticle(article: ArticleStore) {
    const dialogRef = this.modalService.openModal<FormArticleComponent, ArticleStore>(FormArticleComponent, article, true);
    dialogRef.afterClosed().subscribe({
      next: (article: ArticleStore) => {
        if (article) {
          this.refreshArticles();
        }
      }
    });
  }

  deleteArticle(article: any) {
    if (article.id) {
      const dialogRef = this.dialog.open(ConfirmDialogComponent, {
        width: '400px',
        data: {
          title: 'Confirmar eliminación',
          message: `¿Está seguro que desea eliminar el artículo "${article.name}" de esta tienda?`,
          confirmText: 'Eliminar',
          cancelText: 'Cancelar'
        }
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.articleService.deleteArticleStore(article.id).subscribe({
            next: () => {
              this.refreshArticles();
              this.snackBar.open('Artículo eliminado correctamente', 'Cerrar', {
                duration: 3000
              });
            },
            error: (error) => {
              console.error('Error al eliminar artículo:', error);
              this.snackBar.open('Error al eliminar artículo', 'Cerrar', {
                duration: 3000
              });
            }
          });
        }
      });
    }
  }

  refreshArticles() {
    this.loadArticles();
  }
}
