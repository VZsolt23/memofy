using Memofy.ViewModels;

namespace Memofy.Views.Components;

public partial class CreatePersonView : ContentView
{
    public CreatePersonView()
    {
        InitializeComponent();

        this.BindingContext = new CreatePersonViewModel();
    }

    public async void Show()
    {
        IsVisible = true;
        DialogBox.Opacity = 0;
        DialogBox.Scale = 0.8;

        await Task.WhenAll(
            DialogBox.FadeTo(1, 250),
            DialogBox.ScaleTo(1, 250, Easing.SinOut)
        );
    }

    public async void Close(object? sender = null, EventArgs? e = null)
    {
        await Task.WhenAll(
            DialogBox.FadeTo(0, 200),
            DialogBox.ScaleTo(0.8, 200, Easing.SinIn)
        );
        IsVisible = false;
    }

    private void OnNamedayLabelTapped(object sender, EventArgs e)
    {
        NamedaySwitch.IsToggled = !NamedaySwitch.IsToggled;
    }
}