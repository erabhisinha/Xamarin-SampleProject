using SQLite;

namespace Todo.Models
{
    [Table("ToDo")]
    public class TodoItem
    {
        [PrimaryKey]
        public long ID { get; set; }
        public string Name { get; set; }
        public bool Done { get; set; }
    }
}
