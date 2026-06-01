export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  stock: number;
  imageUrl?: string | null;
  categoryId: number;
  categoryName: string;
  sellerId: string | null;
  sellerName: string;
  tags: Tag[];
}

export interface Category {
  id: number;
  name: string;
}

export interface Tag {
  id: number;
  name: string;
}

export interface CurrentUser {
  id: string;
  name: string;
  email: string;
  roles: string[];
}

export interface CreateProductDto {
  name: string;
  description: string;
  price: number;
  stock: number;
  imageUrl?: string | null;
  categoryId: number;
  tagIds?: number[];
}

export interface UpdateProductDto {
  name: string;
  description: string;
  price: number;
  stock: number;
  imageUrl?: string | null;
  categoryId: number;
  tagIds?: number[];
}