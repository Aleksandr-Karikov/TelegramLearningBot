using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TelegramLearningBot.Data.Models
{
    class Questions
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string questionText { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string AnswerText { get; set; }

        public Tests Tests { get; set; }
        public int TestsId { get; set; }

    }

}
