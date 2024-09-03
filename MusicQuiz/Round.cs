using MusicQuiz.Data.Models;

namespace MusicQuiz
{
    public class Round
    {
        public int Number { get; private set; }
        public string Name {  get; private set; }
        public int Score {  get; private set; }
        public List<AudioFile> Songs { get; private set; }

        public Round(int number, string name, List<AudioFile> songs)
        {
            Number = number;
            Name = name;
            Score = 0;
            Songs = songs;
        }
    }
}
