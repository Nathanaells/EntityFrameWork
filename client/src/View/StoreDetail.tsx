import { useEffect, useState } from "react";
import { useNavigate, useSearchParams } from "react-router";
import {
  CreateProduct,
  DeleteProduct,
  FetchProductsByStoreId,
  FetchStroreById,
  UpdateProduct,
} from "../API/FetchAPI";
import { ShowError, ShowSuccess } from "../Constant/UIMessage";
import ConfirmModal from "../Components/ConfirmModal";
import type {
  ProductDTO,
  ProductResponseDTO,
  StoreResponseDTO,
  UpdateProductDTO,
} from "../Types/Types";

export default function StoreDetail() {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const storeId = Number(searchParams.get("storeId"));

  const [store, setStore] = useState<StoreResponseDTO | null>(null);
  const [products, setProducts] = useState<ProductResponseDTO[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [newProduct, setNewProduct] = useState<ProductDTO>({
    id: 0,
    name: "",
    price: 0,
    storeId: storeId || 0,
  });
  const [addErrors, setAddErrors] = useState({
    name: [] as string[],
    price: [] as string[],
    general: [] as string[],
  });
  const [editingId, setEditingId] = useState<number | null>(null);
  const [editForm, setEditForm] = useState<UpdateProductDTO>({
    id: 0,
    name: "",
    price: 0,
    storeId: storeId || 0,
  });
  const [editErrors, setEditErrors] = useState({
    name: [] as string[],
    price: [] as string[],
    general: [] as string[],
  });
  const [pageErrors, setPageErrors] = useState<string[]>([]);
  const [productToDelete, setProductToDelete] =
    useState<ProductResponseDTO | null>(null);

  const loadStore = async () => {
    if (!storeId) {
      return;
    }
    try {
      setIsLoading(true);
      const [storeResult, productResult] = await Promise.allSettled([
        FetchStroreById(storeId),
        FetchProductsByStoreId(storeId),
      ]);

      if (storeResult.status === "fulfilled") {
        if (storeResult.value.status && storeResult.value.data) {
          setStore(storeResult.value.data);
        }
      } else {
        throw storeResult.reason;
      }

      if (productResult.status === "fulfilled") {
        if (productResult.value.status && productResult.value.data) {
          setProducts(productResult.value.data);
        }
      } else {
        const message =
          productResult.reason instanceof Error
            ? productResult.reason.message
            : String(productResult.reason);
        if (
          message.toLowerCase().includes("no products") ||
          message.toLowerCase().includes("no product")
        ) {
          setProducts([]);
        } else {
          ShowError(message);
        }
      }
    } catch (error: unknown) {
      const message = error instanceof Error ? error.message : String(error);
      ShowError(message);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    loadStore();
  }, [storeId]);

  const handleAddProduct = async (e: React.FormEvent<HTMLFormElement>) => {
    try {
      e.preventDefault();
      if (!storeId) {
        return;
      }
      setAddErrors({ name: [], price: [], general: [] });
      const result = await CreateProduct(
        { ...newProduct, storeId, id: 0 },
        storeId,
      );
      if (result.status && result.data) {
        const createdProduct = result.data;
        ShowSuccess(result.message);
        setNewProduct({ id: 0, name: "", price: 0, storeId });
        setProducts((prev) => [...prev, createdProduct]);
      }
    } catch (error: unknown) {
      const messages = Array.isArray(error)
        ? error
        : error instanceof Error
          ? [error.message]
          : [String(error)];
      const nextErrors = {
        name: [],
        price: [],
        general: [],
      } as typeof addErrors;

      messages.forEach((message) => {
        const lower = message.toLowerCase();
        if (lower.includes("product name") || lower.includes("name")) {
          nextErrors.name.push(message);
        } else if (lower.includes("price")) {
          nextErrors.price.push(message);
        } else {
          nextErrors.general.push(message);
        }
      });

      setAddErrors(nextErrors);
    }
  };

  const handleDelete = async (productId: number) => {
    try {
      if (!storeId) {
        return;
      }
      setPageErrors([]);
      const result = await DeleteProduct(storeId, productId);
      if (result.status) {
        setProducts((prev) => prev.filter((item) => item.id !== productId));
      }
    } catch (error: unknown) {
      const messages = Array.isArray(error)
        ? error
        : error instanceof Error
          ? [error.message]
          : [String(error)];
      setPageErrors(messages);
    }
  };

  const handleEdit = (product: ProductResponseDTO) => {
    setEditingId(product.id);
    setEditForm({
      id: product.id,
      name: product.name,
      price: product.price,
      storeId: product.storeId,
    });
    setEditErrors({ name: [], price: [], general: [] });
  };

  const handleUpdate = async (productId: number) => {
    try {
      if (!storeId) {
        return;
      }
      setEditErrors({ name: [], price: [], general: [] });
      const result = await UpdateProduct(editForm, storeId, productId);
      if (result.status && result.data) {
        const updatedProduct = result.data;
        setProducts((prev) =>
          prev.map((item) => (item.id === productId ? updatedProduct : item)),
        );
        setEditingId(null);
      }
    } catch (error: unknown) {
      const messages = Array.isArray(error)
        ? error
        : error instanceof Error
          ? [error.message]
          : [String(error)];
      const nextErrors = {
        name: [],
        price: [],
        general: [],
      } as typeof editErrors;

      messages.forEach((message) => {
        const lower = message.toLowerCase();
        if (lower.includes("product name") || lower.includes("name")) {
          nextErrors.name.push(message);
        } else if (lower.includes("price")) {
          nextErrors.price.push(message);
        } else {
          nextErrors.general.push(message);
        }
      });

      setEditErrors(nextErrors);
    }
  };

  if (!storeId) {
    return (
      <div className="bg-white px-6 py-10">
        <div className="mx-auto max-w-4xl rounded-xl border border-slate-200 p-6 text-center">
          <p className="text-sm text-slate-600">
            Store ID tidak ditemukan. Silakan kembali ke daftar store.
          </p>
          <button
            type="button"
            className="mt-4 rounded-md bg-indigo-600 px-4 py-2 text-sm font-semibold text-white"
            onClick={() => navigate("/")}
          >
            Back to Home
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gradient-to-br from-emerald-50 via-sky-50 to-indigo-50 px-6 py-10 text-slate-900">
      <div className="mx-auto w-full max-w-5xl space-y-6">
        <div>
          <h1 className="text-2xl font-bold text-emerald-900">
            Store Detail {store ? `- ${store.name}` : ""}
          </h1>
          <p className="text-sm text-slate-500">
            {store?.location || "Manage products for this store."}
          </p>
          {pageErrors.length > 0 && (
            <div className="mt-3 rounded-md border border-rose-200 bg-rose-50 px-4 py-3 text-sm text-rose-700">
              <ul className="list-disc space-y-1 pl-5">
                {pageErrors.map((message, index) => (
                  <li key={`${message}-${index}`}>{message}</li>
                ))}
              </ul>
            </div>
          )}
        </div>

        <form
          onSubmit={handleAddProduct}
          className="rounded-2xl border border-emerald-100 bg-white p-5 shadow-lg"
        >
          <h2 className="text-sm font-semibold text-emerald-900">
            Add Product
          </h2>
          {addErrors.general.length > 0 && (
            <div className="mt-3 rounded-md border border-rose-200 bg-rose-50 px-4 py-3 text-sm text-rose-700">
              <ul className="list-disc space-y-1 pl-5">
                {addErrors.general.map((message, index) => (
                  <li key={`${message}-${index}`}>{message}</li>
                ))}
              </ul>
            </div>
          )}
          <div className="mt-3 grid gap-3 md:grid-cols-3">
            <div>
              <div className="mb-2 h-10 overflow-auto">
                {addErrors.name.length > 0 && (
                  <ul className="list-disc space-y-1 pl-5 text-sm text-rose-600">
                    {addErrors.name.map((message, index) => (
                      <li key={`${message}-${index}`}>{message}</li>
                    ))}
                  </ul>
                )}
              </div>
              <input
                name="name"
                value={newProduct.name}
                onChange={(e) =>
                  setNewProduct((prev) => ({
                    ...prev,
                    name: e.target.value,
                  }))
                }
                placeholder="Product name"
                className="w-full rounded-md border border-slate-200 bg-white px-3 py-2 text-sm focus:border-emerald-300 focus:outline-none focus:ring-2 focus:ring-emerald-200"
                required
              />
            </div>
            <div>
              <div className="mb-2 h-10 overflow-auto">
                {addErrors.price.length > 0 && (
                  <ul className="list-disc space-y-1 pl-5 text-sm text-rose-600">
                    {addErrors.price.map((message, index) => (
                      <li key={`${message}-${index}`}>{message}</li>
                    ))}
                  </ul>
                )}
              </div>
              <input
                name="price"
                type="number"
                value={newProduct.price}
                onChange={(e) =>
                  setNewProduct((prev) => ({
                    ...prev,
                    price: Number(e.target.value),
                  }))
                }
                placeholder="Price"
                className="w-full rounded-md border border-slate-200 bg-white px-3 py-2 text-sm focus:border-emerald-300 focus:outline-none focus:ring-2 focus:ring-emerald-200"
                required
              />
            </div>
            <div className="flex items-center justify-end">
              <button
                type="submit"
                className="rounded-md bg-emerald-600 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-emerald-500"
              >
                Add Product
              </button>
            </div>
          </div>
        </form>

        <div className="overflow-hidden rounded-2xl border border-emerald-100 bg-white shadow-lg">
          <table className="w-full border-collapse text-left text-sm">
            <thead className="bg-emerald-600 text-white">
              <tr>
                <th className="px-4 py-3 font-semibold">Name</th>
                <th className="px-4 py-3 font-semibold">Price</th>
                <th className="px-4 py-3 font-semibold">Actions</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-emerald-50">
              {isLoading && (
                <tr>
                  <td colSpan={3} className="px-4 py-6 text-center">
                    Loading products...
                  </td>
                </tr>
              )}
              {!isLoading && products.length === 0 && (
                <tr>
                  <td colSpan={3} className="px-4 py-6 text-center">
                    No products available.
                  </td>
                </tr>
              )}
              {products.map((product) => (
                <tr key={product.id} className="bg-white">
                  <td className="px-4 py-3">
                    {editingId === product.id ? (
                      <div>
                        <div className="mb-2 h-10 overflow-auto">
                          {editErrors.general.length > 0 && (
                            <ul className="list-disc space-y-1 pl-5 text-xs text-rose-600">
                              {editErrors.general.map((message, index) => (
                                <li key={`${message}-${index}`}>{message}</li>
                              ))}
                            </ul>
                          )}
                          {editErrors.name.length > 0 && (
                            <ul className="list-disc space-y-1 pl-5 text-xs text-rose-600">
                              {editErrors.name.map((message, index) => (
                                <li key={`${message}-${index}`}>{message}</li>
                              ))}
                            </ul>
                          )}
                        </div>
                        <input
                          value={editForm.name || ""}
                          onChange={(e) =>
                            setEditForm((prev) => ({
                              ...prev,
                              name: e.target.value,
                            }))
                          }
                          className="w-full rounded-md border border-slate-200 px-2 py-1 focus:border-emerald-300 focus:outline-none focus:ring-2 focus:ring-emerald-200"
                        />
                      </div>
                    ) : (
                      product.name
                    )}
                  </td>
                  <td className="px-4 py-3">
                    {editingId === product.id ? (
                      <div>
                        <div className="mb-2 h-10 overflow-auto">
                          {editErrors.price.length > 0 && (
                            <ul className="list-disc space-y-1 pl-5 text-xs text-rose-600">
                              {editErrors.price.map((message, index) => (
                                <li key={`${message}-${index}`}>{message}</li>
                              ))}
                            </ul>
                          )}
                        </div>
                        <input
                          type="number"
                          value={editForm.price ?? 0}
                          onChange={(e) =>
                            setEditForm((prev) => ({
                              ...prev,
                              price: Number(e.target.value),
                            }))
                          }
                          className="w-full rounded-md border border-slate-200 px-2 py-1 focus:border-emerald-300 focus:outline-none focus:ring-2 focus:ring-emerald-200"
                        />
                      </div>
                    ) : (
                      `Rp ${product.price.toLocaleString("id-ID")}`
                    )}
                  </td>
                  <td className="px-4 py-3">
                    <div className="flex flex-wrap gap-2">
                      <button
                        type="button"
                        className="rounded-md border border-indigo-200 bg-indigo-50 px-3 py-1 text-xs font-medium text-indigo-700 hover:bg-indigo-100"
                        onClick={() =>
                          navigate(
                            `/product?storeId=${storeId}&productId=${product.id}`,
                          )
                        }
                      >
                        Detail
                      </button>
                      {editingId === product.id ? (
                        <>
                          <button
                            type="button"
                            className="rounded-md bg-emerald-600 px-3 py-1 text-xs font-semibold text-white hover:bg-emerald-500"
                            onClick={() => handleUpdate(product.id)}
                          >
                            Update
                          </button>
                          <button
                            type="button"
                            className="rounded-md border border-slate-200 px-3 py-1 text-xs font-medium text-slate-700 hover:bg-slate-50"
                            onClick={() => setEditingId(null)}
                          >
                            Cancel
                          </button>
                        </>
                      ) : (
                        <button
                          type="button"
                          className="rounded-md border border-amber-200 bg-amber-50 px-3 py-1 text-xs font-medium text-amber-700 hover:bg-amber-100"
                          onClick={() => handleEdit(product)}
                        >
                          Edit
                        </button>
                      )}
                      <button
                        type="button"
                        className="rounded-md border border-rose-200 bg-rose-50 px-3 py-1 text-xs font-medium text-rose-600 hover:bg-rose-100"
                        onClick={() => setProductToDelete(product)}
                      >
                        Delete
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>

      <ConfirmModal
        isOpen={Boolean(productToDelete)}
        title="Delete product?"
        description={`Product "${productToDelete?.name ?? ""}" akan dihapus permanen.`}
        confirmText="Delete Product"
        onClose={() => setProductToDelete(null)}
        onConfirm={() => {
          if (productToDelete) {
            handleDelete(productToDelete.id);
          }
          setProductToDelete(null);
        }}
      />
    </div>
  );
}
