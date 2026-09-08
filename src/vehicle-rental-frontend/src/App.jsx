import { BrowserRouter, useRoutes } from "react-router-dom";
import routes from "@/routes";

function AppRoutes() {
  return useRoutes(routes);
}

function App() {
  return (
<<<<<<< HEAD
    <Router>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/" element={<Navigate to="/login" replace />} />
      <Route path="/" element={<Dashboard/>} />
      </Routes>
    </Router>
=======
    <BrowserRouter>
      <AppRoutes />
    </BrowserRouter>
>>>>>>> 0bc83c56409e6571356f7566f0d73f2acaff06ff
  );
}

export default App;
