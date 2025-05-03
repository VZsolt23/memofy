namespace Memofy.Views.Pages;

public partial class PersonListPage : ContentPage
{
    public PersonListPage()
    {
        InitializeComponent();
    }

    private void OnFabClicked(object sender, EventArgs e)
    {
        CreatePerson.Show();
    }
}