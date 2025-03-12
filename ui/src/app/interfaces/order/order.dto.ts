export interface OrderDto {
  id: number;
  articleName: string;
  storeName: string;
  quantity: number;
  price: number;
  totalAmount: number;
  purchaseDate: Date;
}