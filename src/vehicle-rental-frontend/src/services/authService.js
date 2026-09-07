// src/services/authService.js
// This is the FRONTEND service that calls your backend API

const API_URL = "http://localhost:5000/api";

export async function login(username, password) {
    try {
        console.log("Attempting login to:", `${API_URL}/auth/login`);
        console.log("Username:", username);
        
        const response = await fetch(`${API_URL}/auth/login`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ 
                username: username, 
                password: password 
            }),
        });

        console.log("Response status:", response.status);

        // Get response as text first to handle errors better
        const text = await response.text();
        console.log("Raw response:", text);

        // Try to parse JSON
        let data;
        try {
            data = JSON.parse(text);
        } catch (parseError) {
            console.error("Failed to parse response as JSON:", text);
            throw new Error("Server returned invalid response. Make sure backend is running on port 5000.");
        }

        if (!response.ok) {
            throw new Error(data.message || `Login failed with status: ${response.status}`);
        }

        // Store token and user data
        if (data.token) {
            localStorage.setItem("token", data.token);
            localStorage.setItem("user", JSON.stringify({
                id: data.userID,
                username: data.username,
                email: data.email,
                role: data.role,
                expiresAt: data.expiresAt
            }));
            console.log("Login successful for user:", data.username);
        }

        return data;
    } catch (error) {
        console.error("Login service error:", error);
        throw error;
    }
}

export function register(userData) {
    return fetch(`${API_URL}/auth/register`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(userData),
    })
    .then(response => {
        if (!response.ok) {
            return response.json().then(data => {
                throw new Error(data.message || "Registration failed");
            });
        }
        return response.json();
    })
    .catch(error => {
        console.error("Registration error:", error);
        throw error;
    });
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
        // Check if token is expired
        const payload = JSON.parse(atob(token.split('.')[1]));
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

// Helper for authenticated API calls
export async function authenticatedFetch(url, options = {}) {
    const token = getToken();
    
    if (!token) {
        throw new Error("No authentication token found");
    }

    const headers = {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`,
        ...options.headers,
    };

    const response = await fetch(url, {
        ...options,
        headers,
    });

    if (response.status === 401) {
        logout();
        throw new Error("Session expired. Please login again.");
    }

    return response;
}