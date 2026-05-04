import { BASE_URL } from "../Constant/BaseURL";
import type {
  RegisterForm,
  LoginForm,
  User,
  APIResponse,
  LoginData,
  StoreResponseDTO,
  StoreDTO,
  ProductResponseDTO,
  ProductDTO,
  UpdateProductDTO,
} from "../Types/Types";

export async function FetchRegister(
  RegisterForm: RegisterForm,
): Promise<APIResponse<User>> {
  try {
    const response = await fetch(`${BASE_URL}auth/register`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        username: RegisterForm.DisplayName,
        email: RegisterForm.email,
        password: RegisterForm.password,
      }),
    });

    const data: APIResponse<User> = await response.json();

    if (!data.status) {
      throw new Error(data.error);
    }

    return data;
  } catch (error) {
    throw new Error(error instanceof Error ? error.message : String(error));
  }
}

export async function FetchLogin(
  LoginForm: LoginForm,
): Promise<APIResponse<LoginData>> {
  try {
    const response = await fetch(`${BASE_URL}auth/login`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        email: LoginForm.email,
        password: LoginForm.password,
      }),
    });

    const data: APIResponse<LoginData> = await response.json();

    if (!data.status) {
      throw new Error(data.error || "Login failed");
    }

    return data;
  } catch (error) {
    throw new Error(error instanceof Error ? error.message : String(error));
  }
}

//User API

export async function FetchUserData(): Promise<APIResponse<User>> {
  try {
    const token = localStorage.getItem("token");

    const response = await fetch(`${BASE_URL}user/me`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });

    if (!response.ok) {
      throw new Error(await response.text());
    }

    const data: APIResponse<User> = await response.json();

    if (!data.status) {
      throw new Error(data.error || "Failed to fetch user data");
    }

    return data;
  } catch (error) {
    throw new Error(error instanceof Error ? error.message : String(error));
  }
}

export async function FetchUpdateUser(
  update: User,
): Promise<APIResponse<User>> {
  try {
    const token = localStorage.getItem("token");

    const response = await fetch(`${BASE_URL}user/me`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify({
        username: update.userName,
        email: update.email,
      }),
    });

    if (!response.ok) {
      throw new Error(await response.text());
    }

    const data: APIResponse<User> = await response.json();

    if (!data.status) {
      throw new Error(data.error || "Failed to update user data");
    }

    return data;
  } catch (error) {
    throw new Error(error instanceof Error ? error.message : String(error));
  }
}

//Store API Calls

export async function FetchStores(): Promise<APIResponse<StoreResponseDTO[]>> {
  try {
    const token = localStorage.getItem("token");

    const response = await fetch(`${BASE_URL}store`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });

    const data: APIResponse<StoreResponseDTO[]> = await response.json();

    if (!data.status) {
      throw new Error(data.error || "Failed to fetch stores");
    }

    return data;
  } catch (error: unknown) {
    throw new Error(error instanceof Error ? error.message : String(error));
  }
}

export async function CreateStore(
  dto: StoreDTO,
): Promise<APIResponse<StoreResponseDTO>> {
  try {
    const token = localStorage.getItem("token");

    if (!token) {
      throw new Error("User is not authenticated");
    }
    const response = await fetch(`${BASE_URL}store`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify({
        name: dto.name,
        location: dto.location,
      }),
    });

    const data: APIResponse<StoreResponseDTO> = await response.json();

    if (!data.status) {
      throw new Error(data.error);
    }

    return data;
  } catch (error: unknown) {
    throw new Error(error instanceof Error ? error.message : String(error));
  }
}

