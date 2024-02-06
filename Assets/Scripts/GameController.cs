using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	
	
	//private GameObject[] lista;
	public GameObject caselle;
	public GameObject scelta;
	GameObject[] scelte;
	public static List<GameObject> currentCollision;
	public static Image imageZoom;


	void Start () {
		/*
		lista = GameObject.FindGameObjectsWithTag("Image");
		Quicksort(lista, 0, lista.Length - 1);
		*/
		scelte = GameObject.FindGameObjectsWithTag("Scelta");
		Quicksort(scelte, 0, scelte.Length - 1);
		currentCollision = new List<GameObject>();
		imageZoom = GameObject.Find("ImageZoom").GetComponent<Image>();

		ScelteStart();
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Quicksort(GameObject[] elements, int left, int right)
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

	//le immagini da scegliere conterranno una giusta e tre errate disposte in ordine random
	public static void ScelteRandom(int scena){

		int j = Random.Range(0, 4);
		int k = -1;
	
		for (int i=0; i < 4; i++){
			Image tempImg = GameObject.Find("Scelta" + i).GetComponent<Image>();
			if(i == j){
				tempImg.sprite = GameObject.Find("" + scena).GetComponent<SpriteRenderer>().sprite;
			}
			else{
				do{
					k = Random.Range(1, 18);
				} while(k == scena);
				tempImg.sprite = GameObject.Find("" + k).GetComponent<SpriteRenderer>().sprite;
			}
			ChangeAphaColor(tempImg, 1);
		} 
	}


	// Inizializza il gioco con le prime quattro scene disposte in ordine random
	public static void ScelteStart(){

		
		ArrayList arr = new ArrayList();

		for ( int i=0; i < 4; i++){
			
			int k = -1;
			do{
				k = Random.Range(1, 5);
			} while ( ControllaArray(arr, k));
			arr.Add(k);

			GameObject.Find("Scelta" + i).GetComponent<Image>().sprite = GameObject.Find("" + k).GetComponent<SpriteRenderer>().sprite;
		}
	}

	// controlla se un elemento è presente nell'array
	public static bool ControllaArray(ArrayList a, int k){

		foreach ( int element in a){
			if( element == k)
				return true;
		}

		return false;
	}

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

	public static void ChangeAphaColor(Image img, float f){
		var tempColor = img.color;
		tempColor.a = f;
		img.color = tempColor;
	}

	

}
