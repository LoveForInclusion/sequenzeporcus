using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NewDropController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerDownHandler, IPointerUpHandler {

	Image img;
	Vector3 trans;
	bool attached;
	GameObject other;

    private bool zoomFlag;
	
    private GameObject giusto; //Contiene l'immagine "Giusto"
    private Vector2 startMousePos = Vector2.zero;//Vettore a tempo BeginDrag
    private bool startedDrag = false;//Ho cominciato a dragare oltre la soglia di zoom?
    void Awake(){
        giusto = GameObject.Find("Giusto");
    }
	void Start () {
	
		img = GetComponent<Image>();
		//trans = new Vector3(img.GetComponent<RectTransform>().position.x, img.GetComponent<RectTransform>().position.y, img.GetComponent<RectTransform>().position.z);
		
        giusto.SetActive(false);
	}
	
	void Update () {
		
	}

	public void OnPointerDown(PointerEventData eventData){
		trans = new Vector3(img.GetComponent<RectTransform>().position.x, img.GetComponent<RectTransform>().position.y, img.GetComponent<RectTransform>().position.z);
		NewGameController.imageZoom.sprite = img.sprite;
		NewGameController.ChangeAphaColor(NewGameController.imageZoom, img.color.a);
		//NewGameController.imageZoom.gameObject.SetActive(true);
        zoomFlag = true;
        StartCoroutine("ActiveZoom");
		
	}

	public void OnPointerUp(PointerEventData eventData){
        zoomFlag = false;
		NewGameController.imageZoom.gameObject.SetActive(false);
	}

	public void OnBeginDrag(PointerEventData eventData){
		startMousePos = eventData.position;                                             //Imposto posizione di inizio drag
        //Debug.Log("Start position = " + startMousePos);
	}

	public void OnDrag(PointerEventData eventData){
        //Debug.Log("Spostamento = " + (eventData.position-startMousePos).magnitude);
        if((eventData.position-startMousePos).magnitude >= 80 || startedDrag){          //Controllo se sono oltre la soglia o se sto già in drag
            NewGameController.ChangeAphaColor(NewGameController.imageZoom, 0f);
            zoomFlag = false;
            NewGameController.imageZoom.gameObject.SetActive(false);
            img.GetComponent<RectTransform>().position = eventData.position;
            startedDrag = true;
        }
	}

	public void OnEndDrag(PointerEventData enventData){
		img.GetComponent<RectTransform>().position = trans;
        startedDrag = false;                                                            //Finito il drag
	}

    public void OnDrop(PointerEventData eventData)
    {

        if (attached)
        {           // Se l'immagine collide con una casella
            if (other.GetComponentInChildren<Text>().text.Equals(img.sprite.name) /*|| img.gameObject.tag.Equals("FinaleAlternativo"*/)
            {   // Se inserisco l'immagine nella casella giusta
                // fissa l'immagine nella casella
                other.GetComponent<Image>().sprite = img.sprite;
                int t = int.Parse(other.GetComponentInChildren<Text>().text);
                other.GetComponentInChildren<Text>().text = "";

                NewGameController.ChangeAphaColor(img, 0f);
                img.GetComponent<RectTransform>().position = trans;

                //////

                bool flag = true;
                foreach (GameObject coll in NewGameController.currentCollision)
                {
                    if (coll.GetComponentInChildren<Text>().text != "")
                    {
                        flag = false;
                    }
                }

                if (flag)
                {

                    if (Option.modalita == "elaborata" && Option.difficolta.Equals("difficile"))
                    {
					/* 
                        if (Option.difficolta.Equals("facile"))
                        {

                            if (t == NewGameController.finish)
                            {
                                ok.text = "Esatto!";
                                NewGameController.complete = true;
                                for (int i = 1; i < NewGameController.scelte.Length; i++)
                                {
                                    NewGameController.ChangeAphaColor(NewGameController.scelte[i], 0f);
                                }
                                //NewGameController.scelte[0].gameObject.SetActive(false);
                            }
                            else
                            {
                                ok.text = "Scegli tu il Finale!";
                                NewGameController.ScegliFinale();
                            }
                        }
					 */
                        //if (Option.difficolta.Equals("difficile"))
                        
                            if (t == NewGameController.finish)
                            {
                                NewGameController.restarFlag = true;
                                StartCoroutine("ActiveGiusto");
                                GameObject.Find("Indietro3").SetActive(false);

                                NewGameController.complete = true;
                                //NewGameController.scelte[0].gameObject.SetActive(false);
                                for (int i = 1; i < NewGameController.scelte.Length; i++)
                                {
                                    NewGameController.ChangeAphaColor(NewGameController.scelte[i], 0f);
                                }
                            }
                            else
                            {
                                NewGameController.NextScelta(4);
                            }
                        }

                    
                    else
                    {
                        NewGameController.restarFlag = true;
                        StartCoroutine("ActiveGiusto");
                        GameObject.Find("Indietro3").SetActive(false);
                        //NewGameController.scelte[0].gameObject.SetActive(false);
                        for (int i = 1; i < NewGameController.scelte.Length; i++)
                        {
                            NewGameController.ChangeAphaColor(NewGameController.scelte[i], 0f);
                        }
                        NewGameController.complete = true;
                    }

                }

            }
            else
            {
                img.GetComponent<RectTransform>().position = trans;             // se non è la casella giusa torna l'immagine nella posizione iniziale
            }
        }
    }

	void OnTriggerStay2D(Collider2D collider){
		if (collider.gameObject.tag == "Casella"){
			attached = true;
			other = collider.gameObject;
		}	
	}

	void OnTriggerExit2D(Collider2D collider){
		if (collider.gameObject.tag == "Casella"){
			attached = false;
		}	
	}

    private IEnumerator ActiveGiusto(){

        giusto.SetActive(true);
        giusto.GetComponent<AudioSource>().Play();
        yield return new WaitUntil( () => !giusto.GetComponent<AudioSource>().isPlaying);
        yield return new WaitForSeconds(0.3f);
        giusto.SetActive(false);
    }

    private IEnumerator ActiveZoom(){

        yield return new WaitForSeconds(0.65f);
        if(!NewGameController.imageZoom.gameObject.activeSelf && zoomFlag)
            NewGameController.imageZoom.gameObject.SetActive(true);
    }

}
