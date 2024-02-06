using System;
using Model;
using NetworkModel;
using UnityEngine;
using UnityEngine.Networking;

namespace RESTClient.Request
{
	public static class StoryRequest
	{
		public static void GetAllStory(MonoBehaviour mono, Action<Stories> onComplete, Action<string, string> onError)
		{
			ApiClient.Get(mono, RestConfig.BaseUrl+"stories/", null,
				delegate(UnityWebRequest www)
				{
					if (www.error != null)
					{
						onError(www.error, www.downloadHandler.text);
						return;
					}
				//Debug.Log(www.downloadHandler.text.Substring(1, www.downloadHandler.text.Length - 2));
				var jsonArray = "{\"stories\":" + www.downloadHandler.text + "}";
				onComplete(NetworkTranslator.translateStories(JsonUtility.FromJson<NetworkStories>(jsonArray)));
				});
		}

		public static void GetStoryById(MonoBehaviour mono,  string storyId, Action<NetworkStory> onComplete, Action<string, string> onError)
		{
			ApiClient.Get(mono, RestConfig.BaseUrl+"stories/" + storyId, null,
				delegate(UnityWebRequest www)
				{
					if (www.error != null)
					{
						onError(www.error, www.downloadHandler.text);
						return;
					}
					//Debug.Log(www.downloadHandler.text);
					onComplete(JsonUtility.FromJson<NetworkStory>(www.downloadHandler.text));
				});
		}
	}
}
