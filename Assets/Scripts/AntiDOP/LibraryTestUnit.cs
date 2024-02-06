using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RESTClient.Request;
using Model;
using NetworkModel;

public class LibraryTestUnit : MonoBehaviour {

	//public GameObject token, content, details;
	//private List<StoryToken> list;
	//int i = 0;

	//// Use this for initialization
	//void Start () {
	//	list = new List<StoryToken>();
	//	//5a7c69bac85a450007226ed6
	//	testToken(token, "5a7c69bac85a450007226ed6", 1, "shelfLeone.png");
	//	testToken(token, "5a83ffedc85a450007226ed7", 0, "cattivissimoGroem.png");
	//	testToken(token, "5a84056e6d840e00075d6fc2", 1, "iTrePorcellini.png");
	//}
	
	//// Update is called once per frame
	//void Update () {
		
	//}

	//public void testToken(GameObject card, string storyId, int status, string shelfCover){
	//	GameObject gameO = Instantiate(card, content.transform);
	//	StartCoroutine(gameO.GetComponent<StoryToken>().SetupToken(new TadaSuperSystem(), storyId, status, shelfCover));
	//	gameO.name = i+"";
	//	gameO.GetComponent<Button>().onClick.AddListener(showDetails);
	//	list.Add(gameObject.GetComponent<StoryToken>());
	//	i++;
	//}

	//public void changeStatus(){
	//	list.ToArray()[int.Parse(EventSystem.current.currentSelectedGameObject.name)].ChangeStatus(2);
	//}

	//public void showDetails(){

	//	StoryRequest.GetStoryById(this, EventSystem.current.currentSelectedGameObject.transform.GetComponent<StoryToken>().getStoryId(), 
	//	    delegate(NetworkStory s){

	//		Debug.Log(EventSystem.current.currentSelectedGameObject.transform.GetComponent<StoryToken>().getStoryId());
	//			details.GetComponent<StoryDetailPanel>().SetupPanel(NetworkTranslator.translateStory(s));

	//	},
	//	    delegate(string st, string st2){

	//		Debug.Log("Error: " + st + " - " + st2);

	//	});
		
	//}


}
