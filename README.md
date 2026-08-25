# F-Port App Store

A cross-platform app store for Windows that fetches application data from a JSON file on the internet and packages as an MSIX for Microsoft Store distribution.

## Features

- **Dynamic App Catalog**: Fetches app data from a remote JSON file
- **Modern UI**: Built with WinUI 3 for a native Windows 11 experience
- **App Details**: View detailed information about each application
- **Installation Support**: Download and install applications
- **Microsoft Store Ready**: Packaged as MSIX for easy distribution

## Configuration

### JSON Data Source

The app store fetches application data from a JSON file. By default, it uses:
```
https://example.com/apps.json
```

To configure your own JSON source, modify the `DefaultJsonUrl` constant in `Services/AppStoreService.cs`:

```csharp
private const string DefaultJsonUrl = "https://your-domain.com/apps.json";
```

### JSON Format

The JSON file should follow this structure:

```json
[
  {
    "id": "unique-app-id",
    "name": "App Name",
    "description": "App description",
    "version": "1.0.0",
    "developer": "Developer Name",
    "category": "Category",
    "iconUrl": "https://example.com/icon.png",
    "downloadUrl": "https://example.com/app.exe",
    "installSize": "10.5 MB",
    "rating": 4.5,
    "screenshots": [],
    "isInstalled": false
  }
]
```

## Building the MSIX Package

### Prerequisites

- Visual Studio 2022
- Windows 10 SDK (19041 or later)
- .NET 8.0 SDK
- Microsoft Windows App SDK 2.4.0

### Build Steps

1. Open the solution in Visual Studio
2. Select the desired configuration (Release/Debug) and platform (x64, x86, ARM64)
3. Right-click on "F-Port (Package)" project
4. Select "Publish" > "Create App Packages"
5. Choose "Sideloading" or "Microsoft Store" as appropriate
6. Follow the wizard to create the MSIX package

### Command Line Build

```powershell
# Build the project
dotnet build "F-Port\F-Port.csproj" --configuration Release

# Create MSIX package
dotnet publish "F-Port\F-Port.csproj" --configuration Release --runtime win-x64
```

## Microsoft Store Submission

To submit to the Microsoft Store:

1. Update the `Package.appxmanifest` with your publisher information
2. Replace the placeholder identity values with your Microsoft Store registered values
3. Build the release MSIX package
4. Submit through the Microsoft Partner Center
5. Ensure your app passes the Windows App Certification Kit tests

## Permissions

The app requires the following capabilities:
- `runFullTrust`: For application installation
- `systemAIModels`: For AI-related features
- `internetClient`: For fetching JSON data and downloading apps

## Sample Data

A sample `sample_apps.json` file is included in the repository for testing purposes. You can host this file on your web server or use it as a template for your own app catalog.

## Development

### Project Structure

- `F-Port/Models/`: Data models for app store items
- `F-Port/Services/`: HTTP client and data fetching logic
- `F-Port/MainWindow.xaml`: Main UI definition
- `F-Port/MainWindow.xaml.cs`: UI logic and event handlers
- `F-Port (Package)/`: MSIX packaging configuration

### Adding Features

To extend the app store functionality:

1. Add new properties to `AppStoreItem.cs` model
2. Update the JSON structure accordingly
3. Modify the UI in `MainWindow.xaml` to display new information
4. Implement additional services in the `Services/` folder

## License

This project is provided as-is for educational and commercial use.

## Support

For issues or questions, please refer to the project documentation or contact the development team.