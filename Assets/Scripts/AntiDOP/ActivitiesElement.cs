using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;

public class ActivitiesElement : MonoBehaviour
{
    //Components
	[Header("Componenti Element")]
	public Image mini;
	public Text title;
	public Button descButton;
	public Sprite[] icons;

	public void Initialize(){
		this.title = this.transform.Find("Titolo").GetComponent<Text>();
		this.mini = this.transform.Find("ImageStoryMiniature").GetComponent<Image>();
		this.descButton = this.transform.Find("Descrizione").GetComponent<Button>();
	}

	public void SetupElement(Activities activity, GameObject gameInfo, Text titleG, Text descG)
	{
        this.title.text = activity.name;

		if (activity.type.Equals("svago"))
			mini.sprite = icons[0];
		else if (activity.type.Equals("cognitivo"))
			mini.sprite = icons[1];

		this.descButton.GetComponent<Button>().onClick.AddListener(
			delegate () {
			//Debug.Log("Descrizione?");
			titleG.text = activity.name;
			descG.text = activity.description;
			gameInfo.transform.gameObject.SetActive(true);
        });

	}

}
