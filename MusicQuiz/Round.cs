using MusicQuiz.Data.Models;
using MusicQuiz.DTOs;

namespace MusicQuiz
{
    public class Round
    {
        public int Number { get; private set; }
        public string Name {  get; private set; }
        public int Score {  get; set; }
        public List<AudioFileDTO> Songs { get; private set; }

        public Round(int number, string name, List<AudioFileDTO> songs)
        {
            Number = number;
            Name = name;
            Score = 0;
            Songs = songs;
        }
    }
}
