using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TelegramLearningBot.Data.Models
{
    class Users
    {


        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "decimal")]
        public decimal Id { get; set; }
      

        
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }



        public List<Themes> Themes { get; set; }
    }
}
