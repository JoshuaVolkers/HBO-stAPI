﻿using System;
using System.Collections.Generic;
using AutoMapper;
using PoohAPI.Infrastructure.UserDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Helpers;

namespace PoohAPI.Logic.Users.Services
{
    public class UserCommandService : IUserCommandService
    {
        private readonly IUserRepository _userRepository;       
        private readonly IMapper _mapper;
        private readonly IUserReadService _userReadService;

        public UserCommandService(IUserRepository userRepository, IMapper mapper, IUserReadService userReadService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userReadService = userReadService;
        }

        public User RegisterUser(string login, string password, string email)
        {
            var hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 10);
            //SELECT LAST_INSERT_ID() returns the primary key of the created record.
            var query = string.Format("INSERT INTO reg_users (user_email, user_password, user_name, user_salt, user_role, user_role_id) " +
                                      " VALUES(@user_email, @user_password, @user_name, @user_salt, @user_role, @user_role_id);" +
                                      "SELECT LAST_INSERT_ID()");
            var parameters = new Dictionary<string, object>();
            parameters.Add("@user_email", email);
            parameters.Add("@user_password", hashedPassword);
            parameters.Add("@user_name", login);
            parameters.Add("@user_salt", "salt");
            parameters.Add("@user_role", 0);
            parameters.Add("@user_role_id", 0);

            var createdUserId = _userRepository.RegisterUser(query, parameters);

            return _mapper.Map<User>(_userReadService.GetUserById(createdUserId));
        }
    }
}
