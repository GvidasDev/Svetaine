import { useState, useEffect, useCallback } from "react";
import { useNavigate, useParams } from "react-router-dom";
import eventApi from "../../api/eventApi";
import uploadApi from "../../api/uploadApi";
import invitationApi from "../../api/invitationApi";
import expenseApi from "../../api/expenseApi";
import { API_ORIGIN } from "../../api/apiClient";
import "../../styles/App.css";

import PageHeader from "../Layout/PageHeader";
import EventForm from "./EventEditParts/EventForm";
import FinancePanel from "./EventEditParts/FinancePanel";
import BalancesPanel from "./EventEditParts/BalancesPanel";

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

  const handleEventChange = (name, value) => {
    if (name === "creator") return;

    setEvent((p) => ({
      ...p,
      [name]: value
    }));
  };

  const handleFile = async (file) => {
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
      <PageHeader title="Edit Event" onBack={() => navigate(-1)} />

      <div className="event-edit-grid">
        <FinancePanel
          amount={amount}
          note={note}
          onAmountChange={setAmount}
          onNoteChange={setNote}
          onAdd={addExpense}
          message={finMsg}
          expenses={expenses}
        />

        <EventForm
          event={event}
          preview={preview}
          invited={invited}
          onChange={handleEventChange}
          onPickFile={handleFile}
          onSubmit={handleSave}
        />

        <BalancesPanel balances={balances} />
      </div>
    </div>
  );
}
