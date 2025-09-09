import { useEffect, useRef, useState } from "react"
import { useNavigate } from "react-router-dom"

/** simple typewriter effect */
function useTypewriter(lines: string[], typeMs = 80, pauseMs = 1200) {
  const [text, setText] = useState("")
  const [i, setI] = useState(0)   // line index
  const [j, setJ] = useState(0)   // char index
  const [deleting, setDeleting] = useState(false)
  const timer = useRef<number | null>(null)

  useEffect(() => {
    const current = lines[i % lines.length]
    const delay = deleting ? typeMs / 2 : typeMs

    timer.current = window.setTimeout(() => {
      if (!deleting) {
        setText(current.slice(0, j + 1))
        setJ(j + 1)
        if (j + 1 === current.length) {
          setDeleting(true)
        }
      } else {
        setText(current.slice(0, j - 1))
        setJ(j - 1)
        if (j - 1 <= 0) {
          setDeleting(false)
          setI((i + 1) % lines.length)
        }
      }
    }, delay)

    return () => { if (timer.current) window.clearTimeout(timer.current) }
  }, [deleting, i, j, lines, typeMs])

  useEffect(() => {
    const current = lines[i % lines.length]
    if (!deleting && j === current.length) {
      const t = window.setTimeout(() => setDeleting(true), pauseMs)
      return () => window.clearTimeout(t)
    }
  }, [deleting, i, j, lines, pauseMs])

  return text
}

export default function HomePage() {
  const navigate = useNavigate()
  const typed = useTypewriter(
    ["Swimming is Life ", "Dive into STMS ", "Manage Tournaments Easily"],
    80,
    1500
  )

  return (
    <>
      {/* Background */}
      <div className="auth-bg">
        <div className="auth-bg__gradient" />
        <div className="auth-bg__waves">
          <svg className="wave wave--back" viewBox="0 0 1440 320" preserveAspectRatio="none">
            <path fill="#ffffff" d="M0,160L60,170.7C120,181,240,203,360,202.7C480,203,600,181,720,181.3C840,181,960,203,1080,197.3C1200,192,1320,160,1380,144L1440,128L1440,320L0,320Z"/>
          </svg>
          <svg className="wave wave--front" viewBox="0 0 1440 320" preserveAspectRatio="none">
            <path fill="#ffffff" d="M0,224L48,202.7C96,181,192,139,288,112C384,85,480,75,576,85.3C672,96,768,128,864,154.7C960,181,1056,203,1152,192C1248,181,1344,139,1392,117.3L1440,96L1440,320L0,320Z"/>
          </svg>
        </div>
      </div>

      {/* Headline */}
      <div className="min-h-screen grid place-items-center">
        <div style={{
          background: "rgba(255,255,255,0.7)",
          borderRadius: "16px",
          padding: "24px 40px",
          boxShadow: "0 8px 30px rgba(0,0,0,.12)"
        }}>
          <h1 className="text-black text-center font-extrabold"
            style={{ fontSize: "clamp(32px, 6vw, 64px)", textShadow: "0 12px 40px rgba(0,0,0,.35)" }}
          >
            {typed}<span className="ml-1 opacity-80">|</span>
          </h1>
        </div>
      </div>

      {/* Lock button */}
      <button className="lock-btn" onClick={() => navigate("/login")} title="Sign in">
        🔒
      </button>
    </>
  )
}
