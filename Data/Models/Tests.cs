using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TelegramLearningBot.Data.Models
{
    class Tests
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        public Themes Themes { get; set; }
        public int ThemesId { get; set; }
        public List<Questions> Questions { get; set; }
    }
}
