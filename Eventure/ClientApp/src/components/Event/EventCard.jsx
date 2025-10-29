import "../../styles/EventCard.css";
import { useNavigate } from "react-router-dom";

export default function EventCard({ event }) {
  const navigate = useNavigate();

  return (
    <div
      className="event-card"
      onClick={() => navigate(`/events/${event.id}`)}
    >
      <div className="event-logo">LOGO</div>
      <div className="event-info">
        <h3>{event.title}</h3>
        <p>{event.description}</p>
        <small>{new Date(event.date).toISOString().split("T")[0]}</small>
      </div>
    </div>
  );
}
