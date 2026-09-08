import { authenticatedFetch } from "@/modules/auth/_components/api/authApi";

// ============================================
// SHARED (Both Owner & Customer)
// ============================================

// Get booking by ID
export const getBookingById = async (id) => {
  const response = await authenticatedFetch(`/bookings/${id}`);
  return response.json();
};

// Cancel booking (Owner & Customer)
export const cancelBooking = async (id, reason) => {
  const response = await authenticatedFetch(`/bookings/${id}/cancel`, {
    method: "PUT",
    body: JSON.stringify({ reason }),
  });
  return response.json();
};

// ============================================
// OWNER ONLY
// ============================================

// Get bookings for owner's assets
export const getMyAssetBookings = async () => {
  const response = await authenticatedFetch("/bookings/my-asset-bookings");
  return response.json();
};

// Get bookings by asset
export const getBookingsByAsset = async (assetId) => {
  const response = await authenticatedFetch(`/bookings/asset/${assetId}`);
  return response.json();
};

// ============================================
// CUSTOMER ONLY
// ============================================

// Create booking (Customer only)
export const createBooking = async (data) => {
  const response = await authenticatedFetch("/bookings", {
    method: "POST",
    body: JSON.stringify(data),
  });
  return response.json();
};

// Get customer's own bookings
export const getMyBookings = async () => {
  const response = await authenticatedFetch("/bookings/my-bookings");
  return response.json();
};
