import React, { useEffect, useState } from "react";
import invitationApi from "../../api/invitationApi";
import "../../styles/App.css";

export default function InvitePanel({ eventId, canInvite, onChanged }) {
  const [invited, setInvited] = useState([]);
  const [inviteEmail, setInviteEmail] = useState("");
  const [foundUser, setFoundUser] = useState(null);
  const [msg, setMsg] = useState("");

  const loadInvited = async () => {
    const res = await invitationApi.getInvited(eventId);
    setInvited(res.data || []);
  };

  useEffect(() => {
    loadInvited();
  }, [eventId]);

  useEffect(() => {
    const run = async () => {
      setMsg("");
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

  const addInvite = async () => {
    setMsg("");

    const email = (inviteEmail || "").trim();
    if (!email) return;
    if (!foundUser) return;

    const already = invited.some((x) => x.id === foundUser.id);
    if (already) {
      setMsg("Šis vartotojas jau pakviestas.");
      return;
    }

    try {
      await invitationApi.add(eventId, email);
      setInviteEmail("");
      setFoundUser(null);
      await loadInvited();
      if (onChanged) onChanged();
      setMsg("Pakviesta.");
    } catch {
      setMsg("Nepavyko pakviesti.");
    }
  };

  const removeInvite = async (userId) => {
    setMsg("");

    try {
      await invitationApi.remove(eventId, userId);
      await loadInvited();
      if (onChanged) onChanged();
    } catch {
      setMsg("Nepavyko pašalinti.");
    }
  };

  return (
    <div className="invite-panel">
      <div className="invited-box">
        <div className="invite-input-row">
          <input
            type="email"
            className="invite-input"
            value={inviteEmail}
            onChange={(e) => setInviteEmail(e.target.value)}
            placeholder="Enter user email"
            disabled={!canInvite}
          />

          {canInvite && foundUser && (
            <button
              type="button"
              className="invite-add-btn"
              onClick={addInvite}
            >
              Add
            </button>
          )}
        </div>

        <div className="invited-box">
          {invited.length === 0 ? (
            <div className="invited-empty">—</div>
          ) : (
            invited.map((u) => (
              <div className="invited-item" key={u.id}>
                <span className="invited-name">
                  <strong>{u.username}</strong>
                </span>

                {canInvite && (
                  <button
                    type="button"
                    className="invited-remove"
                    onClick={() => removeInvite(u.id)}
                  >
                    ×
                  </button>
                )}
              </div>
            ))
          )}
        </div>
      </div>

      {msg && <div className="invite-message">{msg}</div>}
    </div>
  );
}
