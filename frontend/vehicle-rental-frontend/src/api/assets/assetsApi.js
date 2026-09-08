import { authenticatedFetch } from "@/modules/auth/_components/api/authApi";

// ============================================
// SHARED (Both Owner & Customer)
// ============================================

// Get all available assets
export const getAllAssets = async () => {
  const response = await authenticatedFetch("/assets");
  return response.json();
};

// Get asset by ID
export const getAssetById = async (id) => {
  const response = await authenticatedFetch(`/assets/${id}`);
  return response.json();
};

// ============================================
// OWNER ONLY
// ============================================

// Get owner's own assets
export const getMyAssets = async () => {
  const response = await authenticatedFetch("/assets/my-assets");
  return response.json();
};

// Get assets by user
export const getAssetsByUser = async (userId) => {
  const response = await authenticatedFetch(`/assets/user/${userId}`);
  return response.json();
};

// Create asset
export const createAsset = async (data) => {
  const response = await authenticatedFetch("/assets", {
    method: "POST",
    body: JSON.stringify(data),
  });
  return response.json();
};

// Update asset
export const updateAsset = async (id, data) => {
  const response = await authenticatedFetch(`/assets/${id}`, {
    method: "PUT",
    body: JSON.stringify(data),
  });
  return response.json();
};

// Delete asset (soft delete)
export const deleteAsset = async (id) => {
  await authenticatedFetch(`/assets/${id}`, { method: "DELETE" });
};

// Restore asset
export const restoreAsset = async (id) => {
  const response = await authenticatedFetch(`/assets/${id}/restore`, {
    method: "PUT",
  });
  return response.json();
};
