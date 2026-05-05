import { useEffect, useState } from "react";
import { useNavigate } from "react-router";
import { FetchUpdateUser, FetchUserData } from "../API/FetchAPI";
import { ShowError } from "../Constant/UIMessage";
import type { User } from "../Types/Types";

type PanelMode = "detail" | "edit" | null;

export default function Navbar() {
  const navigate = useNavigate();
  const [user, setUser] = useState<User | null>(null);
  const [panelMode, setPanelMode] = useState<PanelMode>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [form, setForm] = useState({
    username: "",
    email: "",
    password: "",
  });
  const [errors, setErrors] = useState({
    username: [] as string[],
    email: [] as string[],
    password: [] as string[],
    general: [] as string[],
  });

  useEffect(() => {
    let isMounted = true;

    async function loadUser() {
      try {
        setIsLoading(true);
        const result = await FetchUserData();
        if (isMounted && result.status && result.data) {
          setUser(result.data);
          setForm({
            username: result.data.userName ?? "",
            email: result.data.email ?? "",
            password: "",
          });
        }
      } catch (error: unknown) {
        if (isMounted) {
          const message =
            error instanceof Error ? error.message : String(error);
          ShowError(message);
        }
      } finally {
        if (isMounted) {
          setIsLoading(false);
        }
      }
    }

    loadUser();

    return () => {
      isMounted = false;
    };
  }, []);

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/login");
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setForm((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleUpdate = async (e: React.FormEvent<HTMLFormElement>) => {
    try {
      e.preventDefault();
      setErrors({ username: [], email: [], password: [], general: [] });
      const result = await FetchUpdateUser({
        userName: form.username,
        email: form.email,
        password: form.password,
      });

      if (result.status && result.data) {
        setUser(result.data);
        setForm((prev) => ({
          ...prev,
          password: "",
        }));
        setPanelMode("detail");
      }
    } catch (error: unknown) {
      const messages = Array.isArray(error)
        ? error
        : error instanceof Error
          ? [error.message]
          : [String(error)];
      const nextErrors = {
        username: [],
        email: [],
        password: [],
        general: [],
      } as typeof errors;

      messages.forEach((message) => {
        const lower = message.toLowerCase();
        if (lower.includes("username")) {
          nextErrors.username.push(message);
        } else if (lower.includes("email")) {
          nextErrors.email.push(message);
        } else if (lower.includes("password")) {
          nextErrors.password.push(message);
        } else {
          nextErrors.general.push(message);
        }
      });

      setErrors(nextErrors);
    }
  };

  return (
    <header className="border-b border-slate-200 bg-white">
      <div className="mx-auto flex max-w-6xl items-center justify-between px-6 py-4">
        <button
          type="button"
          className="text-lg font-semibold text-slate-900"
          onClick={() => navigate("/")}
        >
          Marketplace
        </button>

        <div className="flex items-center gap-3">
          <span className="text-sm text-slate-500">
            {isLoading ? "Loading..." : user?.userName || "Guest"}
          </span>
          <button
            type="button"
            className="rounded-md border border-slate-200 px-3 py-1.5 text-sm font-medium text-slate-700 hover:bg-slate-50"
            onClick={() =>
              setPanelMode((prev) => (prev === "detail" ? null : "detail"))
            }
          >
            Account
          </button>
        </div>
      </div>

      {panelMode && (
        <div className="border-t border-slate-200 bg-slate-50">
          <div className="mx-auto flex max-w-6xl flex-col gap-4 px-6 py-4">
            <div className="flex flex-wrap items-center gap-3">
              <button
                type="button"
                className={`rounded-md px-3 py-1.5 text-sm font-medium ${
                  panelMode === "detail"
                    ? "bg-indigo-600 text-white"
                    : "border border-slate-200 bg-white text-slate-700"
                }`}
                onClick={() => setPanelMode("detail")}
              >
                Detail User
              </button>
              <button
                type="button"
                className={`rounded-md px-3 py-1.5 text-sm font-medium ${
                  panelMode === "edit"
                    ? "bg-indigo-600 text-white"
                    : "border border-slate-200 bg-white text-slate-700"
                }`}
                onClick={() => setPanelMode("edit")}
              >
                Edit User
              </button>
              <button
                type="button"
                className="rounded-md border border-slate-200 bg-white px-3 py-1.5 text-sm font-medium text-slate-700 hover:bg-slate-100"
                onClick={handleLogout}
              >
                Logout
              </button>
            </div>

            {panelMode === "detail" && (
              <div className="rounded-lg border border-slate-200 bg-white p-4 text-sm text-slate-700">
                <p className="font-medium text-slate-900">User Detail</p>
                <div className="mt-3 space-y-1">
                  <p>
                    <span className="font-semibold">Username:</span>{" "}
                    {user?.userName || "-"}
                  </p>
                  <p>
                    <span className="font-semibold">Email:</span>{" "}
                    {user?.email || "-"}
                  </p>
                </div>
              </div>
            )}

            {panelMode === "edit" && (
              <form
                onSubmit={handleUpdate}
                className="rounded-lg border border-slate-200 bg-white p-4"
              >
                <p className="text-sm font-medium text-slate-900">
                  Update Profile
                </p>
                {errors.general.length > 0 && (
                  <div className="mt-3 rounded-md border border-rose-200 bg-rose-50 px-4 py-3 text-sm text-rose-700">
                    <ul className="list-disc space-y-1 pl-5">
                      {errors.general.map((message, index) => (
                        <li key={`${message}-${index}`}>{message}</li>
                      ))}
                    </ul>
                  </div>
                )}
                <div className="mt-3 grid gap-3 md:grid-cols-2">
                  <label className="text-sm text-slate-700">
                    Username
                    <div className="mt-2 h-10 overflow-auto">
                      {errors.username.length > 0 && (
                        <ul className="list-disc space-y-1 pl-5 text-sm text-rose-600">
                          {errors.username.map((message, index) => (
                            <li key={`${message}-${index}`}>{message}</li>
                          ))}
                        </ul>
                      )}
                    </div>
                    <input
                      name="username"
                      value={form.username}
                      onChange={handleChange}
                      className="mt-1 w-full rounded-md border border-slate-200 px-3 py-2 text-sm"
                      required
                    />
                  </label>
                  <label className="text-sm text-slate-700">
                    Email
                    <div className="mt-2 h-10 overflow-auto">
                      {errors.email.length > 0 && (
                        <ul className="list-disc space-y-1 pl-5 text-sm text-rose-600">
                          {errors.email.map((message, index) => (
                            <li key={`${message}-${index}`}>{message}</li>
                          ))}
                        </ul>
                      )}
                    </div>
                    <input
                      name="email"
                      type="email"
                      value={form.email}
                      onChange={handleChange}
                      className="mt-1 w-full rounded-md border border-slate-200 px-3 py-2 text-sm"
                      required
                    />
                  </label>
                  <label className="text-sm text-slate-700">
                    Password
                    <div className="mt-2 h-10 overflow-auto">
                      {errors.password.length > 0 && (
                        <ul className="list-disc space-y-1 pl-5 text-sm text-rose-600">
                          {errors.password.map((message, index) => (
                            <li key={`${message}-${index}`}>{message}</li>
                          ))}
                        </ul>
                      )}
                    </div>
                    <input
                      name="password"
                      type="password"
                      value={form.password}
                      onChange={handleChange}
                      placeholder="Leave blank to keep current"
                      className="mt-1 w-full rounded-md border border-slate-200 px-3 py-2 text-sm"
                    />
                  </label>
                </div>
                <div className="mt-4 flex justify-end">
                  <button
                    type="submit"
                    className="rounded-md bg-indigo-600 px-4 py-2 text-sm font-semibold text-white hover:bg-indigo-500"
                  >
                    Save
                  </button>
                </div>
              </form>
            )}
          </div>
        </div>
      )}
    </header>
  );
}
