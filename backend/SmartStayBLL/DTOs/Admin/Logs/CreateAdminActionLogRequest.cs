namespace SmartStayBLL
{
    public sealed class CreateAdminActionLogRequest
    {
        public string ActionType { get; set; } =
            "Other";

        public string TargetType { get; set; } =
            "System";

        public Guid? TargetId { get; set; }

        public string? TargetReference { get; set; }

        public string Summary { get; set; } =
            string.Empty;

        public string? Details { get; set; }

        public string? MetadataJson { get; set; }

        public string? IpAddress { get; set; }

        public string? UserAgent { get; set; }
    }
}