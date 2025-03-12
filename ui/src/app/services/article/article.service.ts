import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ArticleStore } from '../../interfaces/article/article-store.interface';
import { Article } from '../../interfaces/article/article.interface';

@Injectable({
  providedIn: 'root'
})
export class ArticleService {
  baseUrlArticle = 'https://localhost:7211/api/v1/Article';
  baseUrlArticleStore = 'https://localhost:7211/api/v1/Article/assign';

  constructor(private http: HttpClient) { }

  getArticles() : Observable<Article[]> {
    return this.http.get<Article[]>(`${this.baseUrlArticle}`);
  }

  createArticle(article: Article) : Observable<number> {
    return this.http.post<number>(`${this.baseUrlArticle}`, article);
  }

  updateArticle(id: number, article: Article) : Observable<number> {
    return this.http.put<number>(`${this.baseUrlArticle}/${id}`, article);
  }

  deleteArticle(id: number) : Observable<any> {
    return this.http.delete<any>(`${this.baseUrlArticle}/${id}`);
  }

  getArticlesStore(storeId: number) : Observable<ArticleStore[]> {
    return this.http.get<ArticleStore[]>(`${this.baseUrlArticleStore}/${storeId}`);
  }

  createArticleStore(articleStore: ArticleStore) : Observable<number> {
    return this.http.post<number>(`${this.baseUrlArticleStore}`, articleStore);
  }

  updateArticleStore(id: number, articleStore: ArticleStore) : Observable<number> {
    return this.http.put<number>(`${this.baseUrlArticleStore}/${id}`, articleStore);
  }

  deleteArticleStore(id: number) : Observable<any> {
    return this.http.delete<any>(`${this.baseUrlArticleStore}/${id}`);
  }

  getArticleById(id: number): Observable<Article> {
    return this.http.get<Article>(`${this.baseUrlArticle}/detail/${id}`);
  }

}
