using System.ComponentModel.DataAnnotations;

namespace Lab04.Models
{
    public enum TaskEntryCategory
    {
        Planowane,
        wTrakcie,
        Ukończone
    }

    public class TaskEntry
    {
        [Required(ErrorMessage = "Należy podać tytuł")]
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskEntryCategory Category { get; set; }
        public bool IsDone { get; set; }
    }
}
