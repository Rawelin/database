using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza
{
    public class User
    {
        private string id;
        private string login;
        private string password;

        public User() { }

        public User(string login, string password)
        {
            this.login = login;
            this.password = password;
        }

        public string Login
        {
            get
            {
                return login;
            }
            set
            {
                this.login = value;
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                this.password = value;
            }
        }
        public string ID
        {
            get
            {
                return id;
            }
            set
            {
                this.id = value;
            }
        }
    }
}
