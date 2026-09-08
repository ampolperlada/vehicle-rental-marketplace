import { getUser } from "@/modules/auth/_components/api/authApi";
import AdminDashboard from "./admin";
import CustomerDashboard from "./customer";

const Dashboard = () => {
  const user = getUser();

  if (user) {
    return <AdminDashboard />;
  }

  return <CustomerDashboard />;
};

export default Dashboard;
