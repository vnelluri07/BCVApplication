# BCV v1.1 — MonsterASP Deployment Guide

## Architecture
- **API**: `BeersCheersVasis.API` → deployed to `beerscheersvasisapi.runasp.net`
- **Frontend**: `BlazorApp3` (Blazor WASM) → deployed as static files or separate app
- **Database**: SQL Server at `db2227.public.databaseasp.net`

## Environment Variables (set in MonsterASP control panel)

The API reads secrets from configuration. In production, set these as environment variables:

```
GoogleAuth__ClientId=SET_VIA_USER_SECRETS_OR_ENV
GoogleAuth__ClientSecret=<your-google-client-secret>
CloudflareTurnstile__SiteKey=SET_VIA_USER_SECRETS_OR_ENV
CloudflareTurnstile__SecretKey=<your-turnstile-secret-key>
Jwt__Key=<your-32-byte-base64-key>
Jwt__Issuer=BeersCheersVasis.API
Jwt__Audience=BeersCheersVasis.App
```

Note: Use double underscores (`__`) for nested config sections in env vars.

## Publish Commands

### API
```bash
dotnet publish BeersCheersVasis.API -c Release -o ./publish/api
```

### Frontend (Blazor WASM)
```bash
dotnet publish BlazorApp3 -c Release -o ./publish/frontend
```
The output will be in `publish/frontend/wwwroot/` — deploy these static files.

## Frontend Configuration

Edit `BlazorApp3/wwwroot/appsettings.json` before publishing:
```json
{
  "AppSettings": {
    "ApiBaseUrl": "http://beerscheersvasisapi.runasp.net/"
  }
}
```

## Google OAuth — Authorized Redirect URIs

In Google Cloud Console, add your production domain to authorized JavaScript origins:
- `https://your-frontend-domain.runasp.net`
- `http://your-frontend-domain.runasp.net`

## Cloudflare Turnstile — Allowed Domains

In Cloudflare dashboard, add your production domain to the Turnstile widget's allowed domains.

## Database

Migration already applied. Connection string is in `appsettings.json`.
If you need to re-run: `dotnet ef database update --project BeersCheersVasis.Data --startup-project BeersCheersVasis.API`

## Post-Deploy Checklist
- [ ] API responds at `/swagger`
- [ ] Frontend loads at root URL
- [ ] Google Sign-In button appears on `/login`
- [ ] Admin redirect works (unauthenticated → `/login`)
- [ ] Categories load on landing page
- [ ] Published scripts appear on `/read`
- [ ] Comments post with Turnstile verification
- [ ] Anonymous pen names generated correctly
