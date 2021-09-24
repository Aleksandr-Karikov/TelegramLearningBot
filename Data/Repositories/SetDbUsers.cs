using System;
using System.Collections.Generic;
using System.Text;
using TelegramLearningBot.Data.Contexts;
using TelegramLearningBot.Data.Models;

namespace TelegramLearningBot.Data.Repositories
{
    class SetDbUsers : IDisposable
    {
        TelegramLearningBotDbContext context;
        public SetDbUsers()
        {
            context = new TelegramLearningBotDbContext();
        }

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }

        public void AddUser(Users user)
        {
            context.users.Add(user);
        }

        public void Save()
        {
            context.SaveChanges();
        }
        public Users GetById(decimal id)
        {
            return context.users.Find(id);
        }
        public void DeleteUser(decimal id)
        {
            var user = context.users.Find(id);
            if (user!=null) context.users.Remove(user);
        }
    }
}
