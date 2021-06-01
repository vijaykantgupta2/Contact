using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ContactAPI.Models
{
	public class JwtRequest
    {
		private string username;
        private string password;

		//need default constructor for JSON Parsing
		public JwtRequest()
		{

		}

		public JwtRequest(string username, string password)
		{
			this.setUsername(username);
			this.setPassword(password);
		}

		public string getUsername()
		{
			return this.username;
		}

		public void setUsername(string username)
		{
			this.username = username;
		}

		public string getPassword()
		{
			return this.password;
		}

		public void setPassword(string password)
		{
			this.password = password;
		}
	}
}
