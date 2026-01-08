import "../../../styles/App.css";
import FinanceList from "./FinanceList";

export default function FinancePanel({
  amount,
  note,
  onAmountChange,
  onNoteChange,
  onAdd,
  message,
  expenses
}) {
  return (
    <div className="side-card">
      <h3 className="side-title">Finances</h3>

      <div className="finance-input-box">
        <input
          className="finance-input"
          value={amount}
          onChange={(e) => onAmountChange(e.target.value)}
          placeholder="Amount"
        />

        <input
          className="finance-input"
          value={note}
          onChange={(e) => onNoteChange(e.target.value)}
          placeholder="Purpose"
        />

        <button type="button" className="finance-add-btn" onClick={onAdd}>
          Add
        </button>
      </div>

      {message ? <div className="invite-message">{message}</div> : null}

      <FinanceList expenses={expenses} />
    </div>
  );
}
