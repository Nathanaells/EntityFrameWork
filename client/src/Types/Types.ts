export interface RegisterForm {
  username: string;
  email: string;
  password: string;
}

export interface LoginForm {
  email: string;
  password: string;
}

export interface User {
  id: string;
  username: string;
  email: string;
}

export interface StoreDTO {
  id: number;
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
  success: boolean;
  message: string;
  data?: T;
  error?: string;
}
