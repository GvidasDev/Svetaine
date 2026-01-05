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
      {task.title}
    </div>
  );
}
