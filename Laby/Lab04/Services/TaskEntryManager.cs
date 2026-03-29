using Lab04.Models;

namespace Lab04.Services
{
    public class TaskEntryManager
    {
        private Dictionary<int, TaskEntry> _tasks = [];
        private int _taskId = 0;

        public void MarkDone(int taskNumber)
        {
            if (_tasks.ContainsKey(taskNumber) == false)
                return;

            _tasks[taskNumber].IsDone = true;
        }

        public void Add(TaskEntry task) => _tasks[_taskId++] = task;

        public List<TaskEntry> GetByCategory(TaskEntryCategory category)
            => [.. _tasks.Where(x => x.Value.Category == category).Select(x => x.Value)];

        public List<TaskEntry> GetTasks() => [.. _tasks.Select(x => x.Value)];
    }
}
