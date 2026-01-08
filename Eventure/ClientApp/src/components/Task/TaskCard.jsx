import React from "react";
import "../../styles/App.css";

export default function TaskCard({ task, onOpen }) {
  const drag = (e) => {
    e.dataTransfer.setData("taskId", task.id);
  };

  return (
    <div
      className="task-card"
      draggable
      onDragStart={drag}
      onClick={onOpen}
    >
      <div className="task-card-title">{task.title}</div>
      {task.eventTitle && <div className="task-card-sub">{task.eventTitle}</div>}
    </div>
  );
}
