namespace EmployeeManagement.Caching
{
    public class CacheSettings
    {
        public int AbsoluteExpiration { get; set; }
        public int SlidingExpiration { get; set; }
        public int Size { get; set; }
    }
}