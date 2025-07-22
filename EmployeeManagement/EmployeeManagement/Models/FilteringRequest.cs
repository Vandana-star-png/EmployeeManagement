namespace EmployeeManagement.Models
{
    public class FilteringRequest
    {
        public string? Term {  get; set; }
        public string? Sort {  get; set; }
        public int Page {  get; set; }
        public int PageSize {  get; set; }
    }
}
