using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ParentalControlScript : MonoBehaviour {

	public Text domanda;

	public List<Button> risposte;
	public GameObject panel;

	public RectTransform rectTransf;

	int rispostaCorretta;

	private void Start(){
		if (this.transform.parent.name.Contains("Canvas"))
		{
			if (this.transform.Find("Prompt") != null)
			{
				this.transform.Find("Prompt").GetComponent<Text>().text = "Risolvi l\'operazione";
				Debug.Log(this.transform.Find("Prompt").GetComponent<RectTransform>().anchoredPosition);
				Debug.Log(this.transform.Find("Prompt").GetComponent<RectTransform>().sizeDelta);
			}
			else
			{
				GameObject go = Instantiate(new GameObject(), this.transform);
				go.name = "Prompt";
				go.AddComponent<RectTransform>();
				RectTransform rt = go.GetComponent<RectTransform>();
				//rt.pivot = Vector2.zero;
				rt.anchorMin = new Vector2(0.34f, 0.7f);
				rt.anchorMax = new Vector2(0.62f, 0.77f);
				rt.anchoredPosition = new Vector2(20f, 0.4f);
				rt.sizeDelta = new Vector2(220.0f, 0f);
				go.AddComponent<Text>();
				go.GetComponent<Text>().font = Resources.GetBuiltinResource<Font>("Arial.ttf");
				go.GetComponent<Text>().resizeTextForBestFit = true;
				go.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
				go.GetComponent<Text>().color = Color.black;
				go.GetComponent<Text>().text = "Risolvi l\'operazione";
				Debug.Log(go.GetComponent<RectTransform>().anchoredPosition);
				Debug.Log(go.GetComponent<RectTransform>().sizeDelta);
			}
		}
	}

	public void generaOperazione(){

		int operatore = Random.Range (0 , 2);

		string op;
        

		int numero1 = Random.Range (1, 10);
		int numero2 = Random.Range (1, 10);


		if (operatore == 1) {
			while (numero1 % numero2 != 0) {
				numero2 = Random.Range (1, 10);
			}
		}

		if (operatore == 0) {
			op = " x ";
			rispostaCorretta = numero1 * numero2;
		} else {
			op = " : ";
			rispostaCorretta = numero1 / numero2;
		}


		domanda.text = numero1 + op + numero2;

		int pulsanteCorretto = Random.Range (0 , 4);

		for(int i =0 ; i < risposte.Count;i++){
			int possibileRisposta;

			do {
				
				possibileRisposta = Random.Range (1, 10) * Random.Range (1, 10);
				
			} while(possibileRisposta == rispostaCorretta);

			risposte [i].gameObject.GetComponentInChildren<Text> ().text = possibileRisposta + "";
			
		}

		risposte [pulsanteCorretto].gameObject.GetComponentInChildren<Text> ().text = rispostaCorretta + "";

	}
		
	public void risposta1(int r){
		if (int.Parse (risposte [r].gameObject.GetComponentInChildren<Text> ().text) == rispostaCorretta) {
			SceneManager.LoadScene ("AudioRec", LoadSceneMode.Single);
		} else {
			this.transform.Find("Prompt").GetComponent<Text>().text = "Risposta errata. Riprova.";
			generaOperazione();
		}
	}

	public void risposta2(int r){
		if (int.Parse (risposte [r].gameObject.GetComponentInChildren<Text> ().text) == rispostaCorretta) {
			domanda.text = "Risolvi l\'operazione.";
			this.gameObject.SetActive (false);
			panel.SetActive (true);
		} else {
			this.transform.Find("Prompt").GetComponent<Text>().text = "Risposta errata. Riprova.";
			generaOperazione();
		}
	}

}
