using System;
using System.Collections.Generic;
using System.Text;
using TelegramLearningBot.Data.Contexts;
using TelegramLearningBot.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TelegramLearningBot.Data.Repositories
{
    class SetDbTheme : IDisposable
    {
        public TelegramLearningBotDbContext context;
        public SetDbTheme()
        {
            context = new TelegramLearningBotDbContext();
        }

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }

        public void AddTheme(Themes theme)
        {
            context.themes.Add(theme);
        }

        public void DeleteTheme(int id)
        {
            var theme = context.themes.Find(id);
            if (theme != null) context.themes.Remove(theme);
        }

        public IEnumerable<Themes> GetAllThemes()
        {
            return context.themes;
        }
        public int GetIdByName(string name,decimal userId)
        {
            var theme = context.themes.FirstOrDefault(c => c.Name == name && c.UsersId == userId);
            return theme.Id;
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
