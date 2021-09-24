using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TelegramLearningBot.Data.Models
{
    class Words
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string LearningWord { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string TranslateOfWord { get; set; }

        public Dictionaryies Dictionaryies { get; set; }
        public int DictionaryiesId { get; set; }

    }
}
