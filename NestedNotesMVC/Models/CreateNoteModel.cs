namespace NestedNotesMVC.Models
{
    public class CreateNoteModel
    {
        public string parentId { get; set; }
        public string title { get; set; }
        public string content { get; set; }
    }
}
