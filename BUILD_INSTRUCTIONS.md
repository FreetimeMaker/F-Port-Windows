# Build Instructions for F-Port MSIX Package

## Prerequisites

- Visual Studio 2022 (Community, Professional, or Enterprise)
- Windows 10 SDK (19041 or later)
- .NET 8.0 SDK
- Microsoft Windows App SDK 2.4.0
- Windows App SDK C# Templates

## Building the MSIX Package

### Method 1: Using Visual Studio (Recommended)

1. **Open the Solution**
   - Launch Visual Studio 2022
   - Open `F-Port.slnx` (the solution file in the root directory)

2. **Configure Build Settings**
   - In the toolbar, select "Release" from the Configuration dropdown
   - Select your target platform (x64, x86, or ARM64) from the Platform dropdown

3. **Build the Package**
   - Right-click on the "F-Port (Package)" project in Solution Explorer
   - Select "Publish" > "Create App Packages"
   - Choose "Sideloading" (for testing) or "Microsoft Store" (for distribution)
   - Follow the wizard to configure package settings
   - Click "Create" to build the MSIX package

4. **Locate the Output**
   - The MSIX package will be created in: `F-Port (Package)\bin\Release\`

### Method 2: Using Command Line with Visual Studio Developer Command Prompt

1. **Open Developer Command Prompt**
   - Press `Win + S` and search for "Developer Command Prompt for VS 2022"
   - Open it as Administrator

2. **Navigate to Project Directory**
   ```cmd
   cd C:\Users\jamie\source\repos\F-Port
   ```

3. **Build the MSIX Package**
   ```cmd
   msbuild "F-Port (Package)\F-Port (Package).wapproj" /p:Configuration=Release /p:Platform=x64
   ```

4. **Alternative: Using dotnet with proper environment**
   ```cmd
   dotnet build "F-Port\F-Port.csproj" --configuration Release
   ```

## Testing the MSIX Package

### Sideloading (Testing)

1. **Enable Sideloading** (if not already enabled)
   - Open Settings > Update & Security > For developers
   - Enable "Sideloading"

2. **Install the Package**
   - Double-click the generated `.msix` file
   - Click "Install" in the prompt
   - The app will be installed and can be launched from Start Menu

3. **Testing**
   - Launch F-Port from Start Menu
   - Test app loading from JSON source
   - Test app installation functionality
   - Verify UI responsiveness and appearance

## Microsoft Store Submission

### Pre-Submission Checklist

1. **Update Package Identity**
   - Edit `F-Port (Package)\Package.appxmanifest`
   - Replace `Name="FreetimeMaker.F-Port"` with your registered Store name
   - Replace `Publisher="CN=FreetimeMaker"` with your publisher ID

2. **Run Windows App Certification Kit**
   - Open "Windows App Certification Kit" from Start Menu
   - Select the built MSIX package
   - Run all tests and fix any failures

3. **Prepare Store Assets**
   - Update logo files in `F-Port (Package)\Images\`
   - Ensure all required sizes are present:
     - StoreLogo.png (50x50)
     - Square150x150Logo.png (150x150)
     - Square44x44Logo.png (44x44)
     - Wide310x150Logo.png (310x150)
     - SplashScreen.png (620x300)

4. **Configure App Store Listing**
   - Prepare screenshots and promotional images
   - Write app description in multiple languages if needed
   - Set appropriate age rating and category

### Submission Process

1. **Access Microsoft Partner Center**
   - Log in to [Microsoft Partner Center](https://partner.microsoft.com/dashboard)
   - Navigate to your app dashboard

2. **Create New Submission**
   - Click "Create a new app"
   - Fill in app details and reserve the name

3. **Upload Package**
   - Upload the MSIX package built in Release configuration
   - Wait for package processing

4. **Complete Store Listing**
   - Add descriptions, screenshots, and promotional materials
   - Set pricing and availability
   - Configure age ratings and content policies

5. **Submit for Certification**
   - Review all information
   - Submit for Microsoft Store certification
   - Wait for approval (typically 1-3 business days)

## Troubleshooting

### Build Errors

**Error: "Microsoft.DesktopBridge.props not found"**
- Solution: Use Visual Studio to build the package instead of command line
- Ensure Windows SDK is properly installed

**Error: "Invalid qualifier" in PRI warning**
- This is a warning about the icon filename and can be ignored
- Rename the icon file to remove special characters if needed

### Runtime Errors

**Error: "Failed to load apps"**
- Check internet connection
- Verify the JSON URL is accessible
- Check JSON format matches the expected structure

**Error: "Installation Failed"**
- Verify download URLs are valid
- Check internet connection
- Ensure sufficient disk space

## Additional Resources

- [Microsoft Windows App SDK Documentation](https://learn.microsoft.com/en-us/windows/apps/windows-app-sdk/)
- [MSIX Packaging Documentation](https://learn.microsoft.com/en-us/windows/msix/)
- [Microsoft Store Submission Guide](https://learn.microsoft.com/en-us/windows/apps/publish/)