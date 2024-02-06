using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using RESTClient;
using RESTClient.Request;
using UnityEngine.Networking;
using Model;
using NetworkModel;

public class userProfileOption : MonoBehaviour {

	public InputField name;
	public InputField surname;
	public InputField pass;
	public InputField confPass;
	public InputField mail;
	public GameObject haveDeleted;
	public GameObject panelMessage;
	public Text t;

	float timer;

	void Start(){
		timer = 1.5f;
		mail.text = PlayerPrefs.GetString ("userMail");

		UserRequest.GetUser (this,
			delegate (User user){
				if(name!=null && surname!=null){
				name.text = user.firstName;
				surname.text = user.lastName;
				}
			},delegate(string code, string error) {
				Debug.Log("Connessione al server fallita,informazoni non reuperate:");
				Debug.Log("Code: " + code + "\nError: " + error);
			});

	}

	public void deleteAllFile(){
		if (Directory.Exists (Application.persistentDataPath + "/dir")) {
			DeleteDirectory (Application.persistentDataPath + "/dir");
			//visualizza eliminazione completata
			haveDeleted.SetActive (true);
		} else {
				panelMessage.SetActive (true);
				t.text = "Nulla da eliminare";
			}
	}

	void Update(){

		if (haveDeleted.activeSelf) {
			if (timer <= 0) {
				haveDeleted.SetActive (false);
				timer = 1.5f;
			}
			else
				timer -= Time.deltaTime;
		}
	}

	public void timerToZero(){
		timer = 0f;
		haveDeleted.SetActive (false);
	}

	public static void DeleteDirectory(string target_dir)
	{
			string[] files = Directory.GetFiles (target_dir);
			string[] dirs = Directory.GetDirectories (target_dir);

			foreach (string file in files) {
				File.SetAttributes (file, FileAttributes.Normal);
				File.Delete (file);
			}

			foreach (string dir in dirs) {
				DeleteDirectory (dir);
			}

			Directory.Delete (target_dir, false);
	}

	public void aggiornaProfilo(){

		/*if(name.text.Replace(" ","").Equals("")){
			panelMessage.SetActive (true);
				t.text = "Campo nome non valido.";
		}
		else if(surname.text.Replace(" ","").Equals("")){
			panelMessage.SetActive (true);
				t.text = "Campo cognome non valido.";
		}
		else if(pass.text.Replace(" ","").Equals("")){
			panelMessage.SetActive (true);
				t.text = "Campo password non valido.";
		}
		else if(!pass.text.Equals (confPass.text)){
			panelMessage.SetActive (true);
				t.text = "Campo conferma password non valido.";
		}
		else if(pass.text.Equals (confPass.text)){*/

			UserRequest.GetUser (this,delegate (User user){
				Debug.Log(JsonUtility.ToJson(user));
				//modifica password user
				if ((!pass.text.Replace(" ", "").Equals("")))
			    {   if (pass.text.Equals(confPass.text))
					{
						user.password = pass.text;
        			} else {
				        panelMessage.SetActive(true);
                        t.text = "Campo conferma password non valido.";
						return;
        			}
    			}
			    user.firstName = (!name.text.Replace(" ", "").Equals("")) ? name.text : user.firstName;
			    user.lastName = (!surname.text.Replace(" ", "").Equals("")) ? surname.text : user.lastName;
				Debug.Log(PlayerPrefs.GetString("token"));
				Debug.Log(JsonUtility.ToJson(user));

			UserRequest.UserUpdate(this, formatJSON(),
				
				delegate(User newuser){
					
					Debug.Log("Password cambiata in: " + newuser.password);
					Debug.Log("Nome cambiato in: " + newuser.firstName);
					Debug.Log("Cognome cambiato in: " + newuser.lastName);
					panelMessage.SetActive (true);
					t.text = "Modifiche salvate con successo.";
		

				},delegate(string errore , string errore2){
				    ErrorModule.SendError(errore2, t, panelMessage);
				});
			},delegate(string code, string error) {
			    ErrorModule.SendError(error, t, panelMessage);
			});

		//}


		/*byte[] myData = System.Text.Encoding.UTF8.GetBytes("This is some test data");
		UnityWebRequest www = UnityWebRequest.Put("https://httpbin.org/put", myData);
		yield return www.SendWebRequest();

		if(www.isNetworkError || www.isHttpError) {
			Debug.Log(www.error);
		}
		else {
			Debug.Log(www.downloadHandler.text);
		}*/


		/*ApiClient.Put (this,"https://httpbin.org/put",null,null,delegate(WWW www){
			Debug.Log(www.text);
			
		});*/
	}

	public void openTadaWeb(){
		Application.OpenURL ("http://www.tadabook.it");
	}

	public string formatJSON(){
		int i = 0;
		string s = "{";

		if (!name.text.Replace(" ", "").Equals(""))
		{
			s += "\"firstName\": \"" + name.text + "\"";
			i++;
		}

		if (!surname.text.Replace(" ", "").Equals(""))
		{
			if (i > 0)
				s += ", ";
			s += "\"lastName\": \"" + surname.text + "\"";
			i++;
		}

		//modifica password user
        if ((!pass.text.Replace(" ", "").Equals("")))
        {
			if (i > 0)
                s += ", ";
            if (pass.text.Equals(confPass.text))
            {
				s += "\"password\": \"" + pass.text + "\"";
            }
        }

		s += "}";

		Debug.Log(s);

		return s;

	}
	
}