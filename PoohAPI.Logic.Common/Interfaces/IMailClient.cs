using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Interfaces
{
    public interface IMailClient
    {
        void SendEmail(string userEmail, string subject, string body);
    }
}
