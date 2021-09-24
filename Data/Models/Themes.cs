using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TelegramLearningBot.Data.Models
{
    class Themes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Users Users { get; set; }
        public decimal UsersId { get; set; }

        public List<Dictionaryies> Dictionaryies { get; set; }
        public List<Tests> Tests { get; set; }
    }
}
