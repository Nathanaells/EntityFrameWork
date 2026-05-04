import { BASE_URL } from "../Constant/BaseURL";
import type {RegisterForm, LoginForm, User, APIResponse, LoginData, StoreResponseDTO, StoreDTO, ProductResponseDTO, ProductDTO, UpdateProductDTO} from "../Types/Types";

function normalizeResponse<T>(data: APIResponse<T>): APIResponse<T> {
  const success = data.success ?? data.status ?? false;
  return { ...data, success };
}


export async function FetchRegister(
  RegisterForm: RegisterForm
): Promise<APIResponse<User>> {
  try {
    const response = await fetch(`${BASE_URL}auth/register`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({username : RegisterForm.DisplayName, email : RegisterForm.email, password : RegisterForm.password})
    });

    if (!response.ok) {
      throw new Error("Network response was not ok");
    }

    const data: APIResponse<User> = await response.json();
    const normalized = normalizeResponse(data);

    if (!normalized.success) {
      const message = normalized.error || normalized.message || "Registration failed";
      throw new Error(message);
    }

    return normalized;
  } catch (error) {
    throw new Error(
      `Registration failed: ${
        error instanceof Error ? error.message : String(error)
      }`
    );
  }
}

export async function FetchLogin(LoginForm: LoginForm) : Promise<APIResponse<LoginData>> {
  try {
    const response = await fetch(`${BASE_URL}auth/login`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({
        LoginForm
      })
    });

    if (!response.ok) {
      throw new Error("Network response was not ok");
    }

    const data : APIResponse<LoginData> = await response.json();
    const normalized = normalizeResponse(data);

    if(!normalized.success) {
      throw new Error(normalized.error || "Login failed");
    }

    return normalized;

  } catch (error) {
    throw new Error(`Login failed: ${error instanceof Error ? error.message : String(error)}`); 
  }
}


//User API

export async function FetchUserData() : Promise<APIResponse<User>> {
  try {
    const token = localStorage.getItem("token");

    const response = await fetch(`${BASE_URL}user/me`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`
      }
    });

    if (!response.ok) {
      throw new Error("Network response was not ok");
    }

    const data: APIResponse<User> = await response.json();
    const normalized = normalizeResponse(data);

    if (!normalized.success) {
      throw new Error(normalized.error || "Failed to fetch user data");
    }

    return normalized;
  } catch (error) {
    throw new Error(`Failed to fetch user data: ${error instanceof Error ? error.message : String(error)}`);
  }
}

export async function FetchUpdateUser(update : User) : Promise<APIResponse<User>> 
{
  try{
    const token = localStorage.getItem("token");

    const response = await fetch(`${BASE_URL}user/me`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`
      },
      body: JSON.stringify(update)
    });

    if (!response.ok) {
      throw new Error("Network response was not ok");
    }

    const data: APIResponse<User> = await response.json();
    const normalized = normalizeResponse(data);

    if (!normalized.success) {
      throw new Error(normalized.error || "Failed to update user data");
    }

    return normalized;
  } catch (error) {
    throw new Error(`Failed to update user data: ${error instanceof Error ? error.message : String(error)}`);
  }
}


//Store API Calls

export async function FetchStores() : Promise<APIResponse<StoreResponseDTO[]>> 
{
  try {
    const token = localStorage.getItem("token");

    const response = await fetch(`${BASE_URL}stores`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`
      }
    });

    if (!response.ok) {
      throw new Error("Network response was not ok");
    }

    const data : APIResponse<StoreResponseDTO[]> = await response.json();
    const normalized = normalizeResponse(data);

    if (!normalized.success) {
      throw new Error(normalized.error || "Failed to fetch stores");
    }

    return normalized;
  }catch (e : unknown){
    throw new Error(`Failed to fetch stores: ${e instanceof Error ? e.message : String(e)}`);
  }
}


export async function CreateStore(dto : StoreDTO) : Promise<APIResponse<StoreResponseDTO>>
{
  try {
    const token = localStorage.getItem("token");
    const response = await fetch(`${BASE_URL}stores`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`
      },
      body: JSON.stringify(dto)
    });

    if (!response.ok) {
      throw new Error("Network response was not ok");
    }

    const data: APIResponse<StoreResponseDTO> = await response.json();
    const normalized = normalizeResponse(data);

    if (!normalized.success) {
      throw new Error(normalized.error || "Failed to create store");
    }

    return normalized;
  } catch (e : unknown){
    throw new Error(`Failed to create store: ${e instanceof Error ? e.message : String(e)}`);
  }
}

