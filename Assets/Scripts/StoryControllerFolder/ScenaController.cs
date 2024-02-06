using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenaController: MonoBehaviour {

	[System.Serializable]
	public class Figure
	{
		public GameObject figura;
		public GameObject animazioneScenica;
		public string nomeAnimazioneScenica;
	}


	public RegistaStoria reg;

	public int numScena;
	public List<Figure> figure= new List<Figure>();

	public Transform spacer;

	float timer;
	void Start(){
		
		timer =0.05f;
		generaFigure ();

	}

	public void generaFigure(){
		List<GameObject> lista = new List<GameObject> ();

		for (int i = 0; i < figure.Count; i++) {
			GameObject figura = Instantiate (figure[i].figura)as GameObject;
			figura.GetComponent<FigureController> ().animazioneScenica = figure [i].animazioneScenica;
			figura.GetComponent<FigureController> ().nomeAnimazioneScenica = figure [i].nomeAnimazioneScenica;
			figura.GetComponent<FigureController> ().reg = this.reg;
			figura.GetComponent<FigureController> ().touch_limit = reg.opzioni.option.ripetizioni;
			figura.GetComponent<FigureController> ().mute = reg.opzioni.option.muto;
			if (reg.opzioni.option.personalizzata) {
				figura.GetComponent<FigureController> ().mute = true;
			}
			lista.Add (figura);
			figura.transform.SetParent (spacer, false);
		}
	}


	void Update(){
		if (timer <= 0.0f)
			spacer.GetComponent<GridLayoutGroup> ().enabled = false;
		else
			timer -= Time.deltaTime;
	}

	public void CallNextScene(){
		ResetFigure ();
		reg.NextScene ();
	}
	public void CallPreviusScene(){
		ResetFigure ();
		reg.PreviusScene ();
	}

	void ResetFigure(){
		if(spacer!=null)
		foreach (Transform child in spacer) {
			child.GetComponent<FigureController> ().ResetMyTouch ();
		}
	}

	public List<GameObject> getChild(){
		Transform parent = transform;
		var children = new List<GameObject> ();
		foreach (Transform child in parent)
			if(child.name.Equals("ButtonNext") || child.name.Equals("ButtonPrev"))
			children.Add (child.GetComponent<GameObject>());
		return children;
	}

	public void tornaHome(){
		reg.caricaSceltaMenu ();
	}

	public void tornaZero()
    {
		tornaHome();
    }

}