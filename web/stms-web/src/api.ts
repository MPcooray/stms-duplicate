import axios from "axios";
import { authHeader } from "./auth";

const api = axios.create({ baseURL: import.meta.env.VITE_API_URL });

// auth
export const login = async (email: string, password: string) =>
  (await api.post("/auth/login", { email, password })).data;

// tournaments
export const listTournaments = async () =>
  (await api.get("/api/tournaments", { headers: authHeader() })).data;
export const createTournament = async (t: any) =>
  (await api.post("/api/tournaments", t, { headers: authHeader() })).data;

// universities
export const listUniversities = async (tid: number) =>
  (await api.get(`/api/tournaments/${tid}/universities`, { headers: authHeader() })).data;
export const createUniversity = async (tid: number, u: any) =>
  (await api.post(`/api/tournaments/${tid}/universities`, u, { headers: authHeader() })).data;

// players
export const listPlayers = async (uid: number) =>
  (await api.get(`/api/universities/${uid}/players`, { headers: authHeader() })).data;
export const createPlayer = async (uid: number, p: any) =>
  (await api.post(`/api/universities/${uid}/players`, p, { headers: authHeader() })).data;

// events & results
export const listEvents = async () =>
  (await api.get(`/api/events`, { headers: authHeader() })).data;
export const listResults = async () =>
  (await api.get(`/api/results`, { headers: authHeader() })).data;
export const createResult = async (r: { playerId:number; eventId:number; timing:string; heat?:number; lane?:number }) =>
  (await api.post(`/api/results`, r, { headers: authHeader() })).data;

export const updateResult = async (id:number, r:{ timing:string; heat?:number; lane?:number }) =>
  (await api.put(`/api/results/${id}`, r, { headers: authHeader() })).data;

export const deleteResult = async (id:number) =>
  (await api.delete(`/api/results/${id}`, { headers: authHeader() })).data;

export const updateTournament = async (id:number, t:any) =>
  (await api.put(`/api/tournaments/${id}`, t, { headers: authHeader() })).data;

export const deleteTournament = async (id:number) =>
  (await api.delete(`/api/tournaments/${id}`, { headers: authHeader() })).data;


export default api;
