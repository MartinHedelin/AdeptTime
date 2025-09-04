# PowerShell script to switch between simple and complex Supabase setups
param(
    [Parameter(Mandatory=$true)]
    [ValidateSet("simple", "complex")]
    [string]$Mode
)

Write-Host "Switching Supabase to $Mode mode..." -ForegroundColor Green

if ($Mode -eq "simple") {
    # Backup current config
    if (Test-Path "supabase/config.toml") {
        Copy-Item "supabase/config.toml" "supabase/config.complex.toml" -Force
        Write-Host "✓ Backed up complex config to config.complex.toml" -ForegroundColor Yellow
    }
    
    # Switch to simple config
    Copy-Item "supabase/config.simple.toml" "supabase/config.toml" -Force
    Write-Host "✓ Switched to simple config" -ForegroundColor Green
    
    # Backup current migrations
    if (Test-Path "supabase/migrations") {
        if (Test-Path "supabase/migrations_complex") {
            Remove-Item "supabase/migrations_complex" -Recurse -Force
        }
        Move-Item "supabase/migrations" "supabase/migrations_complex"
        Write-Host "✓ Backed up complex migrations to migrations_complex/" -ForegroundColor Yellow
    }
    
    # Switch to simple migrations
    if (Test-Path "supabase/migrations_simple") {
        Copy-Item "supabase/migrations_simple" "supabase/migrations" -Recurse -Force
        Write-Host "✓ Switched to simple migrations" -ForegroundColor Green
    }
    
    # Switch to simple seed
    if (Test-Path "supabase/seed.sql") {
        Copy-Item "supabase/seed.sql" "supabase/seed.complex.sql" -Force
    }
    Copy-Item "supabase/seed_simple.sql" "supabase/seed.sql" -Force
    Write-Host "✓ Switched to simple seed data" -ForegroundColor Green
    
} elseif ($Mode -eq "complex") {
    # Switch back to complex config
    if (Test-Path "supabase/config.complex.toml") {
        Copy-Item "supabase/config.complex.toml" "supabase/config.toml" -Force
        Write-Host "✓ Switched to complex config" -ForegroundColor Green
    } else {
        Write-Host "❌ No complex config backup found!" -ForegroundColor Red
        exit 1
    }
    
    # Switch back to complex migrations
    if (Test-Path "supabase/migrations_complex") {
        if (Test-Path "supabase/migrations") {
            Remove-Item "supabase/migrations" -Recurse -Force
        }
        Copy-Item "supabase/migrations_complex" "supabase/migrations" -Recurse -Force
        Write-Host "✓ Switched to complex migrations" -ForegroundColor Green
    }
    
    # Switch back to complex seed
    if (Test-Path "supabase/seed.complex.sql") {
        Copy-Item "supabase/seed.complex.sql" "supabase/seed.sql" -Force
        Write-Host "✓ Switched to complex seed data" -ForegroundColor Green
    }
}

Write-Host ""
Write-Host "🚀 Supabase is now in $Mode mode!" -ForegroundColor Cyan
Write-Host "Run 'supabase stop' then 'supabase start' to apply changes." -ForegroundColor Blue
