using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;

public class RegistaStoria : MonoBehaviour
{

    //Regista Storia 2.0

    private GameObject nextScenePreview; //modifiche

    private GameObject prevScenePreview;

    public string difficolta = "Facile";
    public string difficulty = "Easy";

    public AudioSource player;

    public int currentScene;
    Vector3 nextTarget;

    public float distance = 0f;

    public bool canTouch;

    public bool countdownEnable = false;
    public float countdown;

    int figuraCorrente;
    public int figureCount;

    public Transform scena;

    public OpzioniAudio opzioni;

    public GameObject panel;

    public string idStoria;

    private static bool createdBook = false;

    bool refresh = false;
    bool loaded = false;

    public bool webGL;
    int version = 0;
    float durationClip;
    int ultimaScena;

    bool cha = false;
    int lastFig = 0;
    Transform lastToken = null;

    private string pathName = "";

    private AssetBundle bundleScreens; //mantengo il bundle degli screens caricato
    private string suffissoScreenName;

    private bool flagScreens;
    //Sprite[] spriteScreen = new Sprite[2];

    //Controllo risoluzione
    /*
    #if UNITY_EDITOR
        static float width = Display.main.renderingWidth;
        static float height = Display.main.renderingHeight;
    #else
        static float width = Display.main.systemWidth;
        static float height = Display.main.systemHeight;
    #endif
    float rapporto = width / height; 
    */
    float rapporto;


    private string aspectRatio;
    private int scenaPrecedente;





    AssetBundle assetBundle;
    AssetBundleManifest manifest;

	bool flag = false;

    
    private void Preview(){ //Modifiche

        /**** Preview Scena sucessiva */
        nextScenePreview = new GameObject();
        nextScenePreview.AddComponent<Image>();
        nextScenePreview = Instantiate(nextScenePreview, this.gameObject.GetComponent<RectTransform>());
        
        nextScenePreview.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
        nextScenePreview.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
        nextScenePreview.GetComponent<RectTransform>().pivot = new Vector2(0f, 0f);
        nextScenePreview.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
        nextScenePreview.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);

        nextScenePreview.GetComponent<RectTransform>().SetAsLastSibling();
        nextScenePreview.name = "nextscenePreview";
        /************ */

        /***** Preview scena precedente */
        prevScenePreview = new GameObject();
        prevScenePreview.AddComponent<Image>();
        prevScenePreview = Instantiate(nextScenePreview, this.gameObject.GetComponent<RectTransform>());
        
