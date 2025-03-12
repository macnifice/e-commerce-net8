export interface CartItem {
  id: number;
  articleId: number;
  storeId: number;
  articleStoreId: number;
  storeName: string;
  productName: string;
  productImage: string;
  price: number;
  quantity: number;
}