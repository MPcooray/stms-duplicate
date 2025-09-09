import { useEffect, useState } from "react";
import { createResult, listEvents, listResults, deleteResult, listPlayers } from "../api";
import PageHeader from "../components/PageHeader";

type ResultRow = { id:number; playerId:number; eventId:number; timing:string; heat?:number|null; lane?:number|null };

export default function ResultsPage() {
  const [results, setResults] = useState<ResultRow[]>([]);
  const [events, setEvents] = useState<any[]>([]);
  const [players, setPlayers] = useState<any[]>([]);
  const [universityId, setUniversityId] = useState("");
  const [form, setForm] = useState({ playerId:"", eventId:"", timing:"00:28.450", heat:"", lane:"" });

  const load = async () => {
    setResults(await listResults());
    setEvents(await listEvents());
  };
  useEffect(()=>{ load(); }, []);

  // when universityId changes, load players for that university
  useEffect(() => {
    (async () => {
      if (!universityId) { setPlayers([]); setForm({ ...form, playerId: "" }); return; }
      const ps = await listPlayers(Number(universityId));
      setPlayers(ps);
      // if current player not in list, reset
      if (!ps.find((p:any)=> String(p.id) === form.playerId)) {
        setForm({ ...form, playerId: "" });
      }
    })();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [universityId]);

  const submit = async (e: React.FormEvent) => {
    e.preventDefault();
    await createResult({
      playerId: Number(form.playerId),
      eventId: Number(form.eventId),
      timing: form.timing,
      heat: form.heat ? Number(form.heat) : undefined,
      lane: form.lane ? Number(form.lane) : undefined
    });
    setForm({ ...form, timing:"00:28.000" });
    await load();
  };

  const remove = async (id:number) => {
    if (!confirm("Delete this result?")) return;
    await deleteResult(id);
    await load();
  };

  return (
    <div style={{maxWidth:900, margin:"20px auto"}}>
      <PageHeader title="Results (Timing Entry)" backTo="HISTORY" />

      <form onSubmit={submit} style={{display:"grid", gap:8, gridTemplateColumns:"repeat(6, 1fr)"}}>
        <input
          placeholder="University ID"
          value={universityId}
          onChange={e=>setUniversityId(e.target.value)}
          title="Enter the University ID to load its players"
        />
        <select
          value={form.playerId}
          onChange={e=>setForm({...form, playerId: e.target.value})}
          disabled={!players.length}
          title="Select player (loaded by university)"
        >
          <option value="">{players.length ? "Select player" : "Load players by Univ. ID"}</option>
          {players.map((p:any)=> <option key={p.id} value={p.id}>{p.fullName} (#{p.id})</option>)}
        </select>

        <select
          value={form.eventId}
          onChange={e=>setForm({...form, eventId: e.target.value})}
          title="Select swimming event"
        >
          <option value="">Select event</option>
          {events.map((e:any)=> <option key={e.id} value={e.id}>{e.code} — {e.name}</option>)}
        </select>

        <input placeholder="mm:ss.mmm" value={form.timing} onChange={e=>setForm({...form, timing:e.target.value})}/>
        <input placeholder="Heat (opt)" value={form.heat} onChange={e=>setForm({...form, heat:e.target.value})}/>
        <input placeholder="Lane (opt)" value={form.lane} onChange={e=>setForm({...form, lane:e.target.value})}/>
        <button style={{gridColumn:"span 6"}} disabled={!form.playerId || !form.eventId}>Save</button>
      </form>

      <table style={{width:"100%", marginTop:16, borderCollapse:"collapse"}}>
        <thead><tr>
          <th style={th}>#</th><th style={th}>Player</th><th style={th}>Event</th>
          <th style={th}>Timing</th><th style={th}>Heat</th><th style={th}>Lane</th><th style={th}></th>
        </tr></thead>
        <tbody>
          {results.map((r)=>(
            <tr key={r.id}>
              <td style={td}>{r.id}</td>
              <td style={td}>{r.playerId}</td>
              <td style={td}>{r.eventId}</td>
              <td style={td}>{r.timing}</td>
              <td style={td}>{r.heat ?? "-"}</td>
              <td style={td}>{r.lane ?? "-"}</td>
              <td style={td}>
                <button onClick={()=>remove(r.id)}>Delete</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

const th: React.CSSProperties = { textAlign:"left", borderBottom:"1px solid #e5e7eb", padding:"8px 6px" };
const td: React.CSSProperties = { borderBottom:"1px solid #f1f5f9", padding:"8px 6px" };
