import { Routes, Route } from "react-router-dom";

import Inicio from "../pages/Index";
import Puestos from "../pages/Puestos";

function AppRoutes() {
  return (
    <Routes>
      <Route path="/" element={<Inicio />} />
      <Route path="/puestos" element={<Puestos />} />
    </Routes>
  );
}

export default AppRoutes;
