import { useEffect, useState } from "react";
import { useNavigate, useSearchParams } from "react-router";
import { FetchProductsByStoreId } from "../API/FetchAPI";
import { ShowError } from "../Constant/UIMessage";
import type { ProductResponseDTO } from "../Types/Types";

export default function ProductDetail() {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const storeId = Number(searchParams.get("storeId"));
  const productId = Number(searchParams.get("productId"));
  const [product, setProduct] = useState<ProductResponseDTO | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    async function loadProduct() {
      if (!storeId || !productId) {
        setIsLoading(false);
        return;
      }

      try {
        setIsLoading(true);
        const result = await FetchProductsByStoreId(storeId);
        if (result.status && result.data) {
          const found = result.data.find((item) => item.id === productId);
          setProduct(found ?? null);
        }
      } catch (error: unknown) {
        const message = error instanceof Error ? error.message : String(error);
        ShowError(message);
      } finally {
        setIsLoading(false);
      }
    }

    loadProduct();
  }, [storeId, productId]);

  if (!storeId || !productId) {
    return (
      <div className="bg-white px-6 py-10">
        <div className="mx-auto max-w-3xl rounded-xl border border-slate-200 p-6 text-center">
          <p className="text-sm text-slate-600">
            Product ID atau Store ID tidak ditemukan.
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
    <div className="bg-white px-6 py-10 text-slate-900">
      <div className="mx-auto w-full max-w-3xl space-y-6">
        <div>
          <h1 className="text-2xl font-bold">Product Detail</h1>
          <p className="text-sm text-slate-500">
            Detail product untuk store #{storeId}.
          </p>
        </div>

        <div className="rounded-xl border border-slate-200 bg-white p-6 shadow-sm">
          {isLoading ? (
            <p className="text-sm text-slate-500">Loading product...</p>
          ) : product ? (
            <div className="space-y-3 text-sm text-slate-700">
              <div className="flex justify-between">
                <span className="font-medium text-slate-900">Name</span>
                <span>{product.name}</span>
              </div>
              <div className="flex justify-between">
                <span className="font-medium text-slate-900">Price</span>
                <span>Rp {product.price.toLocaleString("id-ID")}</span>
              </div>
              <div className="flex justify-between">
                <span className="font-medium text-slate-900">Store ID</span>
                <span>{product.storeId}</span>
              </div>
            </div>
          ) : (
            <p className="text-sm text-slate-500">Product tidak ditemukan.</p>
          )}
        </div>

        <div className="flex justify-end">
          <button
            type="button"
            className="rounded-md border border-slate-200 px-4 py-2 text-sm font-medium text-slate-700 hover:bg-slate-50"
            onClick={() => navigate(`/store?storeId=${storeId}`)}
          >
            Back to Store
          </button>
        </div>
      </div>
    </div>
  );
}
