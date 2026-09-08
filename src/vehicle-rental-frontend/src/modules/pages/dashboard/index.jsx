import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { getUser } from "@/modules/auth/_components/api/authApi";
import {
  Car,
  Calendar,
  Users,
  DollarSign,
  TrendingUp,
  Clock,
  PlusCircle,
  ListChecks,
  BarChart3,
  BookOpen,
  LayoutDashboard,
} from "lucide-react";

const Dashboard = () => {
  const user = getUser();
  const isAdmin = user?.role === "Admin";
  const [loading, setLoading] = useState(true);
  const [stats, setStats] = useState({
    totalAssets: 0,
    totalBookings: 0,
    pendingBookings: 0,
    totalRevenue: 0,
    totalUsers: 0,
    activeRentals: 0,
  });

  useEffect(() => {
    const timer = setTimeout(() => {
      setStats({
        totalAssets: 12,
        totalBookings: 45,
        pendingBookings: 5,
        totalRevenue: 125000,
        totalUsers: 8,
        activeRentals: 3,
      });
      setLoading(false);
    }, 1000);

    return () => clearTimeout(timer);
  }, []);

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-gray-900 mx-auto"></div>
          <p className="mt-4 text-gray-600">Loading dashboard...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Welcome Section */}
      <div className="rounded-2xl shadow-lg p-6 text-white">
        <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
          <h1 className="text-2xl md:text-3xl font-bold">
            Welcome back, {user?.username}!
          </h1>
          <span
            className={`px-4 py-2 rounded-full text-sm font-medium ${
              isAdmin ? "bg-purple-600 text-white" : "bg-blue-600 text-white"
            }`}
          >
            {isAdmin ? "🔑 Admin" : "👤 Customer"}
          </span>
        </div>
      </div>

      {/* Stats Grid */}
      {isAdmin ? (
        <div className="grid grid-cols-2 lg:grid-cols-4 gap-3 md:gap-4">
          <div className="bg-white rounded-xl shadow-sm p-4 md:p-6 border border-gray-100 hover:shadow-md transition">
            <div className="flex items-center gap-3">
              <div className="p-2 md:p-3 bg-blue-50 rounded-lg">
                <Car className="w-5 h-5 md:w-6 md:h-6 text-blue-600" />
              </div>
              <div>
                <p className="text-xs md:text-sm text-gray-500">Total Assets</p>
                <p className="text-xl md:text-2xl font-bold">
                  {stats.totalAssets}
                </p>
              </div>
            </div>
          </div>
          <div className="bg-white rounded-xl shadow-sm p-4 md:p-6 border border-gray-100 hover:shadow-md transition">
            <div className="flex items-center gap-3">
              <div className="p-2 md:p-3 bg-green-50 rounded-lg">
                <Calendar className="w-5 h-5 md:w-6 md:h-6 text-green-600" />
              </div>
              <div>
                <p className="text-xs md:text-sm text-gray-500">
                  Total Bookings
                </p>
                <p className="text-xl md:text-2xl font-bold">
                  {stats.totalBookings}
                </p>
              </div>
            </div>
          </div>
          <div className="bg-white rounded-xl shadow-sm p-4 md:p-6 border border-gray-100 hover:shadow-md transition">
            <div className="flex items-center gap-3">
              <div className="p-2 md:p-3 bg-yellow-50 rounded-lg">
                <Clock className="w-5 h-5 md:w-6 md:h-6 text-yellow-600" />
              </div>
              <div>
                <p className="text-xs md:text-sm text-gray-500">
                  Pending Bookings
                </p>
                <p className="text-xl md:text-2xl font-bold">
                  {stats.pendingBookings}
                </p>
              </div>
            </div>
          </div>
          <div className="bg-white rounded-xl shadow-sm p-4 md:p-6 border border-gray-100 hover:shadow-md transition">
            <div className="flex items-center gap-3">
              <div className="p-2 md:p-3 bg-purple-50 rounded-lg">
                <DollarSign className="w-5 h-5 md:w-6 md:h-6 text-purple-600" />
              </div>
              <div>
                <p className="text-xs md:text-sm text-gray-500">
                  Total Revenue
                </p>
                <p className="text-xl md:text-2xl font-bold">
                  ₱{stats.totalRevenue.toLocaleString()}
                </p>
              </div>
            </div>
          </div>
        </div>
      ) : (
        <div className="grid grid-cols-2 md:grid-cols-3 gap-3 md:gap-4">
          <div className="bg-white rounded-xl shadow-sm p-4 md:p-6 border border-gray-100 hover:shadow-md transition">
            <div className="flex items-center gap-3">
              <div className="p-2 md:p-3 bg-blue-50 rounded-lg">
                <Calendar className="w-5 h-5 md:w-6 md:h-6 text-blue-600" />
              </div>
              <div>
                <p className="text-xs md:text-sm text-gray-500">My Bookings</p>
                <p className="text-xl md:text-2xl font-bold">
                  {stats.totalBookings}
                </p>
              </div>
            </div>
          </div>
          <div className="bg-white rounded-xl shadow-sm p-4 md:p-6 border border-gray-100 hover:shadow-md transition">
            <div className="flex items-center gap-3">
              <div className="p-2 md:p-3 bg-green-50 rounded-lg">
                <Car className="w-5 h-5 md:w-6 md:h-6 text-green-600" />
              </div>
              <div>
                <p className="text-xs md:text-sm text-gray-500">
                  Active Rentals
                </p>
                <p className="text-xl md:text-2xl font-bold">2</p>
              </div>
            </div>
          </div>
          <div className="bg-white rounded-xl shadow-sm p-4 md:p-6 border border-gray-100 hover:shadow-md transition">
            <div className="flex items-center gap-3">
              <div className="p-2 md:p-3 bg-yellow-50 rounded-lg">
                <TrendingUp className="w-5 h-5 md:w-6 md:h-6 text-yellow-600" />
              </div>
              <div>
                <p className="text-xs md:text-sm text-gray-500">Total Spent</p>
                <p className="text-xl md:text-2xl font-bold">₱15,000</p>
              </div>
            </div>
          </div>
        </div>
      )}

      {/* Quick Actions & Activity */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <div className="bg-white rounded-xl shadow-sm p-6 border border-gray-100">
          <h3 className="text-lg font-semibold mb-4">⚡ Quick Actions</h3>
          <div className="grid grid-cols-2 gap-3">
            {isAdmin ? (
              <>
                <Link
                  to="/assets/create"
                  className="flex items-center gap-2 p-3 bg-gray-50 rounded-lg hover:bg-gray-100 transition"
                >
                  <PlusCircle className="w-5 h-5 text-blue-600" />
                  <span className="text-sm font-medium">Add Asset</span>
                </Link>
                <Link
                  to="/admin/users"
                  className="flex items-center gap-2 p-3 bg-gray-50 rounded-lg hover:bg-gray-100 transition"
                >
                  <Users className="w-5 h-5 text-purple-600" />
                  <span className="text-sm font-medium">Manage Users</span>
                </Link>
                <Link
                  to="/admin/bookings"
                  className="flex items-center gap-2 p-3 bg-gray-50 rounded-lg hover:bg-gray-100 transition"
                >
                  <ListChecks className="w-5 h-5 text-green-600" />
                  <span className="text-sm font-medium">Manage Bookings</span>
                </Link>
                <Link
                  to="/admin/reports"
                  className="flex items-center gap-2 p-3 bg-gray-50 rounded-lg hover:bg-gray-100 transition"
                >
                  <BarChart3 className="w-5 h-5 text-orange-600" />
                  <span className="text-sm font-medium">Reports</span>
                </Link>
              </>
            ) : (
              <>
                <Link
                  to="/assets"
                  className="flex items-center gap-2 p-3 bg-gray-50 rounded-lg hover:bg-gray-100 transition"
                >
                  <Car className="w-5 h-5 text-blue-600" />
                  <span className="text-sm font-medium">Browse Assets</span>
                </Link>
                <Link
                  to="/bookings"
                  className="flex items-center gap-2 p-3 bg-gray-50 rounded-lg hover:bg-gray-100 transition"
                >
                  <BookOpen className="w-5 h-5 text-green-600" />
                  <span className="text-sm font-medium">My Bookings</span>
                </Link>
                <Link
                  to="/dashboard"
                  className="flex items-center gap-2 p-3 bg-gray-50 rounded-lg hover:bg-gray-100 transition"
                >
                  <LayoutDashboard className="w-5 h-5 text-gray-600" />
                  <span className="text-sm font-medium">Dashboard</span>
                </Link>
              </>
            )}
          </div>
        </div>

        <div className="bg-white rounded-xl shadow-sm p-6 border border-gray-100">
          <h3 className="text-lg font-semibold mb-4">📋 Recent Activity</h3>
          <div className="space-y-3">
            <div className="flex items-center gap-3 p-2 rounded-lg hover:bg-gray-50 transition">
              <div className="w-2 h-2 bg-green-500 rounded-full flex-shrink-0"></div>
              <div className="flex-1 min-w-0">
                <p className="text-sm font-medium truncate">
                  New booking #1005
                </p>
                <p className="text-xs text-gray-500">2 hours ago</p>
              </div>
            </div>
            <div className="flex items-center gap-3 p-2 rounded-lg hover:bg-gray-50 transition">
              <div className="w-2 h-2 bg-blue-500 rounded-full flex-shrink-0"></div>
              <div className="flex-1 min-w-0">
                <p className="text-sm font-medium truncate">
                  Asset "Toyota Vios" approved
                </p>
                <p className="text-xs text-gray-500">5 hours ago</p>
              </div>
            </div>
            <div className="flex items-center gap-3 p-2 rounded-lg hover:bg-gray-50 transition">
              <div className="w-2 h-2 bg-yellow-500 rounded-full flex-shrink-0"></div>
              <div className="flex-1 min-w-0">
                <p className="text-sm font-medium truncate">
                  Payment received ₱6,000
                </p>
                <p className="text-xs text-gray-500">1 day ago</p>
              </div>
            </div>
            <div className="flex items-center gap-3 p-2 rounded-lg hover:bg-gray-50 transition">
              <div className="w-2 h-2 bg-red-500 rounded-full flex-shrink-0"></div>
              <div className="flex-1 min-w-0">
                <p className="text-sm font-medium truncate">
                  Booking #1003 cancelled
                </p>
                <p className="text-xs text-gray-500">2 days ago</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
