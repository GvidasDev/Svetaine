import React, { useState } from "react";
import EventList from "./components/EventList";
import EventForm from "./components/EventForm";

function App() {
  const [refresh, setRefresh] = useState(false);
  const reload = () => setRefresh(!refresh);

  return (
    <div className="container">
      <h1>🎬 Event Manager</h1>
      <EventForm onCreated={reload} />
      <EventList key={refresh} />
    </div>
  );
}

export default App;
