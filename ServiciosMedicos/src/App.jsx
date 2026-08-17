import { BrowserRouter, useLocation } from "react-router-dom";

import Header from "./components/Header";
import AppRoutes from "./routes/App.Routes";

function isAuthenticated() {
  return Boolean(sessionStorage.getItem('token'));
}

function AppContent() {
  const location = useLocation();
  const mostrarHeader = location.pathname !== "/login" && isAuthenticated();

  if (!mostrarHeader) {
    return <AppRoutes />;
  }

  return (
    <Header>
      <AppRoutes />
    </Header>
  );
}

function App() {
  return (
    <BrowserRouter>
      <AppContent />
    </BrowserRouter>
  );
}

export default App;
