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
            var name = await DisplayPromptAsync("Имя пользователя", "Введите имя:", "Продолжить", "Отмена");

            //если значение равно null, выбрана отмена
            if (name == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                name = "Игрок";
            }

            await Navigation.PushAsync(new GamePage(name));
        }
    }

}