        prevScenePreview.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
        prevScenePreview.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
        prevScenePreview.GetComponent<RectTransform>().pivot = new Vector2(0f, 0f);
        prevScenePreview.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
        prevScenePreview.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);

        nextScenePreview.GetComponent<RectTransform>().SetAsLastSibling();
        prevScenePreview.name = "prevscenePreview";
        /******** */

        prevScenePreview.SetActive(false);
        nextScenePreview.SetActive(false);

    }

    void Awake()
    {
       
        if (!createdBook)
        {
            DontDestroyOnLoad(this.gameObject);
            createdBook = true;
        }
        else
            Destroy(this.gameObject);


    }

    public void caricaIdStoria()
    {
        //carico la directory dei salvataggi
        string destinationUtente = Application.persistentDataPath + "/myIdStoria.dat";
        FileStream fileUtente;

        //se i file esistono li apre
        if (File.Exists(destinationUtente))
            fileUtente = File.OpenRead(destinationUtente);
        else
        {
            idStoria = null;
            return;
        }

        //leggo i file locali
        BinaryFormatter bf = new BinaryFormatter();

        idStoria = (string)bf.Deserialize(fileUtente);

        fileUtente.Close();
    }


    //Carica Bundle Screens

    IEnumerator loadBundleScreens(){

            /**** Carico Bundle degli screens */
        AssetBundleRequest assetLoadRequest = null;
        AssetBundleCreateRequest bundleLoadRequest = null;

        bundleLoadRequest = AssetBundle.LoadFromFileAsync(Application.persistentDataPath + "/dir/" + idStoria + "/" + pathName + "/" + difficulty.ToLower() + "/screens_" + difficulty.ToLower() + aspectRatio);
        yield return bundleLoadRequest;

        bundleScreens = bundleLoadRequest.assetBundle;

        if (bundleScreens == null){
            Debug.Log("Failed to Load AssetBundle.");
        }

        Debug.Log("BundleScreens: " + bundleScreens.GetAllAssetNames()[0]);
        /************ */

        assetLoadRequest = bundleScreens.LoadAssetAsync<Sprite>("Scena1" + suffissoScreenName + ".png");

        nextScenePreview.GetComponent<Image>().sprite = assetLoadRequest.asset as Sprite;

        yield break;
    }


    //Carica screen
    IEnumerator changeScreen(int numScena){

        //if(bundleScreens != null){

            AssetBundleRequest assetLoadRequest = null;
            assetLoadRequest = bundleScreens.LoadAssetAsync<Sprite>("Scena" + numScena + suffissoScreenName + ".png");

            nextScenePreview.GetComponent<Image>().sprite = assetLoadRequest.asset as Sprite;

            if(currentScene > 0){
                assetLoadRequest = bundleScreens.LoadAssetAsync<Sprite>("Scena" + (numScena-2) + suffissoScreenName + ".png");
                prevScenePreview.GetComponent<Image>().sprite = assetLoadRequest.asset as Sprite;
            }
            flagScreens = true;
            yield break;
       // }

       // yield break;
    }


    IEnumerator loadObject(List<int> numeroScena)
    {

        //Debug.Log("Scene da scaricare:" + numeroScena.Count);

        foreach (int numero in numeroScena)

            if (webGL)
            {
                //Debug.Log("File in corso" + numero);
                //Debug.Log("Cerco di caricare la scena " + numero);

                yield return new WaitForSeconds(.5f);

                if (Caching.ClearCache())
                    Debug.Log("Cache Svuotata");
                else
                    Debug.Log("Cache in uso");

                yield return new WaitForSeconds(.5f);

                //string u = Application.absoluteURL;
                string u = "http://testtadawebgl2.altervista.org/index.html";
                //Debug.Log("Prima del remove:" + u);


                string sub = "Easy";

                opzioni.caricaOpzioniAudio();

                if (opzioni.option.tipoLettura == 1)
                {
                    difficolta = "Difficile";
                    difficulty = "Advanced";
                    sub = "WebGL/Advanced/";
                    suffissoScreenName = "E";
                }
                else
                {
                    difficolta = "Facile";
                    difficulty = "Easy";
                    sub = "WebGL/Easy/";
                    suffissoScreenName = "S";
                }


                //Debug.Log(sub);
                u = u.Replace("index.html", sub);
                //Debug.Log("Dopo il replace:" + u);
                //Debug.Log("cerco scena:" + u + "scena" + numero);
                WWW www = WWW.LoadFromCacheOrDownload(u + "scena" + numero, version);
                yield return www;
                //Debug.Log("caricamento eseguito");

                AssetBundle bundle = www.assetBundle;
                if (bundle == null)
                    Debug.Log("sono null");

                AssetBundleRequest assetLoadRequest = bundle.LoadAssetAsync<GameObject>("scena" + numero);
                yield return assetLoadRequest;
                //Debug.Log("carico file \"scena\"" + numero);

                GameObject cube = assetLoadRequest.asset as GameObject;

                cube.GetComponentInChildren<ScenaController>().reg = transform.GetComponent<RegistaStoria>();
                cube.GetComponentInChildren<ScenaController>().numScena = numero;
                cube.name = numero + "";
                if (numero != currentScene)
                    cube.SetActive(false);
                Instantiate(cube, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation, this.transform);



                Instantiate(cube);
                //Debug.Log("instanzio");

                //Pulisco dalla cache il bundle scaricato.
                www.Dispose();
                bundle.Unload(false);

                version++;

                contaFigure();
            }
    }


