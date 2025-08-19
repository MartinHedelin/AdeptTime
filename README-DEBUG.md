# 🚀 Debug & Run Configuration Guide

This project is set up with multiple ways to run and debug your Blazor WebAssembly application.

## VS Code (Recommended)

### 🎯 Quick Start
1. Open the project in VS Code
2. Press `F5` or click the **Run and Debug** button in the sidebar
3. Select **"🚀 Launch Blazor WebAssembly"** from the dropdown
4. The application will build and launch automatically in Chrome

### 🔧 Available Debug Configurations

| Configuration | Description | URL |
|---------------|-------------|-----|
| 🚀 Launch Blazor WebAssembly | HTTP launch with debugging | http://localhost:5226 |
| 🌐 Launch Blazor WebAssembly (HTTPS) | HTTPS launch with debugging | https://localhost:7161 |
| 🔧 Attach to running Blazor WebAssembly | Attach debugger to running app | http://localhost:5226 |

### ⚡ Available Tasks
- **build-web** - Build the WebAssembly project only
- **build-all** - Build the entire solution
- **clean** - Clean build artifacts
- **run-web** - Run without debugging
- **watch-web** - Run with hot reload (recommended for development)

### 🔥 Hot Reload Development
For the best development experience, use the watch task:
1. Press `Ctrl+Shift+P` (Windows/Linux) or `Cmd+Shift+P` (Mac)
2. Type "Tasks: Run Task"
3. Select **"watch-web"**
4. Your app will run with hot reload - changes are reflected immediately!

## Terminal Commands

```bash
# Run the application
dotnet run --project parlayrunner.Web

# Run with hot reload
dotnet watch run --project parlayrunner.Web

# Build only
dotnet build parlayrunner.Web

# Clean
dotnet clean
```

## Visual Studio

If you're using Visual Studio:
1. Set `parlayrunner.Web` as the startup project
2. Press `F5` to run with debugging
3. Or press `Ctrl+F5` to run without debugging

## Debugging Features

- ✅ **Breakpoints** in C# code
- ✅ **Hot reload** for instant feedback
- ✅ **Browser developer tools** integration
- ✅ **IntelliSense** and code completion
- ✅ **Error highlighting** and validation
- ✅ **Razor syntax** highlighting

## URLs

- **HTTP**: http://localhost:5226
- **HTTPS**: https://localhost:7161

## Tips

1. **Use hot reload** (`watch-web` task) for fastest development
2. **Set breakpoints** in your `.razor` files and `.cs` files
3. **Use browser dev tools** for CSS/HTML debugging
4. **Check the terminal** for build errors and warnings

Happy coding! 🎉 