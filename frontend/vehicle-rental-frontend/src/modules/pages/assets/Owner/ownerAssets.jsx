import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import {
  PlusCircle,
  Edit,
  Trash2,
  RotateCcw,
  ShoppingBag,
  List,
} from "lucide-react";

import LoadingSpinner from "../../dashboard/_components/loadingSpinner";
import CreateAssetModal from "./_components/modal/createModal";
import {
  getMyAssets,
  getAllAssets,
  deleteAsset,
  restoreAsset,
} from "@/api/assets/assetsApi";

const OwnerAssets = () => {
  const [myAssets, setMyAssets] = useState([]);
  const [availableAssets, setAvailableAssets] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [activeTab, setActiveTab] = useState("my-listings");
  const [isModalOpen, setIsModalOpen] = useState(false);

  useEffect(() => {
    const fetchAssets = async () => {
      try {
        setLoading(true);
        setError("");

        const [myData, allData] = await Promise.all([
          getMyAssets(),
          getAllAssets(),
        ]);

        setMyAssets(myData);
        setAvailableAssets(allData);
      } catch (error) {
        console.error("Error fetching assets:", error);
        setError("Failed to load assets. Please try again.");
      } finally {
        setLoading(false);
      }
    };

    fetchAssets();
  }, []);

  const handleDelete = async (id) => {
    if (!confirm("Are you sure you want to delete this asset?")) return;

    try {
      await deleteAsset(id);
      // Refresh data manually
      const [myData, allData] = await Promise.all([
        getMyAssets(),
        getAllAssets(),
      ]);
      setMyAssets(myData);
      setAvailableAssets(allData);
    } catch (error) {
      console.error("Error deleting asset:", error);
      alert("Failed to delete asset. Please try again.");
    }
  };

  const handleRestore = async (id) => {
    try {
      await restoreAsset(id);
      const [myData, allData] = await Promise.all([
        getMyAssets(),
        getAllAssets(),
      ]);
      setMyAssets(myData);
      setAvailableAssets(allData);
    } catch (error) {
      console.error("Error restoring asset:", error);
      alert("Failed to restore asset. Please try again.");
    }
  };

  if (loading) return <LoadingSpinner message="Loading your assets..." />;

  if (error) {
    return (
      <div className="bg-red-50 text-red-600 p-6 rounded-xl text-center border border-red-200">
        <p className="font-medium">{error}</p>
      </div>
    );
  }

  const currentAssets =
    activeTab === "my-listings" ? myAssets : availableAssets;

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <div>
          <h1 className="text-2xl font-bold text-gray-800">Asset Dashboard</h1>
          <p className="text-sm text-gray-500 mt-1">
            Manage your listings or browse available assets
          </p>
        </div>
        <button
          onClick={() => setIsModalOpen(true)}
          className="inline-flex items-center gap-2 bg-black text-white px-5 py-2.5 rounded-lg hover:bg-gray-800 transition-colors text-sm font-medium shadow-sm hover:shadow-md"
        >
          <PlusCircle className="w-4 h-4" />
          Post New Asset
        </button>
      </div>

      {/* Tabs */}
      <div className="flex gap-2 border-b border-gray-200">
        <button
          onClick={() => setActiveTab("my-listings")}
          className={`flex items-center gap-2 px-4 py-2.5 text-sm font-medium border-b-2 transition-colors ${
            activeTab === "my-listings"
              ? "border-black text-black"
              : "border-transparent text-gray-500 hover:text-gray-700"
          }`}
        >
          <List className="w-4 h-4" />
          My Listings ({myAssets.length})
        </button>
        <button
          onClick={() => setActiveTab("available")}
          className={`flex items-center gap-2 px-4 py-2.5 text-sm font-medium border-b-2 transition-colors ${
            activeTab === "available"
              ? "border-black text-black"
              : "border-transparent text-gray-500 hover:text-gray-700"
          }`}
        >
          <ShoppingBag className="w-4 h-4" />
          Available for Booking ({availableAssets.length})
        </button>
      </div>

      {/* Empty State */}
      {currentAssets.length === 0 ? (
        <div className="bg-white rounded-xl shadow-sm p-16 text-center border border-gray-200">
          <div className="max-w-sm mx-auto">
            <div className="w-16 h-16 bg-gray-100 rounded-full flex items-center justify-center mx-auto mb-4">
              {activeTab === "my-listings" ? (
                <PlusCircle className="w-8 h-8 text-gray-400" />
              ) : (
                <ShoppingBag className="w-8 h-8 text-gray-400" />
              )}
            </div>
            <h3 className="text-lg font-semibold text-gray-800 mb-2">
              {activeTab === "my-listings"
                ? "No assets posted yet"
                : "No assets available for booking"}
            </h3>
            <p className="text-gray-500 text-sm mb-6">
              {activeTab === "my-listings"
                ? "Start listing your vehicles for rent or sale."
                : "Check back later for available assets to book."}
            </p>
            {activeTab === "my-listings" && (
              <button
                onClick={() => setIsModalOpen(true)}
                className="inline-flex items-center gap-2 bg-black text-white px-6 py-2.5 rounded-lg hover:bg-gray-800 transition-colors text-sm font-medium"
              >
                <PlusCircle className="w-4 h-4" />
                Post Your First Asset
              </button>
            )}
          </div>
        </div>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-5">
          {currentAssets.map((asset) => (
            <div
              key={asset.assetID}
              className="bg-white rounded-xl shadow-sm p-5 border border-gray-200 hover:shadow-md transition-shadow duration-200 flex flex-col"
            >
              {/* Title and Status */}
              <div className="flex justify-between items-start gap-2">
                <h3 className="font-semibold text-gray-800 truncate flex-1">
                  {asset.title}
                </h3>
                {activeTab === "my-listings" && (
                  <span
                    className={`text-xs px-2.5 py-1 rounded-full font-medium whitespace-nowrap ${
                      asset.isActive
                        ? "bg-green-100 text-green-700"
                        : "bg-red-100 text-red-700"
                    }`}
                  >
                    {asset.isActive ? "Active" : "Deleted"}
                  </span>
                )}
                {activeTab === "available" && (
                  <span
                    className={`text-xs px-2.5 py-1 rounded-full font-medium whitespace-nowrap ${
                      asset.listingTypeName === "Rent"
                        ? "bg-blue-100 text-blue-700"
                        : "bg-purple-100 text-purple-700"
                    }`}
                  >
                    {asset.listingTypeName === "Rent" ? "For Rent" : "For Sale"}
                  </span>
                )}
              </div>

              {/* Location */}
              <p className="text-sm text-gray-500 mt-2">
                {asset.location || "No location set"}
              </p>

              {/* Category & Listing Type */}
              <div className="flex flex-wrap gap-2 mt-2">
                {asset.categoryName && (
                  <span className="text-xs bg-gray-100 text-gray-700 px-2.5 py-0.5 rounded-full">
                    {asset.categoryName}
                  </span>
                )}
                {activeTab === "available" && asset.listingTypeName && (
                  <span
                    className={`text-xs px-2.5 py-0.5 rounded-full font-medium ${
                      asset.listingTypeName === "Rent"
                        ? "bg-blue-100 text-blue-700"
                        : "bg-purple-100 text-purple-700"
                    }`}
                  >
                    {asset.listingTypeName === "Rent" ? "For Rent" : "For Sale"}
                  </span>
                )}
              </div>

              {/* Pricing */}
              {asset.listingTypeName && (
                <div className="mt-3 pt-3 border-t border-gray-100">
                  {asset.listingTypeName === "Rent" && asset.dailyRate && (
                    <p className="text-sm font-semibold text-blue-600">
                      ₱{asset.dailyRate.toLocaleString()}{" "}
                      <span className="font-normal text-gray-500">/ day</span>
                    </p>
                  )}
                  {asset.listingTypeName === "Sale" && asset.salePrice && (
                    <p className="text-sm font-semibold text-purple-600">
                      ₱{asset.salePrice.toLocaleString()}
                    </p>
                  )}
                </div>
              )}

              {/* Action Buttons */}
              <div className="flex gap-2 mt-4 pt-3 border-t border-gray-100">
                {activeTab === "my-listings" ? (
                  <>
                    <Link
                      to={`/assets/edit/${asset.assetID}`}
                      className="flex-1 inline-flex items-center justify-center gap-1.5 text-sm text-blue-600 hover:text-blue-800 font-medium bg-blue-50 hover:bg-blue-100 py-2 rounded-lg transition-colors"
                      title="Edit"
                    >
                      <Edit className="w-4 h-4" />
                      Edit
                    </Link>

                    {asset.isActive ? (
                      <button
                        onClick={() => handleDelete(asset.assetID)}
                        className="flex-1 inline-flex items-center justify-center gap-1.5 text-sm text-red-600 hover:text-red-800 font-medium bg-red-50 hover:bg-red-100 py-2 rounded-lg transition-colors"
                        title="Delete"
                      >
                        <Trash2 className="w-4 h-4" />
                        Delete
                      </button>
                    ) : (
                      <button
                        onClick={() => handleRestore(asset.assetID)}
                        className="flex-1 inline-flex items-center justify-center gap-1.5 text-sm text-green-600 hover:text-green-800 font-medium bg-green-50 hover:bg-green-100 py-2 rounded-lg transition-colors"
                        title="Restore"
                      >
                        <RotateCcw className="w-4 h-4" />
                        Restore
                      </button>
                    )}
                  </>
                ) : (
                  <Link
                    to={`/assets/${asset.assetID}`}
                    className="w-full inline-flex items-center justify-center gap-1.5 text-sm text-blue-600 hover:text-blue-800 font-medium bg-blue-50 hover:bg-blue-100 py-2 rounded-lg transition-colors"
                  >
                    <ShoppingBag className="w-4 h-4" />
                    View & Book
                  </Link>
                )}
              </div>
            </div>
          ))}
        </div>
      )}

      {/* Create Asset Modal */}
      <CreateAssetModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onSuccess={() => {
          const refreshData = async () => {
            const [myData, allData] = await Promise.all([
              getMyAssets(),
              getAllAssets(),
            ]);
            setMyAssets(myData);
            setAvailableAssets(allData);
          };
          refreshData();
        }}
      />
    </div>
  );
};

export default OwnerAssets;
