using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Memofy.ViewModels;

namespace Memofy.Views.Pages;

public partial class CreatePersonPage : ContentPage
{
    private static readonly SnackbarOptions _successSnackbar = new()
    {
        BackgroundColor = Colors.Green,
        TextColor = Colors.White,
        CornerRadius = new CornerRadius(12),
        ActionButtonTextColor = Colors.White
    };

    private static readonly SnackbarOptions _errorSnackbar = new()
    {
        BackgroundColor = Colors.Red,
        TextColor = Colors.White,
        CornerRadius = new CornerRadius(12),
        ActionButtonTextColor = Colors.White
    };

    private readonly CreatePersonViewModel _viewModel;

    public CreatePersonPage()
    {
        InitializeComponent();
        _viewModel = new CreatePersonViewModel();
        this.BindingContext = _viewModel;
    }

    public async void AddPerson(object sender, EventArgs e)
    {
        var res = await _viewModel.SavePersonAsync();

        var message = res ? "Sikeres mentés!" : "Sikertelen mentés!";
        var snackbar = Snackbar.Make(
            message: message,
            action: null,
            actionButtonText: string.Empty,
            anchor: FormGrid,
            visualOptions: res ? _successSnackbar : _errorSnackbar,
            duration: TimeSpan.FromSeconds(3));

        await snackbar.Show();

        if (res)
            await Shell.Current.GoToAsync("//PersonListPage");
    }
}