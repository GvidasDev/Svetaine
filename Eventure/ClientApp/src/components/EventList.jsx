import { useEffect, useState } from "react";
import eventApi from "../api/eventApi";

export default function EventList() {
  const [events, setEvents] = useState([]);

  const fetchEvents = async () => {
    const res = await eventApi.getAll();
    setEvents(res.data);
  };

  useEffect(() => {
    fetchEvents();
  }, []);

  return (
    <div>
      {events.map((ev) => (
        <div key={ev.id}>
          <h3>{ev.title}</h3>
          <p>{ev.description}</p>
          <small>{new Date(ev.date).toISOString().split("T")[0]}</small>
        </div>
      ))}
    </div>
  );
}
