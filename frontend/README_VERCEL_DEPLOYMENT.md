# SmartStay - Vercel Deployment Guide

## Pre-Deployment Checklist

- [x] `vercel.json` - Configured with Angular build settings
- [x] `.vercelignore` - Excludes unnecessary files
- [x] `.env.production` - Production environment variables
- [x] `package.json` - Build script configured
- [x] `angular.json` - Production build configuration

## Deployment Instructions

### Option 1: Deploy via Vercel CLI

```bash
# Install Vercel CLI (if not already installed)
npm install -g vercel

# Login to Vercel
vercel login

# Deploy
vercel --prod
```

### Option 2: Deploy via GitHub Integration

1. Push your code to GitHub
2. Go to [https://vercel.com](https://vercel.com)
3. Click "New Project"
4. Import your GitHub repository
5. Vercel will auto-detect Angular and use the `vercel.json` configuration
6. Set environment variables if needed:
   - `VITE_API_BASE_URL`
   - `VITE_STRIPE_PUBLISHABLE_KEY`
7. Click "Deploy"

### Option 3: Deploy via Vercel Dashboard

1. Go to [https://vercel.com/dashboard](https://vercel.com/dashboard)
2. Click "Add New..." → "Project"
3. Upload project files or connect Git repository
4. Configure build settings (should auto-detect)
5. Deploy

## Configuration Details

### Build Process

- **Build Command**: `npm run build`
- **Output Directory**: `dist/smart-stay/browser`
- **Environment**: Production

### Key Features

#### SPA Routing

All requests are routed to `/index.html` for Angular routing to work correctly.

#### Cache Headers

- **Static assets** (JS, CSS, images): Cached for 1 year (immutable)
- **HTML files**: Cached for 1 hour with must-revalidate
- Ensures fast subsequent page loads while keeping content fresh

#### Build Optimization

- Production bundle includes:
  - Tree-shaking of unused code
  - Minification of JS/CSS
  - Output hashing for cache busting
  - Lazy-loaded modules

## Environment Variables

Set these in Vercel project settings under "Environment Variables":

```
VITE_API_BASE_URL=https://smartstay-1.runasp.net
VITE_STRIPE_PUBLISHABLE_KEY=pk_test_51TpFaNHJqn79HpZi0L5XvsmfHDe5gCL3haTPD41u6Pu5eHIriJaTCOMZZZnMVy937LALihX7ywejurYSYX1SE3L300S84ANrvH
```

## Verifying the Deployment

After deployment:

1. **Check Build Logs**: Review Vercel build logs for any errors
2. **Test Routes**: Navigate through all pages to ensure routing works
3. **Check API Calls**: Open browser DevTools to verify API calls go to correct backend
4. **Test Features**:
   - AI Chatbot functionality
   - Authentication flows
   - Property search
   - Booking functionality
   - Payment processing

## Troubleshooting

### Build Fails

1. Check Node version compatibility (should use Node 18+)
2. Verify all dependencies are installed: `npm ci`
3. Check build output for specific errors

### Blank Page or 404 Errors

1. Verify `outputDirectory` in `vercel.json` matches your build output
2. Ensure `vercel.json` has the catch-all route rule

### API Calls Failing

1. Check `VITE_API_BASE_URL` is set correctly in environment variables
2. Verify CORS is enabled on backend API
3. Check browser console for network errors

### Slow Initial Load

1. Production build should be optimized
2. Check bundle size in build output
3. Consider enabling Vercel Analytics for performance insights

## Rollback

If deployment has issues:

```bash
vercel --prod --yes
```

Or use Vercel dashboard to revert to previous deployment.

## Additional Resources

- [Vercel Angular Documentation](https://vercel.com/docs/frameworks/angular)
- [Angular Build Documentation](https://angular.io/cli/build)
- [Vercel CLI Reference](https://vercel.com/docs/cli)
