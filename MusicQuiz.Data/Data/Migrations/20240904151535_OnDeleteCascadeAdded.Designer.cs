﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicQuiz.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MusicQuiz.Data.Data.Migrations
{
    [DbContext(typeof(MusicQuizDBContext))]
    [Migration("20240904151535_OnDeleteCascadeAdded")]
    partial class OnDeleteCascadeAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MusicQuiz.Data.Models.AudioFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("FullSongBase64")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<float>("FullSongDuration")
                        .HasColumnType("real");

                    b.Property<float>("GituarSoloDuration")
                        .HasColumnType("real");

                    b.Property<byte[]>("GuitarSoloBase64")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.HasKey("Id");

                    b.ToTable("AudioFiles");
                });

            modelBuilder.Entity("MusicQuiz.Data.Models.SongName", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AudioFileId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AudioFileId")
                        .IsUnique();

                    b.ToTable("SongNames");
                });

            modelBuilder.Entity("MusicQuiz.Data.Models.SongName", b =>
                {
                    b.HasOne("MusicQuiz.Data.Models.AudioFile", "AudioFile")
                        .WithOne("SongName")
                        .HasForeignKey("MusicQuiz.Data.Models.SongName", "AudioFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AudioFile");
                });

            modelBuilder.Entity("MusicQuiz.Data.Models.AudioFile", b =>
                {
                    b.Navigation("SongName");
                });
#pragma warning restore 612, 618
        }
    }
}
