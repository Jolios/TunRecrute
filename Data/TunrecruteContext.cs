using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Tunrecrute.Models;
//using TTunRecrute.Models;


namespace TunRecrute.Data
{
    public class TunrecruteContext : IdentityDbContext<User>
    {
        public TunrecruteContext(DbContextOptions<TunrecruteContext> options)
            : base(options)
        {
        
        }
        public virtual DbSet<Advertisement> Advertisements { get; set; }
        public virtual DbSet<Candidacy> Candidacies { get; set; }
        public virtual DbSet<Education> Educations { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<ProfSkill> ProfSkills { get; set; }
        public virtual DbSet<SocialMedia> SocialMedias { get; set; }
        public virtual DbSet<WorkExperience> WorkExperiences { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Advertisement>(entity =>
            {
                entity.Property(e => e.Posted).HasPrecision(0);

                entity.HasOne(d => d.User)
                .WithMany(p => p.Advertisements)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            });
            builder.Entity<Candidacy>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.AdvertisementId })
                    .HasName("PK_candidacy_user_id");

                entity.Property(e => e.Date).HasPrecision(0);

                entity.HasOne(d => d.Advertisement)
                    .WithMany(p => p.Candidacies)
                    .HasForeignKey(d => d.AdvertisementId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Candidacies)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            builder.Entity<Education>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Educations)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            builder.Entity<Language>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Languages)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            builder.Entity<ProfSkill>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.ProfSkills)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            

            builder.Entity<SocialMedia>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithOne(p => p.SocialMedia)
                    .HasForeignKey<SocialMedia>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                    
            });

            builder.Entity<WorkExperience>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.WorkExperiences)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }
    }
}
