using AutoMapper;
using PoohAPI.Common;
using PoohAPI.Infrastructure.UserDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Users.Services
{
    public class UserReadService : IUserReadService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserReadService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public IEnumerable<User> GetAllUsers(int maxCount, int offset)
        {
            var query = string.Format("SELECT * FROM wp_dev_users LIMIT {0} OFFSET {1}", maxCount, offset);
            var users = _userRepository.GetAllUsers(query);
            return _mapper.Map<IEnumerable<User>>(users);
        }

        public User GetUserById(int id)
        {
            //var query = string.Format("SELECT * FROM wp_dev_users WHERE ID = {0}", id);

            string query = @"
                SELECT u.user_id, u.user_email, u.user_name, s.user_land, s.user_woonplaats, s.user_opleiding_id, 
                    s.user_op_niveau, s.user_taal, s.user_breedtegraad, s.user_lengtegraad, 
                    IF(l.land_naam IS NULL, '', l.land_naam) AS land_naam, IF(t.talen_naam IS NULL, '', t.talen_naam) AS talen_naam, 
                    IF(o.opl_naam IS NULL, '', o.opl_naam) AS opl_naam, IF(op.opn_naam IS NULL, '', op.opn_naam) AS opn_naam 
                FROM reg_users u 
                INNER JOIN reg_user_studenten s ON u.user_id = s.user_id
                LEFT JOIN reg_landen l ON s.user_land = l.land_id
                LEFT JOIN reg_talen t ON s.user_taal = t.talen_id
                LEFT JOIN reg_opleidingen o ON s.user_opleiding_id = o.opl_id
                LEFT JOIN reg_opleidingsniveau op ON s.user_op_niveau = op.opn_id
                WHERE u.user_role = 0 AND u.user_active = 1 AND u.user_id = @id
                ";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);

            var user = _userRepository.GetUser(query, parameters);
            
            return _mapper.Map<User>(user);
        }

        public User Login(string login, string password)
        {
            //var query = string.Format("SELECT * FROM wp_dev_users WHERE user_login = '{0}'", login);
            //var user = _userRepository.GetUser(query);
            //var encoded = PasswordHasher.Encode(password, user.user_pass);
            //if (encoded.Equals(user.user_pass))
            //    return _mapper.Map<User>(user);

            return null;
        }
    }
}
