namespace SmartStayBLL
{
    public sealed class AdminActionLogResponse
    {
        public Guid LogId { get; set; }

        public Guid AdminUserId { get; set; }

        public string AdminName { get; set; } =
            string.Empty;

        public string? AdminEmail { get; set; }

        public string ActionType { get; set; } =
            string.Empty;

        public string TargetType { get; set; } =
            string.Empty;

        public Guid? TargetId { get; set; }

        public string? TargetReference { get; set; }

        public string Summary { get; set; } =
            string.Empty;

        public string? Details { get; set; }

        public string? MetadataJson { get; set; }

        public string? IpAddress { get; set; }

        public string? UserAgent { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}