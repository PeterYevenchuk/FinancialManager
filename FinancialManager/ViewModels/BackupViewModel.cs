using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinancialManager.Services;

namespace FinancialManager.ViewModels;

public partial class BackupViewModel : ObservableObject
{
    private readonly JsonBackupService _backupService;

    [ObservableProperty] private bool isBusy;

    public BackupViewModel(JsonBackupService backupService)
    {
        _backupService = backupService;
    }

    [RelayCommand]
    private async Task ExportBackup()
    {
        IsBusy = true;
        try
        {
            await _backupService.ExportAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert(Resources.Strings.Error, ex.Message, Resources.Strings.Ok);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ImportBackup()
    {
        bool confirm = await Shell.Current.DisplayAlert(Resources.Strings.Warning, Resources.Strings.Backup_ReplaceConfirmation, Resources.Strings.Yes, Resources.Strings.No);
        if (!confirm) return;

        IsBusy = true;
        try
        {
            bool success = await _backupService.ImportAsync();
            if (success)
            {
                await Shell.Current.DisplayAlert(Resources.Strings.Success, Resources.Strings.Backup_RestoreSuccess, Resources.Strings.Ok);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert(Resources.Strings.ImportErrorTitle, ex.Message, Resources.Strings.Ok);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
