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
        
        //Add methods for SELECT, INSERT, UPDATE statements that also manage the connectionstate so as to keep the responsability
        //for the connectionstate it in this class.
        public T GetSingle<T>(string query)
        {
            if (_client.OpenConnection())
            {
                var command = new MySqlCommand(query, _client.Connection());
                var reader = command.ExecuteReader();
                var result = default(T);

                if (reader.HasRows)
                {
                    reader.Read();
                    result = _mapper.Map<IDataReader, T>(reader);
                }                    

                _client.CloseConnection();
                return result;
            }
            return default(T);
        }

        public T GetSingle<T>(string query, Dictionary<string,object> parameters)
        {
            if (_client.OpenConnection())
            {
                var command = new MySqlCommand(query, _client.Connection());

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

                _client.CloseConnection();
                return result;
            }
            return default(T);
        }

        public IEnumerable<T> GetAll<T>(string query)
        {
            if (_client.OpenConnection())
            {
                var command = new MySqlCommand(query, _client.Connection());
                var reader = command.ExecuteReader();
                var result = new List<T>();

                while (reader.Read())
                    result.Add(_mapper.Map<IDataReader, T>(reader)); 

                _client.CloseConnection();
                return result;
            }
            return null;
        }

        public IEnumerable<T> GetAll<T>(string query, Dictionary<string, object> parameters)
        {
            if (_client.OpenConnection())
            {
                var command = new MySqlCommand(query, _client.Connection());

                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                var reader = command.ExecuteReader();
                var result = new List<T>();

                while (reader.Read())
                    result.Add(_mapper.Map<IDataReader, T>(reader));

                _client.CloseConnection();
                return result;
            }
            return null;
        }

        public int NonQuery(string query)
        {
            if (_client.OpenConnection())
            {
                var command = new MySqlCommand(query, _client.Connection());
                var result = command.ExecuteNonQuery();
                _client.CloseConnection();
                return result;
            }

            return 0;
        }

        public int NonQuery(string query, Dictionary<string, object> parameters)
        {
            if (_client.OpenConnection())
            {
                var command = new MySqlCommand(query, _client.Connection());

                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                var result = Convert.ToInt32(command.ExecuteScalar());
                _client.CloseConnection();
                return result;
            }

            return 0;
        }

        //public int NonQueryReturnId(string query, Dictionary<string, object> parameters, string returnId)
        //{
        //    if (_client.OpenConnection())
        //    {
        //        var command = new MySqlCommand(query, _client.Connection());

        //        foreach (KeyValuePair<string, object> parameter in parameters)
        //        {
        //            command.Parameters.AddWithValue(parameter.Key, parameter.Value);
        //        }
        //        if(!string.IsNullOrEmpty(returnId))
        //            command.Parameters.Add("@"+ returnId, MySqlDbType.Int16, 11).Direction = ParameterDirection.Output;

        //        var result = command.ExecuteNonQuery();
        //        _client.CloseConnection();
        //        return result;
        //    }

        //    return 0;
        //}
    }
}
