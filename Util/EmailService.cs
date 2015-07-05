// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Pokefans.Util
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Hier den E-Mail-Dienst einfügen, um eine E-Mail-Nachricht zu senden.
            return Task.FromResult(0);
        }
    }
}
