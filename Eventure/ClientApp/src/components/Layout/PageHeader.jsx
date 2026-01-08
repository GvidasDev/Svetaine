import "../../styles/App.css";

export default function PageHeader({ title, onBack }) {
  return (
    <div className="edit-header edit-header--center">
      <button className="back-btn" onClick={onBack}>
        ← Back
      </button>
      <h2 className="page-title">{title}</h2>
    </div>
  );
}
