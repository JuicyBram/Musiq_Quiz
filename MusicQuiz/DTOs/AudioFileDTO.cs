namespace MusicQuiz.DTOs
{
    public class AudioFileDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } // Simplified: no circular reference
        public byte[] FullSongBase64 { get; set; }
        public byte[] GuitarSoloBase64 { get; set; }
        public float FullSongDuration { get; set; }
        public float GuitarSoloDuration { get; set; }
    }
}

