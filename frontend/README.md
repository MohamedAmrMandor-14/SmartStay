# SmartStay — Frontend

Angular 21 single-page application for the SmartStay vacation-rental platform.

See the [project README](../README.md) for the full overview, architecture and contribution details.

## Requirements

- Node.js 20+
- npm

## Development

```bash
npm ci
npm start
```

Open <http://localhost:4200>. The API base URL and Stripe publishable key are defined in `src/environments/environment.ts`.

## Build

```bash
npm run build
```

Production output is written to `dist/`.

## Structure

| Path | Purpose |
| --- | --- |
| `src/app/Core` | Route guards and HTTP interceptors |
| `src/app/Features` | Guest, host, admin and account feature areas |
| `src/app/Shared` | Reusable components and services |
| `src/environments` | Environment configuration |
| `public` | Static assets |
