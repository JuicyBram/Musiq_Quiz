using MusicQuiz.Controllers;
using MusicQuiz.Data;
using MusicQuiz.Data.Models;

namespace MusicQuiz
{
    public class Quiz
    {

        public int Id { get; private set; }
        public List<Round> Rounds {  get; private set; }
        public int CurrentRound {  get; private set; }
        public int TotalScore {  get; private set; }


        public Quiz()
        {
            Random random = new Random();
            Id = random.Next(1000, 9999);
            TotalScore = 0;
            Rounds = new List<Round>();
            CurrentRound = 0;
            List<FSAudioFile> audioFiles = new List<FSAudioFile>();        
        }

        public void AddRound(string name, List<FSAudioFile> songs)
        {
            CurrentRound++;
            Rounds.Add(new Round(CurrentRound, name, songs));
        }
    }
}
