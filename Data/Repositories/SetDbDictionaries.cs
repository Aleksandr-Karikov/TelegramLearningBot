using System;
using System.Collections.Generic;
using System.Text;
using TelegramLearningBot.Data.Contexts;
using TelegramLearningBot.Data.Models;

namespace TelegramLearningBot.Data.Repositories
{
    class SetDbDictionaries : IDisposable
    {
        public TelegramLearningBotDbContext context;
        public SetDbDictionaries()
        {
            context = new TelegramLearningBotDbContext();
        }

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }
        public void AddDict(Dictionaryies dict)
        {
            context.dictionaryies.Add(dict);
        }

        public void DeleteDict(int id)
        {
            var dict = context.dictionaryies.Find(id);
            if (dict != null) context.dictionaryies.Remove(dict);
            context.SaveChanges();
        }

        public void Save()
        {
            context.SaveChanges();
        }
        public IEnumerable<Dictionaryies> GetDicts()
        {
            return context.dictionaryies;

        }
    }
}
