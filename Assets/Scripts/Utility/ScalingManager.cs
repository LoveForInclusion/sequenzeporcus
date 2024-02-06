using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScalingManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
		if (!this.gameObject.name.Equals("CanvasComprensionGame(Clone)"))
		{
			this.transform.GetComponent<CanvasScaler>().referenceResolution = new Vector2(Screen.width, Screen.height);
		}
		Debug.Log(this.transform.name);

		if (this.gameObject.name.Equals("CanvasScelta(Clone)"))
        {
			StartCoroutine(delayz());
        }

    }

	IEnumerator delayz(){

		yield return new WaitForSeconds(0.1f);

		if (PlayerPrefs.GetInt("Back", 0) == 1)
        {
            this.transform.Find("PanelGiochi").gameObject.SetActive(true);
            PlayerPrefs.SetInt("Back", 0);
        }

	}

}