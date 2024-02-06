using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SequenceController : MonoBehaviour {


	private string prefabName;

	public GameObject contentDifficile;
	public GameObject contentFacile;

	public GameObject contentElaborata;
	public ScrollRect scroll;

		void Start () {

/*
			if(Option.modalita.Equals("elaborata")){
				scroll.content = contentElaborata.GetComponent<RectTransform>();
			}

			if (Option.difficolta.Equals("difficile")){
				scroll.content = contentDifficile.GetComponent<RectTransform>();
				contentFacile.SetActive(false);
			}
			else{
				scroll.content = contentFacile.GetComponent<RectTransform>();
				contentDifficile.SetActive(false);
			}
			

			if (Option.modalita.Equals("semplice")){
				if(Option.difficolta.Equals("facile"))
					prefabName = "seq_semplice_facile";
				else
					prefabName = "seq_semplice_difficile";
			}
			else{
				if(Option.difficolta.Equals("facile"))
					prefabName = "seq_elaborata_facile";
				else
					prefabName = "seq_elaborata_difficile";
			}



			//StartCoroutine("loadCover");
*/
	}

	IEnumerator loadCover(){

		AssetBundleRequest assetLoadRequest = null;
		AssetBundleCreateRequest bundleLoadRequest = null;
	

		bundleLoadRequest = AssetBundle.LoadFromFileAsync(Option.Path + "/SortSequence/" + Option.modalita + "/" + Option.difficolta + "/" + prefabName);
		yield return bundleLoadRequest;
       

		var myLoadedAssetBundle = bundleLoadRequest.assetBundle;

		if (myLoadedAssetBundle == null){
			Debug.Log("Failed to load AssetBundle!");
			yield break;
		}

		assetLoadRequest = myLoadedAssetBundle.LoadAssetAsync(prefabName);
		yield return assetLoadRequest;

		GameObject panelSeq = assetLoadRequest.asset as GameObject;
		panelSeq = Instantiate(panelSeq, this.transform);
		panelSeq.SetActive(true);

		myLoadedAssetBundle.Unload(false);
		assetLoadRequest = null;
		bundleLoadRequest = null;

		Caching.ClearCache();
		yield break;
	}

	
}
