import { authenticatedFetch } from "@/modules/auth/_components/api/authApi";

// Get all categories
export const getCategories = async () => {
  const response = await authenticatedFetch("/categories");
  return response.json();
};

// Get category by ID (optional)
export const getCategoryById = async (id) => {
  const response = await authenticatedFetch(`/categories/${id}`);
  return response.json();
};
