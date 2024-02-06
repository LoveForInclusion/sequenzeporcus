using System;
using UnityEngine;
using RESTClient;
using Model;
using UnityEngine.Networking;


namespace RESTClient.Request
{
	public class DesktopPurchaseRequest
	{
		public static void RequestPurchase(MonoBehaviour mono, string userId, string storyId, Action<DesktopPurchaseResponse> onComplete, Action<string, string> onError)
		{
			Debug.Log("{\n\"userId\":\"" + userId + "\",\n\"storyId\":\"" + storyId + "\"\n}");
			ApiClient.Post(mono, RestConfig.BaseUrl + "purchases",
			               "{\n\"userId\":\"" + userId + "\",\n\"storyId\":\"" + storyId + "\"\n}",
					"eyJhbGciOiJIUzUxMiJ9.eyJyb2xlIjoiUk9MRV9VTklUWSJ9.x2W1_eEMnpM0L-i7xppW9S_s5u0t64f6aBYgC_A_ChGNQk-z0eupwohQgIqVaOKndWldt80j5ENsgKc78_Apjw",
					null,
					delegate (UnityWebRequest www)
					{
						if (www.error != null)
						{
							onError(www.error, www.downloadHandler.text);
							return;
						}
						Debug.Log(www.downloadHandler.text);
				        onComplete(JsonUtility.FromJson<DesktopPurchaseResponse>(www.downloadHandler.text));
					});
		}
	}
}