namespace SmartStayBLL
{
    public interface IAdminActionLogService
    {
        Task<AdminActionLogResponse> CreateAsync(
            Guid adminUserId,
            CreateAdminActionLogRequest request,
            CancellationToken cancellationToken = default);

        Task<AdminActionLogsResponse> GetLogsAsync(
            AdminActionLogSearchRequest request,
            CancellationToken cancellationToken = default);

        Task<AdminActionLogResponse> GetByIdAsync(
            Guid logId,
            CancellationToken cancellationToken = default);
    }
}