import "../../../styles/App.css";
import TextField from "../../Layout/Form/TextField";
import TextAreaField from "../../Layout/Form/TextAreaField";
import DateField from "../../Layout/Form/DateField";
import ImageUploader from "./ImageUploader";
import InvitedList from "./InvitedList";
import CreatorBlock from "./CreatorBlock";

export default function EventForm({
  event,
  preview,
  invited,
  onChange,
  onPickFile,
  onSubmit
}) {
  return (
    <form className="edit-form" onSubmit={onSubmit}>
      <TextField
        label="Title"
        name="title"
        value={event.title}
        required
        onChange={(v) => onChange("title", v)}
      />

      <TextAreaField
        label="Description"
        name="description"
        value={event.description}
        onChange={(v) => onChange("description", v)}
      />

      <DateField
        label="Date"
        name="date"
        value={event.date}
        required
        onChange={(v) => onChange("date", v)}
      />

      <ImageUploader preview={preview} onPickFile={onPickFile} />

      <InvitedList invited={invited} />

      <CreatorBlock creator={event.creator} />

      <button type="submit" className="save-btn">
        Save Changes
      </button>
    </form>
  );
}
