using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using Model;
using RESTClient.Request;

public class login : MonoBehaviour {
	public GameObject panelLoading;

	public GameObject ErrorPanel;
	public Text ErrorMessage;

	string libreria = "Libreria2";

	public void autoLogin(){
		Debug.Log("Starto AutoLog");
		panelLoading.SetActive (true);
		UserRequest.RefreshToken (this,
			delegate(Token token){
				if(token.token == null){
					panelLoading.SetActive(false);
					ErrorMessage.text = "Rieseguire Login";
					ErrorPanel.SetActive(true);
				}
				else{
				    Debug.Log("new token:" + token.token);
					SceneManager.LoadSceneAsync (libreria,LoadSceneMode.Single);
					//SceneManager.LoadScene (libreria,LoadSceneMode.Single);
				}
			},delegate(string code, string error) { 
				var tokenValue = PlayerPrefs.GetString("token");
				if (!tokenValue.Equals("")) {
					SceneManager.LoadScene (libreria,LoadSceneMode.Single);
				}
				else{
				panelLoading.SetActive(false);
				ErrorMessage.text = "Riprovare più tardi";
				ErrorPanel.SetActive(true);
				Debug.Log("errore codice: " + code + " errore: " + error + " errore login"); 
				}
			});
	}


	//Login utente
	public void LoginPressed(string mail,string password){
		if (Application.internetReachability != NetworkReachability.NotReachable)
		{
			panelLoading.SetActive(true);
			//Debug.Log(mail + " - " + password);
			PlayerPrefs.SetString("userMail", mail);
			PlayerPrefs.SetString("ppp", EasyCrypt.encrypt(password));
			UserRequest.Login(this, new UserInput(mail, password, SystemInfo.deviceUniqueIdentifier, SystemInfo.deviceName),
				delegate (Token token)
				{
					Debug.Log("login::LoginPressed - " + token.refreshToken);
					if (token.token == null)
					{
						panelLoading.SetActive(false);
						ErrorMessage.text = "Username o Password errati";
						ErrorPanel.SetActive(true);
					}
					else
					{
						Debug.Log("token:" + token.token);
						SceneManager.LoadScene(libreria, LoadSceneMode.Single);
					}
				}, delegate (string code, string error)
				{
					panelLoading.SetActive(false);
					Debug.Log(code + " - " + error);
					ErrorModule.SendError(error, ErrorMessage, ErrorPanel);
				});
		} else {
			PlayerPrefs.SetInt("OfflineMode", 1);
            Debug.Log("OfflineMode");
            SceneManager.LoadSceneAsync(libreria, LoadSceneMode.Single);
            ErrorMessage.text = "La connessione è assente.\n Attivata la Modalità Offline.";
            ErrorPanel.SetActive(true);
            Debug.Log("Connessione al server fallita,informazoni non reuperate");
		}
	}


	//Registrazione utente
	public void SinginPressed(string mail,string password){
		
		panelLoading.SetActive (true);

		UserRequest.SignupUser (this, new User(null,mail,password,"","",null),
			delegate(User user){
				
				LoginPressed(mail,password);

			},delegate(string code, string error) { 
				panelLoading.SetActive(false);

				if(error.Contains("Email Already Taken")){
					ErrorMessage.text = "Mail già utilizzata.\nControllare le credenziali.\n(Password dimenticata?)";
				} else {
				ErrorModule.SendError(error, ErrorMessage, ErrorPanel);
				}
				ErrorPanel.SetActive(true);
				Debug.Log("errore codice: " + code + " errore: " + error + " errore login"); 
			});
	}

	//Recupera Password utente
	public void ForgotPasswordPressed(string mail){
		panelLoading.SetActive (true);

		UserRequest.RecoveryPassword (this, mail,
			delegate(string s){
				
				panelLoading.SetActive(false);
				ErrorMessage.text = "Controlla la tua casella di posta elettronica \n ti è stata inviata la tua nuova password";
				ErrorPanel.SetActive(true);
				Debug.Log(s);

			},delegate(string code, string error) { 
				panelLoading.SetActive(false);
				ErrorModule.SendError(error, ErrorMessage, ErrorPanel);
				//ErrorMessage.text = "Errore di connesione, riprovare più tardi";
				//ErrorPanel.SetActive(true);
				Debug.Log("errore codice: " + code + " errore: " + error + " errore login");
			});

	}

	void Start(){

		UserRequest.GetUser(this,
            delegate (User user) {
				autoLogin();
			    PlayerPrefs.SetInt("OfflineMode", 0);
				Debug.Log("OnlineMode");
            }, delegate(string code, string error) {
			if (Application.internetReachability == NetworkReachability.NotReachable)
                {
					if (PlayerPrefs.GetInt("logout", 0) == 0)
					{
						PlayerPrefs.SetInt("OfflineMode", 1);
						Debug.Log("OfflineMode");
						SceneManager.LoadSceneAsync(libreria, LoadSceneMode.Single);
						ErrorMessage.text = "La connessione è assente.\n Attivata la Modalità Offline.";
						ErrorPanel.SetActive(true);
						Debug.Log("Connessione al server fallita,informazoni non reuperate");
				} else {
						PlayerPrefs.SetInt("logout", 0);
				}
			} else {
					if (PlayerPrefs.GetInt("logout", 0) == 0)
					{
						if (code.Equals("503"))
						{
							PlayerPrefs.SetInt("OfflineMode", 1);
							Debug.Log("OfflineMode");
							SceneManager.LoadSceneAsync(libreria, LoadSceneMode.Single);
							ErrorMessage.text = "La connessione è assente.\n Attivata la Modalità Offline.";
							ErrorPanel.SetActive(true);
							Debug.Log("Connessione al server fallita,informazoni non reuperate");
						}
				} else {
					PlayerPrefs.SetInt("logout", 0);
				}
			}
				
            });

        /*
		var tokenValue = PlayerPrefs.GetString("token");
		if (!tokenValue.Equals("")) {
			autoLogin ();
		}

		Debug.Log (Application.persistentDataPath);
		*/
	}

}
