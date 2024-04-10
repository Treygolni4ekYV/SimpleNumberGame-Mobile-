using Android.Media;
using System.Security.Principal;
using System.Threading;
using Timer = System.Threading.Timer;

namespace NumberGame;

public partial class GamePage : ContentPage
{
	Random random = new Random();

	private string _playerName;
	private int _score = 0;
	private int _health = 3;

	private IDispatcherTimer timer;
	private int _totalTime = 0;

	private double correctAnswer;
	private const double _delta = 0.1; //максимальная погрешность игрока
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

    ~GamePage() 
	{
		//сделать систему сохранения
		timer.Stop();
	}

	private void TimerCallback()
	{
		_totalTime++;
		TimeLabel.Text = $"Время: {_totalTime}";

		_score--;
	}

    private async void ConfirmAnswer_Click(object sender, EventArgs e)
    {
		if (double.TryParse(AnsverInput.Text, out double number))
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
					await Navigation.PopAsync();//зактырие данного окна
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