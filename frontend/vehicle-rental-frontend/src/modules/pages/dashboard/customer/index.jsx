import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { Car, Calendar, TrendingUp, BookOpen } from "lucide-react";
import { getUser } from "@/modules/auth/_components/api/authApi";
import LoadingSpinner from "../_components/loadingSpinner";

const CustomerDashboard = () => {
  const user = getUser();
  const [loading, setLoading] = useState(true);
  const [stats, setStats] = useState({
    myBookings: 0,
    activeRentals: 0,
    totalSpent: 0,
  });

  useEffect(() => {
    const timer = setTimeout(() => {
      setStats({
        myBookings: 8,
        activeRentals: 2,
        totalSpent: 15000,
      });
      setLoading(false);
    }, 1000);

    return () => clearTimeout(timer);
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

      {/* Customer Stats */}
      <div className="grid grid-cols-2 md:grid-cols-3 gap-3 md:gap-4">
        <div className="bg-white rounded-xl shadow-sm p-4 md:p-6 border border-gray-100">
          <div className="flex items-center gap-3">
            <div className="p-2 md:p-3 bg-blue-50 rounded-lg">
              <Calendar className="w-5 h-5 md:w-6 md:h-6 text-blue-600" />
            </div>
            <div>
              <p className="text-xs md:text-sm text-gray-500">My Bookings</p>
              <p className="text-xl md:text-2xl font-bold">
                {stats.myBookings}
              </p>
            </div>
          </div>
        </div>

        <div className="bg-white rounded-xl shadow-sm p-4 md:p-6 border border-gray-100">
          <div className="flex items-center gap-3">
            <div className="p-2 md:p-3 bg-green-50 rounded-lg">
              <Car className="w-5 h-5 md:w-6 md:h-6 text-green-600" />
            </div>
            <div>
              <p className="text-xs md:text-sm text-gray-500">Active Rentals</p>
              <p className="text-xl md:text-2xl font-bold">
                {stats.activeRentals}
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
              <p className="text-xs md:text-sm text-gray-500">Total Spent</p>
              <p className="text-xl md:text-2xl font-bold">
                ₱{stats.totalSpent.toLocaleString()}
              </p>
            </div>
          </div>
        </div>
      </div>

      {/* Quick Actions */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <div className="bg-white rounded-xl shadow-sm p-6 border border-gray-100">
          <h3 className="text-lg font-semibold mb-4">⚡ Quick Actions</h3>
          <div className="grid grid-cols-2 gap-3">
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
              <div className="w-2 h-2 bg-yellow-500 rounded-full flex-shrink-0"></div>
              <div className="flex-1 min-w-0">
                <p className="text-sm font-medium truncate">
                  Payment received ₱6,000
                </p>
                <p className="text-xs text-gray-500">1 day ago</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CustomerDashboard;
