using System;


namespace Model
{
    [Serializable]
	public class DesktopPurchaseResponse
	{

		public string _id;
		public string userId;
		public string storyId;
		public string url;
		public long timestamp;

		public DesktopPurchaseResponse(string _id, string userId, string storyId, string url, long timestamp){
			this._id = _id;
			this.userId = userId;
			this.storyId = storyId;
			this.url = url;
			this.timestamp = timestamp;
		}

	}
}