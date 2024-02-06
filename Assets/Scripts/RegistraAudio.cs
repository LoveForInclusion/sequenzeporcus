using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistraAudio : MonoBehaviour {

	
	public Image img_rec;
	public GameObject panelRec;
	public Image si;
	public Image no;
	public Button ascolta;
	public static GameObject[] tabs;
	public static float[] timeClip;
	public GameObject ErrorMicrophone;

	static bool flag;
	public static int i;
	float timer;


	void Awake(){

		img_rec = GameObject.Find("ImageRec").GetComponent<Image>();
		tabs = new GameObject[NewGameController.currentCollision.Count];
		timeClip = new float[6];
		i = 0;
		NewGameController.currentCollision.CopyTo(tabs);
		Quicksort(tabs, 0, tabs.Length - 1);
		img_rec.sprite = tabs[0].GetComponent<Image>().sprite;
		NewGameController.ChangeAphaColor(img_rec, 1f);
		flag = false;
	}

	public void ReAwake(){

		//StartCoroutine("ErrorNoMicrophone");

        if (NewGameController.registraFlag)
        {
			ascolta.gameObject.SetActive(false);
            tabs = new GameObject[NewGameController.currentCollision.Count];
            timeClip = new float[6];
            i = 0;
            NewGameController.currentCollision.CopyTo(tabs);
            Quicksort(tabs, 0, tabs.Length - 1);
            img_rec.sprite = tabs[0].GetComponent<Image>().sprite;
            NewGameController.ChangeAphaColor(img_rec, 1f);
            flag = false;
        }
	}

	void RestartReg(){
		i = 0;
		img_rec.sprite = tabs[0].GetComponent<Image>().sprite;
		NewGameController.ChangeAphaColor(img_rec, 1f);

		StartCoroutine("Registra");
		
	}

	void Start () {
	/*
		tabs = new GameObject[NewGameController.currentCollision.Count];
		timeClip = new float[6];
		i = 0;
		NewGameController.currentCollision.CopyTo(tabs);
		Quicksort(tabs, 0, tabs.Length - 1);
		img_rec.sprite = tabs[0].GetComponent<Image>().sprite;
		foreach(GameObject obj in tabs){
			Debug.Log(obj.name);
		}
		NewGameController.ChangeAphaColor(img_rec, 1f);
		flag = false;
		//timer = 0;
	
		//StartCoroutine("Registra");
	*/
	}
	
	void Update () {

		/*
		if (Microphone.IsRecording(Microphone.devices[0])){
			this.GetComponent<Image>().fillAmount -= Time.deltaTime/3 ;
		}
		else {
			this.GetComponent<Image>().fillAmount = 1f;
		}
		*/

		if (flag){
			timer += Time.deltaTime;
		}
		
		

	}

/*
	public IEnumerator Registra00(){

		this.GetComponent<Image>().fillAmount = 1f;

		foreach (GameObject obj in NewGameController.currentCollision){

			yield return new WaitUntil ( () => clickRec); 
			Debug.Log("Entra");
			img_rec.sprite = obj.GetComponent<Image>().sprite;
			AudioClip ac = Microphone.Start("Microfono (Realtek High Definition Audio)", false, 3, 44100);

			yield return ac;

			obj.GetComponent<AudioSource>().clip = ac;
			clickRec = false;
		}
		
		
		panelRec.SetActive(false);
	}
*/

	IEnumerator ErrorNoMicrophone(){
		yield return null;
		Debug.Log(Microphone.devices);
		while(Microphone.devices == null){
			
			ErrorMicrophone.SetActive(true);
			yield return null;
		}

		yield return new WaitWhile( () => ErrorMicrophone.activeSelf);
	}

	IEnumerator Registra(){
		yield return null;
		//this.GetComponent<Button>().enabled = false;
		timer = 0;

		while(Microphone.devices == null){
			
			ErrorMicrophone.SetActive(true);
			yield return null;
		}

		yield return new WaitWhile( () => ErrorMicrophone.activeSelf);

		if ( i < tabs.Length){
			this.GetComponent<Image>().fillAmount = 1f;
			
			AudioClip ac = Microphone.Start(Microphone.devices[0], false, 60, 44100);
			flag = true;		// Start Timer

			yield return new WaitWhile(() => Microphone.IsRecording(Microphone.devices[0]));
			
			//flag = false;		// Stop Timer
			timeClip[i] = timer;
			Debug.Log("timeClip[" + i + "] = " + timeClip[i]);
			//tabs[i].GetComponent<AudioSource>().clip = AudioClip.Create("AudioScena" + i, ac.samples, ac.channels, ac.frequency, false);
			//float[] sample = new float[ac.samples];
			//ac.GetData(sample, 0);
		
			tabs[i].GetComponent<AudioSource>().clip = ac;
			//tabs[i].GetComponent<AudioSource>().clip.SetData(sample, 0);
			
			i ++;

			this.GetComponent<Button>().enabled = true;
			this.GetComponent<Image>().fillAmount = 1f;
			//timer = 0;  		// Reset Timer
			
			if ( i == tabs.Length ){
				img_rec.sprite = null;
				ascolta.gameObject.SetActive(true);
				this.gameObject.SetActive(false);
				GameObject.Find("TextReg").GetComponent<Text>().text = "Premi il pulsante e ascolta la tua voce";
				NewGameController.ChangeAphaColor(panelRec.GetComponent<Image>(), 0f);
				NewGameController.ChangeAphaColor(img_rec, 0f);
			}
			else {
				img_rec.sprite = tabs[i].GetComponent<Image>().sprite;
			}

		}
		

	}

	

	public  void StartReg(Button StopReg){
		this.GetComponent<Button>().enabled = false;
		StopReg.gameObject.SetActive(true);
		
		StartCoroutine("Registra");
	}

	public void StopReg(Button StartReg){
		flag = false;
    	Microphone.End(Microphone.devices[0]);
		StartReg.enabled = true;
		this.gameObject.SetActive(false);
	
	}


	public static void Quicksort(GameObject[] elements, int left, int right)
    {
        int i = left, j = right;
        GameObject pivot = elements[(left + right) / 2];

        while (i <= j) {
            while (elements[i].name.CompareTo(pivot.name) < 0){
                i++;
            }
 
            while (elements[j].name.CompareTo(pivot.name) > 0) {
                j--;
            }
 
            if (i <= j){
            // Swap
              	GameObject tmp = elements[i];
        		elements[i] = elements[j];
            	elements[j] = tmp;
 
                i++;
                j--;
        	}
        }
	}

	
}
