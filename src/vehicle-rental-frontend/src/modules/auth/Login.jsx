import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Eye, EyeOff, Car, Mail, Lock, User } from "lucide-react";
import { login } from "./_components/api/authApi";

function Login() {
  const navigate = useNavigate();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);

  async function handleLogin(event) {
    event.preventDefault();
    setError("");
    setLoading(true);

    try {
      const data = await login(username, password);
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
      navigate(data.role === "Admin" ? "/admin/dashboard" : "/dashboard");
    } catch (error) {
      setError(error.message || "Invalid credentials. Please try again.");
    } finally {
      setLoading(false);
    }
  }

  const togglePasswordVisibility = () => {
    setShowPassword(!showPassword);
  };

  return (
    <div className="flex flex-col justify-center items-center min-h-screen  px-4 font-sans antialiased text-slate-900">
      <div className="w-full max-w-[420px] flex flex-col items-center">
        {/* BRANDING SECTION */}
        <header className="text-center mb-8">
          <div className="flex justify-center mb-4">
            <div className="p-3 bg-black rounded-2xl shadow-xl shadow-black/10">
              <Car className="w-8 h-8 text-white" strokeWidth={2} />
            </div>
          </div>
          <h1 className="text-5xl font-black tracking-tighter uppercase leading-none bg-gradient-to-r from-slate-900 to-slate-600 bg-clip-text text-transparent">
            Taguigluwi
          </h1>
          <p className="text-slate-500 text-sm mt-2 font-medium tracking-[0.15em] uppercase">
            Vehicle Marketplace
          </p>
        </header>

        {/* LOGIN FORM */}
        <main className="w-full bg-white/80 backdrop-blur-sm p-8 rounded-3xl shadow-xl shadow-slate-200/50 border border-slate-100">
          <h2 className="text-2xl font-bold text-center mb-8 text-slate-800">
            Welcome Back
          </h2>
          <p className="text-center text-slate-500 text-sm mb-8 -mt-6">
            Sign in to continue to your dashboard
          </p>

          <form onSubmit={handleLogin} className="space-y-5">
            {/* Username Field */}
            <div className="space-y-1.5">
              <label className="text-xs font-bold text-slate-700 ml-1 uppercase tracking-wider flex items-center gap-2">
                <User className="w-3.5 h-3.5" />
                Username or Email
              </label>
              <div className="relative">
                <input
                  type="text"
                  value={username}
                  onChange={(e) => setUsername(e.target.value)}
                  className="w-full px-4 py-3.5 pl-11 bg-slate-50 border border-slate-200 rounded-xl focus:bg-white focus:border-black focus:ring-4 focus:ring-black/5 outline-none transition-all duration-200 text-base"
                  placeholder="Enter your username"
                  required
                />
                <Mail className="w-4 h-4 text-slate-400 absolute left-3.5 top-1/2 -translate-y-1/2" />
              </div>
            </div>

            {/* Password Field */}
            <div className="space-y-1.5">
              <div className="flex justify-between items-end ml-1">
                <label className="text-xs font-bold text-slate-700 uppercase tracking-wider flex items-center gap-2">
                  <Lock className="w-3.5 h-3.5" />
                  Password
                </label>
                <button
                  type="button"
                  className="text-slate-500 text-xs font-bold hover:text-black transition-colors underline-offset-4 hover:underline"
                >
                  Forgot password?
                </button>
              </div>
              <div className="relative">
                <input
                  type={showPassword ? "text" : "password"}
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  className="w-full px-4 py-3.5 pl-11 pr-12 bg-slate-50 border border-slate-200 rounded-xl focus:bg-white focus:border-black focus:ring-4 focus:ring-black/5 outline-none transition-all duration-200 text-base"
                  placeholder="••••••••"
                  required
                />
                <Lock className="w-4 h-4 text-slate-400 absolute left-3.5 top-1/2 -translate-y-1/2" />
                <button
                  type="button"
                  onClick={togglePasswordVisibility}
                  className="absolute right-3.5 top-1/2 -translate-y-1/2 text-slate-400 hover:text-slate-700 transition-colors"
                  aria-label={showPassword ? "Hide password" : "Show password"}
                >
                  {showPassword ? (
                    <EyeOff className="w-4.5 h-4.5" />
                  ) : (
                    <Eye className="w-4.5 h-4.5" />
                  )}
                </button>
              </div>
            </div>

            {/* Remember Me */}
            <div className="flex items-center gap-2 pt-1">
              <input
                type="checkbox"
                id="remember"
                className="w-4 h-4 accent-black rounded border-slate-300"
              />
              <label htmlFor="remember" className="text-sm text-slate-600">
                Remember me
              </label>
            </div>

            {/* Error Message */}
            {error && (
              <div className="text-sm font-medium text-red-600 bg-red-50 p-3 rounded-xl text-center border border-red-100 flex items-center justify-center gap-2">
                <span>⚠️</span>
                {error}
              </div>
            )}

            {/* Submit Button */}
            <button
              type="submit"
              disabled={loading}
              className="w-full py-4 bg-black text-white rounded-xl font-bold text-base shadow-lg shadow-black/20 hover:bg-slate-800 hover:shadow-xl hover:scale-[1.01] active:scale-[0.98] transition-all duration-200 disabled:bg-slate-300 disabled:shadow-none disabled:scale-100"
            >
              {loading ? (
                <span className="flex items-center justify-center gap-2">
                  <svg
                    className="animate-spin h-5 w-5 text-white"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                  >
                    <circle
                      className="opacity-25"
                      cx="12"
                      cy="12"
                      r="10"
                      stroke="currentColor"
                      strokeWidth="4"
                    ></circle>
                    <path
                      className="opacity-75"
                      fill="currentColor"
                      d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                    ></path>
                  </svg>
                  Authenticating...
                </span>
              ) : (
                "Sign in"
              )}
            </button>
          </form>

          {/* CTA SECTION */}
          <div className="mt-8 text-center">
            <p className="text-slate-500 text-sm">
              New to Taguigluwi?{" "}
              <button
                onClick={() => navigate("/register")}
                className="text-black font-bold hover:underline underline-offset-4 ml-1 transition-colors"
              >
                Join the marketplace
              </button>
            </p>
          </div>
        </main>

        {/* FOOTER */}
        <footer className="mt-12 w-full pt-6 border-t border-slate-200/50">
          <div className="flex flex-col items-center space-y-4">
            <p className="text-[11px] font-medium text-slate-400 uppercase tracking-widest">
              © 2024 Taguigluwi Marketplace
            </p>
            <div className="flex gap-6 text-[11px] font-bold text-slate-400">
              <button className="hover:text-black transition-colors uppercase tracking-tight">
                Privacy
              </button>
              <button className="hover:text-black transition-colors uppercase tracking-tight">
                Terms
              </button>
              <button className="hover:text-black transition-colors uppercase tracking-tight">
                Help
              </button>
            </div>
          </div>
        </footer>
      </div>
    </div>
  );
}

export default Login;