export async function UpdateStore(
  dto: StoreDTO,
  id: number,
): Promise<APIResponse<StoreResponseDTO>> {
  try {
    const token = localStorage.getItem("token");
    const response = await fetch(`${BASE_URL}store/${id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify({
        id,
        name: dto.name,
        location: dto.location,
      }),
    });

    const data: APIResponse<StoreResponseDTO> = await response.json();

    if (!data.status) {
      throw new Error(data.error || "Failed to update store");
    }

    return data;
  } catch (error: unknown) {
    throw new Error(error instanceof Error ? error.message : String(error));
  }
}

export async function DeleteStore(id: number): Promise<APIResponse<boolean>> {
  try {
    const token = localStorage.getItem("token");

    const response = await fetch(`${BASE_URL}store/${id}`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });

    if (!response.ok) {
      throw new Error("Network response was not ok");
    }

    const data: APIResponse<boolean> = await response.json();

    if (!data.status) {
      throw new Error(data.error || "Failed to delete store");
    }

    return data;
  } catch (error: unknown) {
    throw new Error(error instanceof Error ? error.message : String(error));
  }
}

export async function FetchStroreById(
  id: number,
): Promise<APIResponse<StoreResponseDTO>> {
  try {
    const token = localStorage.getItem("token");
    const response = await fetch(`${BASE_URL}store/${id}`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });
    if (!response.ok) {
      throw new Error("Network response was not ok");
    }
    const data: APIResponse<StoreResponseDTO> = await response.json();

    if (!data.status) {
      throw new Error(data.error || "Failed to fetch store");
    }
    return data;
  } catch (error: unknown) {
    throw new Error(error instanceof Error ? error.message : String(error));
  }
}

//Product API calls

export async function FetchProductsByStoreId(
  storeId: number,
): Promise<APIResponse<ProductResponseDTO[]>> {
  try {
    const token = localStorage.getItem("token");
    const response: Response = await fetch(
      `${BASE_URL}store/${storeId}/product`,
      {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      },
    );
    if (!response.ok) {
      throw new Error("Cannot Fetch Products");
    }
    const data: APIResponse<ProductResponseDTO[]> = await response.json();

    return data;
  } catch (error: unknown) {
    throw new Error(error instanceof Error ? error.message : String(error));
  }
}

export async function CreateProduct(
  dto: ProductDTO,
  storeId: number,
): Promise<APIResponse<ProductResponseDTO>> {
  try {
    const token = localStorage.getItem("token");
    const response = await fetch(`${BASE_URL}store/${storeId}/product`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(dto),
    });

    const data: APIResponse<ProductResponseDTO> = await response.json();

    if (!data.status) {
      throw new Error(data.error || "Failed to create product");
    }
    return data;
  } catch (error: unknown) {
    throw new Error(error instanceof Error ? error.message : String(error));
  }
}

export async function UpdateProduct(
  dto: UpdateProductDTO,
  storeId: number,
  productId: number,
): Promise<APIResponse<ProductResponseDTO>> {
  try {
    const token = localStorage.getItem("token");
    const response = await fetch(
      `${BASE_URL}store/${storeId}/product/${productId}`,
      {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(dto),
      },
    );
    if (!response.ok) {
      throw new Error("Network response was not ok");
    }
    const data: APIResponse<ProductResponseDTO> = await response.json();

    if (!data.status) {
      throw new Error(data.error || "Failed to update product");
    }
    return data;
  } catch (error: unknown) {
    throw new Error(error instanceof Error ? error.message : String(error));
  }
}

export async function DeleteProduct(
  storeId: number,
  productId: number,
): Promise<APIResponse<boolean>> {
  try {
    const token = localStorage.getItem("token");
    const response = await fetch(
      `${BASE_URL}store/${storeId}/product/${productId}`,
      {
        method: "DELETE",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      },
    );
    if (!response.ok) {
      throw new Error("Network response was not ok");
    }
    const data: APIResponse<boolean> = await response.json();

    if (!data.status) {
      throw new Error(data.error || "Failed to delete product");
    }
    return data;
  } catch (error: unknown) {
    throw new Error(error instanceof Error ? error.message : String(error));
  }
}
