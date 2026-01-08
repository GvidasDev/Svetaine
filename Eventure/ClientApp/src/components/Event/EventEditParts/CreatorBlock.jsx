import "../../../styles/App.css";

export default function CreatorBlock({ creator }) {
  return (
    <div className="event-creator event-creator--center">
      <span>
        <strong>Creator:</strong>
      </span>
      <strong>{creator || "—"}</strong>
    </div>
  );
}
