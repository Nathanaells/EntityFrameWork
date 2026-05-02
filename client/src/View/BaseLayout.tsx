import { useNavigate, Outlet } from "react-router";
import { useEffect } from "react";

export default function BaseLayout() {
  const navigate = useNavigate();
  const token = localStorage.getItem("token");

  useEffect(() => {
    if (!token) {
      navigate("/login");
    }
  }, [token]);

  return (
    <>
      <Outlet />
    </>
  );
}
