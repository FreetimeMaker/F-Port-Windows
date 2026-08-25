using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using F_Port.Models;
using F_Port.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace F_Port
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private readonly AppStoreService _appStoreService;
        public List<AppStoreItem> Apps { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            _appStoreService = new AppStoreService();
            Apps = new List<AppStoreItem>();
            Activated += MainWindow_Activated;
        }

        private async void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            if (Apps.Count == 0)
            {
                await LoadAppsAsync();
            }
        }

        private async System.Threading.Tasks.Task LoadAppsAsync()
        {
            LoadingRing.IsActive = true;
            AppsGridView.Visibility = Visibility.Collapsed;
            ErrorTextBlock.Visibility = Visibility.Collapsed;

            try
            {
                Apps = await _appStoreService.GetAppsAsync();
                
                if (Apps.Any())
                {
                    AppsGridView.Visibility = Visibility.Visible;
                }
                else
                {
                    ErrorTextBlock.Visibility = Visibility.Visible;
                    ErrorTextBlock.Text = "No apps found in the store.";
                }
            }
            catch (Exception ex)
            {
                ErrorTextBlock.Visibility = Visibility.Visible;
                ErrorTextBlock.Text = $"Error loading apps: {ex.Message}";
            }
            finally
            {
                LoadingRing.IsActive = false;
            }
        }

        private async void RefreshApps()
        {
            await LoadAppsAsync();
        }

        private void AppsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is AppStoreItem app)
            {
                _ = ShowAppDetailsAsync(app);
            }
        }

        private async System.Threading.Tasks.Task ShowAppDetailsAsync(AppStoreItem app)
        {
            var dialog = new ContentDialog
            {
                Title = app.Name,
                Content = CreateAppDetailsContent(app),
                CloseButtonText = "Close",
                PrimaryButtonText = app.IsInstalled ? "Open" : "Install",
                PrimaryButtonStyle = (Style)Application.Current.Resources["AccentButtonStyle"],
                XamlRoot = Content.XamlRoot
            };

            dialog.PrimaryButtonClick += async (s, args) =>
            {
                if (!app.IsInstalled)
                {
                    args.Cancel = true;
                    dialog.Hide();
                    await InstallAppAsync(app);
                }
                else
                {
                    // Open the installed app
                    args.Cancel = true;
                    dialog.Hide();
                    await LaunchAppAsync(app);
                }
            };

            await dialog.ShowAsync();
        }

        private UIElement CreateAppDetailsContent(AppStoreItem app)
        {
            var stackPanel = new StackPanel { Spacing = 12 };

            // Icon
            var icon = new Image
            {
                Source = new Microsoft.UI.Xaml.Media.Imaging.BitmapImage(new Uri(app.IconUrl)),
                Width = 100,
                Height = 100,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            stackPanel.Children.Add(icon);

            // Developer
            stackPanel.Children.Add(new TextBlock
            {
                Text = $"Developer: {app.Developer}",
                Style = (Style)Application.Current.Resources["CaptionTextBlockStyle"]
            });

            // Version
            stackPanel.Children.Add(new TextBlock
            {
                Text = $"Version: {app.Version}",
                Style = (Style)Application.Current.Resources["CaptionTextBlockStyle"]
            });

            // Category
            stackPanel.Children.Add(new TextBlock
            {
                Text = $"Category: {app.Category}",
                Style = (Style)Application.Current.Resources["CaptionTextBlockStyle"]
            });

            // Size
            stackPanel.Children.Add(new TextBlock
            {
                Text = $"Size: {app.InstallSize}",
                Style = (Style)Application.Current.Resources["CaptionTextBlockStyle"]
            });

            // Rating
            var ratingPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 4 };
            ratingPanel.Children.Add(new FontIcon { Glyph = "", Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Gold) });
            ratingPanel.Children.Add(new TextBlock { Text = app.Rating.ToString("F1"), Style = (Style)Application.Current.Resources["CaptionTextBlockStyle"] });
            stackPanel.Children.Add(ratingPanel);

            // Description
            stackPanel.Children.Add(new TextBlock
            {
                Text = app.Description,
                TextWrapping = TextWrapping.Wrap,
                Style = (Style)Application.Current.Resources["BodyTextBlockStyle"]
            });

            return stackPanel;
        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string appId)
            {
                var app = Apps.FirstOrDefault(a => a.Id == appId);
                if (app != null)
                {
                    _ = ShowAppDetailsAsync(app);
                }
            }
        }

        private async System.Threading.Tasks.Task InstallAppAsync(AppStoreItem app)
        {
            try
            {
                // For MSIX packages, we would use Windows.PackageManager APIs
                // For now, this is a placeholder that downloads the file
                var content = new ContentDialog
                {
                    Title = "Installing " + app.Name,
                    Content = new ProgressRing { IsActive = true, Width = 50, Height = 50 },
                    CloseButtonText = "Cancel"
                };
                
                var dialogTask = content.ShowAsync();
                
                // Download the app
                await _appStoreService.DownloadAppAsync(app.DownloadUrl);
                
                // Close the progress dialog
                content.Hide();
                
                // Mark as installed (in a real app, this would be based on actual installation)
                app.IsInstalled = true;
                
                var successDialog = new ContentDialog
                {
                    Title = "Installation Complete",
                    Content = $"{app.Name} has been installed successfully.",
                    CloseButtonText = "OK",
                    XamlRoot = Content.XamlRoot
                };
                await successDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                var errorDialog = new ContentDialog
                {
                    Title = "Installation Failed",
                    Content = $"Failed to install {app.Name}: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = Content.XamlRoot
                };
                await errorDialog.ShowAsync();
            }
        }

        private async System.Threading.Tasks.Task LaunchAppAsync(AppStoreItem app)
        {
            // In a real implementation, this would launch the installed app
            var dialog = new ContentDialog
            {
                Title = "Launch App",
                Content = $"Launching {app.Name}...",
                CloseButtonText = "OK",
                XamlRoot = Content.XamlRoot
            };
            await dialog.ShowAsync();
        }
    }
}
