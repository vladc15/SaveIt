﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SaveIt.Models;

namespace SaveIt.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Pin> Pins { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PinBoard> PinBoards { get; set; }
        public DbSet<PinTag> PinTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // definirea relatiilor many-to-many

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PinBoard>()
                .HasKey(pb => new { pb.Id, pb.PinId, pb.BoardId });

            modelBuilder.Entity<PinBoard>()
                .HasOne(pb => pb.Pin)
                .WithMany(pb => pb.PinBoards)
                .HasForeignKey(pb => pb.PinId);

            modelBuilder.Entity<PinBoard>()
                .HasOne(pb => pb.Board)
                .WithMany(pb => pb.PinBoards)
                .HasForeignKey(pb => pb.BoardId);

            modelBuilder.Entity<PinTag>()
                .HasKey(pt => new { pt.Id, pt.PinId, pt.TagId });

            modelBuilder.Entity<PinTag>()
                .HasOne(pt => pt.Pin)
                .WithMany(pt => pt.PinTags)
                .HasForeignKey(pt => pt.PinId);

            modelBuilder.Entity<PinTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(pt => pt.PinTags)
                .HasForeignKey(pt => pt.TagId);
        }
    }
}