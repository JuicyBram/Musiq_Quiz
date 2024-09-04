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
        var audioFiles = await _context.FSAudioFiles
                                       .Include(af => af.Name)
                                       .ToListAsync();

        var audioFileDtos = audioFiles.Select(af => new AudioFileDTO
        {
            Id = af.Id,
            Name = af.Name?.Name, 
            FullSongBase64 = af.FullSongBase64,
            FullSongDuration = af.FullSongDuration,
        }).ToList();

        return Ok(audioFileDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AudioFileDTO>> GetAudioFile(int id)
    {
        Task<AudioFileDTO> audioFileDto = GetAudioFileDTO(id);
        if (audioFileDto == null)
        {
            return NotFound();
        }

        return Ok(audioFileDto);
    }

    [NonAction]
    public async Task<AudioFileDTO> GetAudioFileDTO(int id)
    {
        var audioFile = await _context.FSAudioFiles
                                      .Include(af => af.Name)
                                      .FirstOrDefaultAsync(af => af.Id == id);

        if (audioFile == null)
        {
            return null;
        }

        var audioFileDto = new AudioFileDTO
        {
            Id = audioFile.Id,
            Name = audioFile.Name.Name,
            FullSongBase64 = audioFile.FullSongBase64,
            FullSongDuration = audioFile.FullSongDuration,
        };

        return audioFileDto;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAudioFile(int id, AudioFileDTO audioFileDTO)
    {
        var audioFile = await _context.FSAudioFiles
                                      .Include(af => af.Name)
                                      .FirstOrDefaultAsync(af => af.Id == id);
        
        if (audioFile == null)
        {
            return NotFound();
        }

        var existingSongName = await _context.SongNames
                                             .FirstOrDefaultAsync(sn => sn.Name == audioFileDTO.Name);

        if (existingSongName != null)
        {
            audioFile.Name = existingSongName;
        }
        else
        {
            var newSongName = new SongName
            {
                Name = audioFileDTO.Name
            };

            _context.SongNames.Add(newSongName);
            audioFile.Name = newSongName;
        }

        audioFile.FullSongBase64 = audioFileDTO.FullSongBase64;
        audioFile.FullSongDuration = audioFileDTO.FullSongDuration;

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
    public async Task<ActionResult<FSAudioFile>> PostAudioFile(CreateAudioFileDTO createAudioFileDTO)
    {
        var existingSongName = await _context.SongNames
            .FirstOrDefaultAsync(sn => sn.Name == createAudioFileDTO.Name);
        
        SongName songName;

        if (existingSongName != null)
        {
            songName = existingSongName;
           
        }
        else
        {
            songName = new SongName
            {
                Name = createAudioFileDTO.Name
            };

        }

        var audioFile = new FSAudioFile
        {
            Name = songName,
            FullSongBase64 = createAudioFileDTO.FullSongBase64,
            FullSongDuration = createAudioFileDTO.FullSongDuration,
        };

        _context.FSAudioFiles.Add(audioFile);
        await _context.SaveChangesAsync();

        _context.SongNames.Add(songName);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAudioFile), new { id = audioFile.Id }, audioFile);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAudioFile(int id)
    {
        var audioFile = await _context.FSAudioFiles.FindAsync(id);
        if (audioFile == null)
        {
            return NotFound();
        }

        _context.FSAudioFiles.Remove(audioFile);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AudioFileExists(int id)
    {
        return _context.FSAudioFiles.Any(e => e.Id == id);
    }
}
