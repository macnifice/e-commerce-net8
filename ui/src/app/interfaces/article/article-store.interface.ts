export interface ArticleStore {
  id?: number;
  price: number;
  stock: number;
  date: Date;
  articleId: number;
  storeId: number;
  isEditing?: boolean;
}
