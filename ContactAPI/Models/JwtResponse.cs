using System;
using ContactAPI.Entities;

namespace ContactAPI.Models
{
    public class JwtResponse
    {
		//public int Id { get; set; }
		//public string FirstName { get; set; }
		//public string LastName { get; set; }
		//public string Username { get; set; }
		public string jwttoken { get; set; }

		public JwtResponse(string token)
		{
			//Id = user.Id;
			//FirstName = user.FirstName;
			//LastName = user.LastName;
			//Username = user.Username;
			jwttoken = token;
		}

	}
}
