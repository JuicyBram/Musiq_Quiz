using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicQuiz.Data.Models;

public class FSAudioFile
{
    public int Id { get; set; }
    public SongName? Name { get; set; }
    public byte[] FullSongBase64 { get; set; }
    public float FullSongDuration {  get; set; }
}
