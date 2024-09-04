using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicQuiz.Data.Models;

public class SongName
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int FSAudioFileId { get; set; }
    public FSAudioFile FSAudioFile { get; set; }
}
