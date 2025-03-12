import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Store } from '../../interfaces/store/store.interface';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StoreService {

  baseUrl = 'https://localhost:7211/api/v1/Store';

  constructor(private http: HttpClient) { }

  getStores(): Observable<Store[]> {
    return this.http.get<Store[]>(`${this.baseUrl}`);
  }

  getStoreById(id: number): Observable<Store> {
    return this.http.get<Store>(`${this.baseUrl}/${id}`);
  }

  createStore(store: Store): Observable<number> {
    return this.http.post<number>(`${this.baseUrl}`, store);
  }

  updateStore(id: number, store: Store): Observable<number> {
    return this.http.put<number>(`${this.baseUrl}/${id}`, store);
  }

  deleteStore(id: number): Observable<any> {
    return this.http.delete<any>(`${this.baseUrl}/${id}`);
  }

}
