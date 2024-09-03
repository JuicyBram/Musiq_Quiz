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

namespace MusicQuiz.Controllers;

[ApiController]
[Route("api/audiofiles")]
public class AudioFileController : ControllerBase
{
    private readonly MusicQuizDBContext _context;

    public AudioFileController(MusicQuizDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AudioFileDTO>>> GetAudioFiles()
    {
        var audioFiles = await _context.AudioFiles
                                       .Include(af => af.Name)
                                       .ToListAsync();

        var audioFileDtos = audioFiles.Select(af => new AudioFileDTO
        {
            Id = af.Id,
            Name = af.Name?.Name, 
            FullSongBase64 = af.FullSongBase64,
            GuitarSoloBase64 = af.GuitarSoloBase64,
            FullSongDuration = af.FullSongDuration,
            GuitarSoloDuration = af.GituarSoloDuration
        }).ToList();

        return Ok(audioFileDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AudioFile>> GetAudioFile(int id)
    {
        var audioFile = await _context.AudioFiles.FindAsync(id);

        if (audioFile == null)
        {
            return NotFound();
        }

        return audioFile;
    }

    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAudioFile(int id, AudioFile audioFile)
    {
        if (id != audioFile.Id)
        {
            return BadRequest();
        }

        _context.Entry(audioFile).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AudioFileExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<AudioFile>> PostAudioFile(AudioFileDTO audioFileDTO)
    {
        // Controleer of de songName al bestaat in de database
        var existingSongName = await _context.SongNames
            .FirstOrDefaultAsync(sn => sn.Name == audioFileDTO.Name);
        
        SongName songName;

        // Als de songName niet bestaat, voeg een nieuwe toe
        if (existingSongName != null)
        {
            songName = existingSongName;
           
        }
        else
        {
            // Gebruik de bestaande songName
            songName = new SongName
            {
                Name = audioFileDTO.Name
            };

        }

        // Creëer een AudioFile object met de data uit de DTO en koppel de songName
        var audioFile = new AudioFile
        {
            Name = songName,
            FullSongBase64 = audioFileDTO.FullSongBase64,
            GuitarSoloBase64 = audioFileDTO.GuitarSoloBase64,
            FullSongDuration = audioFileDTO.FullSongDuration,
            GituarSoloDuration = audioFileDTO.GuitarSoloDuration
        };

        // Voeg het nieuwe AudioFile object toe aan de context en sla op in de database
        _context.AudioFiles.Add(audioFile);
        await _context.SaveChangesAsync();

        // Voeg de nieuwe songName toe aan de context
        _context.SongNames.Add(songName);
        await _context.SaveChangesAsync();

        // Retourneer het gecreëerde AudioFile object
        return CreatedAtAction(nameof(GetAudioFile), new { id = audioFile.Id }, audioFile);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAudioFile(int id)
    {
        var audioFile = await _context.AudioFiles.FindAsync(id);
        if (audioFile == null)
        {
            return NotFound();
        }

        _context.AudioFiles.Remove(audioFile);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AudioFileExists(int id)
    {
        return _context.AudioFiles.Any(e => e.Id == id);
    }
}
