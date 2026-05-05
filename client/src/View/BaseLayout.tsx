import { useNavigate, Outlet } from "react-router";
import { useEffect, useState } from "react";
import Navbar from "../Components/Navbar";

export default function BaseLayout() {
  const navigate = useNavigate();
  const [token, setToken] = useState<string | null>(
    localStorage.getItem("token"),
  );

  useEffect(() => {
    const handleStorage = (event: StorageEvent) => {
      if (event.key === "token") {
        setToken(event.newValue);
      }
    };

    const intervalId = window.setInterval(() => {
      setToken(localStorage.getItem("token"));
    }, 1000);

    window.addEventListener("storage", handleStorage);

    return () => {
      window.removeEventListener("storage", handleStorage);
      window.clearInterval(intervalId);
    };
  }, []);

  useEffect(() => {
    if (!token) {
      navigate("/login");
    }
  }, [token]);

  return (
    <>
      <Navbar />
      <Outlet />
    </>
  );
}
