import { useEffect, useState, useMemo } from "react";
import { Link } from "react-router-dom";
import { MapPin, Filter } from "lucide-react";
import LoadingSpinner from "../../dashboard/_components/loadingSpinner";
import { getAllAssets } from "@/api/assets/assetsApi";

const CustomerAssets = () => {
  const [assets, setAssets] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [filter, setFilter] = useState("all");

  useEffect(() => {
    const fetchAssets = async () => {
      try {
        setLoading(true);
        setError("");
        const data = await getAllAssets();
        setAssets(data);
      } catch (error) {
        console.error("Error fetching assets:", error);
        setError("Failed to load assets. Please try again.");
      } finally {
        setLoading(false);
      }
    };

    fetchAssets();
  }, []);

  // Memoized filter function for better performance
  const filteredAssets = useMemo(() => {
    if (filter === "all") return assets;

    return assets.filter((asset) => {
      const typeMap = {
        rent: "Rent",
        sale: "Sale",
      };
      return asset.listingTypeName === typeMap[filter];
    });
  }, [assets, filter]);

  // Display loading state
  if (loading) {
    return <LoadingSpinner message="Loading available assets..." />;
  }

  // Display error state
  if (error) {
    return (
      <div
        className="bg-red-50 text-red-600 p-4 rounded-lg text-center"
        role="alert"
      >
        {error}
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Header Section */}
      <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <div>
          <h1 className="text-2xl font-bold text-gray-800">Available Assets</h1>
          <p className="text-sm text-gray-500 mt-1">
            Browse vehicles available for rent or purchase
          </p>
        </div>

        {/* Filter Controls */}
        <div className="flex items-center gap-2">
          <Filter className="w-4 h-4 text-gray-400" aria-hidden="true" />
          <select
            value={filter}
            onChange={(e) => setFilter(e.target.value)}
            className="border border-gray-200 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-black focus:border-transparent transition"
            aria-label="Filter assets by listing type"
          >
            <option value="all">All Assets</option>
            <option value="rent">For Rent</option>
            <option value="sale">For Sale</option>
          </select>
        </div>
      </div>

      {/* Assets Grid */}
      {filteredAssets.length === 0 ? (
        <div className="bg-white rounded-xl shadow-sm p-12 text-center border border-gray-100">
          <p className="text-gray-500">
            {assets.length === 0
              ? "No assets available at the moment."
              : "No assets match your current filter."}
          </p>
          {assets.length > 0 && (
            <button
              onClick={() => setFilter("all")}
              className="mt-4 text-sm text-blue-600 hover:text-blue-800 font-medium"
            >
              Clear filter
            </button>
          )}
        </div>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
          {filteredAssets.map((asset) => (
            <div
              key={asset.assetID}
              className="bg-white rounded-xl shadow-sm p-5 border border-gray-100 hover:shadow-md transition-shadow duration-200 flex flex-col"
            >
              {/* Header */}
              <div className="flex justify-between items-start gap-2">
                <h3 className="font-semibold text-gray-800 truncate flex-1">
                  {asset.title}
                </h3>
                <span
                  className={`text-xs px-2.5 py-1 rounded-full font-medium whitespace-nowrap ${
                    asset.listingTypeName === "Rent"
                      ? "bg-blue-100 text-blue-700"
                      : "bg-green-100 text-green-700"
                  }`}
                >
                  {asset.listingTypeName === "Rent" ? "For Rent" : "For Sale"}
                </span>
              </div>

              {/* Category */}
              <p className="text-sm text-gray-500 mt-1">{asset.categoryName}</p>

              {/* Location */}
              <div className="flex items-center gap-1 text-sm text-gray-500 mt-2">
                <MapPin
                  className="w-3.5 h-3.5 flex-shrink-0"
                  aria-hidden="true"
                />
                <span className="truncate">{asset.location}</span>
              </div>

              {/* Pricing */}
              <div className="mt-3 pt-3 border-t border-gray-100">
                {asset.listingTypeName === "Rent" && asset.dailyRate && (
                  <p className="text-sm font-semibold text-blue-600">
                    ₱{asset.dailyRate.toLocaleString()}{" "}
                    <span className="font-normal text-gray-500">/ day</span>
                  </p>
                )}
                {asset.listingTypeName === "Sale" && asset.salePrice && (
                  <p className="text-sm font-semibold text-green-600">
                    ₱{asset.salePrice.toLocaleString()}
                  </p>
                )}
              </div>

              {/* Action Button */}
              <Link
                to={`/assets/${asset.assetID}`}
                className="inline-flex items-center justify-center w-full mt-4 text-sm text-blue-600 hover:text-blue-800 font-medium bg-blue-50 hover:bg-blue-100 py-2.5 rounded-lg transition-colors duration-200"
              >
                View Details
                <span className="ml-1" aria-hidden="true">
                  →
                </span>
              </Link>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default CustomerAssets;
