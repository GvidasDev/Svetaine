import { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import eventApi from "../../api/eventApi";
import "../../styles/App.css";

export default function EventEdit() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [event, setEvent] = useState({
    title: "",
    description: "",
    date: "",
    creator: "",
    imageUrl: "",
    invitedUsers: "",
    isPublic: false,
  });

  useEffect(() => {
    const fetchEvent = async () => {
      const res = await eventApi.getById(id);
      setEvent(res.data);
    };
    fetchEvent();
  }, [id]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setEvent((prev) => ({
      ...prev,
      [name]: type === "checkbox" ? checked : value,
    }));
  };

  const handleSave = async (e) => {
    e.preventDefault();
    await eventApi.update(id, event);
    navigate("/");
  };

  return (
    <div className="edit-container">
      <div className="edit-header">
        <button className="back-btn" onClick={() => navigate(-1)}>
          ← Back
        </button>
        <h2>Edit Event</h2>
      </div>

      <form className="edit-form" onSubmit={handleSave}>
        <label>Title</label>
        <input
          type="text"
          name="title"
          value={event.title}
          onChange={handleChange}
          required
        />

        <label>Description</label>
        <textarea
          name="description"
          value={event.description}
          onChange={handleChange}
        />

        <label>Date</label>
        <input
          type="date"
          name="date"
          value={event.date.split("T")[0]}
          onChange={handleChange}
          required
        />

        <label>Creator</label>
        <input
          type="text"
          name="creator"
          value={event.creator}
          onChange={handleChange}
        />

        <label>Image URL</label>
        <input
          type="text"
          name="imageUrl"
          value={event.imageUrl}
          onChange={handleChange}
        />

        <label>Invited Users</label>
        <input
          type="text"
          name="invitedUsers"
          value={event.invitedUsers}
          onChange={handleChange}
        />

        <button type="submit" className="save-btn">
          Save Changes
        </button>
      </form>
    </div>
  );
}
