namespace NumberGame
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void StartGame_Click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GamePage());
        }
    }

}
