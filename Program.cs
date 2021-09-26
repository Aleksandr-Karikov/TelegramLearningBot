using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramLearningBot.Data;
using TelegramLearningBot.Data.Contexts;
using TelegramLearningBot.Data.Models;
using TelegramLearningBot.Data.Repositories;

namespace TelegramLearningBot
{
    class Program
    {
        
        private static string token { get; set; } = "2032410876:AAGt_NTTOPsc4ZfUWkX7G0LZotx2cPCagzc";
        private static TelegramBotClient client;
        private static Dictionary<decimal,int> UserTheme= new Dictionary<decimal, int>();
        private static Dictionary<int, int> ThemeDict = new Dictionary<int, int>();
        private static Dictionary<decimal, int> UserWord = new Dictionary<decimal, int>();
        [Obsolete]
        static void Main(string[] args)
        {
            client = new TelegramBotClient(token);
            client.StartReceiving();
            client.OnMessage += OnMassageHandler;
            Console.ReadLine();
            client.StopReceiving();
        }

        [Obsolete]
        private static async void OnMassageHandler(object sender, MessageEventArgs e)
        {
            var msg = e.Message;

            //add users in database
            using (SetDbUsers userRep = new SetDbUsers())
            {
                var user = userRep.GetById(msg.From.Id);
                if (user == null)
                {
                    user = new Users() { Id = msg.From.Id, Name = msg.From.FirstName };
                    userRep.AddUser(user);
                    userRep.Save();
                }
            }


            if (msg.ReplyToMessage != null)
            {

                switch (msg.ReplyToMessage.Text)
                {
                    case "Напишите имя темы для добавления":
                        using (SetDbTheme themeRep = new SetDbTheme())
                        {
                            if (!(themeRep.context.themes.Any(c => c.Name == msg.Text && c.UsersId == msg.From.Id)))
                            {
                                themeRep.AddTheme(new Themes() { Name = msg.Text, UsersId = msg.From.Id });
                                themeRep.Save();
                            }
                            else
                            {
                                await client.SendTextMessageAsync(msg.Chat.Id, "Возможно такая тема уже существует", replyMarkup: getUserThemesButtons());
                            }
                        }
                        await client.SendTextMessageAsync(msg.Chat.Id, "Ваши темы: ", replyMarkup: getUserThemesButtons());
                        await client.SendTextMessageAsync(msg.Chat.Id, GetListThemes(msg.From.Id));
                        break;
                    case "Напишите имя темы для удаления":
                        using (SetDbTheme themeRep = new SetDbTheme())
                        {
                            if (themeRep.context.themes.Any(c => c.Name == msg.Text && c.UsersId == msg.From.Id))
                            {
                                var theme = themeRep.context.themes.FirstOrDefault(c => c.Name == msg.Text);
                                themeRep.DeleteTheme(theme.Id);
                            }
                            else
                            {
                                await client.SendTextMessageAsync(msg.Chat.Id, "Возможно такой темы не существует", replyMarkup: getUserThemesButtons());
                            }
                        }
                        await client.SendTextMessageAsync(msg.Chat.Id, "Ваши темы: ", replyMarkup: getUserThemesButtons());
                        await client.SendTextMessageAsync(msg.Chat.Id, GetListThemes(msg.From.Id));
                        break;
                    case "Напишите имя темы":
                        using (SetDbTheme themeRep = new SetDbTheme())
                        {
                            if (themeRep.context.themes.Any(c => c.Name == msg.Text && c.UsersId == msg.From.Id))
                            {
                                var theme = themeRep.context.themes.FirstOrDefault(c => c.Name == msg.Text);
                                if (UserTheme.ContainsKey(msg.From.Id))
                                {
                                    UserTheme[msg.From.Id] = theme.Id;
                                }else
                                {
                                    UserTheme.Add(msg.From.Id, theme.Id);
                                }
                                await client.SendTextMessageAsync(msg.Chat.Id, "Ваши Словари: ", replyMarkup: getUserDictButtons());
                                await client.SendTextMessageAsync(msg.Chat.Id, GetListDict(theme.Id));
                            }
                            else
                            {
                                await client.SendTextMessageAsync(msg.Chat.Id, "Возможно такой темы не существует", replyMarkup: getUserThemesButtons());
                            }
                        }
                        break;
                    case "Напишите имя словаря для добавления":
                        using (SetDbDictionaries dictRep = new SetDbDictionaries())
                        {
                            if (!(dictRep.context.dictionaryies.Any(c => c.Name == msg.Text && c.ThemesId == msg.From.Id)))
                            {
                                dictRep.AddDict(new Dictionaryies() { Name = msg.Text, ThemesId = UserTheme[msg.From.Id] });
                                dictRep.Save();
                            }
                            else
                            {
                                await client.SendTextMessageAsync(msg.Chat.Id, "Возможно такой словарь уже существует");
                            }
                        }
                        await client.SendTextMessageAsync(msg.Chat.Id, "Ваши словари: ", replyMarkup: getUserDictButtons());
                        await client.SendTextMessageAsync(msg.Chat.Id, GetListDict(UserTheme[msg.From.Id]));
                        break;
                    case "Напишите имя словаря для удаления":
                        using (SetDbDictionaries dictRep = new SetDbDictionaries())
                        {
                            if (dictRep.context.dictionaryies.Any(c => c.Name == msg.Text && c.ThemesId == UserTheme[msg.From.Id]))
                            {
                                var dict = dictRep.context.dictionaryies.FirstOrDefault(c => c.Name == msg.Text);
                                dictRep.DeleteDict(dict.Id);
                            }
                            else
                            {
                                await client.SendTextMessageAsync(msg.Chat.Id, "Возможно такого словаря не существует", replyMarkup: getUserThemesButtons());
                            }
                        }
                        await client.SendTextMessageAsync(msg.Chat.Id, "Ваши словари: ", replyMarkup: getUserDictButtons());
                        await client.SendTextMessageAsync(msg.Chat.Id, GetListDict(UserTheme[msg.From.Id]));
                        break;
                    case "Напишите имя словаря":
                        using (SetDbDictionaries dictRep = new SetDbDictionaries())
                        {
                            if (dictRep.context.dictionaryies.Any(c => c.Name == msg.Text && c.ThemesId == UserTheme[msg.From.Id]))
                            {
                                var dict = dictRep.context.dictionaryies.FirstOrDefault(c => c.Name == msg.Text);
                                if (ThemeDict.ContainsKey(dict.ThemesId))
                                {
                                    ThemeDict[dict.ThemesId] = dict.Id;
                                }
                                else
                                {
                                    ThemeDict.Add(dict.ThemesId, dict.Id);
                                }
                                await client.SendTextMessageAsync(msg.Chat.Id, "Ваши cлова: ", replyMarkup: getUserWordsButtons());
                                await client.SendTextMessageAsync(msg.Chat.Id, GetListWords(dict.Id));
                            }
                            else
                            {
                                await client.SendTextMessageAsync(msg.Chat.Id, "Возможно такого словаря не существует", replyMarkup: getUserDictButtons());
                            }
                        }
                        break;
                    case "Напишите слово для заучивания":
                        string first = msg.Text;
                        using (SetDbWords dictRep = new SetDbWords())
                        {
                            if (!(dictRep.context.words.Any(c => c.LearningWord == msg.Text && c.DictionaryiesId == ThemeDict[UserTheme[msg.From.Id]])))
                            {
                                dictRep.AddWord(new Words() { LearningWord = msg.Text, DictionaryiesId = ThemeDict[UserTheme[msg.From.Id]] });
                                dictRep.Save();
                                var word = dictRep.context.words.FirstOrDefault(c => c.LearningWord == msg.Text);
                                if (UserWord.ContainsKey(msg.From.Id))
                                {
                                    UserWord[msg.From.Id] = word.Id;
                                }
                                else
                                {
                                    UserWord.Add(msg.From.Id, word.Id);
                                }
                            }
                            else
                            {
                                await client.SendTextMessageAsync(msg.Chat.Id, "Возможно такое слово уже существует", replyMarkup: getUserWordsButtons());
                            }
                        }
                        await client.SendTextMessageAsync(msg.Chat.Id, "Напишите его перевод", replyMarkup: new ForceReplyMarkup { Selective = true });
                        break;
                    case "Напишите его перевод":
                        using (SetDbWords dictRep = new SetDbWords())
                        {
                            if (!(dictRep.context.words.Any(c => c.LearningWord == msg.Text && c.DictionaryiesId == ThemeDict[UserTheme[msg.From.Id]])))
                            {
                                var word = dictRep.context.words.Find(UserWord[msg.From.Id]);
                                word.TranslateOfWord = msg.Text;
                                dictRep.context.words.Update(word);
                                dictRep.Save();
                            }
                            else
                            {
                                await client.SendTextMessageAsync(msg.Chat.Id, "Возможно такое слово уже существует", replyMarkup: getUserWordsButtons());
                            }
                        }
                        await client.SendTextMessageAsync(msg.Chat.Id, "Слово добавлено", replyMarkup: getUserWordsButtons());
                        await client.SendTextMessageAsync(msg.Chat.Id, "Ваши cлова: ", replyMarkup: getUserWordsButtons());
                        await client.SendTextMessageAsync(msg.Chat.Id, GetListWords(ThemeDict[UserTheme[msg.From.Id]]));
                        break;
                }


            }
            else
            {
                if (msg.Text != null)
                {
                    Console.WriteLine($"Пришло сообщение с текстом {msg.Text}");
                    //    await client.SendTextMessageAsync(msg.Chat.Id,msg.Text, replyMarkup: getStartButtons());       
                    switch (msg.Text)
                    {
                        case "/start":
                            await client.SendTextMessageAsync(msg.Chat.Id, "Начнем обучение :) ", replyMarkup: getStartButtons());
                            break;
                        case "Tемы":
                            await client.SendTextMessageAsync(msg.Chat.Id, "Ваши темы: ", replyMarkup: getUserThemesButtons());
                            await client.SendTextMessageAsync(msg.Chat.Id, GetListThemes(msg.From.Id));
                            break;
                        case "Добавить тему":
                            await client.SendTextMessageAsync(msg.Chat.Id, "Напишите имя темы для добавления", replyMarkup: new ForceReplyMarkup { Selective = true });
                            break;
                        case "Назад":
                            await client.SendTextMessageAsync(msg.Chat.Id,"Селайте выбор", replyMarkup: getStartButtons());
                            break;
                        case "Удалить тему":
                            await client.SendTextMessageAsync(msg.Chat.Id, "Напишите имя темы для удаления", replyMarkup: new ForceReplyMarkup { Selective = true });
                            break;
                        case "Выбрать":
                            await client.SendTextMessageAsync(msg.Chat.Id, "Напишите имя темы", replyMarkup: new ForceReplyMarkup { Selective = true });
                            break;
                        case "Выбрать cловарь":
                            await client.SendTextMessageAsync(msg.Chat.Id, "Напишите имя словаря", replyMarkup: new ForceReplyMarkup { Selective = true });
                            break;
                        case "Добавить словарь":
                            await client.SendTextMessageAsync(msg.Chat.Id, "Напишите имя словаря для добавления", replyMarkup: new ForceReplyMarkup { Selective = true });
                            break;
                        case "Назад к темам":
                            await client.SendTextMessageAsync(msg.Chat.Id, "Селайте выбор", replyMarkup: getUserThemesButtons());
                            break;
                        case "Удалить cловарь":
                            await client.SendTextMessageAsync(msg.Chat.Id, "Напишите имя словаря для удаления", replyMarkup: new ForceReplyMarkup { Selective = true });
                            break;
                        case "Выбрать cлово":
                            await client.SendTextMessageAsync(msg.Chat.Id, "Напишите имя слова", replyMarkup: new ForceReplyMarkup { Selective = true });
                            break;
                        case "Добавить слово":
                            await client.SendTextMessageAsync(msg.Chat.Id, "Напишите слово для заучивания", replyMarkup: new ForceReplyMarkup { Selective = true });
                            break;
                        case "Назад к словарям":
                            await client.SendTextMessageAsync(msg.Chat.Id, "Селайте выбор", replyMarkup: getUserThemesButtons());
                            break;
                        case "Удалить cлово":
                            await client.SendTextMessageAsync(msg.Chat.Id, "Напишите имя словаря для удаления", replyMarkup: new ForceReplyMarkup { Selective = true });
                            break;
                        default:
                            await client.SendTextMessageAsync(msg.Chat.Id, "Такого варианта нет :( ", replyMarkup: getStartButtons());
                            break;
                    }
                }
            }
        }
        private static string GetListThemes(decimal id)
        {
            using (SetDbTheme themeRep = new SetDbTheme())
            {
                var themes = themeRep.GetAllThemes().Where(c => c.UsersId == id).ToList();
                bool flag = true;
                if (themes.Count != 0){
                    string messege = "";
                    foreach (Themes theme in themes)
                    {
                        if (flag)
                        {
                            messege += (theme.Name);
                            flag = false;
                        } else messege += (", "+theme.Name );
                    }
                    return messege;
                }
                return "Список пуст";
            }
            
        }
        private static string GetListDict(int id)
        {
            using (SetDbDictionaries dictRep = new SetDbDictionaries())
            {
                var dicts = dictRep.GetDicts().Where(c => c.ThemesId== id).ToList();
                bool flag = true;
                if (dicts.Count != 0)
                {
                    string messege = "";
                    foreach (Dictionaryies dict in dicts)
                    {
                        if (flag)
                        {
                            messege += (dict.Name);
                            flag = false;
                        }
                        else messege += (", " + dict.Name);
                    }
                    return messege;
                }
                return "Список пуст";
            }

        }

