import { useNavigate, Outlet } from "react-router";
import { useEffect } from "react";

export default function BaseLayout() {
  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (!token) {
      navigate("/login");
    }
  }, [navigate]);

  return (
    <>
      <Outlet />
    </>
  );
}
