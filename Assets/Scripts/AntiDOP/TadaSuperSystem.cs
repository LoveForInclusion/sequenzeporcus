using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using RESTClient.Request;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using UnityEngine.SceneManagement;

public class TadaSuperSystem : MonoBehaviour {

//	public PurchaseEngine IAPEngine;

	private StoryHandler storyHandler;
	private int storyVerify = 0;
	private TransactionHandler transactionHandler;
	private int transactionVerify = 0;

    /*
     * Le variabili verify possono avere i seguenti valori a seconda dello status dell'inizializzazione.
     * -1: ERRORE
     * 0: NON INIZIALIZZATO
     * 1: INIZIALIZZATO
    */

	public static string idStoryToDownloadOrBuy;
	private int shelfCounter = 0;
	private bool shittyVariable = false;



	/******************************************************* */   
	public GameObject spinner;
	public Text installing;
	public GameObject panelError;
    public GameObject panelUpdate;
	public Text testoErrore;
    public GameObject caricamentoInCorsoText;
    public GameObject progBar;
    //Story Token Comps
	public GameObject token, content, details;

    public GameObject panelCantTouch;
	private GameObject activeUpdateToken;

	//Variabili per Mac/Win
	bool sentToWebStore = false;
	//End

	/************************************************* */

	private void Start()
	{
		StartCoroutine(startDelay());
	}

	IEnumerator startDelay(){
		yield return new WaitForSeconds(0.5f);
		SetupSystem();
	}


	public TransactionHandler GetTransactionHandler(){
        return transactionHandler;
    }
	public string GetDBId(){
		return idStoryToDownloadOrBuy;
	}

    /***** Inizializza tutti gli handler necessari. *****/
    private void SetupSystem(){

		storyHandler = new StoryHandler(this);
		transactionHandler = new TransactionHandler(this);
		FileHandler.setUpSystem(this);

        /* Carica innanzitutto la lista di storie salvate sul dispositivo locale */
        Stories temp_s = null;
        FileHandler.caricaFile(ref temp_s, true);
        if (temp_s != null){
            Stories s = new Stories(new List<Story>());
            FileHandler.caricaFile<Stories>(ref s, true);
            storyHandler.setStoriesLocal(s);
			storyVerify = 1;
        }
		if (storyHandler.GetStoriesLocal() == null)
			Debug.Log("NUllo");

        if (Application.internetReachability == NetworkReachability.NotReachable){ /*  Se non c'è connessione */

			// Carica le transaction dal dispositivo locale
			loadLocalTransactions();

        }
        else{
			/* Carica le Transactions tramite API */
			//Debug.Log("Starto transazioni");
			setupTransactions();
			// Fine Caricamento Transactions

			/* Carica le Storie tramite API */
			//Debug.Log("Starto storie");
			setupStories();
            //FINE SETUP STORIE
            //FINE SETUP BUSINESS LOGIC

        }

		//Generazione Libreria
		//Inserire IEnumerator per ritardare Generazione libreria
		caricamentoInCorsoText.transform.Find("Spinner").GetComponent<Image>().fillAmount += 0.25f;
		StartCoroutine(delayLibrary());
		//GenerateLibrary();


    }   
    /************************ FINE SETUP ***************************/



	/********* Gestisce cosa succede quando clicco su una storia in base al suo stato ************/
	public void StoryListener()
    {
		string storyId = null;
		//Cerco la storia desiderata nel pannello dettagli
		if (EventSystem.current.currentSelectedGameObject.transform.parent.parent.GetComponent<StoryDetailPanel>() != null) 
			storyId = EventSystem.current.currentSelectedGameObject.transform.parent.parent.GetComponent<StoryDetailPanel>().GetStory().id;

		//Se non esiste è probabilmente leggibile quindi leggo dal token
		if (storyId == null)
			storyId = EventSystem.current.currentSelectedGameObject.GetComponent<StoryToken>().getStoryId();
		Debug.Log(storyId);

		int statusStoria = storyHandler.checkStory(storyId);

        /* Controlla lo stato della storia dato da CheckStory() */
        switch (statusStoria)
        {
            case -1:    //ComingSoon;

                break;
            case 0:     //Purchasable
				activeUpdateToken = EventSystem.current.currentSelectedGameObject.transform.parent.parent.GetComponent<StoryDetailPanel>().getActiveToken();
				idStoryToDownloadOrBuy = storyId;
				PlayerPrefs.SetString("idDB", storyId);
				Debug.Log(idStoryToDownloadOrBuy);
                PurchaseStory(storyId);
                break;
            case 1:     //Downloadable
				Debug.Log("Downloading");
                DownloadStory(storyId);
                break;
            case 2:     //Readable
				Debug.Log("Opening");
				OpenBook(storyId);
                break;
            case 3:     //Updatable
                panelUpdate.SetActive(true);
				activeUpdateToken = EventSystem.current.currentSelectedGameObject;
				idStoryToDownloadOrBuy = storyId;
                break;
			default:
				Debug.Log("Defaulting");
				break;
        }
    }

