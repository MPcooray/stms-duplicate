import { useEffect, useState } from "react";
import { createTournament, listTournaments, updateTournament, deleteTournament } from "../api";
import { Link } from "react-router-dom";
import PageHeader from "../components/PageHeader";

type T = { id:number; name:string; venue?:string; startDate:string; endDate:string; status:string };

export default function TournamentsPage() {
  const [items, setItems] = useState<T[]>([]);
  const [form, setForm] = useState({ name:"", venue:"", startDate:"", endDate:"", status:"Planned" });
  const [editingId, setEditingId] = useState<number|null>(null);
  const [editForm, setEditForm] = useState({ name:"", venue:"", startDate:"", endDate:"", status:"Planned" });

  const load = async () => setItems(await listTournaments());
  useEffect(()=>{ load(); }, []);

  const submit = async (e: React.FormEvent) => {
    e.preventDefault();
    await createTournament({
      ...form,
      startDate: new Date(form.startDate),
      endDate: new Date(form.endDate),
    });
    setForm({ name:"", venue:"", startDate:"", endDate:"", status:"Planned" });
    await load();
  };

  const startEdit = (t: T) => {
    setEditingId(t.id);
    setEditForm({
      name: t.name,
      venue: t.venue ?? "",
      startDate: t.startDate.slice(0,10),
      endDate: t.endDate.slice(0,10),
      status: t.status,
    });
  };

  const saveEdit = async (id:number) => {
    await updateTournament(id, {
      ...editForm,
      startDate: new Date(editForm.startDate),
      endDate: new Date(editForm.endDate),
    });
    setEditingId(null);
    await load();
  };

  const cancelEdit = () => setEditingId(null);

  const remove = async (id:number) => {
    if (!confirm("Delete this tournament?")) return;
    await deleteTournament(id);
    await load();
  };

  return (
    <div style={{maxWidth:900, margin:"20px auto"}}>
      <PageHeader title="Tournaments" />

      {/* create */}
      <form onSubmit={submit} style={{display:"grid", gap:8, gridTemplateColumns:"repeat(5, 1fr)", marginBottom:16}}>
        <input placeholder="Name" value={form.name} onChange={e=>setForm({...form, name:e.target.value})}/>
        <input placeholder="Venue" value={form.venue} onChange={e=>setForm({...form, venue:e.target.value})}/>
        <input type="date" value={form.startDate} onChange={e=>setForm({...form, startDate:e.target.value})}/>
        <input type="date" value={form.endDate} onChange={e=>setForm({...form, endDate:e.target.value})}/>
        <select value={form.status} onChange={e=>setForm({...form, status:e.target.value})}>
          <option>Planned</option><option>Active</option><option>Completed</option>
        </select>
        <button style={{gridColumn:"span 5"}}>Create</button>
      </form>

      {/* list */}
      <table style={{width:"100%", borderCollapse:"collapse"}}>
        <thead><tr>
          <th style={th}>Name</th><th style={th}>Venue</th><th style={th}>Start</th><th style={th}>End</th><th style={th}>Status</th><th style={th}></th>
        </tr></thead>
        <tbody>
          {items.map(t => (
            <tr key={t.id}>
              {editingId === t.id ? (
                <>
                  <td style={td}><input value={editForm.name} onChange={e=>setEditForm({...editForm, name:e.target.value})}/></td>
                  <td style={td}><input value={editForm.venue} onChange={e=>setEditForm({...editForm, venue:e.target.value})}/></td>
                  <td style={td}><input type="date" value={editForm.startDate} onChange={e=>setEditForm({...editForm, startDate:e.target.value})}/></td>
                  <td style={td}><input type="date" value={editForm.endDate} onChange={e=>setEditForm({...editForm, endDate:e.target.value})}/></td>
                  <td style={td}>
                    <select value={editForm.status} onChange={e=>setEditForm({...editForm, status:e.target.value})}>
                      <option>Planned</option><option>Active</option><option>Completed</option>
                    </select>
                  </td>
                  <td style={td}>
                    <button onClick={()=>saveEdit(t.id)}>Save</button>{" "}
                    <button type="button" onClick={cancelEdit}>Cancel</button>
                  </td>
                </>
              ) : (
                <>
                  <td style={td}><b>{t.name}</b></td>
                  <td style={td}>{t.venue ?? "-"}</td>
                  <td style={td}>{new Date(t.startDate).toLocaleDateString()}</td>
                  <td style={td}>{new Date(t.endDate).toLocaleDateString()}</td>
                  <td style={td}>{t.status}</td>
                  <td style={td}>
                    <Link to={`/tournaments/${t.id}/universities`}>Universities</Link>{" — "}
                    <button onClick={()=>startEdit(t)}>Edit</button>{" "}
                    <button onClick={()=>remove(t.id)}>Delete</button>
                  </td>
                </>
              )}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

const th: React.CSSProperties = { textAlign:"left", borderBottom:"1px solid #e5e7eb", padding:"8px 6px" };
const td: React.CSSProperties = { borderBottom:"1px solid #f1f5f9", padding:"8px 6px" };
