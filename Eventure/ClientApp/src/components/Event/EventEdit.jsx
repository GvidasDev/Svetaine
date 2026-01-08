import { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import eventApi from "../../api/eventApi";
import uploadApi from "../../api/uploadApi";
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
    isPublic: false
  });

  const [preview, setPreview] = useState("");

  const API_ORIGIN = "https://localhost:7192";

  useEffect(() => {
    const fetchEvent = async () => {
      const res = await eventApi.getById(id);
      const data = res.data;

      setEvent(data);

      const img = data.imageUrl || "";
      if (img) {
        setPreview(img.startsWith("http") ? img : API_ORIGIN + img);
      } else {
        setPreview("");
      }
    };

    fetchEvent();
  }, [id]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setEvent((prev) => ({
      ...prev,
      [name]: type === "checkbox" ? checked : value
    }));
  };

  const handleFile = async (e) => {
    const file = e.target.files && e.target.files[0];
    if (!file) return;

    // local preview
    setPreview(URL.createObjectURL(file));

    // upload to backend
    const res = await uploadApi.uploadImage(file);

    // backend returns /uploads/xxx.jpg
    setEvent((prev) => ({
      ...prev,
      imageUrl: res.data.url
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
          value={event.date ? event.date.split("T")[0] : ""}
          onChange={handleChange}
          required
        />

        <label>Creator</label>
        <input
          type="text"
          name="creator"
          value={event.creator ?? ""}
          onChange={handleChange}
        />

        <label>Image</label>

        <div className="event-image">
          {preview ? (
            <img src={preview} alt="preview" />
          ) : (
            <span>IMG</span>
          )}
        </div>

        <input type="file" accept="image/*" onChange={handleFile} />

        <label>Invited Users</label>
        <input
          type="text"
          name="invitedUsers"
          value={event.invitedUsers ?? ""}
          onChange={handleChange}
        />

        <button type="submit" className="save-btn">
          Save Changes
        </button>
      </form>
    </div>
  );
}
