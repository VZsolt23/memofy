using Memofy.Views.Pages;

namespace Memofy
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("MainPage", typeof(MainPage));
            Routing.RegisterRoute("PersonListPage", typeof(PersonListPage));
            Routing.RegisterRoute("CreatePersonPage", typeof(CreatePersonPage));
        }
    }
}
