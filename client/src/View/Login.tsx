import { useState } from "react";
import { useNavigate } from "react-router";
import { FetchLogin } from "../API/FetchAPI";
import { ShowSuccess } from "../Constant/UIMessage";

export default function Login() {
  const navigate = useNavigate();
  const [form, setForm] = useState({
    email: "",
    password: "",
  });
  const [errors, setErrors] = useState({
    email: [] as string[],
    password: [] as string[],
    general: [] as string[],
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setForm((prevForm) => ({
      ...prevForm,
      [name]: value,
    }));
  };

  async function handleSubmit(e: React.SubmitEvent<HTMLFormElement>) {
    try {
      e.preventDefault();
      setErrors({ email: [], password: [], general: [] });
      const result = await FetchLogin(form);

      if (result.status && result.data?.token) {
        localStorage.setItem("token", result.data.token);
        ShowSuccess(result.message);
        navigate("/");
      }
    } catch (error: unknown) {
      const messages = Array.isArray(error)
        ? error
        : error instanceof Error
          ? [error.message]
          : [String(error)];
      const nextErrors = {
        email: [],
        password: [],
        general: [],
      } as typeof errors;

      messages.forEach((message) => {
        const lower = message.toLowerCase();
        if (lower.includes("email")) {
          nextErrors.email.push(message);
        } else if (lower.includes("password")) {
          nextErrors.password.push(message);
        } else {
          nextErrors.general.push(message);
        }
      });

      setErrors(nextErrors);
    }
  }

  return (
    <div className="min-h-screen bg-white px-6 py-12 text-slate-900">
      <div className="mx-auto w-full max-w-md">
        <div className="rounded-2xl border border-slate-200 bg-white p-8 shadow-xl">
          <div className="flex flex-col items-center gap-3">
            <img
              alt="Your Company"
              src="https://tailwindcss.com/plus-assets/img/logos/mark.svg?color=indigo&shade=500"
              className="h-12 w-auto"
            />
            <h2 className="text-2xl font-bold tracking-tight text-slate-900">
              Sign in to your account
            </h2>
            <p className="text-sm text-slate-500">
              Welcome back. Please enter your details.
            </p>
          </div>

          <form onSubmit={handleSubmit} className="mt-8 space-y-5">
            {errors.general.length > 0 && (
              <div className="rounded-md border border-rose-200 bg-rose-50 px-4 py-3 text-sm text-rose-700">
                <ul className="list-disc space-y-1 pl-5">
                  {errors.general.map((message, index) => (
                    <li key={`${message}-${index}`}>{message}</li>
                  ))}
                </ul>
              </div>
            )}
            <div>
              <label className="block text-sm font-medium text-slate-700">
                Email address
              </label>
              <div className="mt-2 h-10 overflow-auto">
                {errors.email.length > 0 && (
                  <ul className="list-disc space-y-1 pl-5 text-sm text-rose-600">
                    {errors.email.map((message, index) => (
                      <li key={`${message}-${index}`}>{message}</li>
                    ))}
                  </ul>
                )}
              </div>
              <div className="mt-2">
                <input
                  id="email"
                  name="email"
                  type="email"
                  required
                  autoComplete="email"
                  className="block w-full rounded-md border border-slate-200 bg-white px-3 py-2 text-base text-slate-900 placeholder:text-slate-400 focus:border-indigo-300 focus:outline-none focus:ring-2 focus:ring-indigo-200 sm:text-sm"
                  onChange={(e) => handleChange(e)}
                />
              </div>
            </div>

            <div>
              <label className="block text-sm font-medium text-slate-700">
                Password
              </label>
              <div className="mt-2 h-10 overflow-auto">
                {errors.password.length > 0 && (
                  <ul className="list-disc space-y-1 pl-5 text-sm text-rose-600">
                    {errors.password.map((message, index) => (
                      <li key={`${message}-${index}`}>{message}</li>
                    ))}
                  </ul>
                )}
              </div>
              <div className="mt-2">
                <input
                  id="password"
                  name="password"
                  type="password"
                  required
                  autoComplete="current-password"
                  className="block w-full rounded-md border border-slate-200 bg-white px-3 py-2 text-base text-slate-900 placeholder:text-slate-400 focus:border-indigo-300 focus:outline-none focus:ring-2 focus:ring-indigo-200 sm:text-sm"
                  onChange={(e) => handleChange(e)}
                />
              </div>
            </div>

            <div>
              <button
                type="submit"
                className="flex w-full items-center justify-center rounded-md bg-indigo-600 px-3 py-2 text-sm font-semibold text-white transition hover:bg-indigo-500 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-500"
              >
                Sign in
              </button>
            </div>
            <div className="text-center text-sm text-slate-500">
              <a
                href="/register"
                className="text-sm text-indigo-600 hover:text-indigo-500"
              >
                Don't have an account? Sign up
              </a>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}
