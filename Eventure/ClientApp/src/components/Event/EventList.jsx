import React, { useEffect, useState } from "react";
import EventCard from "./EventCard";
import eventApi from "../../api/eventApi";
import "../../styles/App.css";

export default function EventList(refresh) {
  const [events, setEvents] = useState([]);

  const fetchEvents = async () => {
    const res = await eventApi.getAll();
    setEvents(res.data);
  };

  useEffect(() => {
    fetchEvents();
  }, []);

  return (
    <div className="event-list">
      {events.map((ev) => (
        <EventCard
          key={ev.id}
          event={ev}
          onEdit={(ev) => console.log("Edit:", ev)}
          onDelete={fetchEvents}
        />
      ))}
    </div>
  );
}
