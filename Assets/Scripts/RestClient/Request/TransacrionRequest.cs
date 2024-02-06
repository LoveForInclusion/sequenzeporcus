using System;
using Model;
using NetworkModel;
using UnityEngine;
using UnityEngine.Networking;

namespace RESTClient.Request
{
	public static class TransactionRequest
	{

		public static void GetAllTransactions(MonoBehaviour mono, Action<Transactions> onComplete, Action<string, string> onError)
		{
			ApiClient.Get(mono, RestConfig.BaseUrl+"transactions/me", null,
				delegate(UnityWebRequest www)
				{
					//Debug.Log(www.downloadHandler.text);

					if (www.error != null)
					{
						onError(www.error, www.downloadHandler.text);
						return;
					}
					var jsonArray = "{\"transactions\":" + www.downloadHandler.text + "}";
					onComplete(NetworkTranslator.translateTransactions(JsonUtility.FromJson<NetworkTransactions>(jsonArray)));
				});
		}

		public static void checkTransaction(MonoBehaviour mono,  string storyId, Action<Transaction> onComplete, Action<string, string> onError)
		{
			ApiClient.Get(mono, RestConfig.BaseUrl+"transactions/check" + storyId, null,
				delegate(UnityWebRequest www)
				{
					if (www.error != null)
					{
						onError(www.error, www.downloadHandler.text);
						return;
					}
					onComplete(JsonUtility.FromJson<Transaction>(www.downloadHandler.text));
				});
		}


		public static void createTransaction(MonoBehaviour mono, Transaction transaction, Action<Transaction> onComplete, Action<string, string> onError){
			ApiClient.Post(mono, RestConfig.BaseUrl + "transactions/", 
				JsonUtility.ToJson(transaction), 
				"eyJhbGciOiJIUzUxMiJ9.eyJyb2xlIjoiUk9MRV9VTklUWSJ9.x2W1_eEMnpM0L-i7xppW9S_s5u0t64f6aBYgC_A_ChGNQk-z0eupwohQgIqVaOKndWldt80j5ENsgKc78_Apjw",
				null,
				delegate(UnityWebRequest www)
				{
					if (www.error != null)
					{
						onError(www.error, www.downloadHandler.text);
						return;
					}
				Debug.Log(www.downloadHandler.text);
				onComplete(NetworkTranslator.translateTransaction(JsonUtility.FromJson<NetworkTransaction>(www.downloadHandler.text)));
				});
		}
		
		public static void makeRefund(MonoBehaviour mono, Transaction ltransaction, Action<Transaction> onComplete, Action<string, string> onError){
			Debug.Log(JsonUtility.ToJson(ltransaction));
			NetworkTransaction transaction = NetworkTranslator.translateNetworkTransaction(ltransaction);
			if(transaction._id!="" || transaction._id != null){
				ApiClient.Get(mono, RestConfig.BaseUrl + "transactions/refund/" + transaction._id,
				"eyJhbGciOiJIUzUxMiJ9.eyJyb2xlIjoiUk9MRV9VTklUWSJ9.x2W1_eEMnpM0L-i7xppW9S_s5u0t64f6aBYgC_A_ChGNQk-z0eupwohQgIqVaOKndWldt80j5ENsgKc78_Apjw",
				null,
				delegate(UnityWebRequest www)
				{
					if (www.error != null)
					{
						onError(www.error, www.downloadHandler.text);
						return;
					}
					Debug.Log(www.downloadHandler.text);
					//Substringing
					int value = www.downloadHandler.text.IndexOf("\"value\"");
					int ok = www.downloadHandler.text.IndexOf("\"ok\"");
					string s = (www.downloadHandler.text.Substring(value, ok-value));
					string d = (s.Substring(8, s.Length-9));
					Debug.Log(d);
					//Substringing
					onComplete(NetworkTranslator.translateTransaction(JsonUtility.FromJson<NetworkTransaction>(d)));//Modded for Adapter
				});
			} else {
				Debug.Log("ID vuoto o nullo.");
			}
		}

	}
}
