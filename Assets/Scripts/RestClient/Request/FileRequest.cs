using System;
using System.IO;
using Model;
using RESTClient;
using UnityEngine;
using UnityEngine.Networking;

namespace RESTClient.Request
{
	public static class FileRequest
	{
		//		public static void DownloadFile(MonoBehaviour mono,string bucketname, string fileName, Action<float> onProgress, Action<String> onComplete, Action<string, string> onError)
		//		{
		//			ApiClient.Get(mono, RestConfig.BaseUrl+"api/files/bucket/" + bucketname + "/" + fileName,
		//				delegate (float progress) 
		//				{
		//					onProgress(progress);
		//
		//				},
		//				delegate(UnityWebRequest www)
		//				{
		//					if (www.error != null)
		//					{
		//						Debug.Log(www.error + "-----" + www.downloadHandler.text);
		//						onError(www.error, www.downloadHandler.text);
		//						return;
		//					}
		//					var fileData = www.downloadHandler.data;
		//					File.WriteAllBytes (Application.persistentDataPath + "/" + fileName , fileData);
		//					onComplete(Application.persistentDataPath + "/" + fileName);
		//				});
		//		}


		public static void DownloadFile2(MonoBehaviour mono, string bucketname, string fileName, Action<float> onProgress, Action<String> onComplete, Action<string, string> onError)
		{

			TokenIBMRequest.getToken(mono,
				delegate (TokenIBM token)
				{

					ApiClient.Get(mono, "https://s3.eu-gb.objectstorage.softlayer.net/" + bucketname + "/" + fileName, "bearer " + token.access_token,
							delegate (float progress)
							{
								onProgress(progress);

							},
							delegate (UnityWebRequest www)
							{
								if (www.error != null)
								{
									Debug.Log("bearer " + token.access_token);
									Debug.Log(www.error + "-----" + www.downloadHandler.text);
									onError(www.error, www.downloadHandler.text);
									return;
								}
								Debug.Log(www.downloadHandler.text);
								var fileData = www.downloadHandler.data;
								File.WriteAllBytes(Application.persistentDataPath + "/" + fileName, fileData);
								onComplete(Application.persistentDataPath + "/" + fileName);
							});

				},
				delegate (string s1, string s2)
				{
					Debug.Log("Error " + s1 + " " + s2);
				});

		}

		public static void DownloadFileBundle(MonoBehaviour mono, string bucketname, string fileName, Action<float> onProgress, Action<String> onComplete, Action<string, string> onError)
		{

			TokenIBMRequest.getToken(mono,
				delegate (TokenIBM token)
				{
					Debug.Log(fileName);
					ApiClient.Get(mono, "https://s3.eu-gb.objectstorage.softlayer.net/" + bucketname + "/" + fileName, "bearer " + token.access_token, Application.persistentDataPath + "/" + fileName,
							delegate (float progress)
							{
								onProgress(progress);

							},
							delegate (UnityWebRequest www)
							{
								if (www.error != null)
								{
									Debug.Log(www.error + "-----" + www.downloadHandler.text);
									onError(www.error, www.downloadHandler.text);
									return;
								}
								//Mod file buffer
								//var fileData = www.downloadHandler.data;
								//File.WriteAllBytes (Application.persistentDataPath + "/" + fileName , fileData);
								onComplete(Application.persistentDataPath + "/" + fileName);
							});

				},
				delegate (string s1, string s2)
				{
					Debug.Log("Error " + s1 + " " + s2);
				});

		}

		public static void DownloadFile3(MonoBehaviour mono, string bucketname, string fileName, Action<float> onProgress, Action<String> onComplete, Action<string, string> onError)
		{

			ApiClient.Post(mono, RestConfig.BaseUrl + "files", "{\"storyId\":\"" + bucketname.Substring(5) + "\", \"fileName\":\"" + fileName + "\"}",
			   delegate (float f)
			   {

			   }, delegate (UnityWebRequest www)
			   {
				   //Debug.Log(JsonUtility.FromJson<downloadUrlJson>(www.downloadHandler.text).downloadUrl);
				   try
				   {
					   ApiClient.NewGetFile(mono, JsonUtility.FromJson<downloadUrlJson>(www.downloadHandler.text).downloadUrl, Application.persistentDataPath + "/" + fileName,
							  delegate (float progress)
							  {
								  onProgress(progress);
							  },
							  delegate (UnityWebRequest www2)
							  {
								  if (www2.error != null)
								  {
							   //Debug.Log("C'è stato un errore.");
							   //Debug.Log(www2.error + "-----" + www2.downloadHandler.text);
									  onError(www2.error, www2.downloadHandler.text);
									  return;
								  }
						   //Mod file buffer
						   //var fileData = www.downloadHandler.data;
						   //File.WriteAllBytes (Application.persistentDataPath + "/" + fileName , fileData);
						   onComplete(Application.persistentDataPath + "/" + fileName);
							  });
				} catch(Exception e){
					ErrorModule.SendError("{}", ((TadaSuperSystem)mono).testoErrore, ((TadaSuperSystem)mono).panelError);//Sostituire
				}
			   });

		}

	}

	[Serializable] //Because FUCK MY LIFE!
    public class downloadUrlJson
    {
        public string downloadUrl;
    }

}
