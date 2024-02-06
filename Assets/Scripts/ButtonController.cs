using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour /* IPointerDownHandler, IPointerUpHandler */ {

	public AudioSource audioTada;
	public GameObject panelMod;		// Panel Modalità
	public GameObject panelDiff;	// Panel Difficoltà
	public GameObject panelSeq;   	// Panel Sequenze
	public GameObject Game;			// Panel Gioco
	public GameObject panelReg;		//Panel Registra
	public GameObject panelAscolta;
	private GameObject currentPanel;
	public Image si;
	public Image no;

	public GameObject contentDifficile;
	public GameObject contentFacile;

	public GameObject contentElaborata;
	public ScrollRect scroll;
	public Image img_rec;

	public GameObject nextImageButton;
	public GameObject ErrorMicrophone; // pannello di errore in caso non rileva alcun microfono
	private bool flag;

	public Text subTitle; //sottotilo pannello sequenze

	
	void Start () {

		IEnumerator playTada = PlayAudio(audioTada);
		StartCoroutine(playTada);

		GeneraPath();
		panelMod.SetActive(true);
		panelDiff.SetActive(false);
		panelSeq.SetActive(false);
		currentPanel = panelMod;
		
		flag = false;
		
	}

	private IEnumerator PlayAudio(AudioSource audio){

		audio.Play();
		yield return new WaitUntil(() => !audio.isPlaying);
		DestroyImmediate(audio.gameObject);
	}
	
	
	void Update () {
		
	}

/* *****************
	public void OnPointerDown(PointerEventData eventData){
		this.GetComponent<ScrollRect>().enabled = false;
	}

	public void  OnPointerUp(PointerEventData eventData){
		this.GetComponent<ScrollRect>().enabled = true;
	}
***************** */

	//*** Change Panel */
	public void ClickButton(Button button){

		if (button.name == "Semplice"){
			Option.modalita = "semplice";
			panelDiff.SetActive(true);
			panelMod.SetActive(false);
			currentPanel = panelDiff;
		}
		if (button.name == "Elaborata"){
			Option.modalita = "elaborata";
			panelDiff.SetActive(true);
			panelMod.SetActive(false);
			currentPanel = panelDiff;
		}
		if (button.name == "Facile" || button.name.Equals("Difficile")){
			Option.difficolta = button.name.ToLower();
			panelDiff.SetActive(false);
			subTitle.text = "Storia " + Option.modalita.ToCharArray()[0].ToString().ToUpper() + Option.modalita.Substring(1) + " - Livello " + Option.difficolta.ToCharArray()[0].ToString().ToUpper() + Option.difficolta.Substring(1);
			ActiveContent();
			panelSeq.SetActive(true);
			currentPanel = panelSeq;
		}
	
	}


	private void ActiveContent(){
		contentDifficile.SetActive(false);
		contentElaborata.SetActive(false);
		contentFacile.SetActive(false);



		if(Option.modalita.Equals("elaborata")){
				scroll.content = contentElaborata.GetComponent<RectTransform>();
				contentElaborata.SetActive(true);
		}
		else
		{
			if (Option.difficolta.Equals("difficile")){
				scroll.content = contentDifficile.GetComponent<RectTransform>();
				contentDifficile.SetActive(true);
			}
			else{
				scroll.content = contentFacile.GetComponent<RectTransform>();
				contentFacile.SetActive(true);
			}
		}
	}


	//*Start the Game */
	public void Sequenze(Button seq){
		Option.sequenza = seq.gameObject.name;
		Game.SetActive(true);
		currentPanel = Game;
		panelSeq.SetActive(false);
		GameObject.Find("AudioButton").GetComponent<AudioSource>().Stop();
		Game.GetComponent<NewGameController>().RestartGame();

	}

	
	public void Indietro(GameObject precPanel){
		if(currentPanel.name.Equals("Game")){
			GameObject.Find("AudioButton").GetComponent<AudioSource>().Play();
			NewGameController.restarFlag = true;
			Close(panelReg);
		}
		precPanel.SetActive(true);
		currentPanel.SetActive(false);
		currentPanel = precPanel;
	}

	public void Nein(){
		//GetComponent<AudioSource>().Play();
		Close(panelReg);
		Close(Game);
		panelSeq.SetActive(true);
		currentPanel = panelSeq;
	}

	

	/*Handles the Soundtrack 
	public void Soundtrack(Button button){
		Animator anim = button.GetComponent<Animator>();

		if (!anim.GetBool("Off")){
			anim.SetBool("Off", true);
			GetComponent<AudioSource>().volume = 0f;
		}
		else {
			anim.SetBool("Off", false);
			GetComponent<AudioSource>().volume = 1f;
		}
	}
	*/

	public void Close(GameObject pannello){
		pannello.SetActive(false);
	}

	public void LoadScene(string scena){
		SceneManager.LoadScene(scena);
	}

	public void Reg(Button b){

		StartCoroutine(ErrorNoMicrophone());
		Debug.Log("!!!!!!!!!!");
		GameObject.Find("TextReg").GetComponent<Text>().text = "Premi il pulsante per registrare la tua voce";
		GameObject.Find("TextReg").GetComponent<Text>().text = "Premi il pulsante per registrare la tua voce";
		NewGameController.ChangeAphaColor(panelReg.GetComponent<Image>(), 1.0f);
		
		if(flag)
			NewGameController.ChangeAphaColor(img_rec, 1f);
		
		b.gameObject.SetActive(true);
		no.gameObject.SetActive(false);
		img_rec.gameObject.SetActive(true);
		img_rec.sprite = RegistraAudio.tabs[0].GetComponent<Image>().sprite;
		si.gameObject.SetActive(false);
		flag = false;
	}

	IEnumerator ErrorNoMicrophone(){
		yield return null;
		Debug.Log(Microphone.devices);
		
		while(Microphone.devices.Length == 0){
			
			ErrorMicrophone.SetActive(true);
			yield return null;
		}

		yield return new WaitWhile( () => ErrorMicrophone.activeSelf);
	}

	public void Ascolta(Image img){

		panelReg.SetActive(false);
		panelAscolta.SetActive(true);
		IEnumerator ie = AscoltaAudio(img);
		StartCoroutine(ie);


	}

	IEnumerator AscoltaAudio(Image img){
		int i = 0;
		foreach(GameObject scene in RegistraAudio.tabs){

			img.sprite = scene.GetComponent<Image>().sprite;
			scene.GetComponent<AudioSource>().Play();

			yield return new WaitForSeconds(RegistraAudio.timeClip[i]);
			//yield return new WaitWhile( ()=> scene.GetComponent<AudioSource>().isPlaying);
			//yield return new WaitForSeconds(scene.GetComponent<AudioSource>().clip.samples * scene.GetComponent<AudioSource>().clip.frequency );
			scene.GetComponent<AudioSource>().Stop();
			nextImageButton.GetComponent<Button>().interactable = true;
			yield return new WaitUntil( () => !nextImageButton.GetComponent<Button>().interactable);
			i++;

		}

		panelAscolta.SetActive(false);
		
		panelReg.SetActive(true);
		NewGameController.ChangeAphaColor(panelReg.GetComponent<Image>(), 0f);
		GameObject.Find("TextReg").GetComponent<Text>().text =  "Vuoi registrare un'altra volta?";
		GameObject.Find("Ascolta").SetActive(false);
		si.gameObject.SetActive(true);
		no.gameObject.SetActive(true);
		RegistraAudio.i = 0;
		flag = true;
		
		
	}


	void GeneraPath(){

		//Option.Path = Application.persistentDataPath + "/dir/" + FileHandler.caricaIdStoria() + "/" + CheckSystem();
		Option.Path = "C:/Users/developer4/Desktop/tada-Bundle/SortSequence/Active/" + CheckSystem();

	}

	string CheckSystem(){

		string so = SystemInfo.operatingSystem;

		if(so.Contains("Android")){
			return "Android";
		}
		else if (so.Contains("Windows")){
			return "Windows";
		}
		else if (so.Contains("iOS")){
			return "Ios";
		}
		else if (so.Contains("Mac")){
			return "Ios";
		}

		return null;


	}


}