    /*****************    ***************/


    /******* Avvia la procedura di acquisto di una storia *******/
    private void PurchaseStory(string storyId){

		Debug.Log ("Attempting to buy: " + storyId);
		spinner.SetActive(true);
		if (SystemInfo.operatingSystem.Contains("Mac") || SystemInfo.operatingSystem.Contains("Windows"))
		{
			sentToWebStore = true;
			DesktopPurchaseRequest.RequestPurchase(this, PlayerPrefs.GetString("userId"), storyId,

       delegate(DesktopPurchaseResponse response)
		{
			Application.OpenURL("https://tadabook.it/webstore/pay?id=" + response.url);
		},
       delegate(string s1, string s2)
        {
			sentToWebStore = false;
			spinner.SetActive(false);
			panelError.GetComponent<Text>().text = "C'è stato un errore con l'inizializzazione dell'acquisto.\nTerminare l'applicazione e provare dopo aver rieffettuato l'accesso.";
            panelError.SetActive(true);
			Debug.Log("Errore: " + s1 + "\n" + s2);
        });
		}
		else
		{
		//	IAPEngine.BuyItem(storyId);
		}
    }

    //Controllo se al ritorno da store web
	void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)//Se il focus è tornato
        {
            if (sentToWebStore) //Se ero stato mandato allo store
            {
                sentToWebStore = false;
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    /******* Avvia la procedura di Download di un storia ********/
    private void DownloadStory(string storyId){

        progBar.SetActive(true);
		installing.gameObject.SetActive(true);
		installing.text = "Download in corso...";
		FileHandler.downloadStory(storyId, this.progBar);
        /* Abilitare pannello can't touch */
        panelCantTouch.SetActive(true);
    }

    /******* Avvia la procedura di installazione ******/
    public void UnzipStory(string path, string storyId){
		installing.text = "Installazione in corso...";
		FileHandler.unZipStory(Application.persistentDataPath + "/" + path, storyId, progBar);
    }

	/******* Avvia la procedura di aggiornamento ******/
	public void UpdateStory(){
		DownloadStory(idStoryToDownloadOrBuy);
		storyHandler.UpdateStory(idStoryToDownloadOrBuy);
	}

	public void ChangeStoryStatus(GameObject token){
		//Controllo se è aggiornamento
		if (activeUpdateToken != null)
			token = activeUpdateToken;
		
		Debug.Log("Cambio Status" + storyHandler.checkStory(token.GetComponent<StoryToken>().getStoryId()));//Token non esiste ancora
		token.GetComponent<Button>().onClick.RemoveAllListeners();
		int status = storyHandler.checkStory(token.GetComponent<StoryToken>().getStoryId());
		token.GetComponent<StoryToken>().ChangeStatus(status);
		if (status < 2)
            token.GetComponent<Button>().onClick.AddListener(showDetails);
        else if (status >= 2)
            token.GetComponent<Button>().onClick.AddListener(StoryListener);

		//Se è aggiornamento ri annullo il token
        if (token == activeUpdateToken)
				activeUpdateToken = null;
	}

	public void ChangeStoryStatus(string id)//Overload di ChangeStoryStatus
    {
		//Controllo se è aggiornamento
		GameObject tokenZ = this.findToken(id);

        Debug.Log("Cambio Status" + storyHandler.checkStory(tokenZ.GetComponent<StoryToken>().getStoryId()));//Token non esiste ancora
        tokenZ.GetComponent<Button>().onClick.RemoveAllListeners();
        int status = storyHandler.checkStory(tokenZ.GetComponent<StoryToken>().getStoryId());
        tokenZ.GetComponent<StoryToken>().ChangeStatus(status);
        if (status < 2)
            tokenZ.GetComponent<Button>().onClick.AddListener(showDetails);
        else if (status >= 2)
            tokenZ.GetComponent<Button>().onClick.AddListener(StoryListener);
    }

   public void OpenBook(string storyId){
		FileHandler.salvaFileId(storyId);
		SceneManager.LoadScene("ScenaScelta", LoadSceneMode.Single);
   }

	public void FuckOff(){
		OpenBook(idStoryToDownloadOrBuy);
	}

    //Setup methods
	private void setupTransactions(){
		/* Carica le Transactions tramite API */
        TransactionRequest.GetAllTransactions(this,
            delegate (Transactions transactions)
            {
                transactionHandler.SetTransactions(transactions);
                FileHandler.salvaFile<Transactions>(transactionHandler.GetTransactions(), false);
				//Debug.Log(transactions.transactions.Count);
				transactionVerify = 1;
            },
            delegate (string code, string error)
            {
				// Non dovrebbe arrivarci ma in caso ci arriva gestiamo l'errore
				loadLocalTransactions();
				ErrorModule.SendError(error, testoErrore, panelError);
			    Debug.LogError(code + "---" + error);
            }
        );
	}

	private void setupStories(){
		/* Carica le Storie tramite API */
        StoryRequest.GetAllStory(this,
            delegate (Stories storiesOnline)
            {
				//Debug.Log("Finito con le storie.");
                int i = 0;
				if (storyHandler.GetStoriesLocal() != null)
				{
					Story stL, stO;
				    //Debug.Log("Verifico le storie.");
					/* Verifica se sono disponibili aggiornamenti per le storie */
					for (; i < storyHandler.GetStoriesLocal().stories.Count; i++)
					{
						stL = storyHandler.GetStoriesLocal().stories.ToArray()[i];
						stO = storiesOnline.stories.ToArray()[i];
						//Controllo last update
						if (!stL.lastUpdate.Equals(stO.lastUpdate))
					    {
							if (!stL.comingSoon)//Se non è qualcosa in arrivo, eseguo la prassi classica per l'update.
							{
								Debug.Log("Updatable! ID: " + stL.id);
								if (FileHandler.checkStoryDirectory(stL.id))
								{
									Debug.Log("Adding to updatable.\nData 1: " + stL.lastUpdate + ".\nData2: " + stL.lastUpdate + ".");
									storyHandler.AddStoryUpdatable(stO);
								}
								else
								{
									storyHandler.GetStoriesLocal().stories.RemoveAt(i);
									storyHandler.GetStoriesLocal().stories.Insert(i, stO);
								}
						} else {
							if(!stO.comingSoon){//Se è in arrivo controllo se la storia è stata rilasciata in tal caso sostituisco
								storyHandler.GetStoriesLocal().stories.RemoveAt(i);
                                storyHandler.GetStoriesLocal().stories.Insert(i, stO);
							}
						}
						}
					}

					if (!storyHandler.GetStoriesLocal().stories.Count.Equals(storiesOnline.stories.Count))
					//Se ci sono storie rilasciate in più rispetto alle locali
					//Le aggiungo direttamente
					{
						for (; i < storiesOnline.stories.Count; i++)
						{
							storyHandler.AddStoryLocal(storiesOnline.stories.ToArray()[i]);
						}
					}
                    //Pulisco
					stL = null;
					stO = null;
			} else {
				    //Debug.Log("Stories local == null.");
					storyHandler.setStoriesLocal(storiesOnline);
					//Controllo poi se è nullo e salvo le storie
				    if(storyHandler.GetStoriesLocal().stories==null)	
				        Debug.Log("Nullo pure dopo? -.-");
			}
				FileHandler.salvaFile(storyHandler.GetStoriesLocal(), true);
				storyVerify = 1;
            },
            delegate (string code, string error)
            {
                Debug.Log(code + "---" + error);
			    ErrorModule.SendError(error, testoErrore, panelError);
                if (File.Exists(Application.persistentDataPath + "/myLibrary.dat"))
                {
                        caricamentoInCorsoText.SetActive(false);
                }
            });
	}

	private void loadLocalTransactions(){
		// Carica le transaction dal dispositivo locale
        Transactions t = new Transactions(new List<Transaction>());
        FileHandler.caricaFile<Transactions>(ref t, false);
        transactionHandler.SetTransactions(t);
		transactionVerify = 1;
	}


    //Controllo cover mancanti
	private void shelfControl(int control){
		for (int i = 0; i < storyHandler.GetStoriesLocal().stories.Count; i++){
			if(!File.Exists(Application.persistentDataPath+"/"+storyHandler.GetStoriesLocal().stories.ToArray()[i].shelfCover)){//Se NON esiste la cover
				if (control==0)
					shelfCounter++;
				else
					FileHandler.DownloadShelf(storyHandler.GetStoriesLocal().stories.ToArray()[i].id, storyHandler.GetStoriesLocal().stories.ToArray()[i].shelfCover);
			}
		}
	}

    //Messaggio di avvenuto download cover
	public void ShelfDownloaded(){
		shelfCounter--;
	}


    //Library Methods

	private IEnumerator delayLibrary(){
		yield return new WaitUntil(() => storyVerify == 1);
		Debug.Log("Storie.");
		caricamentoInCorsoText.transform.Find("Spinner").GetComponent<Image>().fillAmount += 0.25f;
		yield return new WaitUntil(() => transactionVerify == 1);
		Debug.Log("Transazioni.");
		caricamentoInCorsoText.transform.Find("Spinner").GetComponent<Image>().fillAmount += 0.25f;
		shelfControl(0);//Inizializza il counter
        shelfControl(shelfCounter);
		yield return new WaitUntil(() => shelfCounter == 0);
        Debug.Log("Shelf.");
		//caricamentoInCorsoText.transform.Find("Spinner").GetComponent<Image>().fillAmount += 0.25f;
//		IAPEngine.SetupPurchaseEngine(this, this.storyHandler);
	//	yield return new WaitUntil(() => IAPEngine.IsInitialized());
		GenerateLibrary();
		yield return new WaitUntil(() => shittyVariable);
		yield return new WaitForSeconds(0.25f);
		caricamentoInCorsoText.SetActive(false);
	}

	private void GenerateLibrary(){
		for (int i = 0; i < storyHandler.GetStoriesLocal().stories.Count; i++){
			Story story = storyHandler.GetStoriesLocal().stories.ToArray()[i];
			GenerateToken(token, story.id, storyHandler.checkStory(story.id), story.shelfCover);
			caricamentoInCorsoText.transform.Find("Spinner").GetComponent<Image>().fillAmount += 0.25f / (float)storyHandler.GetStoriesLocal().stories.Count;
		}

		caricamentoInCorsoText.transform.Find("Spinner").GetComponent<Image>().fillAmount = 1;
		shittyVariable = true;
	}
    
	private void GenerateToken(GameObject card, string storyId, int status, string shelfCover)//Renderizza il token
    {
        GameObject gameO = Instantiate(card, content.transform);
        StartCoroutine(gameO.GetComponent<StoryToken>().SetupToken(new TadaSuperSystem(), storyId, status, shelfCover));
		gameO.name = storyId;
        if(status<2)
            gameO.GetComponent<Button>().onClick.AddListener(showDetails);
		else if(status >= 2)
			gameO.GetComponent<Button>().onClick.AddListener(StoryListener);
		
    }

	public void showDetails()//Renderizza i dettagli
    {
		Story story = storyHandler.FindStory(EventSystem.current.currentSelectedGameObject.transform.GetComponent<StoryToken>().getStoryId());
		int status = EventSystem.current.currentSelectedGameObject.transform.GetComponent<StoryToken>().getStatus();
		details.GetComponent<StoryDetailPanel>().SetupPanel(this, story, status);
    }

	public void ErrorWrapper(string error){
		ErrorModule.SendError(error, testoErrore, panelError);
	}

	public GameObject findToken(string id){
		for (int i = 0; i < content.transform.childCount; i++){
			StoryToken st = content.transform.GetChild(i).GetComponent<StoryToken>();
			if (st.getStoryId().Equals(id))
				return content.transform.GetChild(i).gameObject;
		}
		return null;//Non dovrebbe mai arrivarci qui... I wish...
	}

	public void RefreshLibrary(){

		for (int i = 0; i < content.transform.childCount; i++)
        {
			GameObject st = content.transform.GetChild(i).gameObject;
			this.ChangeStoryStatus(st);
        }

	}

}
