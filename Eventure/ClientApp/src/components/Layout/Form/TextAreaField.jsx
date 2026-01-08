import "../../../styles/App.css";

export default function TextAreaField({ label, name, value, onChange }) {
  return (
    <>
      <label>
        <strong>{label}</strong>
      </label>
      <textarea
        name={name}
        value={value ?? ""}
        onChange={(e) => onChange(e.target.value)}
      />
    </>
  );
}
