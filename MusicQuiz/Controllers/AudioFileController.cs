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
                                       .Include(af => af.SongName)
                                       .ToListAsync();

        var audioFileDtos = audioFiles.Select(af => new AudioFileDTO
        {
            Id = af.Id,
            Name = af.SongName?.Name, 
            FullSongBase64 = af.FullSongBase64,
            GuitarSoloBase64 = af.GuitarSoloBase64,
            FullSongDuration = af.FullSongDuration,
            GuitarSoloDuration = af.GituarSoloDuration
        }).ToList();

        return Ok(audioFileDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AudioFileDTO>> GetAudioFile(int id)
    {
        var audioFile = await _context.AudioFiles
                                      .Include(af => af.SongName) 
                                      .FirstOrDefaultAsync(af => af.Id == id);  

        if (audioFile == null)
        {
            return NotFound();
        }

        var audioFileDto = new AudioFileDTO
        {
            Id = audioFile.Id,
            Name = audioFile.SongName.Name,
            FullSongBase64 = audioFile.FullSongBase64,
            GuitarSoloBase64 = audioFile.GuitarSoloBase64,
            FullSongDuration = audioFile.FullSongDuration,
            GuitarSoloDuration = audioFile.GituarSoloDuration
        };

        return Ok(audioFileDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAudioFile(int id, CreateAudioFileDTO audioFileDTO)
    {
        var audioFile = await _context.AudioFiles
                                      .Include(af => af.SongName)
                                      .FirstOrDefaultAsync(af => af.Id == id);
        
        if (audioFile == null)
        {
            return NotFound();
        }

        var existingSongName = await _context.SongNames
                                             .FirstOrDefaultAsync(sn => sn.Name == audioFileDTO.Name);

        if (existingSongName != null)
        {
            audioFile.SongName = existingSongName;
        }
        else
        {
            var newSongName = new SongName
            {
                Name = audioFileDTO.Name
            };

            _context.SongNames.Add(newSongName);
            audioFile.SongName = newSongName;
        }

        audioFile.FullSongBase64 = audioFileDTO.FullSongBase64;
        audioFile.GuitarSoloBase64 = audioFileDTO.GuitarSoloBase64;
        audioFile.FullSongDuration = audioFileDTO.FullSongDuration;
        audioFile.GituarSoloDuration = audioFileDTO.GuitarSoloDuration;

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
        var existingSongName = await _context.SongNames
            .FirstOrDefaultAsync(sn => sn.Name == audioFileDTO.Name);
        
        SongName songName;

        if (existingSongName != null)
        {
            songName = existingSongName;
           
        }
        else
        {
            songName = new SongName
            {
                Name = audioFileDTO.Name
            };

        }

        var audioFile = new AudioFile
        {
            SongName = songName,
            FullSongBase64 = audioFileDTO.FullSongBase64,
            GuitarSoloBase64 = audioFileDTO.GuitarSoloBase64,
            FullSongDuration = audioFileDTO.FullSongDuration,
            GituarSoloDuration = audioFileDTO.GuitarSoloDuration
        };

        _context.AudioFiles.Add(audioFile);
        await _context.SaveChangesAsync();

        _context.SongNames.Add(songName);
        await _context.SaveChangesAsync();

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
