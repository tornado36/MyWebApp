using Microsoft.Extensions.Logging;
using MyWebApp.Class;
using MyWebApp.Models;
using MyWebAppCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace MyWebApp.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        private readonly ILogger _logger;
        private readonly ConfigContext _context;

        public UserRepository(ConfigContext context, ILogger<UserRepository> logger) : base(context, logger)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            string sErr = "";
            try
            {
                _logger = logger;
                _context = context;
            }
            catch
            {
                goto Get_Out;
            }

        Get_Out:

            if (sErr.Length > 0)
            {
                _logger.LogError(sErr);
            }
        }
        public User Get(string name)
        {
            User result = null;
            string sErr = "";
            try
            {
                if (String.IsNullOrWhiteSpace(name))
                {
                    sErr += "User name cannot be null!";
                    goto Get_Out;
                }

                result = _context.Users.FirstOrDefault(x => x.Username.ToUpper() == name.ToUpper());

            }
            catch (Exception ex)
            {

                sErr += "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }
        Get_Out:

            if (sErr.Length > 0)
            {
                _logger.LogError(sErr);
            }

            return result;
        }

        public string GetUserID(string name)
        {
            User result = null;
            string sErr = "";
            try
            {
                if (String.IsNullOrWhiteSpace(name))
                {
                    sErr += "User name cannot be null!";
                    goto Get_Out;
                }

                result = _context.Users.FirstOrDefault(x => x.Username.ToUpper() == name.ToUpper());
                sErr = result.ID;

            }
            catch (Exception ex)
            {

                sErr += "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }
        Get_Out:

            if (sErr.Length > 0)
            {
                _logger.LogError(sErr);
            }

            return sErr;
        }
        public List<User> GetAll()
        {
            List<User> users = new List<User>();
            string sErr = "";
            try
            {
                users = _context.Users.ToList();
            }
            catch (Exception ex)
            {

                sErr += "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }
        Get_Out:

            if (sErr.Length > 0)
            {
                _logger.LogError(sErr);
            }

            return users;
        }

        public string Add(User user)
        {
            string sErr = "";
            try
            {
                if (String.IsNullOrWhiteSpace(user.Username))
                {
                    sErr += "User name cannot be null!";
                    goto Get_Out;
                }

                _context.Users.Add(user);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {

                sErr += "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }
        Get_Out:

            if (sErr.Length > 0)
            {
                _logger.LogError(sErr);
            }

            return sErr;
        }
    }
}
