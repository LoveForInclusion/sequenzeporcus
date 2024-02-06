using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NewGameController : MonoBehaviour {

	AssetBundle assetBundle;
	AssetBundleManifest manifest;
	static GameObject battery;
	static GameObject errata;
	static GameObject finali;
	public static Image[] batteryImage;
	public static SpriteRenderer[] errataImage;
	public static SpriteRenderer[] finalImage;
	public static Image imageZoom;
	public static List<GameObject> currentCollision;
	public static Image[] scelte;
	public static Image[] caselle;
	public GameObject caricamento;

	public Text textReg;

	public Sprite casellaBianca;
	static int numCasella;
	static int numScelte;
	public static int finish;
	public GameObject panelRec;
	public static bool complete;

	public GameObject canvas;

	private GameObject buttonIndietro;

	public static bool restarFlag;

	public static bool registraFlag;

	public Image img_rec;

	public ArrayList lastRandom;

	void Start () {

		lastRandom = new ArrayList();
		restarFlag = false;
		registraFlag = false;
		buttonIndietro = GameObject.Find("Indietro3");
		
		caricamento.SetActive(true); //attiva pannello di caricamento
		complete = false;

		currentCollision = new List<GameObject>();
		imageZoom = GameObject.Find("ImageZoom").GetComponent<Image>();
		imageZoom.gameObject.SetActive(false);
		
		scelte = GameObject.Find("Panel Scelte").GetComponentsInChildren<Image>();
		Debug.Log("Start Scelte: " + scelte.Length);
		caselle = GameObject.Find("Panel Caselle").GetComponentsInChildren<Image>();
		Debug.Log("Start Caselle: " + caselle.Length);
		//Quicksort(scelte, 1, scelte.Length - 1);
		//Quicksort(caselle, 1, caselle.Length - 1);

		AttivaCaselle();
		
		IEnumerator ie = LoadCaselle(Option.sequenza);
		StartCoroutine(ie);
		
		
	}


	void Update () {
		
		if(complete){
			StartCoroutine("Rec");
		}

	}

    
	public void RestartGame(){
        if (restarFlag)
        {
			registraFlag = true;
			buttonIndietro.SetActive(true);
            currentCollision = new List<GameObject>();
            caricamento.SetActive(true); //attiva pannello di caricamento
            //restarFlag = false;
            complete = false;
            Destroy(battery);
            Destroy(errata);
            Destroy(finali);
            GameObject.Find("Panel Scelte").SetActive(true);

            Debug.Log("Restart Scelte: " + scelte.Length);
            Debug.Log("Restart Caselle: " + caselle.Length);
           // Quicksort(scelte, 1, scelte.Length - 1);
           // Quicksort(caselle, 1, caselle.Length - 1);
            AttivaCaselle();


            IEnumerator ie = LoadCaselle(Option.sequenza);
            StartCoroutine(ie);
        }
	}


	public void Quicksort(Image[] elements, int left, int right)
    {
        int i = left, j = right;
        Image pivot = elements[(left + right) / 2];
 
        while (i <= j) {
            while (elements[i].gameObject.name.CompareTo(pivot.gameObject.name) < 0){
                i++;
            }
 
            while (elements[j].gameObject.name.CompareTo(pivot.gameObject.name) > 0) {
                j--;
            }
 
            if (i <= j){
            // Swap
              	Image tmp = elements[i];
        		elements[i] = elements[j];
            	elements[j] = tmp;
 
                i++;
                j--;
        	}
        }
	}
    



	

	//******* dipendenze */
	/***************************************
	public bool CheckDependencies(string s){
		IEnumerable b = AssetBundle.GetAllLoadedAssetBundles();
		foreach(AssetBundle a in b){
			if(a.name.Equals(s))
				return true;
		}

		return false;
	}

	
	public void LoadDependencies(string batteria){
		assetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/StandaloneWindows/" + batteria);
		manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
	
		string[] dependencies = manifest.GetAllDependencies(batteria);
	
		foreach (string depe in dependencies){
			if(!CheckDependencies(depe)){
				AssetBundle bundleSharedLoadRequest = AssetBundle.LoadFromFile("Assets/AssetsBundle/StandaloneWindows/resources" );
				bundleSharedLoadRequest.LoadAllAssets<GameObject>();
				bundleSharedLoadRequest = null;
			}
		}
	}
	*******************************************/




// Inizializza le immagini da scegliere all'avvio del gioco
	public void ScelteStart(){
		

		ArrayList arr = new ArrayList();

		for ( int i=1; i < numScelte +1; i++){
			
			int k = -1;
			do{
				k = Random.Range(0, numScelte);
			} while ( ControllaArray(arr, k) );
			arr.Add(k);
			GameObject.Find("Scelta" + i).GetComponent<Image>().sprite = batteryImage[k].sprite;
			ChangeAphaColor(GameObject.Find("Scelta" + i).GetComponent<Image>(), 1.0f);
	
		}

		Debug.Log("Restart Flag: " + restarFlag);
		if(restarFlag && arr.Count.Equals(lastRandom.Count)){
            if (ComparaArray(arr, lastRandom)){
                ScelteStart();
			}
            else{
                lastRandom = arr;
			}
		}
		else
			lastRandom = arr;
	}


	//In "Modalità: Elaborata; Difficoltà: Difficile" gestisce la scelta dell'ultima scena tra una giusta e l'altra errata 
	public static void NextScelta(int nextScena){

		if (Option.modalita == "elaborata"){
			NextCasella();

			caselle[numCasella + 1].gameObject.SetActive(true);
			
			int h = Random.Range(1, 4);

			ArrayList ar = new ArrayList();
			int k = -1;
			

			for (int i=1; i < 4; i++){
				Image tempImg = GameObject.Find("Scelta" + i).GetComponent<Image>();
				

				if(i==h){
					tempImg.sprite = batteryImage[nextScena].sprite;
				}
				else{
				
					do{
						k = Random.Range(0, errataImage.Length);
					} while ( ControllaArray(ar, k) );
					ar.Add(k);

					tempImg.GetComponent<Image>().sprite = errataImage[k].sprite;
				}
				ChangeAphaColor(tempImg, 1);
			}

			GameObject.Find("Scelta" + nextScena).SetActive(false);
		}
	}

	//In "Modalità: Elaborata; Difficoltà: Facile" gestisce il finale a scelta
	public static void ScegliFinale(){

		caselle[numCasella + 1].gameObject.SetActive(true);

		
		for (int i = 3; i < scelte.Length; i++){
			scelte[i].gameObject.SetActive(false);
		}

		int k = Random.Range(0, finalImage.Length);

		scelte[1].sprite = batteryImage[batteryImage.Length - 1].sprite;
		ChangeAphaColor(scelte[1], 1);
		scelte[2].sprite = finalImage[k].sprite;
		ChangeAphaColor(scelte[2], 1);
		scelte[2].tag = "FinaleAlternativo";
	}

	public void CaricaBundle(){
		IEnumerator ie = LoadCaselle(Option.sequenza);
		StartCoroutine(ie);
	}



	IEnumerator LoadCaselle(string sequenza){

		string seq = "";
		if(Option.modalita.Equals("elaborata"))
			seq = sequenza + "/base";
		else
			seq = Option.difficolta + "/" + sequenza;
		
		AssetBundleRequest assetLoadRequest = null;
		AssetBundleCreateRequest bundleLoadRequest = null;
		Debug.Log(Option.Path + "/sortsequence/" + Option.modalita + "/" + seq);

		bundleLoadRequest = AssetBundle.LoadFromFileAsync(Option.Path + "/sortsequence/" + Option.modalita + "/" + seq);
		yield return bundleLoadRequest;
       

		var myLoadedAssetBundle = bundleLoadRequest.assetBundle;

		if (myLoadedAssetBundle == null){
			Debug.Log("Failed to load AssetBundle!");
			yield break;
		}		
		
		assetLoadRequest = myLoadedAssetBundle.LoadAssetAsync(sequenza + "_base");
		yield return assetLoadRequest;

		battery = assetLoadRequest.asset as GameObject;
		
		battery = Instantiate(battery, new Vector3(0,0,0), new Quaternion());
		battery.SetActive(true);

		batteryImage = battery.GetComponentsInChildren<Image>();
		Quicksort(batteryImage, 0, batteryImage.Length - 1);

		ScelteStart();

		myLoadedAssetBundle.Unload(false);
		assetLoadRequest = null;
		bundleLoadRequest = null;

		caricamento.SetActive(false);

		Caching.ClearCache();	

		yield break;

	
	}

	IEnumerator LoadErrata(string batteria){
		
		AssetBundleRequest assetLoadRequest = null;
		AssetBundleCreateRequest bundleLoadRequest = null;

		bundleLoadRequest = AssetBundle.LoadFromFileAsync(Option.Path + "/sortsequence/" + Option.modalita + "/" + batteria + "/noise");
		yield return bundleLoadRequest;

		var myLoadedAssetBundle = bundleLoadRequest.assetBundle;

		if (myLoadedAssetBundle == null){
			Debug.Log("Failed to load AssetBundle!");
			yield break;
		}		
		
		assetLoadRequest = myLoadedAssetBundle.LoadAssetAsync(batteria + "_noise");
		yield return assetLoadRequest;

		errata = assetLoadRequest.asset as GameObject;
		
		errata = Instantiate(errata, new Vector3(0,0,0), new Quaternion());
		errata.SetActive(true);

		errataImage = errata.GetComponentsInChildren<SpriteRenderer>();
		//Quicksort(errataImage, 0, errataImage.Length - 1);
		
		myLoadedAssetBundle.Unload(false);
		assetLoadRequest = null;
		bundleLoadRequest = null;

		Caching.ClearCache();	

		yield break;
	}

	IEnumerator LoadFinale(string batteria){

		AssetBundleRequest assetLoadRequest = null;
		AssetBundleCreateRequest bundleLoadRequest = null;

		bundleLoadRequest = AssetBundle.LoadFromFileAsync(Option.Path + "/" + batteria + "/final");
		yield return bundleLoadRequest;

		var myLoadedAssetBundle = bundleLoadRequest.assetBundle;

		if ( myLoadedAssetBundle == null){
			Debug.Log("Failed to load AssetBundle!");
			yield break;
		}

		assetLoadRequest = myLoadedAssetBundle.LoadAssetAsync(batteria + "_final");
		yield return assetLoadRequest;

		finali = assetLoadRequest.asset as GameObject;
		finali = Instantiate(finali, new Vector3(0,0,0), new Quaternion());

		finalImage = finali.GetComponentsInChildren<SpriteRenderer>();

		myLoadedAssetBundle.Unload(false);
		assetLoadRequest = null;
		bundleLoadRequest = null;
		Caching.ClearCache();

		yield break;
	}

	// Controlla se un elemento è presente nell'array
	public static bool ControllaArray(ArrayList a, int k){

		foreach ( int element in a){
			if( element == k)
				return true;
		}

		return false;
	}

	private bool ComparaArray(ArrayList arr1, ArrayList arr2){

		for(int i =0; i < arr1.Count; i++){
			if(!arr1.ToArray()[i].Equals(arr2.ToArray()[i]))
				return false;
		}
		return true;
	}

/*
	void OnTriggerEnter2D(Collider2D collider){
		if(collider.gameObject.tag == "Casella"){
			currentCollision.Add(collider.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D collider){
		if(collider.gameObject.tag == "Casella"){
			currentCollision.Remove(collider.gameObject);
			//collider.enabled = false;
		}
	}

	*/

	public static void NextCasella(){
		if (numCasella < finish){
			currentCollision.Add(caselle[finish].gameObject);
		}
	}


	public static void ChangeAphaColor(Image img, float f){
		Color tempColor = img.color;
		tempColor.a = f;
		img.color = tempColor;
	}


    //Decide il numero di caselle da mostrare
	public void AttivaCaselle(){

		for(int i = 1; i < caselle.Length; i++){
			caselle[i].gameObject.SetActive(true);
			scelte[i].gameObject.SetActive(true);
			scelte[i].tag = "Scelta";
			ChangeAphaColor(scelte[i], 1.0f);
		}

		if (Option.modalita == "semplice"){

			if (Option.difficolta == "facile"){
				numCasella = 2;
				numScelte = 2;
				finish = 2;
				caselle[3].gameObject.SetActive(false);
				caselle[4].gameObject.SetActive(false);
				caselle[5].gameObject.SetActive(false);

				scelte[3].gameObject.SetActive(false);
				scelte[4].gameObject.SetActive(false);
				scelte[5].gameObject.SetActive(false);
			}

			if (Option.difficolta == "difficile"){
				numCasella = 3;
				numScelte = 3;
				finish = 3;
				caselle[4].gameObject.SetActive(false);
				caselle[5].gameObject.SetActive(false);

				scelte[4].gameObject.SetActive(false);
				scelte[5].gameObject.SetActive(false);
			}
		}

		if (Option.modalita == "elaborata"){
			
			if (Option.difficolta == "facile"){

				//IEnumerator final = LoadFinale(Option.sequenza);
				//StartCoroutine(final);
				
				finish = 4;
				numCasella = 4;
				numScelte = 4;
				caselle[5].gameObject.SetActive(false);
				scelte[5].gameObject.SetActive(false);
			}

			if (Option.difficolta == "difficile"){

				finish = 5;

				IEnumerator err = LoadErrata(Option.sequenza);
				StartCoroutine(err);

				numCasella = 4;
				numScelte = 4;
				caselle[5].gameObject.SetActive(false);
				scelte[5].gameObject.SetActive(false);
			}
		}

		RipristinaCaselle();
		for(int i = 1; i <= numCasella; i++){
			currentCollision.Add(caselle[i].gameObject);
		}
	}

	
    
	public void RipristinaCaselle(){

		for (int i = 1; i < caselle.Length; i++)
		{
			caselle[i].sprite = casellaBianca;
			caselle[i].GetComponentInChildren<Text>().text = i.ToString();

		}
	}

	
	IEnumerator  Rec(){
		complete = false;
		yield return new WaitForSeconds(1.6f);
		if (registraFlag){
			ChangeAphaColor(img_rec, 0.0f);
			ChangeAphaColor(panelRec.GetComponent<Image>(), 0.0f);
		}
		textReg.text = "Vuoi registrare la tua voce per ogni scena?";		
		panelRec.SetActive(true);
		yield break;
	}

}
