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
        <Route path="/register" element={<Register />} />
        <Route path="/login" element={<Login />} />

        <Route element={<BaseLayout />}>
          <Route path="/" element={<Home />} />

          <Route path="/store/:storeId" element={<StoreDetail />}>
            <Route path="product/:productId" element={<ProductDetail />} />
          </Route>
        </Route>
      </Routes>
    </BrowserRouter>
    <ToastContainer />
    </>
  );
}
