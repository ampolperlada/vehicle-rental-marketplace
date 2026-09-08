import { Navigate } from "react-router-dom";
import Login from "@/modules/auth/Login";
import ProtectedRoute from "./outlet/protectedRoute";
import Layout from "./outlet/layout";
import Dashboard from "@/modules/pages/dashboard";
import { ROUTES } from "./constants/route.";
import Assets from "@/modules/pages/assets";

const routes = [
  // Public
  { path: "/login", element: <Login /> },

  // Iisang dashboard route
  {
    element: <ProtectedRoute />,
    children: [
      {
        element: <Layout />,
        children: [
          { path: ROUTES.DASHBOARD, element: <Dashboard /> },
          { path: ROUTES.ASSETS, element: <Assets /> },
        ],
      },
    ],
  },

  // Redirects
  { path: "/", element: <Navigate to={ROUTES.DASHBOARD} replace /> },
  { path: "*", element: <Navigate to={ROUTES.DASHBOARD} replace /> },
];

export default routes;
