import React, { useState } from "react";
import { Routes, Route } from "react-router-dom";
import EventList from "./components/Event/EventList";
import EventModal from "./components/Event/EventModal";
import EventEdit from "./components/Event/EventEdit";
import "./styles/App.css";

function App() {
  const [refresh, setRefresh] = useState(false);
  const [showModal, setShowModal] = useState(false);

  const reload = () => setRefresh(!refresh);

  return (
    <div className="app-container">
      <h1>Events</h1>

      <Routes>
        <Route
          path="/"
          element={
            <>
              <EventList key={refresh} />

              <button
                className="add-event-btn"
                onClick={() => setShowModal(true)}
              >
                +
              </button>

              {showModal && (
                <EventModal
                  onClose={() => setShowModal(false)}
                  onSuccess={reload}
                />
              )}
            </>
          }
        />

        <Route path="/edit/:id" element={<EventEdit />} />
      </Routes>
    </div>
  );
}

export default App;
