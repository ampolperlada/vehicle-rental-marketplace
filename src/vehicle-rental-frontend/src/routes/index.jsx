import { Navigate } from "react-router-dom";
import Login from "@/modules/auth/Login";
import Dashboard from "@/modules/pages/dashboard";
import ProtectedRoute from "./outlet/protectedRoute";
import Layout from "./outlet/layout";

const routes = [
  // PUBLIC ROUTES
  {
    path: "/login",
    element: <Login />,
  },

  // PROTECTED ROUTES
  {
    element: <ProtectedRoute />,
    children: [
      {
        element: <Layout />,
        children: [{ path: "/dashboard", element: <Dashboard /> }],
      },
    ],
  },

  // REDIRECTS
  {
    path: "/",
    element: <Navigate to="/dashboard" replace />,
  },
  {
    path: "*",
    element: <Navigate to="/dashboard" replace />,
  },
];

export default routes;
