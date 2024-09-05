using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicQuiz.Data;
using MusicQuiz.Data.Models;
using MusicQuiz.DTOs;

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
            List<AudioFileDTO> songs = new List<AudioFileDTO>();
            AudioFileController controller = new AudioFileController(_context);
            int[] ids;

            switch (roundnumber){
                case 1:
                    ids = getIdsRound1();
                    break;
                default:
                    ids = [];
                    break;
            }
            foreach(int i in ids)
            {
                AudioFileDTO song = await controller.GetAudioFileDTO(i);
                if (song != null)
                {
                    songs.Add(song);
                }
            }
            quiz.AddRound("solos", songs);
            return quiz.Rounds[roundnumber-1];
        }

        private int[] getIdsRound1()
        {
            Random random = new Random();
            List<int> ids = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                int num = random.Next(2, 8);
                while (ids.Contains(num))
                {
                    num = random.Next(2, 8);
                }
                ids.Add(num);
            }
            return ids.ToArray();
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
