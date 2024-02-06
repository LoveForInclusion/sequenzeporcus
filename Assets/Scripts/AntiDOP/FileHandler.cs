using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using RESTClient.Request;

public class FileHandler {

	private static bool testBundle = false;
	private static TadaSuperSystem system = null;


	// metodi di utility
	public static void setTestBundle(bool setter){ //Imposta testBundle e viceversa

		testBundle = setter;

	}
	public static bool getTestBundle(){
		return testBundle;
	}

	public static void setUpSystem(TadaSuperSystem newSystem){

		system = newSystem;
	}

	public static bool checkStoryDirectory(string id){
		return Directory.Exists(Application.persistentDataPath + "/dir/" + id);
	}

	// Metodi di manipolazione dei file
	public static void salvaFile<T>(T array, bool isStory){

		string destinationToken = Application.persistentDataPath;

		// Controlla se vogliamo salvare una storia o una transazione
		if (isStory)
				destinationToken += "/myLibrary.dat";
			else
				destinationToken += "/myTransaction.dat";

		FileStream fileToken = null;

		//se i file esistono li apre
        if (File.Exists(destinationToken))
            fileToken = File.OpenWrite(destinationToken);
        else
            fileToken = File.Create(destinationToken);

        //salvo i file in locale
        BinaryFormatter bf = new BinaryFormatter();

        bf.Serialize(fileToken, array);
        fileToken.Close();
	}

	public static void salvaFileId(string id)
    {

        string destinationToken = Application.persistentDataPath;

        // Serializzazione ID

		destinationToken += "/myIdStoria.dat";

        FileStream fileToken = null;

        //se i file esistono li apre
        if (File.Exists(destinationToken))
            fileToken = File.OpenWrite(destinationToken);
        else
            fileToken = File.Create(destinationToken);

        //salvo i file in locale
        BinaryFormatter bf = new BinaryFormatter();

        bf.Serialize(fileToken, id);
        fileToken.Close();
    }

	public static void caricaFile<T>(ref T array, bool isStory){

		string destinationToken = Application.persistentDataPath;

		// Controlla se vogliamo salvare una storia o una transazione
		if (isStory)
			destinationToken += "/myLibrary.dat";
		else
			destinationToken += "/myTransaction.dat";

		FileStream fileToken = null;

		//se i file esistono li apre in lettura
		if (File.Exists(destinationToken))
			fileToken = File.OpenRead(destinationToken);
		else{
			array = default(T);
			return;
		}

		//leggo i file locali
		BinaryFormatter bf = new BinaryFormatter();

		array = (T) bf.Deserialize(fileToken);
		fileToken.Close();	
	}

    public static string caricaIdStoria()
    {
        string idStoria;
        //carico la directory dei salvataggi
        string destinationUtente = Application.persistentDataPath + "/myIdStoria.dat";
        FileStream fileUtente;

        //se i file esistono li apre
        if (File.Exists(destinationUtente))
            fileUtente = File.OpenRead(destinationUtente);
        else
        {
            return null;
        }

        //leggo i file locali
        BinaryFormatter bf = new BinaryFormatter();

        idStoria = (string)bf.Deserialize(fileUtente);

        fileUtente.Close();
        return idStoria;
    }

	public static string createZipFileName(){

		//return "iosBundle.zip";//Sostituire

		string fileName = "";

		// Controllo il sistema operativo
		if(SystemInfo.operatingSystem.Contains("iOS"))
			fileName += "ios";
		else if(SystemInfo.operatingSystem.Contains("Android"))
			fileName += "android";
		else if(SystemInfo.operatingSystem.Contains("Mac"))
			fileName += "ios";
		else if(SystemInfo.operatingSystem.Contains("Windows"))
			fileName += "win";

		fileName += "Bundle" + (testBundle?"Dev.zip":".zip"); //Controlla se è in modalità sviluppatore

		return fileName;
	}

	public static string CreatePathName(string storyId){

		//return Application.persistentDataPath + "/dir/" + storyId;//Sostituire
        return Application.persistentDataPath + "/dir/" + storyId;

	}

	public static void downloadStory(string storyId, GameObject progBar){

		Debug.Log(storyId);

		string zipFile = createZipFileName();

		FileRequest.DownloadFile3(system, "tada-" + storyId, zipFile, delegate (float progressDownload) {

            
			progBar.GetComponent<ProgressBar>().advancement(progressDownload);
            //Debug.Log(progressDownload);

        }, delegate {//Delegate On Complete DownloadFile

            Debug.Log("Download complete");


			// Mandare messaggio al SuperSystem
			system.UnzipStory(zipFile, storyId);

        }, delegate (string error, string errortext) {//Delegate OnError Download File
			system.ErrorWrapper(errortext);
			Debug.Log(error + " - " + errortext);
			//**** Mandare al SuperSystem notifica di errore */

        });  //Fine DownloadFile
	}

	public static void DownloadShelf(string storyId, string shelfCover){
		FileRequest.DownloadFile3(system, "tada-" + storyId, shelfCover, delegate {
        }, delegate {
            Debug.Log("Download success");
			system.ShelfDownloaded();
		}, delegate(string error, string errortext) {
			system.ErrorWrapper(errortext);
			Debug.Log(error + " - " + errortext);
        });
	}



	public static void unZipStory(string path, string storyId, GameObject progBar){

		MioZip.NewUnzip(system, (path), (CreatePathName(storyId)),
							delegate (float progressUnzip)//Delegate OnProgress NewUnzip
							{

								//progressDownload = p; // Definire prima SuperSystem
								progBar.GetComponent<ProgressBar>().advancement(progressUnzip);
								//Debug.Log(progressDownload);

							}, delegate (string s)//Delegate OnComplete NewUnzip;
							{
								Debug.Log("Unzipping complete");
								Debug.Log(path);
								File.Delete(path);

								progBar.SetActive(false); //disattiva la progress Bar quando ha finito
								system.panelCantTouch.SetActive(false);

								//Inviare notifica di unzip al SuperSystem
								system.ChangeStoryStatus(system.details.GetComponent<StoryDetailPanel>().getActiveToken());

							});
			//Fine NewUnzip*/

	}

    //Caricamento texture
	public static Texture2D LoadTexture(string fileName)
    {

        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails

        Texture2D Tex2D;
        byte[] FileData;
		string FilePath = Application.persistentDataPath + "/" + fileName;


        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);
            if (Tex2D.LoadImage(FileData))
                return Tex2D;
        }
        return null;
    }

	public static Sprite LoadSprite(string filename){
		
		Texture2D image = FileHandler.LoadTexture(filename);
        return Sprite.Create(image, new Rect(0, 0, image.width, image.height), Vector2.zero);

	}



}
