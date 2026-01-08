import "../../../styles/App.css";

export default function InvitedList({ invited }) {
  return (
    <>
      <label>
        <strong>Invited Users</strong>
      </label>

      <div className="invited-box">
        {invited.length === 0 ? (
          <div className="invited-empty">—</div>
        ) : (
          invited.map((u) => (
            <div className="invited-item" key={u.userId}>
              <strong>{u.username}</strong>
            </div>
          ))
        )}
      </div>
    </>
  );
}
