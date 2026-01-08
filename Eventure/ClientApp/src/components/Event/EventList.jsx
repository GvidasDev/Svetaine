import React, { useEffect, useState } from "react";
import eventApi from "../../api/eventApi";
import EventCard from "./EventCard";
import Modal from "../Modal/Modal";
import InvitePanel from "../Invite/InvitePanel";
import "../../styles/App.css";

export default function EventList() {
  const [events, setEvents] = useState([]);
  const [inviteEvent, setInviteEvent] = useState(null);

  const load = async () => {
    const res = await eventApi.getAll();
    setEvents(res.data || []);
  };

  useEffect(() => {
    load();
  }, []);

  return (
    <div className="event-list-container">
      <div className="event-list">
        {events.map((ev) => (
          <EventCard
            key={ev.id}
            event={ev}
            onDelete={load}
            onInvite={(e) => setInviteEvent(e)}
          />
        ))}
      </div>

      {inviteEvent && (
        <Modal onClose={() => setInviteEvent(null)}>
          <InvitePanel
            eventId={inviteEvent.id}
            canInvite={!!inviteEvent.canInvite}
            onChanged={load}
          />
        </Modal>
      )}
    </div>
  );
}
