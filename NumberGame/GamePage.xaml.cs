using Newtonsoft.Json;
using NumberGame.Models;

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
	private const double _delta = 0.02; //максимальная погрешность игрока
	private const int correctAnswerPointsCount = 50;

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
		TimeLabel.Text = $"Время: {_totalTime}";

		_score--;
		ScoreLabel.Text = $"Счет: {_score}";
	}

    private async void ConfirmAnswer_Click(object sender, EventArgs e)
    {
		string input = AnsverInput.Text;
		double number = 0;

		if (double.TryParse(input, out number) & string.IsNullOrWhiteSpace(input))
		{
			if (correctAnswer + _delta >= number && correctAnswer - _delta <= number)
			{
				_score += correctAnswerPointsCount;
				ScoreLabel.Text = $"Счет: {_score}";
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

					//сделать сохранение данных пользователя

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
		//короче, при каждом варианте вычислений мы будет геренить разные числа, бо иначе оно делает не укусна

		AnsverInput.Text = "";

		double firstNumber = 0, secondNumber = 0; 

		string move = "";


		switch (random.Next(4))
		{
			case 0:
				move = "+";

				do
				{
					firstNumber = Math.Round(random.NextDouble() * 10, 2);
					secondNumber = Math.Round(random.NextDouble() * 10, 2);
				} while (firstNumber <= 0 | secondNumber <= 0);

                correctAnswer = firstNumber + secondNumber;
				break;

			case 1:
				move = "-";

				//генерим до тех пор, пока не исключим отрицательный вариант
				do
				{
					do
					{
						correctAnswer = Math.Round(random.NextDouble() * 10, 2);
						firstNumber = Math.Round(random.NextDouble() * 10, 1);
					} while (correctAnswer <= 0 | firstNumber <= 0);

					secondNumber = Math.Round(correctAnswer - firstNumber, 2);
				} while (firstNumber - secondNumber <= 0 | secondNumber <= 0);


				break;

			case 2:
				move = "*";

                do
                {
                    firstNumber = Math.Round(random.NextDouble() * 10, 1);
                    secondNumber = Math.Round(random.NextDouble() * 10, 1);
                } while (firstNumber <= 0 | secondNumber <= 0);

                correctAnswer = firstNumber * secondNumber;
				break;

			case 3:
				move = "/";

                do
                {
                    firstNumber = Math.Round(random.NextDouble() * 10, 1);
                    secondNumber = Math.Round(random.NextDouble() * 5, 0);
                } while (firstNumber <= 0 | secondNumber <= 1);

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