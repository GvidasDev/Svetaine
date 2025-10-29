import { useState } from "react";
import eventApi from "../../api/eventApi";

export default function EventForm({ onSubmitSuccess }) {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [date, setDate] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    await eventApi.create({ title, description, date });
    onSubmitSuccess();
  };

  return (
    <form className="event-form" onSubmit={handleSubmit}>
      <h2>Add Event</h2>
      <input
        type="text"
        placeholder="Title"
        value={title}
        onChange={(e) => setTitle(e.target.value)}
        required
      />
      <textarea
        placeholder="Description"
        value={description}
        onChange={(e) => setDescription(e.target.value)}
        required
      />
      <input
        type="date"
        value={date}
        onChange={(e) => setDate(e.target.value)}
        required
      />
      <button type="submit">Save</button>
    </form>
  );
}
