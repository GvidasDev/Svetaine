import "../../../styles/App.css";

export default function BalanceList({ balances }) {
  return (
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
              Paid: {Number(b.paid).toFixed(2)} € <br/> All debt:{" "}
              {Number(b.share).toFixed(2)} €
            </div>
          </div>
        ))
      )}
    </div>
  );
}
