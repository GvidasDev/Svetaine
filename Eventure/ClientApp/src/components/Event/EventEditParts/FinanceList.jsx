import "../../../styles/App.css";

export default function FinanceList({ expenses }) {
  return (
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
  );
}
