using Memofy.ViewModels;

namespace Memofy.Views.Pages;

public partial class PersonListPage : ContentPage
{
    private readonly PersonListViewModel _viewModel;

    public PersonListPage()
    {
        InitializeComponent();
        _viewModel = new PersonListViewModel();
        this.BindingContext = _viewModel;
    }

    public async void OnFabClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("CreatePersonPage");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadPersonsAsync();
    }
}