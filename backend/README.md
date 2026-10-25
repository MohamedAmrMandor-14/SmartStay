# SmartStay — Backend

ASP.NET Core 10 Web API for the SmartStay vacation-rental platform.

See the [project README](../README.md) for the full overview, architecture and contribution details.

## Solution layout

| Project | Responsibility |
| --- | --- |
| `SmartStay.Api` | Controllers, middleware, authentication, background services, Swagger |
| `SmartStayBLL` | Business services, DTOs, Stripe / Cloudinary / OAuth integrations |
| `SmartStayDAL` | EF Core entities, configurations, migrations and seed data |

## Requirements

- .NET 10 SDK
- SQL Server

## Configuration

`SmartStay.Api/appsettings.json` contains **placeholders only**. Provide real values with user secrets in development:

```bash
cd SmartStay.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=...;Database=SmartStay;User Id=...;Password=...;TrustServerCertificate=True"
dotnet user-secrets set "JwtSettings:Key" "<random 32+ byte string>"
dotnet user-secrets set "OtpSettings:HashKey" "<random 32+ byte string>"
dotnet user-secrets set "Stripe:SecretKey" "sk_test_..."
dotnet user-secrets set "Stripe:PublishableKey" "pk_test_..."
dotnet user-secrets set "Stripe:WebhookSecret" "whsec_..."
dotnet user-secrets set "Cloudinary:CloudName" "..."
dotnet user-secrets set "Cloudinary:ApiKey" "..."
dotnet user-secrets set "Cloudinary:ApiSecret" "..."
dotnet user-secrets set "SmtpSettings:Username" "..."
dotnet user-secrets set "SmtpSettings:Password" "..."
dotnet user-secrets set "Authentication:Google:ClientId" "..."
dotnet user-secrets set "Authentication:Facebook:AppId" "..."
dotnet user-secrets set "Authentication:Facebook:AppSecret" "..."
```

In production, use environment variables with `:` as the separator (for example `JwtSettings__Key`).

## Run

```bash
dotnet tool restore
dotnet ef database update --project ../SmartStayDAL
dotnet run
```

Swagger UI is served at the application root.

## Main endpoint groups

| Group | Prefix |
| --- | --- |
| Authentication | `/api/auth`, `/api/account` |
| Profile | `/api/profile` |
| Properties | `/api/properties` |
| Bookings | `/api/bookings` |
| Payments | `/api/payments`, `/api/stripe-webhooks` |
| Wishlist | `/api/wishlists` |
| Reviews | `/api/reviews`, `/api/property-reviews` |
| Host | `/api/host-*` |
| Admin | `/api/admin/*` |
| Support | `/api/support-tickets` |
| Notifications | `/api/notifications` |
