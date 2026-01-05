import React, { useState } from "react";
import Modal from "./Modal";

export default function TaskModal({
  payload,
  events,
  onClose,
  onCreate,
  onSave,
  onDelete
}) {
  const isCreate = payload.create;
  const task = payload.task;

  const [title, setTitle] = useState(task?.title || "");
  const [description, setDescription] = useState(task?.description || "");
  const [eventId, setEventId] = useState(task?.eventId ?? "");

  return (
    <Modal onClose={onClose}>
      <h2>{isCreate ? "Create Task" : "Edit Task"}</h2>

      {/* Title */}
      <input
        type="text"
        placeholder="Task title"
        className="modal-input"
        value={title}
        onChange={(e) => setTitle(e.target.value)}
      />

      {/* Description */}
      <textarea
        placeholder="Description"
        className="modal-textarea"
        value={description}
        onChange={(e) => setDescription(e.target.value)}
      />

      {/* Event Select */}
      <select
        value={eventId}
        onChange={(e) => setEventId(Number(e.target.value))}
        className="input-select"
      >
        <option value="">Choose event</option>
        {events.map((ev) => (
          <option key={ev.id} value={ev.id}>
            {ev.title}
          </option>
        ))}
      </select>

      {/* ACTION BUTTONS */}
      <div className="modal-actions">
        {isCreate ? (
          <button
            className="save-btn"
            onClick={() => {
              onCreate(payload.column.id, {
                title,
                description,
                eventId
              });
              onClose(); // modalas užsidaro po CREATE
            }}
          >
            Create
          </button>
        ) : (
          <>
            <button
              className="save-btn"
              onClick={() => {
                onSave(task.id, {
                  ...task,
                  title,
                  description,
                  eventId
                });
                onClose();
              }}
            >
              Save changes
            </button>

            <button
              className="delete-btn"
              onClick={() => {
                onDelete(task.id);
                onClose();
              }}
            >
              Delete
            </button>
          </>
        )}
      </div>
    </Modal>
  );
}
