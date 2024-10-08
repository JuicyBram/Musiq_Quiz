﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicQuiz.Data.Models;


namespace MusicQuiz.Data;

public class MusicQuizDBContext : DbContext
{
    public MusicQuizDBContext(DbContextOptions<MusicQuizDBContext> options)
            : base(options)
    {
    }

    public DbSet<FSAudioFile> FSAudioFiles { get; set; }
    public DbSet<GSAudioFile> GSAudioFiles { get; set; }

    public DbSet<SongName> SongNames { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FSAudioFile>()
            .HasOne(a => a.Name)
            .WithOne(s => s.FSAudioFile)
            .HasForeignKey<SongName>(s => s.FSAudioFileId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade); // Stel de Foreign Key in op SongName

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=music-quiz-11117.7tc.aws-eu-central-1.cockroachlabs.cloud;Port=26257;Username=juicybram;Password=kHS2didnYbXrEUj97vMuYQ;Database=defaultdb;SslMode=Require");
        }
    }
}
