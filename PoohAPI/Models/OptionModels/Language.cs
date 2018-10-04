using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoohAPI.Models.OptionModels
{
    /// <summary>
    /// Language that is supported by hbo-stagemarkt
    /// </summary>
    public class Language
    {
        public int Id { get; set; }
        public string LanguageName { get; set; }
    }
}
