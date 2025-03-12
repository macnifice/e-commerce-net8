import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { NavComponent } from '../../components/nav/nav.component';
import { ArticleService } from '../../services/article/article.service';
import { User } from '../../interfaces/user/user.interface';
import { CartItem } from '../../interfaces/cart/cart.interface';
import { ProductWithStores, StoreInfo } from '../../interfaces/store/store.interface';


@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatButtonModule,
    MatCardModule,
    MatDividerModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    NavComponent
  ],
  templateUrl: './product-detail.component.html',
  styleUrl: './product-detail.component.css'
})
export class ProductDetailComponent implements OnInit {
  product: ProductWithStores | null = null;
  userData: User | null = null;

  constructor(
    private route: ActivatedRoute,
    private articleService: ArticleService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const productId = Number(params.get('id'));
      if (productId) {
        this.loadProduct(productId);
      }
    });
    this.userData = JSON.parse(localStorage.getItem('user') || '{}');
  }

  loadProduct(productId: number): void {
    this.articleService.getArticleById(productId).subscribe({
      next: (article: any) => {
        this.product = article as ProductWithStores;
      },
      error: (error) => {
        console.error('Error al cargar el producto:', error);
        this.snackBar.open('Error al cargar la información del producto', 'Cerrar', {
          duration: 3000
        });
      }
    });
  }

  addToCart(store: StoreInfo): void {
    if (!this.product) return;

    const userJson = localStorage.getItem('user');
    if (!userJson) {
      this.snackBar.open('Debe iniciar sesión para agregar productos al carrito', 'Ir a Login', {
        duration: 5000
      }).onAction().subscribe(() => {
        window.location.href = '/login';
      });
      return;
    }

    let cart: CartItem[] = [];
    const cartJson = localStorage.getItem('cart');
    if (cartJson) {
      cart = JSON.parse(cartJson);
    }

    const existingItemIndex = cart.findIndex(item =>
      item.articleId === this.product!.id && item.storeId === store.id
    );

    if (existingItemIndex >= 0) {
      cart[existingItemIndex].quantity += 1;
    } else {

      const newItem: CartItem = {
        id: Date.now(),
        articleId: this.product.id!,
        storeId: store.id,
        articleStoreId: store.articleStoreId,
        storeName: store.name,
        productName: this.product.name,
        productImage: this.product.imagePath,
        price: store.price,
        quantity: 1
      };
      cart.push(newItem);
    }
    localStorage.setItem('cart', JSON.stringify(cart));

    this.snackBar.open(`${this.product.name} agregado al carrito`, 'Ver Carrito', {
      duration: 3000
    }).onAction().subscribe(() => {
      window.location.href = '/cart';
    });
  }
}
