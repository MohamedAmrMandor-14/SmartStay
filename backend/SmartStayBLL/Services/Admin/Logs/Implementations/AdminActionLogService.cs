using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartStayDAL;

namespace SmartStayBLL
{
    public sealed class AdminActionLogService
        : IAdminActionLogService
    {
        private const int MaximumPageSize =
            100;

        private const int MaximumTargetReferenceLength =
            200;

        private const int MaximumSummaryLength =
            500;

        private const int MaximumDetailsLength =
            4000;

        private const int MaximumMetadataJsonLength =
            8000;

        private const int MaximumIpAddressLength =
            64;

        private const int MaximumUserAgentLength =
            512;

        private readonly SmartStayDbContext
            _dbContext;

        private readonly UserManager<ApplicationUser>
            _userManager;

        public AdminActionLogService(
            SmartStayDbContext dbContext,
            UserManager<ApplicationUser> userManager)
        {
            ArgumentNullException.ThrowIfNull(
                dbContext);

            ArgumentNullException.ThrowIfNull(
                userManager);

            _dbContext =
                dbContext;

            _userManager =
                userManager;
        }

        public async Task<AdminActionLogResponse> CreateAsync(
            Guid adminUserId,
            CreateAdminActionLogRequest request,
            CancellationToken cancellationToken = default)
        {
            ValidateUserIdentifier(
                adminUserId);

            ArgumentNullException.ThrowIfNull(
                request);

            await EnsureActiveAdminExistsAsync(
                adminUserId,
                cancellationToken);

            var actionType =
                ParseActionType(
                    request.ActionType);

            var targetType =
                ParseTargetType(
                    request.TargetType);

            var targetReference =
                NormalizeOptionalText(
                    request.TargetReference,
                    MaximumTargetReferenceLength);

            var summary =
                NormalizeRequiredText(
                    request.Summary,
                    "The admin action summary is required.",
                    MaximumSummaryLength);

            var details =
                NormalizeOptionalText(
                    request.Details,
                    MaximumDetailsLength);

            var metadataJson =
                NormalizeOptionalText(
                    request.MetadataJson,
                    MaximumMetadataJsonLength);

            var ipAddress =
                NormalizeOptionalText(
                    request.IpAddress,
                    MaximumIpAddressLength);

            var userAgent =
                NormalizeOptionalText(
                    request.UserAgent,
                    MaximumUserAgentLength);

            var currentTime =
                DateTimeOffset.UtcNow;

            var log =
                new AdminActionLog
                {
                    Id =
                        Guid.NewGuid(),

                    AdminUserId =
                        adminUserId,

                    ActionType =
                        actionType,

                    TargetType =
                        targetType,

                    TargetId =
                        request.TargetId,

                    TargetReference =
                        targetReference,

                    Summary =
                        summary,

                    Details =
                        details,

                    MetadataJson =
                        metadataJson,

                    IpAddress =
                        ipAddress,

                    UserAgent =
                        userAgent,

                    CreatedAt =
                        currentTime
                };

            await _dbContext
                .Set<AdminActionLog>()
                .AddAsync(
                    log,
                    cancellationToken);

            await _dbContext.SaveChangesAsync(
                cancellationToken);

            return await GetByIdAsync(
                log.Id,
                cancellationToken);
        }

        public async Task<AdminActionLogsResponse> GetLogsAsync(
            AdminActionLogSearchRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(
                request);

            var page =
                NormalizePage(
                    request.Page);

            var pageSize =
                NormalizePageSize(
                    request.PageSize);

            var query =
                _dbContext
                    .Set<AdminActionLog>()
                    .AsNoTracking()
                    .Include(
                        log =>
                            log.AdminUser)
                    .AsQueryable();

            if (request.AdminUserId.HasValue)
            {
                if (request.AdminUserId.Value == Guid.Empty)
                {
                    throw new ArgumentException(
                        "The admin user identifier is invalid.");
                }

                query =
                    query.Where(
                        log =>
                            log.AdminUserId ==
                            request.AdminUserId.Value);
            }

            if (!string.IsNullOrWhiteSpace(
                    request.ActionType))
            {
                var actionType =
                    ParseActionType(
                        request.ActionType);

                query =
                    query.Where(
                        log =>
                            log.ActionType == actionType);
            }

            if (!string.IsNullOrWhiteSpace(
                    request.TargetType))
            {
                var targetType =
                    ParseTargetType(
                        request.TargetType);

                query =
                    query.Where(
                        log =>
                            log.TargetType == targetType);
            }

            if (request.TargetId.HasValue)
            {
                if (request.TargetId.Value == Guid.Empty)
                {
                    throw new ArgumentException(
                        "The target identifier is invalid.");
                }

                query =
                    query.Where(
                        log =>
                            log.TargetId ==
                            request.TargetId.Value);
            }

            if (request.From.HasValue)
            {
                query =
                    query.Where(
                        log =>
                            log.CreatedAt >= request.From.Value);
            }

            if (request.To.HasValue)
            {
                query =
                    query.Where(
                        log =>
                            log.CreatedAt <= request.To.Value);
            }

            if (request.From.HasValue
                &&
                request.To.HasValue
                &&
                request.From.Value > request.To.Value)
            {
                throw new ArgumentException(
                    "The start date cannot be after the end date.");
            }

            if (!string.IsNullOrWhiteSpace(
                    request.Search))
            {
                var search =
                    request.Search.Trim();

                var likePattern =
                    $"%{search}%";

                query =
                    query.Where(
                        log =>
                            EF.Functions.Like(
                                log.Summary,
                                likePattern)
                            ||
                            log.Details != null
                            &&
                            EF.Functions.Like(
                                log.Details,
                                likePattern)
                            ||
                            log.TargetReference != null
                            &&
                            EF.Functions.Like(
                                log.TargetReference,
                                likePattern)
                            ||
                            log.AdminUser.Email != null
                            &&
                            EF.Functions.Like(
                                log.AdminUser.Email,
                                likePattern));
            }

            var totalCount =
                await query.CountAsync(
                    cancellationToken);

            var logs =
                await query
                    .OrderByDescending(
                        log =>
                            log.CreatedAt)
                    .Skip(
                        (page - 1) * pageSize)
                    .Take(
                        pageSize)
                    .ToListAsync(
                        cancellationToken);

            var items =
                logs
                    .Select(
                        MapToResponse)
                    .ToList();

            return new AdminActionLogsResponse
            {
                Page =
                    page,

                PageSize =
                    pageSize,

                TotalCount =
                    totalCount,

                TotalPages =
                    CalculateTotalPages(
                        totalCount,
                        pageSize),

                Items =
                    items
            };
        }

        public async Task<AdminActionLogResponse> GetByIdAsync(
            Guid logId,
            CancellationToken cancellationToken = default)
        {
            ValidateLogIdentifier(
                logId);

            var log =
                await _dbContext
                    .Set<AdminActionLog>()
                    .AsNoTracking()
                    .Include(
                        item =>
                            item.AdminUser)
                    .SingleOrDefaultAsync(
                        item =>
                            item.Id == logId,
                        cancellationToken);

            if (log is null)
            {
                throw new KeyNotFoundException(
                    "The admin action log was not found.");
            }

            return MapToResponse(
                log);
        }

        private async Task EnsureActiveAdminExistsAsync(
            Guid adminUserId,
            CancellationToken cancellationToken)
        {
            var adminUser =
                await _dbContext.Users
                    .SingleOrDefaultAsync(
                        user =>
                            user.Id == adminUserId
                            &&
                            user.IsActive,
                        cancellationToken);

            if (adminUser is null)
            {
                throw new UnauthorizedAccessException(
                    "The admin user was not found or is inactive.");
            }

            var isAdmin =
                await _userManager.IsInRoleAsync(
                    adminUser,
                    RoleNames.Admin);

            if (!isAdmin)
            {
                throw new UnauthorizedAccessException(
                    "Only admins can create admin action logs.");
            }
        }

        private static AdminActionLogResponse MapToResponse(
            AdminActionLog log)
        {
            return new AdminActionLogResponse
            {
                LogId =
                    log.Id,

                AdminUserId =
                    log.AdminUserId,

                AdminName =
                    BuildFullName(
                        log.AdminUser.FirstName,
                        log.AdminUser.LastName,
                        log.AdminUser.Email),

                AdminEmail =
                    log.AdminUser.Email,

                ActionType =
                    log.ActionType.ToString(),

                TargetType =
                    log.TargetType.ToString(),

                TargetId =
                    log.TargetId,

                TargetReference =
                    log.TargetReference,

                Summary =
                    log.Summary,

                Details =
                    log.Details,

                MetadataJson =
                    log.MetadataJson,

                IpAddress =
                    log.IpAddress,

                UserAgent =
                    log.UserAgent,

                CreatedAt =
                    log.CreatedAt
            };
        }

        private static AdminActionType ParseActionType(
            string value)
        {
            if (Enum.TryParse<AdminActionType>(
                    value,
                    ignoreCase: true,
                    out var actionType)
                &&
                Enum.IsDefined(
                    actionType))
            {
                return actionType;
            }

            throw new ArgumentException(
                "The admin action type is invalid.");
        }

        private static AdminActionTargetType ParseTargetType(
            string value)
        {
            if (Enum.TryParse<AdminActionTargetType>(
                    value,
                    ignoreCase: true,
                    out var targetType)
                &&
                Enum.IsDefined(
                    targetType))
            {
                return targetType;
            }

            throw new ArgumentException(
                "The admin action target type is invalid.");
        }

        private static void ValidateUserIdentifier(
            Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new UnauthorizedAccessException(
                    "The access token does not contain a valid user identifier.");
            }
        }

