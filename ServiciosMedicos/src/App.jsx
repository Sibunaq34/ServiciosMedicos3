import { BrowserRouter, useLocation } from "react-router-dom";

import Header from "./components/Header";
import Sidebar from "./components/Sidebar";
import Footer from "./components/Footer";
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
    <div className="app-layout">
      <Sidebar />
      <div className="app-content">
        <Header />
        <main className="app-main">
          <AppRoutes />
        </main>
        <Footer />
      </div>
    </div>
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
