const API_URL = import.meta.env.VITE_API_URL;

export async function login(username, password) {
  try {
    const response = await fetch(`${API_URL}/auth/login`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ username, password }),
    });

    const text = await response.text();
    let data;
    try {
      data = JSON.parse(text);
    } catch {
      console.error("Failed to parse response as JSON:", text);
      throw new Error("Server returned invalid response.");
    }

    if (!response.ok) {
      throw new Error(
        data.message || `Login failed with status: ${response.status}`,
      );
    }

    if (data.token) {
      localStorage.setItem("token", data.token);
      localStorage.setItem(
        "user",
        JSON.stringify({
          id: data.userID,
          username: data.username,
          email: data.email,
          role: data.role,
          expiresAt: data.expiresAt,
        }),
      );
    }

    return data;
  } catch (error) {
    console.error("Login error:", error);
    throw error;
  }
}

export async function register(userData) {
  try {
    const response = await fetch(`${API_URL}/auth/register`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(userData),
    });

    const text = await response.text();
    let data;
    try {
      data = JSON.parse(text);
    } catch {
      console.error("Failed to parse response as JSON:", text);
      throw new Error("Server returned invalid response.");
    }

    if (!response.ok) {
      throw new Error(data.message || "Registration failed");
    }

    return data;
  } catch (error) {
    console.error("Registration error:", error);
    throw error;
  }
}

export function getToken() {
  return localStorage.getItem("token");
}

export function getUser() {
  try {
    const user = localStorage.getItem("user");
    return user ? JSON.parse(user) : null;
  } catch {
    return null;
  }
}

export function isAuthenticated() {
  const token = getToken();
  if (!token) return false;

  try {
    const payload = JSON.parse(atob(token.split(".")[1]));
    const expired = payload.exp * 1000 < Date.now();
    if (expired) {
      logout();
      return false;
    }
    return true;
  } catch {
    return false;
  }
}

export function isAdmin() {
  const user = getUser();
  return user?.role === "Admin";
}

export function logout() {
  localStorage.removeItem("token");
  localStorage.removeItem("user");
  window.location.href = "/login";
}

export async function authenticatedFetch(url, options = {}) {
  const token = getToken();

  if (!token) {
    throw new Error("No authentication token found");
  }

  const headers = {
    "Content-Type": "application/json",
    Authorization: `Bearer ${token}`,
    ...options.headers,
  };

  const response = await fetch(`${API_URL}${url}`, {
    ...options,
    headers,
  });

  if (response.status === 401) {
    logout();
    throw new Error("Session expired. Please login again.");
  }

  return response;
}
