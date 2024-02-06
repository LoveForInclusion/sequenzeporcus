using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DevModer : MonoBehaviour {

	public Transform devAlert;
	private int counter = 0;
	private bool devMode = false;
	private string tooltip;
	// Use this for initialization
	void Start () {
		tooltip = devAlert.Find("PanelParentalControl").transform.Find("Text").GetComponent<Text>().text;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setDevMode(){

		FileHandler.setTestBundle(!FileHandler.getTestBundle());
		devMode = !devMode;
		devAlert.gameObject.SetActive(false);
	}

	public void DevMode(){
		StopCoroutine(devTimer());
		counter++;
		if(counter >= 5){
			if(devMode){
				devAlert.Find("PanelParentalControl").transform.Find("Text").GetComponent<Text>().text = tooltip.Replace("abilitare", "disabilitare");
			} else {
				devAlert.Find("PanelParentalControl").transform.Find("Text").GetComponent<Text>().text = tooltip.Replace("disabilitare", "abilitare");
			}
			devAlert.gameObject.SetActive(true);
			counter = 0;
		} else {
			StartCoroutine(devTimer());
		}

	}

	IEnumerator devTimer(){
		yield return new WaitForSeconds(7);
		counter = 0;

	}

}
