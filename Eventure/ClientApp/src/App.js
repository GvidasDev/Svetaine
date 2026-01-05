import React, { useState } from "react";
import { Routes, Route } from "react-router-dom";

import Layout from "./components/Layout/Layout";

import EventList from "./components/Event/EventList";
import EventForm from "./components/Event/EventForm";
import EventEdit from "./components/Event/EventEdit";

import ProtectedRoute from "./components/Auth/ProtectedRoute";
import Login from "./components/Auth/Login";
import Register from "./components/Auth/Register";

import TaskPage from "./components/Task/TaskPage";

import Modal from "./components/Modal/Modal";

import "./styles/App.css";

function App() {
  const [refresh, setRefresh] = useState(false);
  const [showModal, setShowModal] = useState(false);

  const reload = () => setRefresh(!refresh);

  return (
    <div className="app-container">
      <Routes>
        
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />

        <Route
          path="/"
          element={
            <ProtectedRoute>
              <Layout>
                <EventList key={refresh} />

                <button
                  className="add-event-btn"
                  onClick={() => setShowModal(true)}
                >
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
              </Layout>
            </ProtectedRoute>
          }
        />

        <Route
          path="/edit/:id"
          element={
            <ProtectedRoute>
              <Layout>
                <EventEdit />
              </Layout>
            </ProtectedRoute>
          }
        />

        <Route
          path="/tasks"
          element={
            <ProtectedRoute>
              <Layout>
                <TaskPage />
              </Layout>
            </ProtectedRoute>
          }
        />

      </Routes>
    </div>
  );
}

export default App;
