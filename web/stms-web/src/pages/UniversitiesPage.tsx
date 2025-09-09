import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { createUniversity, listUniversities } from "../api";
import PageHeader from "../components/PageHeader";

export default function UniversitiesPage() {
  const { tid } = useParams(); const tournamentId = Number(tid);
  const [items, setItems] = useState<any[]>([]);
  const [name, setName] = useState("");

  const load = async () => setItems(await listUniversities(tournamentId));
  useEffect(()=>{ load(); }, [tournamentId]);

  const submit = async (e: React.FormEvent) => {
    e.preventDefault();
    await createUniversity(tournamentId, { name });
    setName(""); await load();
  };

  return (
    <div style={{maxWidth:700, margin:"20px auto"}}>
      <PageHeader title={`Universities (Tournament #${tournamentId})`} backTo="/tournaments" />
      <form onSubmit={submit} style={{display:"flex", gap:8, marginBottom:12}}>
        <input placeholder="University name" value={name} onChange={e=>setName(e.target.value)} />
        <button>Add</button>
      </form>
      <ul>
        {items.map(u => (
          <li key={u.id}>
            {u.name} —{" "}
            <Link
              to={`/universities/${u.id}/players`}
              state={{ tid: tournamentId }}               // pass tournament id for a correct "back"
            >
              Players
            </Link>
          </li>
        ))}
      </ul>
    </div>
  );
}
