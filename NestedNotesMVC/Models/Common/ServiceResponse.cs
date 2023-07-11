namespace NestedNotesMVC.Models.Common
{
    public class ServiceResponse<T>
    {
        public T Value { get; set; }
        public string Id { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }

    }
}
