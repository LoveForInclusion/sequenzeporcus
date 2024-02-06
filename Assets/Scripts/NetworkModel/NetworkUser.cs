using System;
using System.Collections;

namespace NetworkModel
{
    [Serializable]
    public class NetworkUser
    {
        public string email;
        public string password;
        public string firstName;
        public string lastName;
        public string role;

        public NetworkUser()
        {
        }

        public NetworkUser(string email, string password, string firstName, string lastName, string role)
        {
            this.email = email;
            this.password = password;
            this.firstName = firstName;
            this.lastName = lastName;
            this.role = role;
        }
    }
}