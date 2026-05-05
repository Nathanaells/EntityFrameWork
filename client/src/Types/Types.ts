export interface RegisterForm {
  DisplayName: string;
  email: string;
  password: string;
}

export interface LoginForm {
  email: string;
  password: string;
}

export interface LoginData {
  token: string;
}

export interface User {
  id?: string;
  userName: string;
  email: string;
}

export interface UpdateUserPayload {
  userName?: string;
  email?: string;
  password?: string;
}

export interface StoreDTO {
  name: string;
  location: string;
}

export interface ProductDTO {
  id: number;
  name: string;
  price: number;
  storeId: number;
}

export interface APIResponse<T> {
  status?: boolean;
  message: string;
  data?: T;
  error?: string | string[];
}

export interface StoreResponseDTO {
  id: number;
  name: string;
  location: string;
}

export interface ProductResponseDTO {
  id: number;
  name: string;
  price: number;
  storeId: number;
}

export interface UpdateProductDTO {
  id: number;
  name?: string;
  price?: number;
  storeId: number;
}
