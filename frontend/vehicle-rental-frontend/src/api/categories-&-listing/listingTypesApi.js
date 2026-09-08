import { authenticatedFetch } from "@/modules/auth/_components/api/authApi";

// Get all listing types
export const getListingTypes = async () => {
  const response = await authenticatedFetch("/listingtypes");
  return response.json();
};

// Get listing type by ID (optional)
export const getListingTypeById = async (id) => {
  const response = await authenticatedFetch(`/listingtypes/${id}`);
  return response.json();
};
