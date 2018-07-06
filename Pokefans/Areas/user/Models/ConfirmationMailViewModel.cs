using System;
using Pokefans.Data;
namespace Pokefans.Areas.user.Models
{
    public class ConfirmationMailViewModel
    {
        public User User { get; set; }
        public string CallbackUrl { get; set; }
        public string ConfirmationKey { get; set; }
    }
}
