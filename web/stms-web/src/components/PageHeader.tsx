import { Link, useNavigate } from "react-router-dom";

type Props = {
  title: string;
  backTo?: string | "HISTORY";  // "HISTORY" uses browser back()
  right?: React.ReactNode;
};

export default function PageHeader({ title, backTo, right }: Props) {
  const nav = useNavigate();
  const Back =
    backTo === "HISTORY" ? (
      <button
        onClick={() => nav(-1)}
        style={{ fontSize: 20, border: "none", background: "transparent", cursor: "pointer" }}
        title="Back"
      >
        ←
      </button>
    ) : backTo ? (
      <Link to={backTo} style={{ textDecoration: "none", fontSize: 20 }} title="Back">
        ←
      </Link>
    ) : null;

  return (
    <div style={{ display: "flex", alignItems: "center", gap: 12, padding: "12px 0" }}>
      {Back}
      <h2 style={{ margin: 0 }}>{title}</h2>
      <div style={{ marginLeft: "auto" }}>{right}</div>
    </div>
  );
}
