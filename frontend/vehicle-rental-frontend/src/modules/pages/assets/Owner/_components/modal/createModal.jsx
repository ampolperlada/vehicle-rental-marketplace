import { useState, useEffect } from "react";
import { X } from "lucide-react";
import { createAsset } from "@/api/assets/assetsApi";
import { getCategories } from "@/api/categories-&-listing/categoriesApi";
import { getListingTypes } from "@/api/categories-&-listing/listingTypesApi";

const CreateAssetModal = ({ isOpen, onClose, onSuccess }) => {
  const [loading, setLoading] = useState(false);
  const [categories, setCategories] = useState([]);
  const [listingTypes, setListingTypes] = useState([]);
  const [formData, setFormData] = useState({
    title: "",
    description: "",
    categoryId: "",
    listingTypeId: "",
    dailyRate: "",
    salePrice: "",
    location: "",
  });
  const [error, setError] = useState("");

  useEffect(() => {
    if (!isOpen) return;

    const loadFormData = async () => {
      try {
        const [cats, types] = await Promise.all([
          getCategories(),
          getListingTypes(),
        ]);
        setCategories(cats);
        setListingTypes(types);
      } catch (error) {
        console.error("Error fetching form data:", error);
      }
    };

    loadFormData();
  }, [isOpen]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const resetForm = () => {
    setFormData({
      title: "",
      description: "",
      categoryId: "",
      listingTypeId: "",
      dailyRate: "",
      salePrice: "",
      location: "",
    });
    setError("");
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      const data = {
        title: formData.title,
        description: formData.description,
        categoryId: parseInt(formData.categoryId),
        listingTypeId: parseInt(formData.listingTypeId),
        dailyRate:
          formData.listingTypeId === "1"
            ? parseFloat(formData.dailyRate)
            : null,
        salePrice:
          formData.listingTypeId === "2"
            ? parseFloat(formData.salePrice)
            : null,
        location: formData.location,
      };

      await createAsset(data);
      onSuccess();
      onClose();
      resetForm();
    } catch (error) {
      setError(error.message || "Failed to create asset. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 backdrop-blur-sm">
      <div className="bg-white rounded-2xl shadow-2xl max-w-2xl w-full mx-4 max-h-[90vh] overflow-y-auto">
        {/* Header */}
        <div className="flex justify-between items-center p-6 border-b border-gray-100">
          <div>
            <h2 className="text-xl font-bold text-gray-800">Post New Asset</h2>
            <p className="text-sm text-gray-500 mt-1">
              List your vehicle for rent or sale
            </p>
          </div>
          <button
            onClick={onClose}
            className="p-2 hover:bg-gray-100 rounded-lg transition"
          >
            <X className="w-5 h-5 text-gray-500" />
          </button>
        </div>

        {/* Form */}
        <form onSubmit={handleSubmit} className="p-6 space-y-5">
          {/* Title */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Title <span className="text-red-500">*</span>
            </label>
            <input
              type="text"
              name="title"
              value={formData.title}
              onChange={handleChange}
              placeholder="e.g., Toyota Vios 2020"
              className="w-full px-4 py-2.5 border border-gray-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-black focus:border-transparent"
              required
            />
          </div>

          {/* Description */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Description <span className="text-red-500">*</span>
            </label>
            <textarea
              name="description"
              value={formData.description}
              onChange={handleChange}
              placeholder="Describe your vehicle..."
              rows={3}
              className="w-full px-4 py-2.5 border border-gray-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-black focus:border-transparent resize-none"
              required
            />
          </div>

          {/* Category & Listing Type */}
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Category <span className="text-red-500">*</span>
              </label>
              <select
                name="categoryId"
                value={formData.categoryId}
                onChange={handleChange}
                className="w-full px-4 py-2.5 border border-gray-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-black focus:border-transparent"
                required
              >
                <option value="">Select category</option>
                {categories.map((cat) => (
                  <option key={cat.categoryId} value={cat.categoryId}>
                    {cat.name}
                  </option>
                ))}
              </select>
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Listing Type <span className="text-red-500">*</span>
              </label>
              <select
                name="listingTypeId"
                value={formData.listingTypeId}
                onChange={handleChange}
                className="w-full px-4 py-2.5 border border-gray-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-black focus:border-transparent"
                required
              >
                <option value="">Select type</option>
                {listingTypes.map((type) => (
                  <option key={type.listingTypeId} value={type.listingTypeId}>
                    {type.name}
                  </option>
                ))}
              </select>
            </div>
          </div>

          {/* Daily Rate & Sale Price */}
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Daily Rate (₱)
                {formData.listingTypeId === "1" && (
                  <span className="text-red-500"> *</span>
                )}
              </label>
              <input
                type="number"
                name="dailyRate"
                value={formData.dailyRate}
                onChange={handleChange}
                placeholder="e.g., 1500"
                className="w-full px-4 py-2.5 border border-gray-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-black focus:border-transparent"
                required={formData.listingTypeId === "1"}
                disabled={formData.listingTypeId === "2"}
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Sale Price (₱)
                {formData.listingTypeId === "2" && (
                  <span className="text-red-500"> *</span>
                )}
              </label>
              <input
                type="number"
                name="salePrice"
                value={formData.salePrice}
                onChange={handleChange}
                placeholder="e.g., 450000"
                className="w-full px-4 py-2.5 border border-gray-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-black focus:border-transparent"
                required={formData.listingTypeId === "2"}
                disabled={formData.listingTypeId === "1"}
              />
            </div>
          </div>

          {/* Location */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Location <span className="text-red-500">*</span>
            </label>
            <input
              type="text"
              name="location"
              value={formData.location}
              onChange={handleChange}
              placeholder="e.g., Manila"
              className="w-full px-4 py-2.5 border border-gray-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-black focus:border-transparent"
              required
            />
          </div>

          {/* Error */}
          {error && (
            <div className="bg-red-50 text-red-600 p-3 rounded-lg text-sm">
              {error}
            </div>
          )}

          {/* Actions */}
          <div className="flex gap-3 pt-4 border-t border-gray-100">
            <button
              type="button"
              onClick={onClose}
              className="flex-1 px-4 py-2.5 border border-gray-200 text-gray-700 rounded-lg hover:bg-gray-50 transition font-medium"
            >
              Cancel
            </button>
            <button
              type="submit"
              disabled={loading}
              className="flex-1 px-4 py-2.5 bg-black text-white rounded-lg hover:bg-gray-800 transition font-medium disabled:opacity-50"
            >
              {loading ? "Posting..." : "Post Asset"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default CreateAssetModal;
