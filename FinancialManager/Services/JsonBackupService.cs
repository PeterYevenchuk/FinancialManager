using FinancialManager.Data.Repositories;
using FinancialManager.Models;
using System.Text.Json;

namespace FinancialManager.Services;

public class JsonBackupService
{
    private readonly ITransactionRepository _transactionRepo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly ITransactionTypeRepository _typeRepo;
    private readonly ILocalizationRepository _localizationRepo;

    public JsonBackupService(
        ITransactionRepository transactionRepo,
        ICategoryRepository categoryRepo,
        ITransactionTypeRepository typeRepo,
        ILocalizationRepository localizationRepo)
    {
        _transactionRepo = transactionRepo;
        _categoryRepo = categoryRepo;
        _typeRepo = typeRepo;
        _localizationRepo = localizationRepo;
    }

    public async Task ExportAsync()
    {
        var backup = new BackupData
        {
            Categories = await _categoryRepo.GetAsync(),
            TransactionTypes = await _typeRepo.GetAsync(),
            Transactions = await _transactionRepo.GetAsync(),
            Localizations = await _localizationRepo.GetAsync()
        };
        string jsonString = JsonSerializer.Serialize(backup, new JsonSerializerOptions { WriteIndented = true });

        string fileName = $"FinancialManager_Backup_{DateTime.Now:yyyyMMdd}.json";
        string tempPath = Path.Combine(FileSystem.CacheDirectory, fileName);
        await File.WriteAllTextAsync(tempPath, jsonString);

        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = Resources.Strings.Backup_SaveTitle,
            File = new ShareFile(tempPath)
        });
    }

    public async Task<bool> ImportAsync()
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = Resources.Strings.Backup_PickTitle,
            FileTypes = FilePickerFileType.Jpeg
        });

        if (result == null) return false;

        using var stream = await result.OpenReadAsync();
        using var reader = new StreamReader(stream);
        string jsonString = await reader.ReadToEndAsync();

        var backup = JsonSerializer.Deserialize<BackupData>(jsonString);
        if (backup == null) return false;

        //TODO for now we will just delete all data and write new, but in future we can add some merging logic
        foreach (var item in await _transactionRepo.GetAsync()) await _transactionRepo.DeleteAsync(item);
        foreach (var item in await _localizationRepo.GetAsync()) await _localizationRepo.DeleteAsync(item);
        foreach (var item in await _categoryRepo.GetAsync()) await _categoryRepo.DeleteAsync(item);
        foreach (var item in await _typeRepo.GetAsync()) await _typeRepo.DeleteAsync(item);

        foreach (var item in backup.Categories) await _categoryRepo.SaveAsync(item);
        foreach (var item in backup.TransactionTypes) await _typeRepo.SaveAsync(item);
        foreach (var item in backup.Transactions) await _transactionRepo.SaveAsync(item);
        foreach (var item in backup.Localizations) await _localizationRepo.SaveAsync(item);

        return true;
    }
}
