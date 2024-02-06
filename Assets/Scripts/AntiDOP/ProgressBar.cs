using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

    
	public Text percent;

	void Awake(){
		
	}
	
	void Start () {
		this.GetComponent<Slider>().value = 0f;
	}
	

	/***** Gestisce l'avanzamento della barra di caricamento ******/
	public void advancement(float progress){
		//this.value = 0;//Questo impedisce alla barra di aggiornarsi
		this.GetComponent<Slider>().value = progress * 100;
		percent.text = (Mathf.RoundToInt (progress * 100)) + "%";

	}
}