        private static void ValidateLogIdentifier(
            Guid logId)
        {
            if (logId == Guid.Empty)
            {
                throw new ArgumentException(
                    "The admin action log identifier is invalid.");
            }
        }

        private static string NormalizeRequiredText(
            string? value,
            string errorMessage,
            int maximumLength)
        {
            if (string.IsNullOrWhiteSpace(
                    value))
            {
                throw new ArgumentException(
                    errorMessage);
            }

            var normalizedValue =
                value.Trim();

            if (normalizedValue.Length >
                maximumLength)
            {
                throw new ArgumentException(
                    $"The value cannot exceed {maximumLength} characters.");
            }

            return normalizedValue;
        }

        private static string? NormalizeOptionalText(
            string? value,
            int maximumLength)
        {
            if (string.IsNullOrWhiteSpace(
                    value))
            {
                return null;
            }

            var normalizedValue =
                value.Trim();

            if (normalizedValue.Length >
                maximumLength)
            {
                throw new ArgumentException(
                    $"The value cannot exceed {maximumLength} characters.");
            }

            return normalizedValue;
        }

        private static int NormalizePage(
            int page)
        {
            return page <= 0
                ? 1
                : page;
        }

        private static int NormalizePageSize(
            int pageSize)
        {
            if (pageSize <= 0)
            {
                return 20;
            }

            return pageSize > MaximumPageSize
                ? MaximumPageSize
                : pageSize;
        }

        private static int CalculateTotalPages(
            int totalCount,
            int pageSize)
        {
            if (totalCount <= 0)
            {
                return 0;
            }

            return (int)Math.Ceiling(
                totalCount / (double)pageSize);
        }

        private static string BuildFullName(
            string? firstName,
            string? lastName,
            string? fallback)
        {
            var fullName =
                string.Join(
                    " ",
                    new[]
                    {
                        firstName,
                        lastName
                    }
                    .Where(
                        value =>
                            !string.IsNullOrWhiteSpace(
                                value))
                    .Select(
                        value =>
                            value!.Trim()));

            if (!string.IsNullOrWhiteSpace(
                    fullName))
            {
                return fullName;
            }

            return fallback
                ??
                "Unknown Admin";
        }
    }
}