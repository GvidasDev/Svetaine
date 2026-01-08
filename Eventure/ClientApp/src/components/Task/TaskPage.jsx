import React, { useEffect, useState } from "react";
import taskApi from "../../api/taskApi";
import eventApi from "../../api/eventApi";
import taskStateApi from "../../api/taskStateApi";

import TaskColumn from "./TaskColumn";
import TaskModal from "../Modal/TaskModal";
import AddColumnModal from "../Modal/AddColumnModal";

import "../../styles/App.css";

export default function TasksPage() {
  const [tasks, setTasks] = useState([]);
  const [events, setEvents] = useState([]);
  const [columns, setColumns] = useState([]);

  const [openTaskModal, setOpenTaskModal] = useState(null);
  const [showAddColumn, setShowAddColumn] = useState(false);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    const t = await taskApi.getAll();
    const e = await eventApi.getAll();
    const c = await taskStateApi.getAll();

    setTasks(t.data);
    setEvents(e.data);
    setColumns(c.data);
  };

  const handleCreateTask = async (column, data) => {
  await taskApi.create({
    title: data.title,
    description: data.description,
    taskStatus: column.id,
    eventId: column.eventId ?? data.eventId ?? null
  });

  loadData();
};

  const handleMoveTask = async (taskId, newStatus) => {
    const t = tasks.find(x => x.id === taskId);
    if (!t) return;

    const payload = {
      title: t.title,
      description: t.description,
      taskStatus: newStatus,
      eventId: t.eventId ?? null
    };

    await taskApi.update(taskId, payload);
    loadData();
  };

  const handleCreateColumn = async (name) => {
    await taskStateApi.create({ name });
    loadData();
  };

  const handleDeleteColumn = async (columnId) => {
    const hasTasks = tasks.some(t => t.taskStatus === columnId);

    if (hasTasks) {
      alert("Negalima ištrinti kolonos, nes ji turi priskirtų užduočių.");
      return;
    }

    await taskStateApi.delete(columnId);
    loadData();
  };

  return (
    <div className="tasks-page">
      <div className="task-columns-wrapper">
        {columns.map(col => (
          <TaskColumn
            key={col.id}
            column={col}
            tasks={tasks.filter(t => t.taskStatus === col.id)}
            onAddTask={() => setOpenTaskModal({ create: true, column: col })}
            onMoveTask={handleMoveTask}
            onOpenTask={(t) => setOpenTaskModal({ create: false, task: t })}
            onDelete={handleDeleteColumn}
          />
        ))}

        <button
          className="add-column-btn"
          onClick={() => setShowAddColumn(true)}
        >
          +
        </button>
      </div>

      {showAddColumn && (
        <AddColumnModal
          onClose={() => setShowAddColumn(false)}
          onCreate={(name) => {
            handleCreateColumn(name);
            setShowAddColumn(false);
          }}
        />
      )}

      {openTaskModal && (
        <TaskModal
          payload={openTaskModal}
          events={events}
          onClose={() => setOpenTaskModal(null)}
          onCreate={async (data) => {
            await handleCreateTask(openTaskModal.column, data);
            setOpenTaskModal(null);
          }}
          onSave={async (id, updated) => {
            await taskApi.update(id, updated);
            setOpenTaskModal(null);
            loadData();
          }}
          onDelete={async (id) => {
            await taskApi.delete(id);
            setOpenTaskModal(null);
            loadData();
          }}
        />
      )}
    </div>
  );
}
