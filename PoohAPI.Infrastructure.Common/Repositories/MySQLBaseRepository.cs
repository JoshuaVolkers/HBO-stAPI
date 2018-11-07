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
        private readonly IMapper _mapper;
        private readonly IMySQLClient _client;

        public MySQLBaseRepository(IMapper mapper, IMySQLClient client)
        {
            _mapper = mapper;
            _client = client;
        }
        
        public T GetSingle<T>(string query)
        {
            var connection = _client.Connection();
            connection.Open();
            
            var command = new MySqlCommand(query, connection);
            var reader = command.ExecuteReader();
            var result = default(T);

            if (reader.HasRows)
            {
                reader.Read();
                result = _mapper.Map<IDataReader, T>(reader);
            }                    
            
            connection.Close();
            return result;
        }

        public T GetSingle<T>(string query, Dictionary<string,object> parameters)
        {
            var connection = _client.Connection();
            connection.Open();
            
            var command = new MySqlCommand(query, connection);

            foreach(KeyValuePair<string,object> parameter in parameters)
            {
                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }

            var reader = command.ExecuteReader();
            var result = default(T);

            if (reader.HasRows)
            {
                reader.Read();
                result = _mapper.Map<IDataReader, T>(reader);
            }

            connection.Close();
            return result;
        }

        public IEnumerable<T> GetAll<T>(string query)
        {
            var connection = _client.Connection();
            connection.Open();
            
            var command = new MySqlCommand(query, connection);
            var reader = command.ExecuteReader();
            var result = new List<T>();

            while (reader.Read())
                result.Add(_mapper.Map<IDataReader, T>(reader));

            connection.Close();
            return result;
        }

        public IEnumerable<T> GetAll<T>(string query, Dictionary<string, object> parameters)
        {
            var connection = _client.Connection();
            connection.Open();
            
            var command = new MySqlCommand(query, connection);

            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }

            var reader = command.ExecuteReader();
            var result = new List<T>();

            while (reader.Read())
                result.Add(_mapper.Map<IDataReader, T>(reader));

            connection.Close();
            return result;
        }

        public int NonQuery(string query, Dictionary<string, object> parameters)
        {
            var connection = _client.Connection();
            connection.Open();
            
            var command = new MySqlCommand(query, connection);

            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }

            var result = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
            return result;
        }
    }
}
