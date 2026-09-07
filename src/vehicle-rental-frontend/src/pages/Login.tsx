import { useState, FormEvent } from "react";
import { useNavigate } from "react-router-dom";
import { login } from "../services/authService";

interface LoginResponse {
    token: string;
    email: string;
    username: string;
    role: string;
    userID: number;
    expiresAt: string;
}

function Login() {
    const navigate = useNavigate();
    const [username, setUsername] = useState<string>("");
    const [password, setPassword] = useState<string>("");
    const [error, setError] = useState<string>("");
    const [loading, setLoading] = useState<boolean>(false);

    async function handleLogin(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();
        setError("");
        setLoading(true);

        try {
            const data: LoginResponse = await login(username, password);
            localStorage.setItem("token", data.token);
            localStorage.setItem("user", JSON.stringify({
                id: data.userID,
                username: data.username,
                email: data.email,
                role: data.role,
                expiresAt: data.expiresAt
            }));
            navigate(data.role === "Admin" ? "/admin/dashboard" : "/dashboard");
        } catch (error: any) {
            setError(error.message || "Invalid credentials. Please try again.");
        } finally {
            setLoading(false);
        }
    }

    return (
        <div className="flex flex-col justify-center items-center min-h-screen bg-white px-4 font-sans antialiased text-slate-900">
            <div className="w-full max-w-[400px] flex flex-col items-center">
                
                {/* 1. BRANDING SECTION */}
                <header className="text-center mb-10">
                    <div className="flex justify-center mb-3">
                        <div className="p-2 bg-slate-50 rounded-full">
                            <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                                <path d="M19 17h2c.6 0 1-.4 1-1v-3c0-.9-.7-1.7-1.5-1.9C18.7 10.6 16 10 16 10s-1.3-1.4-2.2-2.3c-.5-.4-1.1-.7-1.8-.7H5c-.6 0-1.1.4-1.4.9l-1.4 2.9A3.7 3.7 0 0 0 2 13.1V16c0 .6.4 1 1 1h2" />
                                <circle cx="7" cy="17" r="2" />
                                <path d="M9 17h6" />
                                <circle cx="17" cy="17" r="2" />
                            </svg>
                        </div>
                    </div>
                    <h1 className="text-4xl font-black tracking-tighter uppercase leading-none">
                        Taguigluwi
                    </h1>
                    <p className="text-slate-500 text-sm mt-2 font-medium tracking-wide uppercase">
                        Vehicle Marketplace
                    </p>
                </header>

                {/* 2. LOGIN FORM */}
                <main className="w-full">
                    <h2 className="text-xl font-semibold text-center mb-8">Sign in</h2>
                    
                    <form onSubmit={handleLogin} className="space-y-5">
                        <div className="space-y-1.5">
                            <label className="text-[13px] font-bold text-slate-700 ml-1 uppercase tracking-wider">
                                Username or Email
                            </label>
                            <input
                                type="text"
                                value={username}
                                onChange={(e) => setUsername(e.target.value)}
                                className="w-full px-4 py-3.5 bg-slate-50 border border-slate-200 rounded-xl focus:bg-white focus:border-blue-500 focus:ring-4 focus:ring-blue-500/10 outline-none transition-all duration-200 text-base"
                                placeholder="Enter your username"
                                required
                            />
                        </div>

                        <div className="space-y-1.5">
                            <div className="flex justify-between items-end ml-1">
                                <label className="text-[13px] font-bold text-slate-700 uppercase tracking-wider">
                                    Password
                                </label>
                                <button type="button" className="text-blue-600 text-xs font-bold hover:underline underline-offset-4">
                                    Forgot?
                                </button>
                            </div>
                            <input
                                type="password"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                className="w-full px-4 py-3.5 bg-slate-50 border border-slate-200 rounded-xl focus:bg-white focus:border-blue-500 focus:ring-4 focus:ring-blue-500/10 outline-none transition-all duration-200 text-base"
                                placeholder="••••••••"
                                required
                            />
                        </div>

                        {error && (
                            <div className="text-sm font-medium text-red-600 bg-red-50 p-3 rounded-lg text-center border border-red-100">
                                {error}
                            </div>
                        )}

                        <button
                            type="submit"
                            disabled={loading}
                            className="w-full py-4 bg-black text-white rounded-xl font-bold text-base shadow-lg shadow-black/10 hover:bg-slate-800 active:scale-[0.98] transition-all disabled:bg-slate-300 disabled:shadow-none"
                        >
                            {loading ? "Authenticating..." : "Sign in"}
                        </button>
                    </form>

                    {/* 3. CTA SECTION */}
                    <div className="mt-10 text-center">
                        <p className="text-slate-500 text-sm">
                            New to Taguigluwi?{" "}
                            <button 
                                onClick={() => navigate("/register")}
                                className="text-black font-bold hover:underline underline-offset-4 ml-1"
                            >
                                Join the marketplace
                            </button>
                        </p>
                    </div>
                </main>

                {/* 4. FOOTER */}
                <footer className="mt-20 w-full pt-8 border-t border-slate-100">
                    <div className="flex flex-col items-center space-y-4">
                        <p className="text-[11px] font-medium text-slate-400 uppercase tracking-widest">
                            © 2024 Taguigluwi Marketplace
                        </p>
                        <div className="flex gap-6 text-[12px] font-bold text-slate-400">
                            <button className="hover:text-black transition-colors uppercase tracking-tight">Privacy</button>
                            <button className="hover:text-black transition-colors uppercase tracking-tight">Terms</button>
                            <button className="hover:text-black transition-colors uppercase tracking-tight">Help</button>
                        </div>
                    </div>
                </footer>
            </div>
        </div>
    );
}

export default Login;