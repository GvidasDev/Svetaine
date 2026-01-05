import React, { useState } from "react";
import Modal from "./Modal";

export default function AddColumnModal({ onClose, onCreate }) {
  const [name, setName] = useState("");

  return (
    <Modal onClose={onClose}>
      <h2>Create New Column</h2>

      <input
        type="text"
        className="modal-input"
        placeholder="Column name"
        value={name}
        onChange={(e) => setName(e.target.value)}
      />

      <button
        className="save-btn"
        onClick={() => {
          if (name.trim().length > 0) {
            onCreate(name);
          }
        }}
      >
        Save
      </button>
    </Modal>
  );
}
