using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using TelegramLearningBot.Data.Models;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TelegramLearningBot.Data.Contexts
{
    class TelegramLearningBotDbContext: DbContext
    {
        //public TelegramLearningBotDbContext()
        //{

        //}
        //public TelegramLearningBotDbContext(DbContextOptions options) : base(options)
        //{
        //}
        //public TelegramLearningBotDbContext(DbContextOptions<TelegramLearningBotDbContext> options)
        //    : base(options) { Database.EnsureCreated(); }


        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //}
        private readonly string _connectionString;

        public TelegramLearningBotDbContext()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
        public DbSet<Users> users { get; set; }
        public DbSet<Themes> themes { get; set; }
        public DbSet<Tests> tests { get; set; }
        public DbSet<Dictionaryies> dictionaryies { get; set; }
        public DbSet<Questions> questions { get; set; }
        public DbSet<Words> words { get; set; }
    }
}
