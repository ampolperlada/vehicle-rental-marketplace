import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import {
  Car,
  Calendar,
  PlusCircle,
  ListChecks,
  TrendingUp,
  DollarSign,
} from "lucide-react";
import LoadingSpinner from "../_components/loadingSpinner";
import { getUser } from "@/modules/auth/_components/api/authApi";
import { getMyAssetBookings } from "@/api/bookings/bookingsApi";
import { getMyAssets } from "@/api/assets/assetsApi";

const AdminDashboard = () => {
  const user = getUser();
  const [loading, setLoading] = useState(true);
  const [stats, setStats] = useState({
    totalAssets: 0,
    totalBookings: 0,
    pendingBookings: 0,
    totalRevenue: 0,
    availableAssets: 0,
  });
  const [recentBookings, setRecentBookings] = useState([]);

  useEffect(() => {
    const fetchDashboardData = async () => {
      try {
        setLoading(true);

        const [assets, bookings] = await Promise.all([
          getMyAssets(),
          getMyAssetBookings(),
        ]);

        const totalAssets = assets.length || 0;
        const availableAssets = assets.filter((a) => a.isAvailable).length || 0;
        const totalBookings = bookings.length || 0;
        const pendingBookings =
          bookings.filter((b) => b.status === "Pending").length || 0;
        const totalRevenue = bookings
          .filter((b) => b.status === "FullyPaid")
          .reduce((sum, b) => sum + b.totalRentalPrice, 0);

        setStats({
          totalAssets,
          totalBookings,
          pendingBookings,
          totalRevenue,
          availableAssets,
        });

        setRecentBookings(bookings.slice(0, 5));
      } catch (error) {
        console.error("Error fetching dashboard:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchDashboardData();
  }, []);

  if (loading) {
    return <LoadingSpinner message="Loading dashboard..." />;
  }

  return (
    <div className="space-y-6">
      <div className="rounded-2xl shadow-lg p-6 md:p-8">
        <h1 className="text-2xl md:text-3xl font-bold">
          Welcome back, {user?.username}! 👋
        </h1>
      </div>

      {/* Stats Grid */}
      <div className="grid grid-cols-2 lg:grid-cols-4 gap-3 md:gap-4">
        <div className="bg-white rounded-xl shadow-sm p-4 md:p-6 border border-gray-100">
          <div className="flex items-center gap-3">
            <div className="p-2 md:p-3 bg-blue-50 rounded-lg">
              <Car className="w-5 h-5 md:w-6 md:h-6 text-blue-600" />
            </div>
            <div>
              <p className="text-xs md:text-sm text-gray-500">Posted Assets</p>
              <p className="text-xl md:text-2xl font-bold">
                {stats.totalAssets}
              </p>
            </div>
          </div>
        </div>

        <div className="bg-white rounded-xl shadow-sm p-4 md:p-6 border border-gray-100">
          <div className="flex items-center gap-3">
            <div className="p-2 md:p-3 bg-green-50 rounded-lg">
              <Calendar className="w-5 h-5 md:w-6 md:h-6 text-green-600" />
            </div>
            <div>
              <p className="text-xs md:text-sm text-gray-500">Total Bookings</p>
              <p className="text-xl md:text-2xl font-bold">
                {stats.totalBookings}
              </p>
            </div>
          </div>
        </div>

        <div className="bg-white rounded-xl shadow-sm p-4 md:p-6 border border-gray-100">
          <div className="flex items-center gap-3">
            <div className="p-2 md:p-3 bg-yellow-50 rounded-lg">
              <TrendingUp className="w-5 h-5 md:w-6 md:h-6 text-yellow-600" />
            </div>
            <div>
              <p className="text-xs md:text-sm text-gray-500">
                Available Assets
              </p>
              <p className="text-xl md:text-2xl font-bold">
                {stats.availableAssets}
              </p>
            </div>
          </div>
        </div>

        <div className="bg-white rounded-xl shadow-sm p-4 md:p-6 border border-gray-100">
          <div className="flex items-center gap-3">
            <div className="p-2 md:p-3 bg-purple-50 rounded-lg">
              <DollarSign className="w-5 h-5 md:w-6 md:h-6 text-purple-600" />
            </div>
            <div>
              <p className="text-xs md:text-sm text-gray-500">Total Revenue</p>
              <p className="text-xl md:text-2xl font-bold">
                ₱{stats.totalRevenue.toLocaleString()}
              </p>
            </div>
          </div>
        </div>
      </div>

      {/* Quick Actions */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <div className="bg-white rounded-xl shadow-sm p-6 border border-gray-100">
          <h3 className="text-lg font-semibold mb-4">⚡ Quick Actions</h3>
          <div className="grid grid-cols-2 gap-3">
            <Link
              to="/assets/create"
              className="flex items-center gap-2 p-3 bg-gray-50 rounded-lg hover:bg-gray-100 transition"
            >
              <PlusCircle className="w-5 h-5 text-blue-600" />
              <span className="text-sm font-medium">Post New Asset</span>
            </Link>
            <Link
              to="/admin/bookings"
              className="flex items-center gap-2 p-3 bg-gray-50 rounded-lg hover:bg-gray-100 transition"
            >
              <ListChecks className="w-5 h-5 text-green-600" />
              <span className="text-sm font-medium">Manage Bookings</span>
            </Link>
          </div>
        </div>

        <div className="bg-white rounded-xl shadow-sm p-6 border border-gray-100">
          <h3 className="text-lg font-semibold mb-4">📋 Recent Bookings</h3>
          {recentBookings.length > 0 ? (
            <div className="space-y-3">
              {recentBookings.map((booking) => (
                <div
                  key={booking.bookingID}
                  className="flex items-center justify-between p-2 rounded-lg hover:bg-gray-50 transition"
                >
                  <div className="flex items-center gap-3">
                    <div
                      className={`w-2 h-2 rounded-full flex-shrink-0 ${
                        booking.status === "FullyPaid"
                          ? "bg-green-500"
                          : booking.status === "Pending"
                            ? "bg-yellow-500"
                            : "bg-red-500"
                      }`}
                    ></div>
                    <div>
                      <p className="text-sm font-medium truncate">
                        {booking.assetTitle}
                      </p>
                      <p className="text-xs text-gray-500">
                        {new Date(booking.createdAt).toLocaleDateString()}
                      </p>
                    </div>
                  </div>
                  <span
                    className={`text-xs px-2 py-1 rounded-full ${
                      booking.status === "FullyPaid"
                        ? "bg-green-100 text-green-700"
                        : booking.status === "Pending"
                          ? "bg-yellow-100 text-yellow-700"
                          : "bg-red-100 text-red-700"
                    }`}
                  >
                    {booking.status}
                  </span>
                </div>
              ))}
            </div>
          ) : (
            <p className="text-gray-500 text-sm">No bookings yet.</p>
          )}
        </div>
      </div>
    </div>
  );
};

export default AdminDashboard;
