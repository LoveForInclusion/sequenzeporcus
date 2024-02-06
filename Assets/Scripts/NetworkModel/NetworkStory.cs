using System;
using System.Collections.Generic;
using Model;
namespace NetworkModel 
{

    [Serializable]
    public class NetworkStory
    {
        public string _id;
		public string androidData;
		public string iosData;
		public string macData;
        public string winData;
		public string linuxData;
        public List<NetworkAddon> addons;
        public long created;
        public long lastUpdate;
        public string webserverUrl;
		public List<Activities> activities;
		public bool ComingSoon;
		public string Cover;
		public string ShelfCover;
		public string category;
		public float Price;
		public string Age;
		public string Author;
		public string Description;
		public string Title;
		public bool isDownloaded;
       
		public NetworkStory(string _id,
			string androidData,
			string iosData,
            string macData,
            string winData,
            string linuxData,
			List<NetworkAddon> addons,
			long created,
			long lastUpdate,
			string webserverUrl,
			List<Activities> activities,
			bool ComingSoon,
			string Cover,
			string ShelfCover,
			string category,
			float Price,
			string Age,
			string Author,
			string Description,
			string Title,
			bool isDownloaded = false){

			this.androidData = androidData;
			this.iosData=iosData;
			this.macData = macData;
			this.winData=winData;
			this.linuxData = linuxData;
			this.addons = addons;
			this.created = created;
			this.lastUpdate = lastUpdate;
			this.webserverUrl = webserverUrl;
			this.activities = activities;
			this.ComingSoon = ComingSoon;
			this.Cover = Cover;
			this.ShelfCover = ShelfCover;
			this.category = category;
			this.Price = Price;
			this.Age = Age;
			this.Author = Author;
			this.Description = Description;
			this.Title = Title;
			this.isDownloaded = isDownloaded;
			
		}
			
    }

    [Serializable]
    public class NetworkStories
    {
        public List<NetworkStory> stories;

        public NetworkStories(List<NetworkStory> stories)
        {
            this.stories = stories;
        }
    }

}
