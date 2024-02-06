using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorModule : MonoBehaviour
{
    static string contact = "\nPer info e supporto invia una mail a: support@tadabook.it\n";

    public static void SendError(string code, Text msgText, GameObject panel)
    {
        ErrorJSON error = JsonUtility.FromJson<ErrorJSON>(code);
        ErrorStackTrace stackTrace = JsonUtility.FromJson<ErrorStackTrace>(JsonHelper.GetJsonObject(code, "stackTrace"));
        Debug.Log(code);
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            switch (error.errorCode)
            {
                case "AU02":
					if(!stackTrace.message.Trim().Equals(""))
                        msgText.text = stackTrace.message.Trim() + contact;
					else 
						msgText.text = "Numero massimo di dispositivi associabili a questo account raggiunto." + contact;
                    panel.SetActive(true);
                    break;
                case "AU03":
                    msgText.text = "Errore nel login, nome utente o password errati.";
                    panel.SetActive(true);
                    break;
                case "TR06":
                    msgText.text = "C'è stato un errore nella la creazione della transazione. Se l'acquisto sullo store è andato a buon fine, chiudere e riavviare l'applicazione." + contact;
                    panel.SetActive(true);
                    break;
				case "AU07":
                    msgText.text = "C'è stato un errore con la richiesta di ripristino password, riprova più tardi."+ contact;
                    panel.SetActive(true);
                    break;
                default:
					msgText.text = code + contact;
                    panel.SetActive(true);
                    break;
            }
        }
        else
        {
            msgText.text = "ATTENZIONE!\nNon sei connesso ad Internet.\nAlcune operazioni per essere effettive necessitano di connessione ad Internet.";
            panel.SetActive(true);
        }
    }




    //Classe per deserializzare l'errore.
    [Serializable]
    public class ErrorJSON
    {
        public string errorCode;
        public string stackTrace;

    }
    [Serializable]
    public class ErrorStackTrace
    {
        public string message;
    }
}