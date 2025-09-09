import React from "react"
import ReactDOM from "react-dom/client"
import {
  createBrowserRouter,
  RouterProvider,
  Navigate,
} from "react-router-dom"

import App from "./App"
import HomePage from "./pages/HomePage"
import LoginPage from "./pages/LoginPage"
import TournamentsPage from "./pages/TournamentsPage"
import UniversitiesPage from "./pages/UniversitiesPage"
import PlayersPage from "./pages/PlayersPage"
import ResultsPage from "./pages/ResultsPage"
import { getToken } from "./auth"
import "./index.css"

// Protect only private pages
const Private = ({ children }: { children: React.ReactElement }) =>
  getToken() ? children : <Navigate to="/login" replace />

const router = createBrowserRouter([
  { path: "/", element: <HomePage /> },       // public splash
  { path: "/login", element: <LoginPage /> }, // public login

  {
    element: (
      <Private>
        <App />
      </Private>
    ),
    children: [
      { path: "/tournaments", element: <TournamentsPage /> },
      { path: "/tournaments/:tid/universities", element: <UniversitiesPage /> },
      { path: "/universities/:uid/players", element: <PlayersPage /> },
      { path: "/results", element: <ResultsPage /> },
    ],
  },

  { path: "*", element: <Navigate to="/" replace /> },
])

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>
)
