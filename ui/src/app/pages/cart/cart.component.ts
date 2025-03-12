import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { NavComponent } from '../../components/nav/nav.component';
import { CartItem } from '../../interfaces/cart/cart.interface';
import { OrderService } from '../../services/order/order.service';
import { CreateOrderDto } from '../../interfaces/order/create-order.dto';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    MatSnackBarModule,
    MatDialogModule,
    NavComponent
  ],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent implements OnInit {
  cartItems: CartItem[] = [];

  constructor(
    private router: Router,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
    private orderService: OrderService
  ) { }

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart(): void {
    const cartJson = localStorage.getItem('cart');
    if (cartJson) {
      this.cartItems = JSON.parse(cartJson);
    }
  }

  saveCart(): void {
    localStorage.setItem('cart', JSON.stringify(this.cartItems));
  }


  increaseQuantity(item: CartItem): void {
    const index = this.cartItems.findIndex(cartItem => cartItem.id === item.id);
    if (index !== -1) {
      this.cartItems[index].quantity += 1;
      this.saveCart();
    }
  }

  decreaseQuantity(item: CartItem): void {
    const index = this.cartItems.findIndex(cartItem => cartItem.id === item.id);
    if (index !== -1 && this.cartItems[index].quantity > 1) {
      this.cartItems[index].quantity -= 1;
      this.saveCart();
    }
  }

  removeItem(item: CartItem): void {
    this.cartItems = this.cartItems.filter(cartItem => cartItem.id !== item.id);
    this.saveCart();
    this.snackBar.open('Producto eliminado del carrito', 'Cerrar', {
      duration: 3000
    });
  }

  getTotalItems(): number {
    return this.cartItems.reduce((total, item) => total + item.quantity, 0);
  }

  getTotalAmount(): number {
    return this.cartItems.reduce((total, item) => total + (item.price * item.quantity), 0);
  }

  checkout(): void {
    const userJson = localStorage.getItem('user');
    if (!userJson) {
      this.snackBar.open('Debe iniciar sesión para finalizar la compra', 'Ir a Login', {
        duration: 5000
      }).onAction().subscribe(() => {
        this.router.navigate(['/login']);
      });
      return;
    }

    const user = JSON.parse(userJson);
    const userId = user.id;

    const orderRequests: CreateOrderDto[] = this.cartItems.map(item => {
      return {
        quantity: item.quantity,
        userId: userId,
        articleStoreId: item.articleStoreId,
        purchaseDate: new Date()
      };
    });

    debugger;
    this.orderService.createOrder(orderRequests).subscribe({
      next: (response) => {
        this.snackBar.dismiss();
        this.snackBar.open('¡Compra realizada con éxito!', 'Cerrar', {
          duration: 5000
        });

        this.cartItems = [];
        this.saveCart();

        setTimeout(() => {
          this.router.navigate(['/']);
        }, 3000);
      },
      error: (error) => {
        console.error(error);
        this.snackBar.dismiss();
        console.error('Error al procesar la compra:', error);

        this.snackBar.open('Error al procesar la compra. Por favor, inténtelo de nuevo.', 'Cerrar', {
          duration: 5000
        });
      }
    });

  }

  clearCart(): void {
    this.cartItems = [];
    this.saveCart();
    this.snackBar.open('Carrito vaciado', 'Cerrar', {
      duration: 3000
    });
  }
}
