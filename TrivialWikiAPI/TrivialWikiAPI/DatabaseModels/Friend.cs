using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrivialWikiAPI.DatabaseModels
{
    public class Friend
    {
        [Key]
        public int Id { get; set; }
        public User FirstUser { get; set; }
        public User SecondUser { get; set; }
    }
}