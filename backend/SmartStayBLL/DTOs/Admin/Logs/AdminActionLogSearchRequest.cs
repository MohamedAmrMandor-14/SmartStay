namespace SmartStayBLL
{
    public sealed class AdminActionLogSearchRequest
    {
        public int Page { get; set; } =
            1;

        public int PageSize { get; set; } =
            20;

        public Guid? AdminUserId { get; set; }

        public string? ActionType { get; set; }

        public string? TargetType { get; set; }

        public Guid? TargetId { get; set; }

        public string? Search { get; set; }

        public DateTimeOffset? From { get; set; }

        public DateTimeOffset? To { get; set; }
    }
}