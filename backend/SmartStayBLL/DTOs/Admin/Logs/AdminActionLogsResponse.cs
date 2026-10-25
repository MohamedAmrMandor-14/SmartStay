namespace SmartStayBLL
{
    public sealed class AdminActionLogsResponse
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public IReadOnlyList<AdminActionLogResponse> Items { get; set; } =
            new List<AdminActionLogResponse>();
    }
}