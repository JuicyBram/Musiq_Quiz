using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicQuiz.Data.Models
{
    public class GSAudioFile
    {
        public int Id { get; set; }
        public byte[] GuitarSoloBase64 { get; set; }
        public float GituarSoloDuration { get; set; }
    }
}
