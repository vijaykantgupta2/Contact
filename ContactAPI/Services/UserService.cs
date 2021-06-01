using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ContactAPI.Entities;
using ContactAPI.Helpers;
using ContactAPI.Models;

namespace ContactAPI.Services
{
    public interface IUserService
    {
        JwtResponse Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<User> _users = new List<User>
        {
            new User { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test" }
        };

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public JwtResponse Authenticate(string username, string password)
        {
            var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new JwtResponse(token);
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }

        // helper methods

        private string generateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_appSettings.Issuer, _appSettings.Issuer,
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}