using EmployeeManagement.Models;

namespace EmployeeManagement.Caching
{
    public static class CacheKeys
    {
        public static string Employee(string? term, string? sort, int page, int pageSize) =>
                   $"Employee_{term ?? "all"}_{sort ?? "default"}_{page}_{pageSize}";

        public static string Employee(FilteringRequest request, int defaultPage, int defaultPageSize)
        {
            int finalPage = request.Page > 0 ? request.Page : defaultPage;
            int finalPageSize = request.PageSize > 0 ? request.PageSize : defaultPageSize;
            return Employee(request.Term, request.Sort, finalPage, finalPageSize);
        }

        public static string EmployeeById(int id) => $"Employee_{id}";
    }
}
