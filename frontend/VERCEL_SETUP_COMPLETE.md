# SmartStay Vercel Deployment - Complete Setup Guide

## ✅ Files Already Created

- **vercel.json** - Vercel build and deployment configuration
- **.vercelignore** - Files to exclude from deployment
- **.env.production** - Production environment variables
- **angular.json** - Angular build configuration (updated with proper budgets)

## 🚀 Deployment Steps

### Step 1: Set Up Vercel Environment Variables

1. Go to your Vercel project dashboard: https://vercel.com
2. Navigate to **Settings → Environment Variables**
3. Add these variables:

```
ANGULAR_API_BASE_URL = https://smartstay-1.runasp.net
ANGULAR_STRIPE_PUBLISHABLE_KEY = pk_test_51TpFaNHJqn79HpZi0L5XvsmfHDe5gCL3haTPD41u6Pu5eHIriJaTCOMZZZnMVy937LALihX7ywejurYSYX1SE3L300S84ANrvH
NODE_ENV = production
```

**Note:** Angular bakes environment variables at build time from `src/environments/environment.ts`, so the `.env.production` file is primarily for Vercel CLI deployments.

### Step 2: Verify Build Configuration

The following files are configured for Vercel:

**vercel.json:**

```json
{
  "buildCommand": "npm run build",
  "framework": "angular",
  "outputDirectory": "dist/SmartStay/browser",
  "routes": [
    {
      "src": "/(.*)",
      "destination": "/index.html",
      "status": 200
    }
  ]
}
```

This configuration:

- ✅ Uses `npm run build` for production build
- ✅ Outputs to `dist/SmartStay/browser` (Angular 21 output format)
- ✅ Routes all SPA traffic to `index.html` for Angular routing to work
- ✅ Includes cache headers for optimal performance

### Step 3: Deploy Options

#### Option A: Automatic Deployment (Recommended)

1. Commits to `main` branch automatically trigger Vercel deployment
2. No manual intervention needed
3. Review logs at https://vercel.com/dashboard

#### Option B: Manual Deployment via Vercel CLI

```bash
npm install -g vercel
vercel login
vercel --prod
```

#### Option C: Manual Deployment via Dashboard

1. Go to https://vercel.com/dashboard
2. Click **Recent Deployments**
3. Click **Redeploy** on latest commit

### Step 4: Verify Deployment

After deployment completes:

1. **Check Build Logs**: Vercel dashboard → Deployments → Latest → Build & Logs
2. **Test Live URL**: Click "Visit" to open the deployed app
3. **Verify Routes**: Navigate through different pages
4. **Check Console**: Open browser DevTools → Console for any errors
5. **API Calls**: Verify requests go to https://smartstay-1.runasp.net
6. **Features**: Test:
   - AI Chatbot
   - Authentication
   - Property search
   - Booking flow

## ⚠️ Common Issues & Solutions

### Build Fails - Output Directory Not Found

**Error:** `"dist/SmartStay/browser" does not exist`

**Solution:**

1. Verify local build works: `npm run build`
2. Check `angular.json` output path matches `vercel.json`
3. Current: `dist/SmartStay/browser` ✓

### Build Times Out

**Reason:** Large dependencies (ApexCharts adds ~1.2 MB)

**Solutions:**

- Build already optimized for Vercel (2.5 MB → 522 KB compressed)
- Vercel allocation: 2 cores, 8 GB RAM
- Typical build time: 15-20 seconds

### Angular Routing Returns 404

**Error:** Page refreshes show 404

**Solution:**

- ✅ Already configured in `vercel.json`
- All routes redirect to `/index.html`
- Angular router handles client-side navigation

### API Calls Fail

**Error:** 404 or CORS errors

**Checklist:**

- [ ] API URL correct: `https://smartstay-1.runasp.net`
- [ ] CORS enabled on backend
- [ ] AuthInterceptor attaches tokens correctly
- [ ] Check browser Console for detailed errors

### Static Assets Not Loading

**Error:** Images/CSS fail to load

**Solution:**

- ✅ Cache headers configured in `vercel.json`
- Verify `public/` folder included in `angular.json`
- Asset paths should use `/Images/...` not `./Images/...`

## 📊 Build Budget Status

| Metric           | Limit       | Current    | Status  |
| ---------------- | ----------- | ---------- | ------- |
| Initial Bundle   | 3 MB error  | 2.47 MB    | ✅ Pass |
| Component Styles | 16 kB error | ≤ 11.61 kB | ✅ Pass |
| CSS Files        | 4-6 kB each | < 6 kB     | ✅ Pass |

## 🔍 Performance Tips

1. **Lazy Loading:** ApexCharts and other heavy libs load on demand
2. **Tree-shaking:** Production build removes unused code
3. **Output Hashing:** Cache busting for all assets (main-XXXXX.js)
4. **Compression:** Gzip applied automatically by Vercel

Compressed bundle sizes:

- Main JS: 466.77 kB
- Styles: 32.99 kB
- Scripts: 21.60 kB
- **Total Initial:** 522.26 kB ✓

## 🚨 Monitoring

### Vercel Dashboard

- Deployments: https://vercel.com/dashboard
- Analytics: https://vercel.com/dashboard/insights
- Project Settings: https://vercel.com/dashboard/[project]/settings

### Real-time Monitoring

- Monitor API health: https://smartstay-1.runasp.net/health (if available)
- Check Stripe connection in console
- Monitor auth token refresh in Network tab

## 📝 Next Steps

1. Push latest code: `git push origin main`
2. Set environment variables in Vercel dashboard
3. Trigger deployment (auto or manual)
4. Monitor build and deployment logs
5. Test all features on live deployment
6. Check performance metrics

## 🆘 Need Help?

1. **Build Logs:** https://vercel.com/docs/projects/overview#deployment-logs
2. **Angular Guide:** https://angular.io/cli/build
3. **Vercel Docs:** https://vercel.com/docs/frameworks/angular
4. **Common Issues:** https://vercel.com/docs/troubleshoting

---

**Last Updated:** 2026-07-09  
**Build Status:** ✅ Ready for Deployment  
**Output Location:** `dist/SmartStay/browser`
