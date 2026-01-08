import "../../../styles/App.css";

export default function DateField({ label, name, value, required, onChange }) {
  const v = value ? value.split("T")[0] : "";

  return (
    <>
      <label>
        <strong>{label}</strong>
      </label>
      <input
        type="date"
        name={name}
        value={v}
        onChange={(e) => onChange(e.target.value)}
        required={!!required}
      />
    </>
  );
}
