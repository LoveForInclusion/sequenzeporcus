using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryToken : MonoBehaviour {

	//Instance Variables
	private TadaSuperSystem system;
	private string storyId;
	private int status;
	private string shelfCover;
	private Image overlay;
	public Sprite[] overlays;

	public IEnumerator SetupToken(TadaSuperSystem system, string storyId, int status, string shelfCover){
		this.system = system;
		this.storyId = storyId;
		this.status = status;
		this.shelfCover = shelfCover;
		this.overlay = this.transform.Find("Overlay").GetComponent<Image>();

		this.transform.GetComponent<Image>().sprite = FileHandler.LoadSprite(shelfCover);

		//Whether its status
		this.overlay.sprite = this.overlays[status+1];
		this.overlay.color = Color.white;

		yield return null;
		Debug.Log("Token Inizializzato con system, id: " + storyId + ", status: " + status + ", e cover: " + shelfCover + ".");

	}

	public void ChangeStatus(int newStatus){
		this.status = newStatus;
		this.overlay.sprite = this.overlays[status + 1];
		Debug.Log("Cambio status a " + this.status + "\n Nome: " + this.storyId);
	}

	public void CheckToken(){
		//this.system.StoryListener(this.storyId, this.status);
	}
	public string getStoryId(){
		return storyId;
	}
	public int getStatus()
    {
		return this.status;
    }

}
