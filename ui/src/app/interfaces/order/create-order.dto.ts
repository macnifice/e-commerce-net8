export interface CreateOrderDto {
  quantity: number;
  userId: number;
  articleStoreId: number;
  purchaseDate: Date;
}