        private static string GetListWords(int id)
        {
            using (SetDbWords wordRep = new SetDbWords())
            {
                var words= wordRep.GetAllWords().Where(c => c.DictionaryiesId == id).ToList();
                bool flag = true;
                if (words.Count != 0)
                {
                    string messege = "";
                    foreach (Words word in words)
                    {
                        if (flag)
                        {
                            messege += (word.LearningWord);
                            messege += (" " + word.TranslateOfWord);
                            flag = false;
                        }
                        else
                        {
                            messege += ("\n" + word.LearningWord);
                            messege += (" " + word.TranslateOfWord);
                        }
                    }
                    return messege;
                }
                return "Список пуст";
            }

        }
        private static IReplyMarkup getStartButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton> { new KeyboardButton { Text = "Tемы" } }
                }
            };
        }
        private static IReplyMarkup getUserThemesButtons()
        {
            return new ReplyKeyboardMarkup()
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton> { new KeyboardButton { Text = "Добавить тему" }, new KeyboardButton { Text = "Удалить тему" }, new KeyboardButton { Text = "Назад" }, new KeyboardButton { Text = "Выбрать" } }
                    
                }
            };
           
        }

        private static IReplyMarkup getUserDictButtons()
        {
            return new ReplyKeyboardMarkup()
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton> { new KeyboardButton { Text = "Добавить словарь" }, new KeyboardButton { Text = "Удалить cловарь" }, new KeyboardButton { Text = "Назад к темам" }, new KeyboardButton { Text = "Выбрать cловарь" } }

                }
            };

        }

        private static IReplyMarkup getUserWordsButtons()
        {
            return new ReplyKeyboardMarkup()
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton> { new KeyboardButton { Text = "Добавить слово" }, new KeyboardButton { Text = "Удалить cлово" }, new KeyboardButton { Text = "Назад к словарям" }, new KeyboardButton { Text = "Начать заучивание" } }

                }
            };

        }
    }
}
