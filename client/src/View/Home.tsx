import { useEffect, useState } from "react";
import { useNavigate } from "react-router";
import {
  CreateStore,
  DeleteStore,
  FetchStores,
  UpdateStore,
} from "../API/FetchAPI";
import { ShowError, ShowSuccess } from "../Constant/UIMessage";
import ConfirmModal from "../Components/ConfirmModal";
import type { StoreDTO, StoreResponseDTO } from "../Types/Types";

export default function Home() {
  const navigate = useNavigate();
  const [stores, setStores] = useState<StoreResponseDTO[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [newStore, setNewStore] = useState<StoreDTO>({
    name: "",
    location: "",
  });
  const [editingId, setEditingId] = useState<number | null>(null);
  const [editForm, setEditForm] = useState<StoreDTO>({
    name: "",
    location: "",
  });
  const [storeToDelete, setStoreToDelete] = useState<StoreResponseDTO | null>(
    null,
  );

  const loadStores = async () => {
    try {
      setIsLoading(true);
      const result = await FetchStores();
      if (result.status && result.data) {
        setStores(result.data);
      }
    } catch (error: unknown) {
      const message = error instanceof Error ? error.message : String(error);
      if (
        message.toLowerCase().includes("no stores found") ||
        message.toLowerCase().includes("no stores")
      ) {
        setStores([]);
        return;
      }
      ShowError(message);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    loadStores();
  }, []);

  const handleAddStore = async (e: React.FormEvent<HTMLFormElement>) => {
    try {
      e.preventDefault();
      const result = await CreateStore(newStore);
      if (result.status && result.data) {
        ShowSuccess(result.message);
        setNewStore({ name: "", location: "" });
        loadStores();
      }
    } catch (error: unknown) {
      const message = error instanceof Error ? error.message : String(error);
      ShowError(message);
    }
  };

  const handleDelete = async (id: number) => {
    try {
      const result = await DeleteStore(id);
      if (result.status) {
        ShowSuccess(result.message);
        setStores((prev) => prev.filter((store) => store.id !== id));
      }
    } catch (error: unknown) {
      const message = error instanceof Error ? error.message : String(error);
      ShowError(message);
    }
  };

  const handleEdit = (store: StoreResponseDTO) => {
    setEditingId(store.id);
    setEditForm({ name: store.name, location: store.location });
  };

  const handleUpdate = async (id: number) => {
    try {
      const result = await UpdateStore(editForm, id);

      console.log(result);
      if (result.status && result.data) {
        const updatedStore = result.data;
        ShowSuccess(result.message);
        setStores((prev) =>
          prev.map((store) => (store.id === id ? updatedStore : store)),
        );
        setEditingId(null);
      }
    } catch (error: unknown) {
      const message = error instanceof Error ? error.message : String(error);
      ShowError(message);
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-indigo-50 via-sky-50 to-emerald-50 px-6 py-10 text-slate-900">
      <div className="mx-auto w-full max-w-5xl space-y-6">
        <div className="rounded-2xl border border-indigo-100 bg-white/80 p-6 shadow-lg backdrop-blur">
          <div className="flex flex-col gap-2">
            <h1 className="text-2xl font-bold text-indigo-900">Store List</h1>
            <p className="text-sm text-slate-500">
              Manage your stores and open each store to see its products.
            </p>
          </div>
        </div>

        <form
          onSubmit={handleAddStore}
          className="rounded-2xl border border-indigo-100 bg-white p-5 shadow-lg"
        >
          <h2 className="text-sm font-semibold text-indigo-900">Add Store</h2>
          <div className="mt-3 grid gap-3 md:grid-cols-2">
            <input
              name="name"
              value={newStore.name}
              onChange={(e) =>
                setNewStore((prev) => ({ ...prev, name: e.target.value }))
              }
              placeholder="Store name"
              className="rounded-md border border-slate-200 bg-white px-3 py-2 text-sm focus:border-indigo-300 focus:outline-none focus:ring-2 focus:ring-indigo-200"
              required
            />
            <input
              name="location"
              value={newStore.location}
              onChange={(e) =>
                setNewStore((prev) => ({
                  ...prev,
                  location: e.target.value,
                }))
              }
              placeholder="Location"
              className="rounded-md border border-slate-200 bg-white px-3 py-2 text-sm focus:border-indigo-300 focus:outline-none focus:ring-2 focus:ring-indigo-200"
              required
            />
          </div>
          <div className="mt-3 flex justify-end">
            <button
              type="submit"
              className="rounded-md bg-indigo-600 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-500"
            >
              Add Store
            </button>
          </div>
        </form>

        <div className="overflow-hidden rounded-2xl border border-indigo-100 bg-white shadow-lg">
          <table className="w-full border-collapse text-left text-sm">
            <thead className="bg-indigo-600 text-white">
              <tr>
                <th className="px-4 py-3 font-semibold">Name</th>
                <th className="px-4 py-3 font-semibold">Location</th>
                <th className="px-4 py-3 font-semibold">Actions</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-indigo-50">
              {isLoading && (
                <tr>
                  <td colSpan={3} className="px-4 py-6 text-center">
                    Loading stores...
                  </td>
                </tr>
              )}
              {!isLoading && stores.length === 0 && (
                <tr>
                  <td colSpan={3} className="px-4 py-6 text-center">
                    No stores available.
                  </td>
                </tr>
              )}
              {stores.map((store) => (
                <tr key={store.id} className="bg-white">
                  <td className="px-4 py-3">
                    {editingId === store.id ? (
                      <input
                        value={editForm.name}
                        onChange={(e) =>
                          setEditForm((prev) => ({
                            ...prev,
                            name: e.target.value,
                          }))
                        }
                        className="w-full rounded-md border border-slate-200 px-2 py-1 focus:border-indigo-300 focus:outline-none focus:ring-2 focus:ring-indigo-200"
                      />
                    ) : (
                      store.name
                    )}
                  </td>
                  <td className="px-4 py-3">
                    {editingId === store.id ? (
                      <input
                        value={editForm.location}
                        onChange={(e) =>
                          setEditForm((prev) => ({
                            ...prev,
                            location: e.target.value,
                          }))
                        }
                        className="w-full rounded-md border border-slate-200 px-2 py-1 focus:border-indigo-300 focus:outline-none focus:ring-2 focus:ring-indigo-200"
                      />
                    ) : (
                      store.location
                    )}
                  </td>
                  <td className="px-4 py-3">
                    <div className="flex flex-wrap gap-2">
                      <button
                        type="button"
                        className="rounded-md border border-indigo-200 bg-indigo-50 px-3 py-1 text-xs font-medium text-indigo-700 hover:bg-indigo-100"
                        onClick={() => navigate(`/store?storeId=${store.id}`)}
                      >
                        Detail
                      </button>
                      {editingId === store.id ? (
                        <>
                          <button
                            type="button"
                            className="rounded-md bg-emerald-600 px-3 py-1 text-xs font-semibold text-white hover:bg-emerald-500"
                            onClick={() => handleUpdate(store.id)}
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
                          onClick={() => handleEdit(store)}
                        >
                          Edit
                        </button>
                      )}
                      <button
                        type="button"
                        className="rounded-md border border-rose-200 bg-rose-50 px-3 py-1 text-xs font-medium text-rose-600 hover:bg-rose-100"
                        onClick={() => setStoreToDelete(store)}
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
        isOpen={Boolean(storeToDelete)}
        title="Delete store?"
        description={`Store "${storeToDelete?.name ?? ""}" akan dihapus permanen.`}
        confirmText="Delete Store"
        onClose={() => setStoreToDelete(null)}
        onConfirm={() => {
          if (storeToDelete) {
            handleDelete(storeToDelete.id);
          }
          setStoreToDelete(null);
        }}
      />
    </div>
  );
}
