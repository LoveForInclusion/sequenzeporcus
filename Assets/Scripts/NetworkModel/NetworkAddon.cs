using System;

namespace NetworkModel 
{
    [Serializable]
    public class NetworkAddon
    {
        public string _id;
        public string storyId;
        public long created;
        public long lastUpdate;
        public string webserverUrl;
        public string Category;
        public float Price;
        public string Age;
        public bool ComingSoon;
        public string ShelfCover;
        public string Data;
        public string Title;

        public NetworkAddon(string _id, string storyId, long created, long lastUpdate, string webserverUrl, string Category, float Price, string Age, bool ComingSoon, string ShelfCover, string Data, string Title)
        {
            this._id = _id;
            this.storyId = storyId;
            this.created = created;
            this.lastUpdate = lastUpdate;
            this.webserverUrl = webserverUrl;
            this.Category = Category;
            this.Price = Price;
            this.Age = Age;
            this.ComingSoon = ComingSoon;
            this.ShelfCover = ShelfCover;
            this.Data = Data;
            this.Title = Title;
        }
    }
}
