namespace NestedNotesMVC.Models
{
    public class NotesVM
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<NotesVM> Notes { get; set; }

        public NotesVM()
        {
            Notes = new List<NotesVM>();
        }
    }
}
