import ModalBase from "../Modal/ModalBase";

export default function EventModal({ onClose }) {
  return (
    <ModalBase title="Create Event" onClose={onClose}>
      <form className="event-form">
        <input type="text" placeholder="Event title" required />
        <textarea placeholder="Description" />
        <input type="date" />
        <button type="submit" className="submit-btn">
          Save Event
        </button>
      </form>
    </ModalBase>
  );
}
