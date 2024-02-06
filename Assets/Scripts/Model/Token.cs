using System;

namespace Model
{
    [Serializable]
    public class Token
    {
        public string userId;
        public string token;
		public string refreshToken;
		public RefreshToken internalClass;

        public Token(string userId, string token, string refreshToken)
        {
            this.userId = userId;
            this.token = token;
			this.refreshToken = refreshToken;
			this.internalClass = new RefreshToken(userId, refreshToken);
        }
    }

	[Serializable]
	public class RefreshToken{
		public string userId;
		public string refreshToken;

		public RefreshToken(string userId, string refreshToken){
			this.userId = userId;
			this.refreshToken = refreshToken;
		}
	}

}