public bool checkDependencies(string s){
		IEnumerable b = AssetBundle.GetAllLoadedAssetBundles();
		foreach(AssetBundle a in b){
			if(a.name.Equals(s))
			return true;
		}
		return false;
	}

    public void loadBundleShared(int numeroScena)
    {
		
		//Controlla ambiente
        string pathName = "";
        if (SystemInfo.operatingSystem.Contains("iOS"))
        {
            pathName = "Ios";
        }
        else if (SystemInfo.operatingSystem.Contains("Android"))
        {
            pathName = "Android";
        }
        else if (SystemInfo.operatingSystem.Contains("Mac"))
        {
            pathName = "Ios";
        }
        else if (SystemInfo.operatingSystem.Contains("Windows"))
        {
            pathName = "Windows";
        }
        else
        {
            pathName = "Linux";
        }
		//pathName = "Android";//Sostituire
        
        //Fine Controlla Ambiente

        if (File.Exists(Application.persistentDataPath + "/dir/" + idStoria + "/" + pathName + "/" + difficulty + "/scena" + numeroScena))
        {
			////Debug.Log("File trovato." + Application.persistentDataPath + "/dir/" + idStoria + "/" + pathName + "/" + difficulty + "/scena" + numeroScena);
            //carico i file che mi occorrono per il bundle principale
            string[] dependencies = manifest.GetAllDependencies((difficulty + "/scena" + numeroScena).ToLower());
            foreach (string depe in dependencies)
            {
				if(!checkDependencies(depe)){
                    AssetBundle bundlesharedLoadRequest = AssetBundle.LoadFromFile(Application.persistentDataPath + "/dir/" + idStoria + "/" + pathName + "/" + depe);
					bundlesharedLoadRequest.LoadAllAssets<GameObject>();
                    Debug.Log("!!!!!! " + depe);
                    if(depe.Equals(difficulty.ToLower() + "/screens_" + difficulty.ToLower() + aspectRatio )){
                        bundleScreens = bundlesharedLoadRequest;
                    }
                    else{
                        bundlesharedLoadRequest = null;
                    }

                    
				}
            }



/*
            AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromFile(Application.persistentDataPath + "/dir/" + idStoria + "/" + pathName + "/" + difficulty + "/scena" + numeroScena);
            if (myLoadedAssetBundle == null)
            {
                //Debug.Log("Failed to load AssetBundle!");
                return;
            }
            var assetLoadRequest = myLoadedAssetBundle.LoadAsset<GameObject>("scena" + numeroScena);

            GameObject cube = assetLoadRequest as GameObject;

            cube.GetComponentInChildren<ScenaController>().reg = transform.GetComponent<RegistaStoria>();
            cube.GetComponentInChildren<ScenaController>().numScena = numeroScena;
            cube.name = numeroScena + "";
            if (numeroScena != currentScene)
                cube.SetActive(false);
            Instantiate(cube, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation, this.transform);
*/
            //changeScreen(numeroScena);

            if (bundleScreens == null)
            {
                IEnumerator loadScreen = loadBundleScreens();
                StartCoroutine(loadScreen);
            }

            IEnumerator num = loadObject(numeroScena);
            StartCoroutine(num);

		} //else {
			////Debug.Log("Il file non esiste. -.- " + Application.persistentDataPath + "/dir/" + idStoria + "/" + pathName + "/" + difficulty + "/scena" + numeroScena);
		//}

    }

	IEnumerator loadObject(int numeroScena)
    {

        //Preview();
        flagScreens = false;

        if(currentScene == 0)
            yield return new WaitUntil( () => bundleScreens != null);


        Debug.Log("SCENA CORRENTE: " + currentScene);
        Debug.Log("NumScena: " + numeroScena);
        string sceneName = "Scena" + numeroScena + suffissoScreenName;
        if (currentScene != 0)
        {
            Debug.Log("SceneName: " + sceneName);
            Debug.Log("ImageName: " + nextScenePreview.GetComponent<Image>().sprite.name);
            if (sceneName.Equals(nextScenePreview.GetComponent<Image>().sprite.name))
            {
                nextScenePreview.SetActive(true);
                nextScenePreview.GetComponent<Transform>().SetAsLastSibling();
                prevScenePreview.SetActive(false);
            }
            else
            {
                prevScenePreview.SetActive(true);
                prevScenePreview.GetComponent<Transform>().SetAsLastSibling();
                nextScenePreview.SetActive(false);
                
            }
        }



        //Controlla ambiente
        string pathName = "";
        if (SystemInfo.operatingSystem.Contains("iOS"))
        {
            pathName = "Ios";
        }
        else if (SystemInfo.operatingSystem.Contains("Android"))
        {
            pathName = "Android";
        }
        else if (SystemInfo.operatingSystem.Contains("Mac"))
        {
            pathName = "Ios";
        }
        else if (SystemInfo.operatingSystem.Contains("Windows"))
        {
            pathName = "Windows";
        }
        else
        {
            pathName = "Linux";
        }
		//pathName = "Android";//Sostituire
        //Fine Controlla Ambiente

        if (File.Exists(Application.persistentDataPath + "/dir/" + idStoria + "/" + pathName + "/" + difficulty + "/scena" + numeroScena))
        {

            AssetBundleRequest assetLoadRequest = null;
            AssetBundleCreateRequest bundleLoadRequest = null;

            bundleLoadRequest = AssetBundle.LoadFromFileAsync(Application.persistentDataPath + "/dir/" + idStoria + "/" + pathName + "/" + difficulty + "/scena" + numeroScena);
            yield return bundleLoadRequest;

            var myLoadedAssetBundle = bundleLoadRequest.assetBundle;
            if (myLoadedAssetBundle == null)
            {
                //Debug.Log("Failed to load AssetBundle!");
                yield break;
            }

            assetLoadRequest = myLoadedAssetBundle.LoadAssetAsync<GameObject>("scena" + numeroScena);
            yield return assetLoadRequest;

            GameObject cube = assetLoadRequest.asset as GameObject;

            cube.GetComponentInChildren<ScenaController>().reg = transform.GetComponent<RegistaStoria>();
            cube.GetComponentInChildren<ScenaController>().numScena = numeroScena;
            cube.name = numeroScena + "";
            if (numeroScena != currentScene)
                cube.SetActive(false);
            Instantiate(cube, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation, this.transform);

            //Debug.Log("instanzio");

            nextScenePreview.SetActive(false); //Modifiche
            prevScenePreview.SetActive(false);

            Debug.Log(bundleScreens.GetAllAssetNames()[0]);


            //Pulisco dalla cache il bundle scaricato.
            myLoadedAssetBundle.Unload(false);
            assetLoadRequest = null;
            bundleLoadRequest = null;
            System.GC.Collect();
			Caching.ClearCache();
            contaFigure();

            Debug.Log(bundleScreens.GetAllAssetNames()[0]);
            IEnumerator ie = changeScreen(numeroScena+1);
            StartCoroutine(ie);
            yield return new WaitUntil(() => flagScreens);
        }

    }

    public void checkUltimaScena()
    {
		//Controlla ambiente
        string pathName = "";
        if (SystemInfo.operatingSystem.Contains("iOS"))
        {
            pathName = "Ios";
        }
        else if (SystemInfo.operatingSystem.Contains("Android"))
        {
            pathName = "Android";
        }
        else if (SystemInfo.operatingSystem.Contains("Mac"))
        {
            pathName = "Ios";
        }
        else if (SystemInfo.operatingSystem.Contains("Windows"))
        {
            pathName = "Windows";
        }
        else
        {
            pathName = "Linux";
        }
		//pathName = "Android";//Sostituire
        //Fine Controlla Ambiente

        int sceneQ = 0;
        bool nextQ = true;
        while (nextQ)
        {
            if (File.Exists(Application.persistentDataPath + "/dir/" + idStoria + "/" + pathName + "/" + difficulty + "/scena" + sceneQ))
            {
                ultimaScena = sceneQ;
                sceneQ++;
            }
            else
            {
                nextQ = false;
            }
        }
    }

    void Start()
    {

        


        this.transform.position = new Vector3(0, 0, 0);

        opzioni.caricaOpzioniAudio();

		//Debug.Log("Caricate le opzioni Audio");

        if (opzioni.option.tipoLettura == 1)
        {
            difficolta = "Difficile";
            difficulty = "Advanced";
            suffissoScreenName = "E";
        }
        else
        {
            difficolta = "Facile";
            difficulty = "Easy";
            suffissoScreenName = "S";
        }

        //webGL = Camera.main.gameObject.GetComponent<LoadObjectFromBundle>().webgl;

        if (!webGL)
            caricaIdStoria();

        figuraCorrente = 0;
        figureCount = 0;

        countdown = 0f;
        countdownEnable = false;
        canTouch = false;
        player = GetComponent<AudioSource>();

        currentScene = 0;
          checkUltimaScena();


		//Controlla ambiente
        pathName = "";
        if (SystemInfo.operatingSystem.Contains("iOS"))
        {
            pathName = "Ios";
        }
        else if (SystemInfo.operatingSystem.Contains("Android"))
        {
            pathName = "Android";
        }
        else if (SystemInfo.operatingSystem.Contains("Mac"))
        {
            pathName = "Ios";
        }
        else if (SystemInfo.operatingSystem.Contains("Windows"))
        {
            pathName = "Windows";
        }
        else
        {
            pathName = "Linux";
        }


        /*Controllo Aspect Ratio */
        if (!pathName.Equals("Windows") && !pathName.Equals("Mac"))
        {
            float width = Display.main.systemWidth;
            float height = Display.main.systemHeight;

            if (rapporto == (4 / 3))
                aspectRatio = "_4-3";
            else
                aspectRatio = "_16-9";
        }
        else{
            aspectRatio = "_16-9";
        }
        
		//pathName = "Android";//Sostituire
        //Fine Controlla Ambiente
        assetBundle = AssetBundle.LoadFromFile(Application.persistentDataPath + "/dir/" + idStoria + "/" + pathName + "/" + pathName);
        manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");


        
        Preview(); //Modifiche
        if (!webGL)
        {
            loadBundleShared(0);          
        }
        else
        {
            List<int> numero = new List<int>();
            numero.Add(0);
            IEnumerator num = loadObject(numero);
            StartCoroutine(num);
        }

        
        UpdateScenesVieved(refresh);

    }

    public void contaFigure()
    {
        if (currentScene > 0)
        {
            Transform child = this.transform.Find(currentScene + "(Clone)");
            figureCount = child.GetComponent<ScenaController>().figure.Count;
            //Debug.Log("la scena ha:" + figureCount);
        }
    }

    void scenaPlayer()
    {

        if (currentScene == 0 || currentScene == ultimaScena)
            canTouch = true;

        else
        {
            //normal play
            if (opzioni.option.standard)
            {
                if (!countdownEnable && !canTouch)
                {
                    if (figuraCorrente < figureCount)
                    {



                        Transform child = this.transform.Find(currentScene + "(Clone)");
                        for (int i = 0; i < child.GetComponent<ScenaController>().figure.Count; i++)
                            if (child.GetComponent<ScenaController>().figure[i].figura.GetComponent<FigureController>().playerIsInPla() || (child.GetComponent<ScenaController>().figure[i].figura.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("buttonBounce")))
                            {

                            }
                            else
                            {
                                //Modifica controllo
                                if (lastToken != null)
                                {
                                    lastToken.SetSiblingIndex(lastFig);
                                    cha = false;
                                }
                                //Fine Modifica
                                scena = transform.Find(currentScene + "(Clone)");
                                scena = scena.GetComponent<ScenaController>().spacer;
                                scena.GetChild(figuraCorrente).GetComponent<FigureController>().playSoloAudioEAnimation();

                                //Modifica assegnamento
                                if (!cha)
                                {
                                    lastToken = scena.GetChild(figuraCorrente);
                                    //Debug.Log("Assegno scena e setto ad ultimo figlio.");
                                    lastFig = figuraCorrente;
                                    lastToken.SetAsLastSibling();
                                    cha = true;
                                }
                                //Fine Modifica

                                //Debug.Log("Sto ascoltando la " + figuraCorrente + " su un totale di " + figureCount);
                                figuraCorrente++;
                                //Debug.Log ("Sono dopo a figure++ fig=" + figuraCorrente);
                            }
                    }
                    else
                    {
                        Transform child = this.transform.Find(currentScene + "(Clone)");
                        //if(child!=null)
                        for (int i = 0; i < child.GetComponent<ScenaController>().figure.Count; i++)
                            if (child.GetComponent<ScenaController>().figure[i].figura.GetComponent<FigureController>().playerIsInPla())
                            {

                            }
                            else
                            {
                                canTouch = true;
                                lastFig = 0;
                                lastToken = null;
                                cha = false;
                            }
                    }
                }
            }
            else if (opzioni.option.personalizzata)
            {
                Transform child = this.transform.Find(currentScene + "(Clone)");

                if (!loaded && child != null)
                {

                    player.clip = null;

					string profilo="";

					if(opzioni.option.tipoLettura == 0)
						profilo = opzioni.option.nomiProfiliOriginaliEasy[opzioni.option.profileSelected];
					else
						profilo = opzioni.option.nomiProfiliOriginaliAdvanced[opzioni.option.profileSelected];

                    string nomeFile = (currentScene - 1) + ".dat";

                    string path = Path.Combine(Application.persistentDataPath, "ProfiliAudio");
                    path = Path.Combine(path, idStoria);
                    path = Path.Combine(path, difficolta);
                    path = Path.Combine(path, profilo);
                    path = Path.Combine(path, nomeFile);

                    //Debug.Log(path);
                    //Debug.Log(currentScene);

                    durationClip = AudSav.LoadAudioClipFromDisk(player, path, nomeFile);

                    player.Play();
                    //Debug.Log(durationClip);

                    loaded = true;

                }
                else
                {
                    durationClip -= Time.deltaTime;
                    if (durationClip <= 0f)
                    {
                        canTouch = true;
                    }
                }

            }
            else if (opzioni.option.tasti)
            {
                canTouch = true;
            }
            else if (opzioni.option.muto)
            {
                canTouch = true;
            }
        }
    }


    void FixedUpdate()
    {
        panel.transform.SetAsLastSibling();
        panel.SetActive(!canTouch);
        spostafigli();
        scenaPlayer();
    }


    public void UpdateScenesVieved(bool refr)
    {

        bool successivoEsistente = false;
        bool precedenteEsistente = false;
        bool correnteEsistente = false;
        Transform parent = transform;
        var children = new List<GameObject>();
        List<int> numeri = new List<int>();

        foreach (Transform child in parent)
        {
            if (!child.gameObject.name.Equals("PanelCanTouch") && !child.gameObject.name.Contains("scenePreview")){
                /*
                if (child.GetComponentInChildren<ScenaController>().numScena < currentScene - 1
                    || child.GetComponentInChildren<ScenaController>().numScena > currentScene + 1)
                {
                    children.Add(child.gameObject);
                    child.gameObject.SetActive(false);
                }
                else if (child.GetComponentInChildren<ScenaController>().numScena == currentScene + 1)
                {
                    successivoEsistente = true;
                }
                else if (child.GetComponentInChildren<ScenaController>().numScena == currentScene - 1)
                {
                    precedenteEsistente = true;
                }
                else if (child.GetComponentInChildren<ScenaController>().numScena == currentScene)
                {
                    correnteEsistente = true;
                }
                else{
                    //Debug.Log(child.GetComponentInChildren<ScenaController>().numScena);
                }
                */

                //if(child.GetComponentInChildren<Transform>().gameObject.name != "scenePreview"){}//modifiche

                if (child.GetComponentInChildren<ScenaController>().numScena == currentScene) //modifiche
                {
                    correnteEsistente = true;
                }
                else{
                    children.Add(child.gameObject);
                    child.gameObject.SetActive(false);
                }
                     //modifiche
                
            } //modifiche

            
        }

        
        //distruggi tutte le scene che non mi occorrono
        //children.ForEach(child => DestroyImmediate(child)); //Modifiche

        for(int i=0; i<children.Count; i++){//Modifiche
            if(!children.ToArray()[i].gameObject.name.Contains("scenePreview")){
                Debug.Log("Destroy:" + children.ToArray()[i].gameObject.name);
                DestroyImmediate(children.ToArray()[i]);
            }
        }



        //Se la scena che mi occorre non c'è
        /* inizio Modifiche
        if (!successivoEsistente)
        {
			//Debug.Log("Carico prossima scena: " + (currentScene+1));
			loadBundleShared((currentScene + 1));
        }

        if (!precedenteEsistente && currentScene > 0)
        {
			loadBundleShared((currentScene - 1));
        }
        */ // fine modifiche

        if (!correnteEsistente && currentScene > 0)
        {
                loadBundleShared(currentScene);
        }

/*
        //se devo refreshare le scene
        if (refr)
        {
            //ho sempre la scena corrente ma non funziona alla scena 0
            if (correnteEsistente && currentScene != 0)
            {
                //Debug.Log("Resetto animazioni scena "+currentScene);
                    //loadBundleShared(currentScene);
                    resettaScena(currentScene);
            }
            
            //ho sempre la successiva tranne alla fine
            if (successivoEsistente)
            {   
                //Debug.Log("Resetto animazioni scena "+(currentScene+1));
                    //loadBundleShared(currentScene+1);
                    resettaScena(currentScene+1);
            }

            
            if (precedenteEsistente)
            {
                //Destroy(transform.Find((currentScene-1) + "(Clone)").gameObject);
                if (!webGL){
                    //loadBundleShared(currentScene-1);
                    //resettaScena(currentScene-1);
                }
                else
                {
                    //Debug.Log("Aggiungo:" + (currentScene - 1));
                    numeri.Add(currentScene - 1);
                }
            }
            
    }*/

        if (webGL)
        {
            IEnumerator num = loadObject(numeri);
            StartCoroutine(num);
        }

        countdownEnable = true;

        cambiaScenaScomparsa();


    }

    public void resettaScena(int sceneToReset){
        //Debug.Log("scena "+sceneToReset);
        Transform child = this.transform.Find(sceneToReset + "(Clone)");

		for (int i = 0; i < child.GetComponent<ScenaController>().figure.Count; i++){

			string nomeDaCercare = transform.Find((sceneToReset) + "(Clone)").gameObject.GetComponent<ScenaController>().figure[i].figura.gameObject.name;
			Transform scena = transform.Find(sceneToReset + "(Clone)");
            scena = scena.GetComponent<ScenaController>().spacer;
			//Debug.Log("Resetto figura: " + scena.Find(nomeDaCercare + "(Clone)").name);
			scena.Find(nomeDaCercare + "(Clone)").SetSiblingIndex(i);
            scena.Find(nomeDaCercare + "(Clone)").GetComponent<FigureController>().ResetMyTouch();
            scena.Find(nomeDaCercare + "(Clone)").GetComponent<FigureController>().resetAnimationInScene();

        }
    }

    public void spostafigli()
    {
        if (countdownEnable)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f)
            {
                countdownEnable = false;
                countdown = 1f;
            }
        }
    }

    public void cambiaScenaScomparsa()
    {
        Transform parent = transform;
        foreach (Transform child in parent)
        {
            if (!child.gameObject.name.Equals("PanelCanTouch")){
                if ((!child.gameObject.name.Contains("scenePreview"))/*modifiche */ && (child.GetComponent<ScenaController>().numScena == currentScene))
                {
                    child.gameObject.SetActive(true);
                }
                else
                {
                    //child.gameObject.SetActive(false);
                    //Debug.Log("detroy: " + child.gameObject.name);
                    //DestroyImmediate(child.gameObject); //Modifiche
                }
            }

        }

        //scenePreview.SetActive(false); //Modifiche
    }


    public void SaltaScena()
    {
        figuraCorrente = 0;
        loaded = false;

        if (canTouch && !countdownEnable)
        {

            opzioni.caricaOpzioniAudio();
            if (opzioni.option.tipoLettura == 1)
            {
                difficolta = "Difficile";
                difficulty = "Advanced";
                suffissoScreenName = "E";
            }
            else
            {
                difficolta = "Facile";
                difficulty = "Easy";
                suffissoScreenName = "S";
            }

            currentScene++;
            UpdateScenesVieved(refresh);
            if (refresh)
            {
                refresh = false;
            }
            clearcache();
            canTouch = false;
        }
    }

    public void NextScene()
    {
        figuraCorrente = 0;
        loaded = false;
        if (currentScene == 0)
        {
            int oldoption = opzioni.option.tipoLettura;
            opzioni.caricaOpzioniAudio();
            if (opzioni.option.tipoLettura == 1)
            {
                difficolta = "Difficile";
                difficulty = "Advanced";
            }
            else
            {
                difficolta = "Facile";
                difficulty = "Easy";
            }
            if (oldoption != opzioni.option.tipoLettura)
            {
                refresh = true;
            }
        }

        if (currentScene != 0)
        {
             resettaScena(currentScene);
        }

        if (canTouch && !countdownEnable)
        {
            currentScene++;
            UpdateScenesVieved(refresh);
            if (refresh)
            {
                refresh = false;
            }
			canTouch = false;
            clearcache();
            
        }
    }

    public void PreviusScene()
    {
        figuraCorrente = 0;
        loaded = false;
        if (currentScene != 0)
        {
             resettaScena(currentScene);
        }
        if (canTouch && !countdownEnable)
        {
            currentScene--;
			refresh = true;
            UpdateScenesVieved(refresh);
            clearcache();
            canTouch = false;
        }
    }

    public void caricaSceltaMenu()
    {

        foreach (GameObject go in FindObjectsOfType<GameObject>())
        {
            if (go.name.Equals("Main Camera"))
            {
                go.GetComponent<LoadObjectFromBundle>().destroyMe();
            }
        }

		AssetBundle.UnloadAllAssetBundles(false);
        SceneManager.LoadScene("ScenaScelta", LoadSceneMode.Single);

        createdBook = false;

        Destroy(this.gameObject);

    }

	public void saltaAZero()
	{
		currentScene = 0;
		SaltaScena();
		//Debug.Log("Saltato");
	}

    public void clearcache()
    {
        SceneManager.LoadScene("ScenaLettura", LoadSceneMode.Single);
    }

}