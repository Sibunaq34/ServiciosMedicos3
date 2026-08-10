import { BrowserRouter, Routes, Route } from "react-router-dom";

import Header from "./layouts/Header";

import Inicio from "./pages/Index";
import Puestos from "./pages/Puestos";

function App() {
  return (
  <BrowserRouter>
    <Header />
      <main>
        <Routes>
          <Route path="/" element={<Inicio />}/>
          <Route path="/puestos" element={<Puestos />}/>
        </Routes>
      </main>
  </BrowserRouter>
    );
}

export default App;