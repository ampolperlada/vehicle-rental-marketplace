import { Navigate, Outlet } from "react-router-dom";
import {
  isAuthenticated,
  getUser,
} from "../../modules/auth/_components/api/authApi";

const ProtectedRoute = ({ allowedRoles }) => {
  const authenticated = isAuthenticated();
  const user = getUser();

  // If not authenticated, redirect to login
  if (!authenticated) {
    return <Navigate to="/login" replace />;
  }

  // If roles are specified, check if user has the required role
  if (allowedRoles && !allowedRoles.includes(user?.role)) {
    return <Navigate to="/dashboard" replace />;
  }

  // Render child routes
  return <Outlet />;
};

export default ProtectedRoute;
