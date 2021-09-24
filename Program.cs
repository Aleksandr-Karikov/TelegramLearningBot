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
                using (SetDbTheme themeRep = new SetDbTheme())
                {
                    switch (msg.ReplyToMessage.Text)
                    {
                        case "Напишите имя темы для добавления":
                            if (!(themeRep.context.themes.Any(c => c.Name == msg.Text && c.UsersId == msg.From.Id)))
                            {
                                themeRep.AddTheme(new Themes() { Name = msg.Text, UsersId = msg.From.Id });
                                themeRep.Save();
                                var themes = themeRep.GetAllThemes().Where(c => c.UsersId == msg.From.Id).ToList();
                                string messege = "";
                                foreach (Themes theme in themes)
                                {
                                    messege += (theme.Name + " ");
                                }
                                await client.SendTextMessageAsync(msg.Chat.Id, "Ваши темы: ", replyMarkup: getUserThemesButtons());
                                await client.SendTextMessageAsync(msg.Chat.Id, messege);
                            }
                            else
                            {
                               // await client.SendTextMessageAsync(msg.Chat.Id, "Возможно такая тема уже существует", replyMarkup: getUserThemesButtons());
                            }
                            
                            break;
                        case "Напишите имя темы для удаления":
                            // themeRep.Dellete(new Themes() { userId = msg.From.Id, name = msg.Text });
                            themeRep.Save();
                           // await client.SendTextMessageAsync(msg.Chat.Id, "Ваши темы: ", replyMarkup: getUserThemesButtons());
                            break;
                        default :

                            break;
                    }
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

                            using (SetDbTheme themeRep = new SetDbTheme())
                            {
                                var themes = themeRep.GetAllThemes().Where(c => c.UsersId == msg.From.Id).ToList();
                                string messege = "";
                                foreach (Themes theme in themes)
                                {
                                    messege += (theme.Name+" ");
                                }
                                await client.SendTextMessageAsync(msg.Chat.Id, messege);
                            }

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
                            await client.SendTextMessageAsync(msg.Chat.Id, "Напишите имя темы чтобы перейти к ней", replyMarkup: new ForceReplyMarkup { Selective = true });
                            break;
                        default:
                            await client.SendTextMessageAsync(msg.Chat.Id, "Такого варианта нет :( ", replyMarkup: getStartButtons());
                            break;
                    }
                }
            }
                //await client.SendTextMessageAsync(msg.Chat.Id, "Готовые темы: ", replyMarkup: getUserThemesButtons());
                //await client.SendTextMessageAsync(msg.Chat.Id, "Выберите и напишите имя темы в чат");
            
            
            
            
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


    }
}
