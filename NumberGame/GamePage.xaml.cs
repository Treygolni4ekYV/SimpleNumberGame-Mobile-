using System.Security.Principal;
using System.Timers;
using Timer = System.Timers.Timer;

namespace NumberGame;

public partial class GamePage : ContentPage
{
	Random random = new Random();

	private string _playerName;
	private int _score;

	private Timer _timer;
	private int _totalTime;

	private double _correctAnswer;
	private double _delta = 0.1; //максимальная погрешность игрока

	public GamePage(string playerName)
	{
		InitializeComponent();

		_playerName = playerName;
		
		//запускаем таймер
		_timer = new Timer(1000);
        _timer.Elapsed += _timer_Elapsed;
		_timer.AutoReset = true;
		_timer.Start();

		GenerateRandomEquation();
	}

    ~GamePage() 
	{
		_timer.Stop();

		//сделать систему сохранения
	}
    private void _timer_Elapsed(object? sender, ElapsedEventArgs e)
    {
		_score -= 1;
		_totalTime += 1;
    }

    private void ConfirmAnswer_Click(object sender, EventArgs e)
    {
		GenerateRandomEquation();
		if (true)
		{

		}
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
				_correctAnswer = firstNumber + secondNumber;
				break;

			case 1:
				move = "-";
				_correctAnswer = firstNumber - secondNumber;
				break;

			case 2:
				move = "*";
				_correctAnswer = firstNumber * secondNumber;
				break;

			case 3:
				move = "/";
				_correctAnswer = firstNumber / secondNumber;
				break;
		}

		EquationLabel.Text = $"{firstNumber} {move} {secondNumber} = ?";
    }
}