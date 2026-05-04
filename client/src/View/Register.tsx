import { useState } from "react";
import { FetchRegister } from "../API/FetchAPI";
import { ShowSuccess, ShowError } from "../Constant/UIMessage";
import { useNavigate } from "react-router";

export default function Register() {
  const navigate = useNavigate();
  const [form, setForm] = useState({
    DisplayName: "",
    email: "",
    password: "",
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setForm((prevForm) => ({
      ...prevForm,
      [name]: value,
    }));
  };

  async function HandleSubmit(e: React.SubmitEvent<HTMLFormElement>) {
    try {
      e.preventDefault();
      const result = await FetchRegister(form);

      if (result.status) {
        ShowSuccess(result.message);
        navigate("/login");
      }
    } catch (error: unknown) {
      if (error instanceof Error) {
        ShowError(error.message);
      } else {
        ShowError(String(error));
      }
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
              Create a new account
            </h2>
            <p className="text-sm text-slate-500">
              Join the marketplace and start selling today.
            </p>
          </div>

          <form
            onSubmit={HandleSubmit}
            method="POST"
            className="mt-8 space-y-5"
          >
            <div>
              <label className="block text-sm font-medium text-slate-700">
                Username
              </label>
              <div className="mt-2">
                <input
                  id="DisplayName"
                  name="DisplayName"
                  type="text"
                  required
                  autoComplete="DisplayName"
                  className="block w-full rounded-md border border-slate-200 bg-white px-3 py-2 text-base text-slate-900 placeholder:text-slate-400 focus:border-indigo-300 focus:outline-none focus:ring-2 focus:ring-indigo-200 sm:text-sm"
                  onChange={(e) => handleChange(e)}
                />
              </div>
            </div>
            <div>
              <label className="block text-sm font-medium text-slate-700">
                Email address
              </label>
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
                Sign up
              </button>
            </div>
            <div className="text-center text-sm text-slate-500">
              <a
                href="/login"
                className="text-sm text-indigo-600 hover:text-indigo-500"
              >
                Already have an account? Sign in
              </a>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}
