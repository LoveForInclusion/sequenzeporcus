using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public class saltaSceneScript : MonoBehaviour {


//	public GameObject canvas;


//	public List<Sprite> screenEasy = new List<Sprite>();
//	public List<Sprite> screenAdvanced = new List<Sprite>();
//	public List<Sprite> screen;

//	public Image destra;
//	public Image sinistra;
//	public Image central;

//	public int numeroDestra;
//	public int numeroSinistra;
//	public int numeroCentro;

//	int page;

//	int numero = 0;

//	public void saltaSx(){
		
//		gameObject.GetComponent<ScenaController> ().reg.currentScene = numero;
//		gameObject.GetComponent<ScenaController> ().reg.SaltaScena ();
//	}

//	public void saltaCr(){
//		gameObject.GetComponent<ScenaController> ().reg.currentScene = numero+1;
//		gameObject.GetComponent<ScenaController> ().reg.SaltaScena ();
//	}

//	public void saltaDx(){
//		gameObject.GetComponent<ScenaController> ().reg.currentScene = numero+2;
//		gameObject.GetComponent<ScenaController> ().reg.SaltaScena ();
//	}

//	public void generate(){
		
//		if (screen.Count > numero) {
//			sinistra.GetComponent<Image> ().sprite = screen [numero];
//			sinistra.gameObject.SetActive (true);
//			numeroSinistra = numero;
//		}
//		else
//			sinistra.gameObject.SetActive (false);
		
//		if (screen.Count > numero + 1) {
//			central.GetComponent<Image> ().sprite = screen [numero + 1];
//			central.gameObject.SetActive (true);
//			numeroCentro = numero + 1;
//		}
//		else
//			central.gameObject.SetActive (false);
		
//		if (screen.Count > numero + 2) {
//			destra.GetComponent<Image> ().sprite = screen [numero + 2];
//			destra.gameObject.SetActive (true);
//			numeroDestra = numero + 2;
//		}
//		else
//			destra.gameObject.SetActive (false);
		
//	}

//	void Start () {
//		resetGenerate ();
//	}

//	public void resetGenerate(){
//		numero = 0;
//		OpzioniAudio op = new OpzioniAudio ();
//		op.caricaOpzioniAudio ();
//		if (op.option.tipoLettura == 1)
//			screen = screenAdvanced;
//		else
//			screen = screenEasy;
//		generate ();
//	}

//	public void next(){
//		numero += 3;
//		if (screen.Count <= numero)
//			numero = 0;
//		generate ();
//	}


//	public void prev(){
//		numero -= 3;
//		if (numero < 0) {
//			page = Mathf.CeilToInt(screen.Count / 3);
//			if (screen.Count % 3 > 0)
//				page++;
//			page--;
//			numero = page * 3;
//		}
//		generate ();
//	}
//}
public class saltaSceneScript : MonoBehaviour
{


    public GameObject canvas;
    public GameObject sceneButton;
    public GameObject scrollContent;
	public GameObject modal;

    public List<Sprite> screenEasy = new List<Sprite>();
    public List<Sprite> screenAdvanced = new List<Sprite>();
    public List<Sprite> screen;

	public void salta()
    {
        string s = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.name;
        gameObject.GetComponent<ScenaController>().reg.currentScene = int.Parse(s);
        modal.SetActive(true);
    }

    public void confirm()
    {
        modal.SetActive(false);
        gameObject.GetComponent<ScenaController>().reg.SaltaScena();
    }

    public void generate()
    {
		scrollContent.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.UpperCenter;
        for (int i = 0; i < screen.Count; i++)
        {
            Transform t = Instantiate(sceneButton).transform;
			t.GetComponent<RectTransform>().sizeDelta =  new Vector2(Screen.currentResolution.width-(Screen.currentResolution.width/1.64f), Screen.currentResolution.height - (Screen.currentResolution.height / 1.64f));
			t.SetParent(scrollContent.transform);
			t.name = (""+i);
            t.GetComponent<Image>().sprite = screen[i];
            t.GetComponent<Button>().onClick.AddListener(salta);
            t.gameObject.SetActive(true);
        }

    }

    void Start()
    {
        resetGenerate();
    }

    public void resetGenerate()
    {   
		if(scrollContent.transform.childCount == screen.Count){
			Debug.Log("Cancello figli.");
			int c = scrollContent.transform.childCount;
			Debug.Log(c);
			for (int i = 0; i < c; i++){
				Destroy(scrollContent.transform.GetChild(i).gameObject);
			}
		}
        OpzioniAudio op = new OpzioniAudio();
        op.caricaOpzioniAudio();
        if (op.option.tipoLettura == 1)
            screen = screenAdvanced;
        else
            screen = screenEasy;
        generate();
    }
}
