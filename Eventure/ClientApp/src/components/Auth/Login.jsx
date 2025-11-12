import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import authApi from "../../api/authApi";

export default function Login() {
  const navigate = useNavigate();
  const [form, setForm] = useState({ username: "", password: "" });
  const [err, setErr] = useState("");

  const submit = async (e) => {
    e.preventDefault();
    setErr("");
    try {
      const res = await authApi.login(form);
      localStorage.setItem("token", res.data.token);
      localStorage.setItem("username", res.data.username);
      navigate("/");
    } catch (ex) {
      setErr("Invalid credentials.");
    }
  };

  return (
    <div className="edit-container">
      <div className="edit-form" style={{ maxWidth: 420 }}>
        <h2 style={{ marginTop: 0 }}>Login</h2>

        <form onSubmit={submit} className="event-form" style={{ gap: "0.8rem" }}>
          <input
            type="text"
            placeholder="Username"
            value={form.username}
            onChange={(e) => setForm({ ...form, username: e.target.value })}
            required
          />
          <input
            type="password"
            placeholder="Password"
            value={form.password}
            onChange={(e) => setForm({ ...form, password: e.target.value })}
            required
          />

          {err && <small style={{ color: "var(--accent-mid)" }}>{err}</small>}

          <button type="submit" className="save-btn">Login</button>
        </form>

        <div style={{ marginTop: "0.75rem", display: "flex", justifyContent: "space-between" }}>
          <Link to="/register" style={{ color: "var(--accent-mid)" }}>Register</Link>
          <button
            className="back-btn"
            onClick={() => alert("Password reset flow to be implemented")}
          >
            Forgot password?
          </button>
        </div>
      </div>
    </div>
  );
}
