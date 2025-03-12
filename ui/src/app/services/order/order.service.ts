import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateOrderDto } from '../../interfaces/order/create-order.dto';
import { OrderDto } from '../../interfaces/order/order.dto';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private baseUrl = 'https://localhost:7211/api/v1/Order';

  constructor(private http: HttpClient) { }

  createOrder(orderDto: CreateOrderDto[]): Observable<any> {
    return this.http.post(this.baseUrl, orderDto);
  }

  getOrders(): Observable<OrderDto[]> {
    return this.http.get<OrderDto[]>(this.baseUrl);
  }
}