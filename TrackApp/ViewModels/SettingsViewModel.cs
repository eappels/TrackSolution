using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TrackApp.Services.Interfaces;

namespace TrackApp.ViewModels;

public partial class SettingsViewModel : ObservableObject
{

    private readonly IDBService dbService;

    public SettingsViewModel(IDBService dbService)
    {
        this.dbService = dbService ?? throw new ArgumentNullException(nameof(dbService));
    }

    [RelayCommand]
    private async Task ClearDatabaseAsync()
    {
        await dbService.ClearDatabase();
    }
}