export async function UpdateStore(dto : StoreDTO, id : number) : Promise<APIResponse<StoreResponseDTO>>
{
  try {
    const token = localStorage.getItem("token");
    const response = await fetch(`${BASE_URL}stores/${id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`
      },
      body: JSON.stringify(dto)
    }); 

      if(!response.ok) {
        throw new Error("Network response was not ok");
      }
      const data : APIResponse<StoreResponseDTO> = await response.json();
      return normalizeResponse(data);
  }
    catch (e : unknown){
    throw new Error(`Failed to update store: ${e instanceof Error ? e.message : String(e)}`);
  }
}

export async function DeleteStore(id : number) : Promise<APIResponse<boolean>>
{
  try {
    const token = localStorage.getItem("token");

    const response = await fetch(`${BASE_URL}stores/${id}`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`
      }
    });

    if (!response.ok) {
      throw new Error("Network response was not ok");
    }

    const data: APIResponse<boolean> = await response.json();
    const normalized = normalizeResponse(data);

    if (!normalized.success) {
      throw new Error(normalized.error || "Failed to delete store");
    }

    return normalized;
  } catch (e : unknown) {
    throw new Error(`Failed to delete store: ${e instanceof Error ? e.message : String(e)}`);
  }
}

export async function FetchStroreById(id : number) : Promise<APIResponse<StoreResponseDTO>>
{
  try {
    const token = localStorage.getItem("token");  
    const response = await fetch(`${BASE_URL}stores/${id}`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`
      }
    }); 
    if (!response.ok) {
      throw new Error("Network response was not ok");
    }
    const data : APIResponse<StoreResponseDTO> = await response.json();
    const normalized = normalizeResponse(data);

    if (!normalized.success) {
      throw new Error(normalized.error || "Failed to fetch store");
    }
    return normalized;
  } catch (e : unknown) {
    throw new Error(`Failed to fetch store: ${e instanceof Error ? e.message : String(e)}`);
  }
}


//Product API calls


export async function FetchProductsByStoreId(storeId : number) : Promise<APIResponse<ProductResponseDTO[]>>
{
  try {
    const token = localStorage.getItem("token");
    const response : Response = await fetch(`${BASE_URL}stores/${storeId}/products`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`
      }
    });
    if (!response.ok) {
      throw new Error("Cannot Fetch Products");
    }
    const data : APIResponse<ProductResponseDTO[]> = await response.json();
    return normalizeResponse(data);
  }
  catch (e : unknown) {
    throw new Error(`Failed to fetch products: ${e instanceof Error ? e.message : String(e)}`);
  }
}


export async function CreateProduct(dto : ProductDTO, storeId : number) : Promise<APIResponse<ProductResponseDTO>>
{
  try{
    const token = localStorage.getItem("token");
    const response = await fetch(`${BASE_URL}stores/${storeId}/products`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`
      },
      body: JSON.stringify(dto)
    }); 


    const data : APIResponse<ProductResponseDTO> = await response.json();
    const normalized = normalizeResponse(data);

    if (!normalized.success) {
      throw new Error(normalized.error || "Failed to create product");
    }
    return normalized;
  }
  catch (e : unknown) {
    throw new Error(`Failed to create product: ${e instanceof Error ? e.message : String(e)}`);
  }
}


export async function UpdateProduct(dto : UpdateProductDTO, storeId : number, productId : number) : Promise<APIResponse<ProductResponseDTO>>
{
  try {
    const token = localStorage.getItem("token");
    const response = await fetch(`${BASE_URL}stores/${storeId}/products/${productId}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`
      },
      body: JSON.stringify(dto)
    });
    if (!response.ok) {
      throw new Error("Network response was not ok");
    }
    const data : APIResponse<ProductResponseDTO> = await response.json();
    const normalized = normalizeResponse(data);

    if (!normalized.success) {
      throw new Error(normalized.error || "Failed to update product");
    }
    return normalized;
  } catch (e : unknown) {
    throw new Error(`Failed to update product: ${e instanceof Error ? e.message : String(e)}`);
  } 
}

export async function DeleteProduct(storeId : number, productId : number) : Promise<APIResponse<boolean>>
{
  try{
    const token = localStorage.getItem("token");
    const response = await fetch(`${BASE_URL}stores/${storeId}/products/${productId}`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`
      }
    });
    if (!response.ok) {
      throw new Error("Network response was not ok");
    }
    const data : APIResponse<boolean> = await response.json();
    const normalized = normalizeResponse(data);
    if (!normalized.success) {
      throw new Error(normalized.error || "Failed to delete product");
    }
    return normalized;
  }
  catch (e : unknown) {
    throw new Error(`Failed to delete product: ${e instanceof Error ? e.message : String(e)}`);
  }
}