using System;
using System.Runtime.Serialization;

namespace Model
{
    [Serializable]
    public class UserInput
    {
        public string email;
        public string password;
		public string device;
		public string deviceName;

        public UserInput(string email, string password, string device, string deviceName)
        {
            this.email = email;
            this.password = password;
			this.device = device;
			this.deviceName = deviceName;
        }
    }
}