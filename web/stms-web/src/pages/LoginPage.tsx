"use client"

import type React from "react"
import { useState } from "react"
import { login } from "../api"
import { saveToken } from "../auth"

export default function LoginPage() {
  const [email, setEmail] = useState("admin@stms.com")
  const [password, setPassword] = useState("Admin#123")
  const [error, setError] = useState("")

  const submit = async (e: React.FormEvent) => {
    e.preventDefault()
    try {
      const { token } = await login(email, password)
      saveToken(token)
      window.location.href = "/tournaments"
    } catch {
      setError("Invalid email or password")
    }
  }

  return (
    <>
      {/* Animated background */}
      <div className="auth-bg">
        <div className="auth-bg__gradient" />
        <div className="auth-bg__waves">
          {/* back wave */}
          <svg
            className="wave wave--back"
            viewBox="0 0 1440 320"
            preserveAspectRatio="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              fill="#ffffff"
              d="M0,160L60,170.7C120,181,240,203,360,202.7C480,203,600,181,720,181.3C840,181,960,203,1080,197.3C1200,192,1320,160,1380,144L1440,128L1440,320L1380,320C1320,320,1200,320,1080,320C960,320,840,320,720,320C600,320,480,320,360,320C240,320,120,320,60,320L0,320Z"
            />
          </svg>
          {/* front wave */}
          <svg
            className="wave wave--front"
            viewBox="0 0 1440 320"
            preserveAspectRatio="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              fill="#ffffff"
              d="M0,224L48,202.7C96,181,192,139,288,112C384,85,480,75,576,85.3C672,96,768,128,864,154.7C960,181,1056,203,1152,192C1248,181,1344,139,1392,117.3L1440,96L1440,320L1392,320C1344,320,1248,320,1152,320C1056,320,960,320,864,320C768,320,672,320,576,320C480,320,384,320,288,320C192,320,96,320,48,320L0,320Z"
            />
          </svg>
        </div>
      </div>

      {/* Centered login card */}
      <div className="auth-wrap">
        <div className="card">
          <h2>🏊 STMS Login</h2>

          <form onSubmit={submit}>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="Email Address"
              autoComplete="username"
            />
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="Password"
              autoComplete="current-password"
            />

            {error && (
              <div style={{ color: "#d1242f", fontSize: 14, marginTop: 4 }}>
                {error}
              </div>
            )}

            <button type="submit">Sign In</button>
          </form>
        </div>
      </div>
    </>
  )
}
