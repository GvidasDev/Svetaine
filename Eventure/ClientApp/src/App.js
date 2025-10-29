import React, { useState } from "react";
import EventList from "./components/Event/EventList";
import EventForm from "./components/Event/EventForm";
import Modal from "./components/Modal/Modal";
import "./App.css";

function App() {
  const [refresh, setRefresh] = useState(false);
  const [showModal, setShowModal] = useState(false);

  const reload = () => setRefresh(!refresh);

  return (
    <div className="app-container">
      <h1>🎬 Event Manager</h1>
      <EventList key={refresh} />
      
      <button className="add-event-btn" onClick={() => setShowModal(true)}>
        +
      </button>

      {showModal && (
        <Modal onClose={() => setShowModal(false)}>
          <EventForm
            onSubmitSuccess={() => {
              setShowModal(false);
              reload();
            }}
          />
        </Modal>
      )}
    </div>
  );
}

export default App;
