using System.Collections.Generic;
using AutoMapper;
using PoohAPI.Infrastructure.UserDB.Models;
using PoohAPI.Infrastructure.UserDB.Repositories;
using PoohAPI.Logic.Common.Enums;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using System.Net.Mail;
using System;
using System.Web;

namespace PoohAPI.Logic.Users.Services
{
    public class UserCommandService : IUserCommandService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IMapAPIReadService mapAPIReadService;
        private readonly IQueryBuilder queryBuilder;
        private readonly IUserReadService userReadService;
        private readonly IMailClient mailClient;

        public UserCommandService(IUserRepository userRepository, IMapper mapper,
            IMapAPIReadService mapAPIReadService, IQueryBuilder queryBuilder, IUserReadService userReadService,
            IMailClient mailClient)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.mapAPIReadService = mapAPIReadService;
            this.queryBuilder = queryBuilder;
            this.userReadService = userReadService;
            this.mailClient = mailClient;
        }

        public JwtUser RegisterUser(string login, string email, UserAccountType accountType, string password = null)
        {
            //SELECT LAST_INSERT_ID() returns the primary key of the created record.
            var query = string.Format("INSERT INTO reg_users (user_email, user_password, user_name,  user_role, user_role_id, user_account_type, user_active, user_refresh_token) " +
                                      " VALUES(@user_email, @user_password, @user_name, @user_role, @user_role_id, @user_account_type, @user_active, @user_refresh_token);" +
                                      "SELECT LAST_INSERT_ID()");
            var parameters = new Dictionary<string, object>();
            parameters.Add("@user_email", email);
            parameters.Add("@user_name", login);
            parameters.Add("@user_role", 0);
            parameters.Add("@user_role_id", 0);
            parameters.Add("@user_account_type", (int)accountType);
            parameters.Add("@user_active", 0);
            parameters.Add("@user_refresh_token", Guid.NewGuid().ToString("N"));

            if (!string.IsNullOrEmpty(password))
            {
                var hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 10);
                parameters.Add("@user_password", hashedPassword);
            }
            else
            {
                parameters.Add("@user_password", null);
            }

            var createdUserId = this.userRepository.UpdateDelete(query, parameters);

            string emailVerificationToken = this.CreateEmailVerificationToken(createdUserId);
            var user = this.userReadService.GetUserById(createdUserId, false);

            int minutes = 15;
            this.CreateEmailVerification(createdUserId, emailVerificationToken, DateTime.Now.AddMinutes(minutes));
            this.SendVerificationEmail(user, emailVerificationToken, minutes);

            return this.mapper.Map<JwtUser>(user);
        }

        public void ResetPassword(string email, int userid)
        {
            string emailVerificationToken = this.CreateEmailVerificationToken(userid);
            var user = this.userReadService.GetUserByEmail<User>(email);
            int minutes = 15;
            this.CreateEmailVerification(userid, emailVerificationToken, DateTime.Now.AddMinutes(minutes));
            this.SendResetEmail(user, emailVerificationToken, minutes);
        }

        private void SendVerificationEmail(User user, string emailVerificationToken, int expirationMinutes)
        {
            string url = "http://localhost:60824/users/verify?token=" + emailVerificationToken;

            string subject = "e-mail verification hbo-stagemarkt";
            string body = "Beste " + user.NiceName + ",<br/><br/>";
            body += "Dank je voor het aanmelden bij hbo-stagemarkt. Klik op de volgende link om je account te bevestigen: <br/><br/> ";
            body += "<a href=\"" + url + "\" target=\"_blank\" >" + url + "</a><br/><br/> ";
            body += "Deze link is " + expirationMinutes.ToString() + " minuten geldig.<br/><br/>";
            body += "Met vriendelijke groet, <br/><br/>";
            body += "Stichting ELBHO";

            this.mailClient.SendEmail(user.EmailAddress, subject, body);
        }

        private void SendResetEmail(User user, string emailVerificationToken, int expirationMinutes)
        {
            string url = "http://localhost:49970/users/verifyreset?token=" + emailVerificationToken;

            string subject = "wachtwoord reset hbo-stagemarkt";
            string body = "Beste " + user.NiceName + ",<br/><br/>";
            body += "Deze mail wordt gestuurd omdat het wachtwoord van uw hbo-stagemarkt account is gereset.: <br/><br/> ";
            body += "Klik op de volgende link om je nieuwe wachtwoord te genereren. Als u niet uw wachtwoord wilt resetten, dan klikt u niet op de link: <br/><br/> ";
            body += "<a href=\"" + url + "\" target=\"_blank\" >" + url + "</a><br/><br/> ";
            body += "Deze link is " + expirationMinutes.ToString() + " minuten geldig.<br/><br/>";
            body += "Met vriendelijke groet, <br/><br/>";
            body += "Stichting ELBHO";

            this.mailClient.SendEmail(user.EmailAddress, subject, body);
        }

        /// <summary>
        /// Create token for e-mail verification
        /// </summary>
        /// <returns>string</returns>
        public string CreateEmailVerificationToken(int userId)
        {
            UserEmailVerification validation = this.userReadService.GetUserEmailVerificationByUserId(userId);

            if (validation != null)
            {
                // Destroy existing validation for this user
                this.DeleteEmailVerificationToken(userId);
            }

            string emailValidationToken = "";

            //TODO: maybe refactor this, total number of unique GUIDS is 2^128 (very many), so do-while is overbodig.
            // Create token until it is unique
            do
            {
                emailValidationToken = Guid.NewGuid().ToString();
                validation = this.userReadService.GetUserEmailVerificationByToken(emailValidationToken);
            }
            while (validation != null);

            return emailValidationToken;
        }

        public void DeleteEmailVerificationToken(int userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@ver_user_id", userId);

            string query = "DELETE FROM reg_user_verification WHERE ver_user_id = @ver_user_id";

            this.userRepository.UpdateDelete(query, parameters);
        }

        public void CreateEmailVerification(int userId, string token, DateTime expirationDate)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@user_id", userId);
            parameters.Add("@token", token);
            parameters.Add("@expiration", expirationDate);

            string query = @"INSERT INTO reg_user_verification (ver_user_id, ver_token, ver_expiration) 
                             VALUES (@user_id, @token, @expiration)";

            this.userRepository.UpdateDelete(query, parameters);
        }

        public User VerifyUserEmail(string token)
        {
            UserEmailVerification userEmailValidation = this.userReadService.GetUserEmailVerificationByToken(token);

            if (userEmailValidation is null)
                return null;

            if (userEmailValidation.ExpirationDate < DateTime.Now)
            {
                this.DeleteEmailVerificationToken(userEmailValidation.UserId);
                return null;
            }

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", userEmailValidation.UserId);

            string query = @"UPDATE reg_users SET user_active = 1 WHERE user_id = @id";
            this.userRepository.UpdateDelete(query, parameters);
            this.DeleteEmailVerificationToken(userEmailValidation.UserId);

            return this.mapper.Map<User>(this.userReadService.GetUserById(userEmailValidation.UserId));
        }

        public string VerifyResetPassword(string token)
        {
            UserEmailVerification userEmailValidation = this.userReadService.GetUserEmailVerificationByToken(token);

            if(userEmailValidation is null)
            {
                return null;
            }

            if (userEmailValidation.ExpirationDate < DateTime.Now)
            {
                this.DeleteEmailVerificationToken(userEmailValidation.UserId);
                return null;
            }

            else
            {
                string newPassword = Guid.NewGuid().ToString("d").Substring(1, 12);
                this.UpdatePassword(userEmailValidation.UserId, newPassword, null, true);
                return newPassword;
            }
        }

        public void DeleteUser(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@id", id);

            string query = @"UPDATE reg_reviews SET review_student_id = 0 WHERE review_student_id = @id;
                             DELETE FROM reg_vacatures_favoriet WHERE vf_user_id = @id;
                             DELETE FROM reg_user_studenten WHERE user_id = @id;
                             DELETE FROM reg_users WHERE user_id = @id;
                            ";

            this.userRepository.UpdateDelete(query, parameters);
        }

        public User UpdateUser(int countryId, string city, int educationId, int educationalAttainmentId, int preferredLanguageId, int userId, string AdditionalLocationIdentifier = null)
        {
            this.InsertStudentDataIfNotExist(userId);

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            
            this.queryBuilder.SetUpdate("reg_user_studenten");
            this.queryBuilder.AddUpdateSet(@"user_land = @countryId, user_woonplaats = @cityName, user_opleiding_id = @educationId, 
                                 user_op_niveau = @educationLevelId, user_taal = @languageId");
            this.queryBuilder.AddWhere("user_id = @id");

            parameters.Add("@countryId", countryId);
            parameters.Add("@cityName", city);
            parameters.Add("@educationId", educationId);
            parameters.Add("@educationLevelId", educationalAttainmentId);
            parameters.Add("@languageId", preferredLanguageId);
            parameters.Add("@id", userId);

            Coordinates coordinates = this.mapAPIReadService.GetMapCoordinates(city, null, AdditionalLocationIdentifier);

            if (coordinates is Coordinates)
            {
                this.queryBuilder.AddUpdateSet("user_breedtegraad = @latitude");
                this.queryBuilder.AddUpdateSet("user_lengtegraad = @longitude");
                parameters.Add("@latitude", coordinates.Latitude);
                parameters.Add("@longitude", coordinates.Longitude);
            }

            string query = this.queryBuilder.BuildUpdate();

            this.userRepository.UpdateDelete(query, parameters);

            return this.userReadService.GetUserById(userId);
        }

        public void DeleteRefreshToken(string refreshToken)
        {
            //Using "user_id <> 0" is a hacky way of avoiding the "You are using safe update mode and you tried to update a table without a WHERE that uses a KEY column" error.
            string query = @"UPDATE reg_users SET user_refresh_token = NULL WHERE user_refresh_token = @refreshToken AND user_id <> 0";
            var parameters = new Dictionary<string, object>
            {
                { "@refreshToken", refreshToken }
            };

            this.userRepository.UpdateDelete(query, parameters);
        }

        public string UpdateRefreshToken(int userId)
        {
            var token = Guid.NewGuid().ToString("N");
            string query = @"UPDATE reg_users SET user_refresh_token = @refreshToken WHERE user_id = @id";
            var parameters = new Dictionary<string, object>
            {
                { "@refreshToken", token },
                { "@id", userId }
            };

            userRepository.UpdateDelete(query, parameters);
            return token;
        }

        private void InsertStudentDataIfNotExist(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string query = "SELECT * FROM reg_user_studenten WHERE user_id = @id";

            parameters.Add("@id", id);

            DBUser dbUser = this.userRepository.GetUser(query, parameters);

            if (dbUser != null)
                return;

            parameters.Clear();
            parameters.Add("@id", id);

            string command = @"INSERT INTO reg_user_studenten
                               VALUES (@id, 0, '', 0, 0, 0, 0, 0, 0)";

            this.userRepository.UpdateDelete(command, parameters);
        }

        public bool UpdatePassword(int userid, string newPassword, string oldPassword = null, bool isreset = false)
        {
            JwtUser jwtUser;

            if (isreset)
            {
                jwtUser = null;
            }

            else
            {
                jwtUser = userReadService.Login(oldPassword, null, userid);
            }

            if (isreset || jwtUser != null)
            {
                string hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(newPassword, 10);
                string query = @"UPDATE reg_users SET user_password = @password WHERE user_id = @userid";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@userid", userid },
                    {"@password", hashedPassword }
                };

                userRepository.UpdateDelete(query, parameters);

                return true;
            }

            else
            {
                return false;
            }
        }
    }
}
