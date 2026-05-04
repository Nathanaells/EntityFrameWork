import { BrowserRouter, Routes, Route } from "react-router";
import Login from "./View/Login";
import Register from "./View/Register";
import BaseLayout from "./View/BaseLayout";
import Home from "./View/Home";
import ProductDetail from "./View/ProductDetail";
import StoreDetail from "./View/StoreDetail";
import { ToastContainer } from "react-toastify";
export default function App() {
  return (
    <>
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />

          <Route element={<BaseLayout />}>
            <Route path="/" element={<Home />} />
            <Route path="/product/:id" element={<ProductDetail />} />
            <Route path="/store/:id" element={<StoreDetail />} />
          </Route>
        </Routes>
      </BrowserRouter>
      <ToastContainer />
    </>
  );
}
