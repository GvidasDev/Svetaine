import { useState, useEffect, useCallback } from "react";
import { useNavigate, useParams } from "react-router-dom";
import eventApi from "../../api/eventApi";
import uploadApi from "../../api/uploadApi";
import invitationApi from "../../api/invitationApi";
import expenseApi from "../../api/expenseApi";
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

  const [expenses, setExpenses] = useState([]);
  const [balances, setBalances] = useState([]);

  const [amount, setAmount] = useState("");
  const [note, setNote] = useState("");
  const [finMsg, setFinMsg] = useState("");

  const fetchAll = useCallback(async () => {
    const ev = await eventApi.getById(id);
    const data = ev.data;

    setEvent(data);

    const img = data.imageUrl || "";
    setPreview(img ? (img.startsWith("http") ? img : API_ORIGIN + img) : "");

    const inv = await invitationApi.getInvited(id);
    setInvited(inv.data || []);

    const ex = await expenseApi.getAll(id);
    setExpenses(ex.data || []);

    const bal = await expenseApi.getBalances(id);
    setBalances(bal.data || []);
  }, [id]);

  useEffect(() => {
    fetchAll();
  }, [fetchAll]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    if (name === "creator") return;

    setEvent((p) => ({
      ...p,
      [name]: type === "checkbox" ? checked : value
    }));
  };

  const handleFile = async (e) => {
    const file = e.target.files?.[0];
    if (!file) return;

    setPreview(URL.createObjectURL(file));

    const res = await uploadApi.uploadImage(file);
    setEvent((p) => ({ ...p, imageUrl: res.data.url }));
  };

  const addExpense = async () => {
    setFinMsg("");

    const value = Number((amount || "").trim().replace(",", "."));
    if (!Number.isFinite(value) || value <= 0) {
      setFinMsg("Neteisinga suma.");
      return;
    }

    try {
      await expenseApi.add(id, { amount: value, note: (note || "").trim() });
      setAmount("");
      setNote("");
      await fetchAll();
    } catch {
      setFinMsg("Nepavyko pridėti išlaidų.");
    }
  };

  const handleSave = async (e) => {
    e.preventDefault();

    await eventApi.update(id, {
      title: event.title,
      description: event.description,
      date: event.date,
      imageUrl: event.imageUrl,
      isPublic: event.isPublic
    });

    navigate("/");
  };

  return (
    <div className="edit-container">
      <div className="edit-header edit-header--center">
        <button className="back-btn" onClick={() => navigate(-1)}>
          ← Back
        </button>
        <h2 className="page-title">Edit Event</h2>
      </div>

      <div className="event-edit-grid">
        <div className="side-card">
          <h3 className="side-title">Finansai</h3>

          <div className="finance-input-box">
            <input
              className="finance-input"
              value={amount}
              onChange={(e) => setAmount(e.target.value)}
              placeholder="Suma"
            />

            <input
              className="finance-input"
              value={note}
              onChange={(e) => setNote(e.target.value)}
              placeholder="Už ką"
            />

            <button
              type="button"
              className="finance-add-btn"
              onClick={addExpense}
            >
              Add
            </button>
          </div>

          {finMsg && <div className="invite-message">{finMsg}</div>}

          <div className="finance-list">
            {expenses.length === 0 ? (
              <div className="invited-empty">—</div>
            ) : (
              expenses.map((x) => (
                <div className="finance-item" key={x.id}>
                  <div className="finance-top">
                    <strong>{x.username}</strong>
                    <strong>{Number(x.amount).toFixed(2)} €</strong>
                  </div>
                  <div className="finance-sub">{x.note || "—"}</div>
                </div>
              ))
            )}
          </div>
        </div>

        <form className="edit-form" onSubmit={handleSave}>
          <label><strong>Title</strong></label>
          <input name="title" value={event.title} onChange={handleChange} required />

          <label><strong>Description</strong></label>
          <textarea name="description" value={event.description} onChange={handleChange} />

          <label><strong>Date</strong></label>
          <input
            type="date"
            name="date"
            value={event.date ? event.date.split("T")[0] : ""}
            onChange={handleChange}
            required
          />

          <label><strong>Image</strong></label>
          <div className="event-image event-image--center">
            {preview ? <img src={preview} alt="" /> : <span>IMG</span>}
          </div>

          <input type="file" accept="image/*" onChange={handleFile} />

          <label><strong>Invited Users</strong></label>
          <div className="invited-box">
            {invited.length === 0 ? (
              <div className="invited-empty">—</div>
            ) : (
              invited.map((u) => (
                <div className="invited-item" key={u.userId}>
                  <strong>{u.username}</strong>
                </div>
              ))
            )}
          </div>

          <div className="event-creator event-creator--center">
            <span><strong>Creator:</strong></span>
            <strong>{event.creator || "—"}</strong>
          </div>

          <button type="submit" className="save-btn">Save Changes</button>
        </form>

        <div className="side-card">
          <h3 className="side-title">Skolos</h3>

          <div className="balance-list">
            {balances.length === 0 ? (
              <div className="invited-empty">—</div>
            ) : (
              balances.map((b) => (
                <div className="balance-item" key={b.userId}>
                  <div className="finance-top">
                    <strong>{b.username}</strong>
                    <strong>{Number(b.owes).toFixed(2)} €</strong>
                  </div>
                  <div className="finance-sub">
                    Sumokėjo: {Number(b.paid).toFixed(2)} € · Dalies suma: {Number(b.share).toFixed(2)} €
                  </div>
                </div>
              ))
            )}
          </div>
        </div>
      </div>
    </div>
  );
}
