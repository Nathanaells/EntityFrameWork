import { useState } from "react";
import { FetchRegister} from "../API/FetchAPI";
import {ShowSuccess, ShowError} from "../Constant/UIMessage";
import {useNavigate} from "react-router";



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

      console.log(result.success);
      console.log(result.message);
      if (result.success) {
        ShowSuccess(result.message);
        navigate("/login");
      }

    } catch (error) {
      ShowError("Registration failed. Please try again.");
    }
  }

  return (
    <div className="min-h-screen bg-gradient-to-b from-blue-950 via-blue-900 to-slate-900 px-6 py-12 text-blue-50">
      <div className="mx-auto w-full max-w-md">
        <div className="rounded-2xl border border-white/10 bg-blue-900/40 p-8 shadow-2xl backdrop-blur">
          <div className="flex flex-col items-center gap-3">
            <img
              alt="Your Company"
              src="https://tailwindcss.com/plus-assets/img/logos/mark.svg?color=indigo&shade=500"
              className="h-12 w-auto"
            />
            <h2 className="text-2xl font-bold tracking-tight text-white">
              Create a new account
            </h2>
            <p className="text-sm text-blue-100/80">
              Join the marketplace and start selling today.
            </p>
          </div>

          <form onSubmit={HandleSubmit} method="POST" className="mt-8 space-y-5">
            <div>
              <label className="block text-sm font-medium text-blue-100">
                Username
              </label>
              <div className="mt-2">
                <input
                  id="DisplayName"
                  name="DisplayName"
                  type="text"
                  required
                  autoComplete="DisplayName"
                  className="block w-full rounded-md border border-white/10 bg-blue-950/60 px-3 py-2 text-base text-white placeholder:text-blue-200/60 focus:border-blue-300 focus:outline-none focus:ring-2 focus:ring-blue-400 sm:text-sm"
                  onChange={(e) => handleChange(e)}
                />
              </div>
            </div>
            <div>
              <label className="block text-sm font-medium text-blue-100">
                Email address
              </label>
              <div className="mt-2">
                <input
                  id="email"
                  name="email"
                  type="email"
                  required
                  autoComplete="email"
                  className="block w-full rounded-md border border-white/10 bg-blue-950/60 px-3 py-2 text-base text-white placeholder:text-blue-200/60 focus:border-blue-300 focus:outline-none focus:ring-2 focus:ring-blue-400 sm:text-sm"
                  onChange={(e) => handleChange(e)}
                />
              </div>
            </div>

            <div>
              <div className="flex items-center justify-between">
                <label className="block text-sm font-medium text-blue-100">
                  Password
                </label>
                <div className="text-sm">
                  <a
                    href="#"
                    className="font-semibold text-blue-200 hover:text-blue-100"
                  >
                    Forgot password?
                  </a>
                </div>
              </div>
              <div className="mt-2">
                <input
                  id="password"
                  name="password"
                  type="password"
                  required
                  autoComplete="current-password"
                  className="block w-full rounded-md border border-white/10 bg-blue-950/60 px-3 py-2 text-base text-white placeholder:text-blue-200/60 focus:border-blue-300 focus:outline-none focus:ring-2 focus:ring-blue-400 sm:text-sm"
                  onChange={(e) => handleChange(e)}
                />
              </div>
            </div>

            <div>
              <button
                type="submit"
                className="flex w-full items-center justify-center rounded-md bg-blue-500 px-3 py-2 text-sm font-semibold text-white transition hover:bg-blue-400 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-blue-300"
              >
                Sign in
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}
