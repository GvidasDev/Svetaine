import { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import eventApi from "../../api/eventApi";
import uploadApi from "../../api/uploadApi";
import invitationApi from "../../api/invitationApi";
import { API_ORIGIN } from "../../api/apiClient";
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
    isPublic: false,
    canInvite: false
  });

  const [preview, setPreview] = useState("");

  const [invited, setInvited] = useState([]);
  const [inviteEmail, setInviteEmail] = useState("");
  const [foundUser, setFoundUser] = useState(null);
  const [invMsg, setInvMsg] = useState("");

  useEffect(() => {
    const fetchAll = async () => {
      const res = await eventApi.getById(id);
      const data = res.data;

      setEvent(data);

      const img = data.imageUrl || "";
      if (img) setPreview(img.startsWith("http") ? img : API_ORIGIN + img);
      else setPreview("");

      const inv = await invitationApi.getInvited(id);
      setInvited(inv.data || []);
    };

    fetchAll();
  }, [id]);

  useEffect(() => {
    const run = async () => {
      setInvMsg("");
      const e = (inviteEmail || "").trim();
      if (!e) {
        setFoundUser(null);
        return;
      }

      try {
        const res = await invitationApi.lookup(e);
        setFoundUser(res.data);
      } catch {
        setFoundUser(null);
      }
    };

    const t = setTimeout(run, 250);
    return () => clearTimeout(t);
  }, [inviteEmail]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;

    if (name === "creator") return;

    setEvent((prev) => ({
      ...prev,
      [name]: type === "checkbox" ? checked : value
    }));
  };

  const handleFile = async (e) => {
    const file = e.target.files && e.target.files[0];
    if (!file) return;

    setPreview(URL.createObjectURL(file));

    const res = await uploadApi.uploadImage(file);

    setEvent((prev) => ({
      ...prev,
      imageUrl: res.data.url
    }));
  };

  const refreshInvited = async () => {
    const inv = await invitationApi.getInvited(id);
    setInvited(inv.data || []);
  };

  const handleAddInvite = async () => {
    setInvMsg("");

    const email = (inviteEmail || "").trim();
    if (!email) return;
    if (!foundUser) return;

    const already = invited.some((x) => x.id === foundUser.id);
    if (already) {
      setInvMsg("Šis vartotojas jau pakviestas.");
      return;
    }

    try {
      await invitationApi.add(id, email);
      setInviteEmail("");
      setFoundUser(null);
      await refreshInvited();
      setInvMsg("Pakviesta.");
    } catch {
      setInvMsg("Nepavyko pakviesti.");
    }
  };

  const handleRemoveInvite = async (userId) => {
    setInvMsg("");

    try {
      await invitationApi.remove(id, userId);
      await refreshInvited();
    } catch {
      setInvMsg("Nepavyko pašalinti.");
    }
  };

  const handleSave = async (e) => {
    e.preventDefault();

    const payload = {
      title: event.title,
      description: event.description,
      date: event.date,
      imageUrl: event.imageUrl,
      isPublic: event.isPublic
    };

    await eventApi.update(id, payload);
    navigate("/");
  };

  const canInvite = !!event.canInvite;

  return (
    <div className="edit-container">
      <div className="edit-header">
        <button className="back-btn" onClick={() => navigate(-1)}>
          ← Back
        </button>
        <h2>Edit Event</h2>
      </div>

      <form className="edit-form" onSubmit={handleSave}>
        <label><strong>Title</strong></label>
        <input
          type="text"
          name="title"
          value={event.title}
          onChange={handleChange}
          required
        />

        <label><strong>Description</strong></label>
        <textarea
          name="description"
          value={event.description}
          onChange={handleChange}
        />

        <label><strong>Date</strong></label>
        <input
          type="date"
          name="date"
          value={event.date ? event.date.split("T")[0] : ""}
          onChange={handleChange}
          required
        />

        <label><strong>Image</strong></label>

        <div className="event-image">
          {preview ? <img src={preview} alt="preview" /> : <span>IMG</span>}
        </div>

        <input type="file" accept="image/*" onChange={handleFile} />

        <label><strong>Invited Users</strong></label>

        <div className="invited-box">
          {invited.length === 0 ? (
            <div className="invited-empty">—</div>
          ) : (
            invited.map((u) => (
              <div className="invited-item" key={u.id}>
                <span className="invited-name"><strong>{u.username}</strong></span>
                {canInvite && (
                  <button
                    type="button"
                    className="invited-remove"
                    onClick={() => handleRemoveInvite(u.id)}
                  >
                    ×
                  </button>
                )}
              </div>
            ))
          )}
        </div>

        {canInvite && (
          <div className="invite-input-row">
            <input
              type="email"
              value={inviteEmail}
              onChange={(e) => setInviteEmail(e.target.value)}
              placeholder="Enter user email"
            />

            {foundUser && (
              <button
                type="button"
                className="invite-add-btn"
                onClick={handleAddInvite}
              >
                Add
              </button>
            )}
          </div>
        )}

        {invMsg && <div className="invite-message">{invMsg}</div>}

        <div className="event-creator">
          <span><strong>Creator:</strong></span>
          <strong>{event.creator ?? "—"}</strong>
        </div>

        <button type="submit" className="save-btn">
          Save Changes
        </button>
      </form>
    </div>
  );
}
