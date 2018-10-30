using System;
using System.Collections.Generic;
using PoohAPI.Logic.Common.Enums;

namespace PoohAPI.Logic.Common.Models.BaseModels
{
    /// <summary>
    /// BaseUser that contains the same information as the wp_users table in WordPress.
    /// </summary>
    public class BaseUser
    {
        public int Id { get; set; }
        public string NiceName { get; set; }
        public string EmailAddress { get; set; }
        public UserRole Role { get; set; }
        public bool Active { get; set; }
    }
}
