import { useEffect, useState } from "react";
import { useLocation, useParams } from "react-router-dom";
import { createPlayer, listPlayers } from "../api";
import PageHeader from "../components/PageHeader";

export default function PlayersPage() {
  const { uid } = useParams(); const universityId = Number(uid);
  const location = useLocation() as any;
  const fromTid: number | undefined = location.state?.tid; // came from UniversitiesPage link

  const [items, setItems] = useState<any[]>([]);
  const [form, setForm] = useState({ fullName:"", gender:"M", dateOfBirth:"", externalId:"" });

  const load = async () => setItems(await listPlayers(universityId));
  useEffect(()=>{ load(); }, [universityId]);

  const submit = async (e: React.FormEvent) => {
    e.preventDefault();
    await createPlayer(universityId, { ...form, dateOfBirth: new Date(form.dateOfBirth) });
    setForm({ fullName:"", gender:"M", dateOfBirth:"", externalId:"" });
    await load();
  };

  const backTo = fromTid ? `/tournaments/${fromTid}/universities` : "HISTORY";

  return (
    <div style={{maxWidth:800, margin:"20px auto"}}>
      <PageHeader title={`Players (University #${universityId})`} backTo={backTo} />
      <form onSubmit={submit} style={{display:"grid", gap:8, gridTemplateColumns:"repeat(4, 1fr)", marginBottom:12}}>
        <input placeholder="Full name" value={form.fullName} onChange={e=>setForm({...form, fullName:e.target.value})}/>
        <select value={form.gender} onChange={e=>setForm({...form, gender:e.target.value})}>
          <option value="M">M</option><option value="F">F</option><option value="U">U</option>
        </select>
        <input type="date" value={form.dateOfBirth} onChange={e=>setForm({...form, dateOfBirth:e.target.value})}/>
        <input placeholder="External ID" value={form.externalId} onChange={e=>setForm({...form, externalId:e.target.value})}/>
        <button style={{gridColumn:"span 4"}}>Add Player</button>
      </form>

      <ul>
        {items.map(p => (
          <li key={p.id}>{p.fullName} ({p.gender}) — {new Date(p.dateOfBirth).toLocaleDateString()} [#{p.id}]</li>
        ))}
      </ul>
    </div>
  );
}
