using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ambroladze_backend.Models;
using ambroladze_backend.Controllers;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;

namespace ambroladze_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public struct LoginData {
            public string login { get; set; }
            public string password { get; set; }
        }

        private readonly OrderContext _context;

        public AuthController(OrderContext context)
        {
            _context = context;
        }

        private string HashStr(string value)
        {
            var str = Encoding.UTF8.GetBytes(value);
            var sb = new StringBuilder();
            foreach (var b in MD5.HashData(str))
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        [HttpPost]
        public object GetToken([FromBody] LoginData ld)
        {
            ld.password = HashStr(ld.password);
            var user = _context.Clients.FirstOrDefault(u => u.Login == ld.login && u.Password == ld.password);
            if (user == null)
            {
                Response.StatusCode = 401;
                return new { message = "wrong login/password" };
            }
            return Auth.GenerateToken(user/*, user.IsWorker*/);
        }
        [HttpGet("users")]
        public List<Client> GetUsers()
        {
            return _context.Clients.ToList();
        }
        //[HttpGet("token")]
        //public object GetToken()
        //{
        //    return Auth.GenerateToken();
        //}
        //[HttpGet("token/secret")]
        //public object GetAdminToken()
        //{
        //    return Auth.GenerateToken(true);
        //}
        //[HttpGet("token/worker")]
        //public object GetWorkerToken()
        //{
        //    return Auth.GenerateToken(false, true);
        //}
    }
}
