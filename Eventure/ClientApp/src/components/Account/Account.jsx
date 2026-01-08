import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import accountApi from "../../api/accountApi";
import "../../styles/App.css";

export default function Account() {
  const navigate = useNavigate();

  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);

  const [form, setForm] = useState({
    username: "",
    email: "",
    firstName: "",
    lastName: ""
  });

  const [message, setMessage] = useState("");

  useEffect(() => {
    loadMe();
  }, []);

  const loadMe = async () => {
    try {
      setLoading(true);
      const res = await accountApi.getMe();
      const u = res.data;

      setForm({
        username: u.username ?? "",
        email: u.email ?? "",
        firstName: u.firstName ?? "",
        lastName: u.lastName ?? ""
      });
    } catch (e) {
      setMessage("Nepavyko užkrauti profilio duomenų.");
    } finally {
      setLoading(false);
    }
  };

  const onChange = (key) => (e) => {
    setForm((p) => ({ ...p, [key]: e.target.value }));
  };

  const onSave = async (e) => {
    e.preventDefault();
    setMessage("");

    try {
      setSaving(true);
      await accountApi.updateMe({
        email: form.email,
        firstName: form.firstName,
        lastName: form.lastName
      });

      navigate("/");
    } catch (e) {
      setMessage("Nepavyko išsaugoti. Patikrink email (ar ne užimtas) ir bandyk dar kartą.");
    } finally {
      setSaving(false);
    }
  };

  const onLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("username");
    navigate("/login");
  };

  return (
    <div className="edit-container">
      <div className="edit-header edit-header--center">
        <h2 className="page-title">Account</h2>
      </div>

      {loading ? (
        <div className="edit-form">
          <p className="muted-text">Kraunama...</p>
        </div>
      ) : (
        <form className="edit-form" onSubmit={onSave}>
          <label><strong>Username</strong></label>
          <input value={form.username} disabled placeholder="Username" />

          <label><strong>email</strong></label>
          <input
            value={form.email}
            onChange={onChange("email")}
            placeholder="Email"
            type="email"
            required
          />

          <label><strong>First name</strong></label>
          <input
            value={form.firstName}
            onChange={onChange("firstName")}
            placeholder="First name"
            required
          />

          <label><strong>Last name</strong></label>
          <input
            value={form.lastName}
            onChange={onChange("lastName")}
            placeholder="Last name"
            required
          />

          <button className="save-btn" type="submit" disabled={saving}>
            {saving ? "Saving..." : "Save changes"}
          </button>

          <button
            type="button"
            className="logout-btn"
            onClick={onLogout}>
            Log out
          </button>

          {message && <p className="muted-text">{message}</p>}
        </form>
      )}
    </div>
  );
}
