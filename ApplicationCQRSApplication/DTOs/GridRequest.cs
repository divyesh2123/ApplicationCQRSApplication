namespace ApplicationCQRSApplication.DTOs
{
    public class GridRequest
    {
        public string? Search { get; set; }

        public string? SortColumn { get; set; }

        public string SortDirection { get; set; } = "asc";

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
