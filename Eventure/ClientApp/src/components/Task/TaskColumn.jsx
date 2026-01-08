import React from "react";
import TaskCard from "./TaskCard";
import "../../styles/App.css";


export default function TaskColumn({ column, tasks, onAddTask, onMoveTask, onOpenTask, onDelete }) {
  
  const handleDrop = (e) => {
    const id = e.dataTransfer.getData("taskId");
    onMoveTask(parseInt(id), column.id);
  };

  return (
    <div
      className="task-column"
      onDragOver={(e) => e.preventDefault()}
      onDrop={handleDrop}
    >
    <div className="task-column-header">
        <h3 className="task-column-title">
          {column.eventTitle ?? column.name}
        </h3>

        <button
        className="column-delete-btn"
        onClick={() => onDelete(column.id)}
        >
            ×
        </button>
    </div>

      <div className="task-column-content">
        {tasks.map((t) => (
          <TaskCard key={t.id} task={t} onOpen={() => onOpenTask(t)} />
        ))}
      </div>

      <button className="task-add-btn" onClick={onAddTask}>
        + Add Task
      </button>
    </div>
  );
}
