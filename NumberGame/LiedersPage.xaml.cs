using NumberGame.Models;
using Newtonsoft.Json;

namespace NumberGame;

public partial class LiedersPage : ContentPage
{
    const string dataKey = "MySuperSecretKey123";

    public LiedersPage()
	{
		InitializeComponent();
		getDataAsync();
	}

	async void getDataAsync()
	{
		string? data = await SecureStorage.Default.GetAsync(dataKey);
		if (data != null)
		{
			List<Player>? players = JsonConvert.DeserializeObject<List<Player>>(data!);

			if (players != null)
			{
				for (int i = 0; i < players.Count; i++)
				{
					PlayersDataContainer.RowDefinitions.Add(new RowDefinition());

					int rowCount = PlayersDataContainer.RowDefinitions.Count;

                    PlayersDataContainer.Add(
						new Label
						{
							Text = $"{players[i].Username}",
							FontSize = 18,
							VerticalOptions = LayoutOptions.Start,
							HorizontalTextAlignment = TextAlignment.Start,
						}, 0, rowCount
					);
					PlayersDataContainer.Add(
						new Label
						{
							Text = $"{players[i].Score}",
							FontSize = 18,
                            VerticalOptions = LayoutOptions.Start,
							HorizontalTextAlignment = TextAlignment.End,
                        }, 1, rowCount
					);
				}
			}
		}

	}

    private async void GoToMenuClick(object sender, EventArgs e)
    {
		await Navigation.PopToRootAsync();
    }

    private async void ClearAllData(object sender, EventArgs e)
    {
		bool resp = await DisplayAlert(
			"Удаление данных",
			"Вы уверены, что хотите удалить все данные?",
			"Да, я уверен",
			"Нет");

		if (resp) 
		{
			await SecureStorage.SetAsync(dataKey, "");
            await Navigation.PopToRootAsync();
        }
    }
}