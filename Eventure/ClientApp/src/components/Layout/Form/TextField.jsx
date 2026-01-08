import "../../../styles/App.css";

export default function TextField({ label, name, value, required, onChange }) {
  return (
    <>
      <label>
        <strong>{label}</strong>
      </label>
      <input
        name={name}
        value={value ?? ""}
        onChange={(e) => onChange(e.target.value)}
        required={!!required}
      />
    </>
  );
}
