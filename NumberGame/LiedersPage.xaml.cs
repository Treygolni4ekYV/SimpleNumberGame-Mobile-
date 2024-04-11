using NumberGame.Models;
using Newtonsoft.Json;
using Microsoft.Maui.Controls.Shapes;

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
				for (int i = players.Count-1; i > 0; i--)
				{
					PlayersDataContainer.ColumnDefinitions.Add(new ColumnDefinition());

                    PlayersDataContainer.Add(
						new Label
						{
							Text = $"{players[i].Username}",
							HorizontalOptions = LayoutOptions.Fill,
						},0,15-i
					);
                    PlayersDataContainer.Add(
						new Label
						{
							Text = $"{players[i].Score}",
							HorizontalTextAlignment = TextAlignment.End,	
						},2,15-i
					);
				}
			}
		}

	}
}