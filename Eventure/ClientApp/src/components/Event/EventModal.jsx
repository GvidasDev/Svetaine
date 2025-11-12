import ModalBase from "../Modal/ModalBase";
import EventForm from "./EventForm";
import "../../styles/App.css";


export default function EventModal({ onClose, onSuccess }) {
  return (
    <ModalBase onClose={onClose}>
      <EventForm
        onSubmitSuccess={() => {
          onSuccess();
          onClose();
        }}
      />
    </ModalBase>
  );
}
