import React, { useEffect, useState } from "react";
import Modal from "./Modal";
import "../../styles/App.css";

export default function TaskModal({ payload, events, onClose, onCreate, onSave, onDelete }) {
  const isCreate = !!payload?.create;
  const column = payload?.column ?? null;
  const task = payload?.task ?? null;

  const forcedEventId = isCreate && column && column.eventId ? column.eventId : null;

  const [form, setForm] = useState({
    title: "",
    description: "",
    eventId: ""
  });

  useEffect(() => {
    if (isCreate) {
      setForm({
        title: "",
        description: "",
        eventId: forcedEventId ? String(forcedEventId) : ""
      });
      return;
    }

    if (task) {
      setForm({
        title: task.title ?? "",
        description: task.description ?? "",
        eventId: task.eventId ? String(task.eventId) : ""
      });
    }
  }, [isCreate, task, forcedEventId]);

  const change = (key) => (e) => {
    setForm((p) => ({ ...p, [key]: e.target.value }));
  };

  const submitCreate = async (e) => {
    e.preventDefault();

    const eventIdFinal = forcedEventId
      ? forcedEventId
      : form.eventId
      ? parseInt(form.eventId, 10)
      : null;

    await onCreate({
      title: form.title,
      description: form.description,
      eventId: eventIdFinal
    });
  };

  const submitSave = async (e) => {
    e.preventDefault();

    const eventIdFinal = form.eventId ? parseInt(form.eventId, 10) : null;

    await onSave(task.id, {
      id: task.id,
      title: form.title,
      description: form.description,
      taskStatus: task.taskStatus,
      eventId: eventIdFinal,
      userId: task.userId
    });
  };

  return (
    <Modal onClose={onClose}>
        <h2>{isCreate ? "Create Task" : "Edit Task"}</h2>

        <form onSubmit={isCreate ? submitCreate : submitSave}>
          <input
            className="modal-input"
            value={form.title}
            onChange={change("title")}
            placeholder="Title"
            required
          />

          <textarea
            className="modal-textarea"
            value={form.description}
            onChange={change("description")}
            placeholder="Description"
            rows={4}
          />

          <select
            className="modal-select"
            value={forcedEventId ? String(forcedEventId) : form.eventId}
            onChange={change("eventId")}
            disabled={!!forcedEventId}
          >
            <option value="">—</option>
            {events.map((ev) => (
              <option key={ev.id} value={ev.id}>
                {ev.title}
              </option>
            ))}
          </select>

          <div className="modal-actions">
            <button className="save-btn" type="submit">
              {isCreate ? "Create" : "Save"}
            </button>

            {!isCreate && (
              <button
                className="delete-btn"
                type="button"
                onClick={() => onDelete(task.id)}
              >
                Delete
              </button>
            )}
          </div>
        </form>
    </Modal>
  );
}
