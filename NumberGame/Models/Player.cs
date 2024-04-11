using System.ComponentModel.DataAnnotations;

namespace NumberGame.Models
{
    public class Player
    {
        public string Username { get; set; } = "";
        public int Score { get; set; } = 0;
    }
}
