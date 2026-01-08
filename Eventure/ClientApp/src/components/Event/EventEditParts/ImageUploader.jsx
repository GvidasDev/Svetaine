import "../../../styles/App.css";

export default function ImageUploader({ preview, onPickFile }) {
  return (
    <>
      <label>
        <strong>Image</strong>
      </label>

      <div className="event-image event-image--center">
        {preview ? <img src={preview} alt="" /> : <span>IMG</span>}
      </div>

      <input
        type="file"
        accept="image/*"
        onChange={(e) => onPickFile(e.target.files?.[0] || null)}
      />
    </>
  );
}
