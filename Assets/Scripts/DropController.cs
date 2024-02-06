using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class DropController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerDownHandler, IPointerUpHandler {

	Image img;
	Vector3 trans;
	bool attached;
	GameObject other;			//Casella con cui collide
	Text ok;
	Scrollbar barScroll;
	public static int casellaVuota;
	static float maxScrollValue;
	static int indexNextCasella;


	void Start () {
		
		img = GetComponent<Image>();
		trans = new Vector3(img.GetComponent<RectTransform>().position.x, img.GetComponent<RectTransform>().position.y, img.GetComponent<RectTransform>().position.z);
		attached = false;
		ok = GameObject.Find("OK test").GetComponent<Text>();
		ok.text = "";
		barScroll = GameObject.Find("Scrollbar").GetComponent<Scrollbar>();
		
		
	}
	
	
	void Update () {
		
		if (barScroll.value > maxScrollValue){
			barScroll.value = maxScrollValue;
		}
	}

	
	public void OnPointerDown(PointerEventData eventData){
		GameController.imageZoom.sprite = img.sprite;
		GameController.ChangeAphaColor(GameController.imageZoom, 0.85f);
		GameController.imageZoom.gameObject.SetActive(true);
	}

	public void OnPointerUp(PointerEventData eventData){
		GameController.imageZoom.gameObject.SetActive(false);
	}

	public void OnBeginDrag(PointerEventData eventData){
		GameController.ChangeAphaColor(GameController.imageZoom, 0f);		
	}

	public void OnDrag(PointerEventData eventData){

		img.GetComponent<RectTransform>().position = eventData.position;
	}

	public void OnEndDrag(PointerEventData enventData){
		img.GetComponent<RectTransform>().position = trans;	
	}

	

	public void OnDrop(PointerEventData eventData){
		Debug.Log(attached);
		if(attached){ 																// Se l'immagine collide con una casella
			if(other.GetComponentInChildren<Text>().text.Equals(img.sprite.name)){	// Se inserisco l'immagine nella casella giusta
																					// fissa l'immagine nella casella
				other.GetComponent<Image>().sprite = img.sprite;
				other.GetComponentInChildren<Text>().text = "";
				//other.GetComponent<BoxCollider2D>().enabled = false;
				img.sprite = null;
				var tempColor = img.color;
				tempColor.a = 0f;
				img.color = tempColor;
				img.GetComponent<RectTransform>().position = trans;

				//////
				
				bool flag = true;
				foreach(GameObject coll in GameController.currentCollision){
					
					/*if( coll.GetComponent<Image>().sprite == null){
						flag = false;
					}*/

					//ok.text = "Entra";

					if (coll.GetComponentInChildren<Text>().text != ""){
						flag = false;
						//ok.text = "Entra";
					}
				}

				if(flag){
					
					//int indexNextCasella = int.Parse(other.GetComponent<Image>().sprite.name) + 1;
					int indexNextCasella = trovaMassimo(GameController.currentCollision) + 1;

				

					Debug.Log(indexNextCasella);
					

					if ( casellaVuota == 17){
						ok.text = "ESATTO!";
					} 
					else{
						//Debug.Log("prima: " + indexNextCasella);
						GameObject.Find("Image (" + indexNextCasella + ")").GetComponent<BoxCollider2D>().enabled = true;
						Debug.Log("Entra");
						StartCoroutine("ShiftCasella");
						//Debug.Log("dopo: " + trovaMassimo(GameController.currentCollision));
						//int indexNextCasella = trovaMassimo(GameController.currentCollision) + 1;
					}
					
				}


			}
			else{
				img.GetComponent<RectTransform>().position = trans;				// se non è la casella giusa torna l'immagine nella posizione iniziale
			}
		}		
	}

	IEnumerator ShiftCasella(){
		maxScrollValue = barScroll.value + 0.077f;
		while(barScroll.value < maxScrollValue ){
			barScroll.value = Mathf.Lerp(barScroll.value, maxScrollValue, Time.deltaTime * 10);
			if (barScroll.value >= (maxScrollValue - 0.003)){
				barScroll.value = Mathf.Lerp(barScroll.value, maxScrollValue, 1f);
			}
			yield return new WaitForEndOfFrame();
		}

		yield break;
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

	
	int trovaMassimo(List<GameObject> array){
		int max = 0;
		foreach(GameObject element in array){
			int e = int.Parse(element.GetComponent<Image>().sprite.name);
			if (e > max){
				max = e;
			}
		}

		return max;
	}	

}
