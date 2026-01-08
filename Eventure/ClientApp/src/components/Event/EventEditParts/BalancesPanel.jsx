import "../../../styles/App.css";
import BalanceList from "./BalanceList";

export default function BalancesPanel({ balances }) {
  return (
    <div className="side-card">
      <h3 className="side-title">Debts</h3>
      <BalanceList balances={balances} />
    </div>
  );
}
