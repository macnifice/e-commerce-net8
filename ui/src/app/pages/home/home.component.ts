import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatPaginatorModule } from '@angular/material/paginator';
import { RouterModule } from '@angular/router';
import { NavComponent } from '../../components/nav/nav.component';
import { ArticleService } from '../../services/article/article.service';
import { Article } from '../../interfaces/article/article.interface';


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule,
    MatFormFieldModule,
    MatPaginatorModule,
    RouterModule,
    NavComponent
  ],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  selectedCategory = 'all';
  sortBy = 'popularity';

  allProducts: Article[] = [];

  constructor(private articleService: ArticleService) { }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts() {
    this.articleService.getArticles().subscribe((articles) => {
      this.allProducts = articles;
    });
  }
}
