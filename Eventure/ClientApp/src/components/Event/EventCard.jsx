import { useNavigate } from "react-router-dom";
import eventApi from "../../api/eventApi";
import "../../styles/App.css";

export default function EventCard({ event, onDelete }) {
  const navigate = useNavigate();

  const handleDelete = async (e) => {
    e.stopPropagation();
    await eventApi.delete(event.id);
    if (onDelete) onDelete();
  };

  return (
    <div className="event-card">
      <div className="event-left">
        <div className="event-image">
          {event.imageUrl ? (
            <img src={event.imageUrl} alt={event.title} />
          ) : (
            <span>IMG</span>
          )}
        </div>

        <div className="event-details">
          <h3>{event.title}</h3>
          <p>{event.description}</p>
          <small>📅 
            {new Date(event.date).toLocaleDateString()}
          </small>
          <br />
          <small>⏰ Remaining: {event.remainingDays} days</small>
        </div>
      </div>

      <div className="event-right">
        <button
          className="event-btn edit"
          onClick={() => navigate(`/edit/${event.id}`)}
        >
          Edit
        </button>
        <button className="event-btn invite">Invite</button>
        <button className="event-btn delete" onClick={handleDelete}>
          Delete
        </button>
      </div>

      <div className="event-footer">
        <span className="creator">
          👤 Creator: <strong>{event.creator ?? "—"}</strong>
        </span>
        <span className="invited">
          🧑‍🤝‍🧑 Invited:{" "}
          <strong>{event.invitedUsers ?? "—"}</strong>
        </span>
      </div>
    </div>
  );
}
