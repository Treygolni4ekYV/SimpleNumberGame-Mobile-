using Newtonsoft.Json;
using NumberGame.Models;
using System.Threading;
using Timer = System.Threading.Timer;

namespace NumberGame;

public partial class GamePage : ContentPage
{
    const string dataKey = "MySuperSecretKey123";
    Random random = new Random();

	private string _playerName;
	private int _score = 0;

	private IDispatcherTimer timer;
	private int _totalTime = 0;

	private double correctAnswer;
	private const double _delta = 0.1; //максимальна€ погрешность игрока
	private const int correctAnswerPointsCount = 15;

	public GamePage(string playerName)
	{
		InitializeComponent();

		_playerName = playerName;

		//запускаем таймер
		timer = Dispatcher.CreateTimer();
		timer.Interval = TimeSpan.FromSeconds(1);
		timer.Tick += (s,e) => TimerCallback();
		timer.Start();

		GenerateRandomEquation();
	}

	private void TimerCallback()
	{
		_totalTime++;
		TimeLabel.Text = $"¬рем€: {_totalTime}";

		_score--;
		ScoreLabel.Text = $"—чет: {_score}";
	}

    private async void ConfirmAnswer_Click(object sender, EventArgs e)
    {
		if (double.TryParse(AnsverInput.Text, out double number))
		{
			if (correctAnswer + _delta >= number && correctAnswer - _delta <= number)
			{
				_score += correctAnswerPointsCount;
				ScoreLabel.Text = $"—чет: {_score}";
			}
			else
			{
				int count = 0;
				if ((count = HealthContainer.Children.Count) > 1)
				{
					HealthContainer.Children.RemoveAt(count-1);
				}
				else
				{
					//сделать вывод об очках игрока и тд

					//сделать сохранение данных пользовател€

					List<Player> players = new List<Player>();

					string? value = await SecureStorage.Default.GetAsync(dataKey);
					if (string.IsNullOrWhiteSpace(value))
					{
						players.Add(
							new Player
							{
								Username = _playerName,
								Score = _score
							});
					}
					else
					{
						players = JsonConvert.DeserializeObject<List<Player>>(value!);

						players.Add(
							new Player
							{
								Username = _playerName,
								Score = _score
							});
						players.OrderBy((player) => player.Score);
						if (players.Count > 15)
						{
							players.RemoveAt(0);
						}
					}

					string data = JsonConvert.SerializeObject(players);
                    await SecureStorage.Default.SetAsync(dataKey, data);

					await Navigation.PopAsync();//закрытие окна
				}
			}
		}
		GenerateRandomEquation();
    }

	private void GenerateRandomEquation()
	{
		double firstNumber = Math.Round(random.NextDouble() * 10, 2);
		double secondNumber = Math.Round(random.NextDouble() * 10, 2);

		string move = "";


		switch (random.Next(4))
		{
			case 0:
				move = "+";
				correctAnswer = firstNumber + secondNumber;
				break;

			case 1:
				move = "-";
				correctAnswer = firstNumber - secondNumber;
				break;

			case 2:
				move = "*";
				correctAnswer = firstNumber * secondNumber;
				break;

			case 3:
				move = "/";
				correctAnswer = firstNumber / secondNumber;
				break;
		}

		EquationLabel.Text = $"{firstNumber} {move} {secondNumber} = ?";
    }

    private void EditNumber_Click(object sender, EventArgs e)
    {
		if (sender is Button button)
		{
			if (int.TryParse(button.Text, out int buttonNumber))
			{
				AnsverInput.Text += buttonNumber;
			}
			else if (button.Text == "DEL")
			{
				int length;
				if ((length = AnsverInput.Text.Length) > 0)
				{
					AnsverInput.Text = AnsverInput.Text.Remove(length - 1);
				}
			}
			else//точка
			{
				AnsverInput.Text += ".";
			}
		}

    }
}