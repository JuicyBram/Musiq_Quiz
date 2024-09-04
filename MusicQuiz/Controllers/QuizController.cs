using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicQuiz.Data;
using MusicQuiz.Data.Models;

namespace MusicQuiz.Controllers
{
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly MusicQuizDBContext _context;


        public QuizController(MusicQuizDBContext context)
        {
            _context = context;
        }

        [HttpGet("startquiz")]
        public async Task<ActionResult<Quiz>> GetAudioFiles()
        {
            Quiz quiz = new Quiz();
            while(GameList.Quizzes.FindIndex(q => q.Id == quiz.Id) >= 0)
            {
                quiz = new Quiz();
            }
            GameList.Quizzes.Add(quiz);
            return quiz;
        }

        [HttpGet("openquizzes")]
        public async Task<ActionResult<IEnumerable<Quiz>>> GetOpenQuizzes()
        {
            return GameList.Quizzes;
        }

        [HttpGet("roundinfo/{id}/{roundnumber}")]
        public async Task<ActionResult<Round>> GetRoundInfo(int id, int roundnumber)
        {
            Quiz quiz = GameList.Quizzes.Find(q => q.Id == id);
            if (quiz == null || quiz.Id <= 999 || quiz.Id >9999)
            {
                return null;
            }

            //get list of songs for the current round from DB
            List<AudioFile> songs = new List<AudioFile>();
            for (int i = 2; i < 4; i++)
            {
                FSAudioFile song = await _context.FSAudioFiles.FindAsync(i);
                if (song != null)
                {
                    songs.Add(song);
                }
            }
            quiz.AddRound("solos", songs);
            return quiz.Rounds[roundnumber-1];
        }

        [HttpPost("roundscore/{id}/{roundnumber}/{score}")]
        public async Task<ActionResult<int>> PostRoundScore(int id, int roundnumber, int score)
        {
            Quiz quiz = GameList.Quizzes.Find(q => q.Id == id);
            if (quiz == null || quiz.Id <= 999 || quiz.Id > 9999)
            {
                return 0;
            }
            quiz.TotalScore = 0;
            quiz.Rounds[roundnumber - 1].Score = score;
            foreach (Round round in quiz.Rounds)
            {
                quiz.TotalScore += round.Score;
            }
            return quiz.TotalScore;
        }

    }
}
