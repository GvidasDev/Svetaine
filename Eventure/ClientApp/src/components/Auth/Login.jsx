import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import authApi from "../../api/authApi";
import "../../styles/App.css";

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
      <div className="edit-form auth-card auth-card--sm">
        <h2 className="form-title">Login</h2>

        <form onSubmit={submit} className="auth-form">
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

          {err && <small className="form-error">{err}</small>}

          <button type="submit" className="save-btn">Login</button>
        </form>

        <div className="auth-footer auth-footer--split">
          <Link to="/register" className="auth-link">Register</Link>

          <button
            className="back-btn"
            type="button"
            onClick={() => alert("Password reset flow to be implemented")}
          >
            Forgot password?
          </button>
        </div>
      </div>
    </div>
  );
}
