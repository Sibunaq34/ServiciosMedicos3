import { BrowserRouter } from "react-router-dom";

import Header from "./components/Header";
import AppRoutes from "./routes/App.Routes";

function App() {
  return (
    <BrowserRouter>
      <Header>
        <AppRoutes />
      </Header>
    </BrowserRouter>
  );
}

export default App;