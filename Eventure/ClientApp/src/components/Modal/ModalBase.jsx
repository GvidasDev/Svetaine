import "./ModalStyles.css";

export default function ModalBase({ title, children, onClose }) {
  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <header>
          <h3>{title}</h3>
          <button className="close-btn" onClick={onClose}>
            ✕
          </button>
        </header>
        <main>{children}</main>
      </div>
    </div>
  );
}
