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

        private List<Quiz> QuizList;

        public QuizController(MusicQuizDBContext context)
        {
            _context = context;
            QuizList = new List<Quiz>();
        }

        [HttpGet("startquiz")]
        public async Task<ActionResult<Quiz>> GetAudioFiles()
        {
            Quiz quiz = new Quiz();
            while(QuizList.FindIndex(q => q.Id == quiz.Id) >= 0)
            {
                quiz = new Quiz();
            }

            //get list of songs for first round from DB
            List<AudioFile> songs = new List<AudioFile>();
            for(int i = 2; i < 4; i++)
            {
                AudioFile song = await _context.AudioFiles.FindAsync(i);
                if (song != null)
                {
                    songs.Add(song);
                }
            }
            quiz.AddRound("solos", songs);
            return quiz;
        }
    }
}
