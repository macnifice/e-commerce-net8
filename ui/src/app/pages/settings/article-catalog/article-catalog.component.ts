import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormDialogService } from '../../../services/dialog/form-dialog.service';
import { ArticleService } from '../../../services/article/article.service';
import { Article } from '../../../interfaces/article/article.interface';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormArticleCatalogComponent } from './form-article-catalog/form-article-catalog.component';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../../../components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-article-catalog',
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
  templateUrl: './article-catalog.component.html',
  styleUrls: ['./article-catalog.component.css']
})
export class ArticleCatalogComponent implements OnInit {
  displayedColumns: string[] = ['id', 'name', 'code', 'description', 'actions'];
  dataSource = new MatTableDataSource<Article>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private modalService: FormDialogService,
    private articleService: ArticleService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    this.loadArticles();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  loadArticles() {
    this.articleService.getArticles().subscribe({
      next: (articles: Article[]) => {
        this.dataSource.data = articles;
      },
      error: (error) => {
        console.error('Error al cargar artículos:', error);
      }
    });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  openNewArticleDialog() {
    const dialogRef = this.modalService.openModal<FormArticleCatalogComponent, any>(FormArticleCatalogComponent);
    dialogRef.afterClosed().subscribe({
      next: (article: Article) => {
        if (article) {
          this.loadArticles();
        }
      }
    });
  }

  editArticle(article: Article) {
    const dialogRef = this.modalService.openModal<FormArticleCatalogComponent, Article>(
      FormArticleCatalogComponent,
      article,
      true
    );
    dialogRef.afterClosed().subscribe({
      next: (updatedArticle: Article) => {
        if (updatedArticle) {
          this.loadArticles();
        }
      }
    });
  }

  deleteArticle(article: Article) {
    if (article.id) {
      const dialogRef = this.dialog.open(ConfirmDialogComponent, {
        width: '400px',
        data: {
          title: 'Confirmar eliminación',
          message: `¿Está seguro que desea eliminar el artículo "${article.name}", este proceso eliminará el artículo de todos las tiendas?`,
          confirmText: 'Eliminar',
          cancelText: 'Cancelar'
        }
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.articleService.deleteArticle(article.id!).subscribe({
            next: () => {
              this.loadArticles();
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
}
