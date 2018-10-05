using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MySql.Data.MySqlClient;

namespace PoohAPI.Infrastructure.Common.Repositories
{
    public abstract class MySQLBaseRepository : IMySQLBaseRepository
    {
        private MySqlConnection _connection;
        private string _server;
        private string _database;
        private string _uid;
        private string _password;

        private readonly IMapper _mapper;

        public MySQLBaseRepository(IMapper mapper)
        {
            _mapper = mapper;
            Init();
        }

        //Add methods for SELECT, INSERT, UPDATE statements that also manage the connectionstate so as to keep the responsability
        //for the connectionstate it in this class.
        public T GetSingle<T>(string query)
        {
            if (OpenConnection())
            {
                var command = new MySqlCommand(query, _connection);
                var reader = command.ExecuteReader();
                CloseConnection();
                if (reader.HasRows)
                    return _mapper.Map<IDataReader, T>(reader);
            }

            return default(T);
        }

        public IEnumerable<T> GetAll<T>(string query)
        {
            if (OpenConnection())
            {
                var command = new MySqlCommand(query, _connection);
                var reader = command.ExecuteReader();
                CloseConnection();
                if (reader.HasRows)
                    return _mapper.Map<IDataReader, List<T>>(reader);
            }
            return null;
        }

        public void NonQuery(string query)
        {
            if (OpenConnection())
            {
                var command = new MySqlCommand(query, _connection);
                command.ExecuteNonQuery();
                CloseConnection();
            }
        }

        private void Init()
        {
            _server = "test";
            _database = "test";
            _uid = "test";
            _password = "test";
            var connectionString = string.Format("server={0};database={1};uid={2};password={3};", _server, _database,
                _uid, _password);
            _connection = new MySqlConnection(connectionString);
        }

        private bool OpenConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
                return true;

            _connection.Open();
            return true;
        }

        private bool CloseConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
                return true;

            _connection.Close();
            return true;
        }
    }
}
