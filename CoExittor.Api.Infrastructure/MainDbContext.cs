using CoExittor.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CoExittor.Api.Infrastructure
{
    internal class MainDbContext(DbContextOptions<MainDbContext> options) : DbContext(options)
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Participation> Participations { get; set; }
        public DbSet<Voting> Votings { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureEvent(modelBuilder);
            ConfigureParticipation(modelBuilder);
            ConfigureVoting(modelBuilder);
            ConfigureUser(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private static void ConfigureUser(ModelBuilder modelBuilder)
        {
            var user = modelBuilder.Entity<User>();
            user.HasKey(u => u.ID);

            user.HasMany(u => u.Participations)
                .WithOne(p => p.LinkedUser)
                .HasForeignKey(p => p.LinkedUserID);
        }

        private static void ConfigureEvent(ModelBuilder modelBuilder)
        {
            var eventBuilder = modelBuilder.Entity<Event>();
            eventBuilder.HasKey(e => e.ID);

            eventBuilder.HasMany(e => e.Participants)
                .WithOne(p => p.Event)
                .HasForeignKey(p => p.EventID);
        }

        private static void ConfigureVoting(ModelBuilder modelBuilder)
        {
            var voting = modelBuilder.Entity<Voting>();
            voting.HasKey(e=> e.ID);

            // Связь с Participation в ConfigureParticipation
        }

        private static void ConfigureParticipation(ModelBuilder modelBuilder)
        {
            var participation = modelBuilder.Entity<Participation>();
            participation.HasKey(p => p.ID);

            participation.HasMany(p => p.Votings)
                .WithOne(v => v.Participation)
                .HasForeignKey(v => v.ParticipationID);

            // Связь с Event в ConfigureEvent
        }
    }
}
