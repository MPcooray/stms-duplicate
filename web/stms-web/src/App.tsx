import { Link, Outlet } from "react-router-dom"
import { logout } from "./auth"

export default function App() {
  return (
    <>
      {/* Top bar for protected pages */}
      <div
        style={{
          padding: "12px 16px",
          display: "flex",
          alignItems: "center",
          gap: 12,
          borderBottom: "1px solid #ddd",
          background: "#fff",
        }}
      >
        <Link to="/tournaments" style={{ textDecoration: "none", color: "inherit" }}>
          <h2 style={{ margin: 0 }}>STMS</h2>
        </Link>

        <div style={{ marginLeft: "auto", display: "flex", gap: 12 }}>
          <Link to="/results" style={{ textDecoration: "none" }}>
            Results
          </Link>
          <button
            onClick={() => {
              logout()
              window.location.href = "/login"
            }}
          >
            Logout
          </button>
        </div>
      </div>

      {/* Page content */}
      <div>
        <Outlet />
      </div>
    </>
  )
}
