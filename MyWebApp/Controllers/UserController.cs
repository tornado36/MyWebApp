using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyWebApp.Class;
using MyWebApp.Repositories;
using MyWebAppCore.Class;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using static MyWebApp.Enum.MyWebAppEnum;

namespace MyWebApp.Controllers
{
    [Route("v1/users")]
    [ApiController]
    public class UserController : ConfigDbContextController
    {
        private readonly UserRepository _userRepository;
        private readonly ILogger _logger;
        public UserController(User user, ILoggerFactory loggerFactory)
        {
            if (user == null) throw new ArgumentNullException(nameof(user), "user name cannot be null");
            _userRepository = new UserRepository(base._configDbContext, loggerFactory.CreateLogger<UserRepository>());
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<JsonResult> CreateUser([FromBody] UserInput input)
        {
            string sErr = "";
            JsonResult response = null;
            User user = null;
            try
            {
                Project existProject = null;
                if (String.IsNullOrEmpty(input.Username))
                {
                    sErr = "User name cannot be empty!";
                    goto Get_Out;
                }
                await Task.Run(() =>
                {
                    user = _userRepository.Get(input.Username);
                });
                if (user != null)
                {
                    sErr = "Username cannot be null!";
                    goto Get_Out;
                }
                else
                {
                    User newuser = new User()
                    {
                        Username = input.Username,
                        Height = input.Height,
                        Gender = input.Gender
                    };
                    await Task.Run(() =>
                    {
                        sErr = _userRepository.Add(newuser);
                    });
                    if (sErr != "") goto Get_Out;
                }

                //using (ConfigContext db = new ConfigContext(new DbContextOptions<ConfigContext>()))
                //{
                //    db.Porjects.Add(project);
                //    db.SaveChanges();
                //}
            }
            catch (Exception ex)
            {

                sErr += "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }
        Get_Out:
            if (!String.IsNullOrEmpty(sErr))
            {
                sErr += "\r\nRoutine=" + MethodInfo.GetCurrentMethod().ReflectedType.Name + "." + MethodInfo.GetCurrentMethod().ToString();
                //_logger.LogError(sErr);
                response = Json(new AjaxResponse
                {
                    Succeed = false,
                    Status = "failed",
                    Message = "Create user failed:" + sErr
                });

                return response;
            }

            else
            {
                sErr = "Create Model Success!";
                //_logger.LogInformation(sErr);
                response = Json(new AjaxResponse
                {
                    Succeed = true,
                    Status = "success",
                    Message = "Create user success!"
                });

                return response;
            }

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<JsonResult> GetAllUsers()
        {
            string sErr = "";
            JsonResult response = null;
            List<User> users = new List<User>();
            try
            {
                await Task.Run(() =>
                 {
                     users = _userRepository.GetAll();
                 });
            }
            catch (Exception ex)
            {

                sErr += ex.Message + "\r\n" + ex.StackTrace;
            }
        Get_Out:
            if (!String.IsNullOrEmpty(sErr))
            {
                response = Json(new AjaxResponse
                {
                    Succeed = false,
                    Status = "failed",
                    Message = "Get users failed:" + sErr
                });
                return response;
            }
            else
            {
                response = Json(new AjaxResponse
                {
                    Succeed = true,
                    Status = "success",
                    Message = "Get users success",
                    Data = users.ToArray()
                });
                return response;
            }
        }



        public class UserInput
        {
            public string Username { get; set; }
            public double Height { get; set; }
            public Gender Gender { get; set; }
        }
    }

}

