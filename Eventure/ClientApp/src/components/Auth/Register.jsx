import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import authApi from "../../api/authApi";
import "../../styles/App.css";

export default function Register() {
  const navigate = useNavigate();
  const [form, setForm] = useState({
    username: "",
    email: "",
    firstName: "",
    lastName: "",
    password: "",
    confirmPassword: ""
  });
  const [err, setErr] = useState("");

  const submit = async (e) => {
    e.preventDefault();
    setErr("");
    try {
      const res = await authApi.register(form);
      localStorage.setItem("token", res.data.token);
      localStorage.setItem("username", res.data.username);
      navigate("/");
    } catch (ex) {
      setErr("Registration failed.");
    }
  };

  return (
    <div className="edit-container">
      <div className="edit-form auth-card auth-card--md">
        <h2 className="form-title">Register</h2>

        <form onSubmit={submit} className="auth-form">
          <input
            type="text"
            placeholder="Username"
            value={form.username}
            onChange={(e) => setForm({ ...form, username: e.target.value })}
            required
          />
          <input
            type="email"
            placeholder="Email"
            value={form.email}
            onChange={(e) => setForm({ ...form, email: e.target.value })}
            required
          />
          <input
            type="text"
            placeholder="First name"
            value={form.firstName}
            onChange={(e) => setForm({ ...form, firstName: e.target.value })}
            required
          />
          <input
            type="text"
            placeholder="Last name"
            value={form.lastName}
            onChange={(e) => setForm({ ...form, lastName: e.target.value })}
            required
          />
          <input
            type="password"
            placeholder="Password"
            value={form.password}
            onChange={(e) => setForm({ ...form, password: e.target.value })}
            required
          />
          <input
            type="password"
            placeholder="Confirm password"
            value={form.confirmPassword}
            onChange={(e) => setForm({ ...form, confirmPassword: e.target.value })}
            required
          />

          {err && <small className="form-error">{err}</small>}

          <button type="submit" className="save-btn">Create account</button>
        </form>

        <div className="auth-footer auth-footer--right">
          <Link to="/login" className="auth-link">
            Back to login
          </Link>
        </div>
      </div>
    </div>
  );
}
