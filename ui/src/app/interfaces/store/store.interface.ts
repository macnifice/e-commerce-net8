import { Article } from "../article/article.interface";

export interface Store {
  id?: number;
  name: string;
  address: string;
  isEditing?: boolean;
}

export interface ProductWithStores extends Article {
  store: StoreInfo[];
}

export interface StoreInfo {
  id: number;
  name: string;
  address: string;
  price: number;
  stock: number;
  articleStoreId: number;
}
