namespace DataAccess.Models
{
    public class UserChangeRequest
    {
        public string Field { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}