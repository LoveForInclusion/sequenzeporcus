using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;
using UnityEngine.EventSystems;

public class StoryDetailPanel : MonoBehaviour {

	//Story Comps
	[Header("Componenti StoryInfo")]
	public Text title;
	public Text specs1;
	public Text specs2;
	public Text description;
	public Image mini;
	public Button button;
	//Activity Comps
	[Header("Componenti ActivityInfo")]
	public Text titleA;
	public Transform scroll;
	public Transform scrollElement;
	[Header("Componenti GameInfo")]
	public Transform gameInfo;
	public Text titleG;
	public Text descG;
	[Header("Bottoni per status")]
	public Sprite[] buttons;

	//Instance Variables
	Story story;
	int status;

	GameObject token;


	public void SetupPanel(TadaSuperSystem system, Story story, int status){
		this.story = story;
		this.status = status;
		this.token = EventSystem.current.currentSelectedGameObject;

        //Setup StoryInfo
		this.title.text = story.title;
		this.specs1.text = story.price > 0 ? story.price+"€" : "Download Gratuito";
		this.specs2.text = story.age;
		this.description.text = story.description;
		this.mini.sprite = FileHandler.LoadSprite(story.shelfCover);
		this.button.GetComponent<Image>().sprite = buttons[status];
		this.button.onClick.RemoveListener(system.StoryListener);
		this.button.onClick.AddListener(system.StoryListener);

		//Setup ActivitiesInfo
		this.titleA.text = story.title;

		cleanScroll();//Cancello eventuali card

		for (int i = 0; i < story.activities.Count; i++)
		{
			GameObject obj = Instantiate(scrollElement.gameObject, scroll);
			//Setuppare l'element
			obj.GetComponent<ActivitiesElement>().Initialize();
			obj.GetComponent<ActivitiesElement>().SetupElement(story.activities.ToArray()[i], gameInfo.gameObject, titleG, descG);

		}

		this.gameObject.SetActive(true);
	}

    //Utility Methods
	private void cleanScroll(){
		if(scroll.childCount>0){
			for (int i = 0; i < scroll.childCount; i++){
				Debug.Log("Elimino: " + scroll.GetChild(i).GetComponent<ActivitiesElement>().title.text);
				Destroy(scroll.GetChild(i).gameObject);
			}
		}
	}

    //Getters
	public Story GetStory(){
		return story;
	}

	public GameObject getActiveToken(){
		return token;
	}

}
