using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Infrastructure.Common
{
    public interface IMySQLClient
    {
        //bool OpenConnection();
        //bool CloseConnection();
        MySqlConnection Connection();
    }
}
