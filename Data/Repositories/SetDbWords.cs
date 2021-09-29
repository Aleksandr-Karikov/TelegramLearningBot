using System;
using System.Collections.Generic;
using System.Text;
using TelegramLearningBot.Data.Contexts;
using TelegramLearningBot.Data.Models;

namespace TelegramLearningBot.Data.Repositories
{
    class SetDbWords:IDisposable
    {
        public TelegramLearningBotDbContext context;
        public SetDbWords()
        {
            context = new TelegramLearningBotDbContext();
        }

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }
        public void AddWord(Words word)
        {
            context.words.Add(word);
        }

        public void DeleteWord(int id)
        {
            var Dict = context.words.Find(id);
            if (Dict != null) context.words.Remove(Dict);
            context.SaveChanges();
        }

        public IEnumerable<Words> GetAllWords()
        {
            return context.words;
        }

        public List<string> GetListLearningWords(int dictId)
        {
            var words = context.words;
            if (words == null) return null;
            List<string> output = new List<string>();
            foreach (Words word in words)
            {
                if (word.DictionaryiesId==dictId)
                output.Add(word.LearningWord);
            }
            return output;
        }
        public List<string> GetListTranslateWords(int dictId)
        {
            var words = context.words;
            if (words == null) return null;
            List<string> output = new List<string>();
            foreach (Words word in words)
            {
                if (word.DictionaryiesId == dictId)
                    output.Add(word.TranslateOfWord);
            }
            return output;
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
