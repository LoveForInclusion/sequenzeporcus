using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using RESTClient.Request;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class StoryHandler{

	private Stories storiesLocal = null;
	private TadaSuperSystem system;

	private List<Story> storiesUpdatable = null;

	// Constructors
	public StoryHandler(){}

	public StoryHandler(TadaSuperSystem system){
		//this.storiesLocal = new Stories(new List<Story>());
		this.system = system;
	}

	public void setUpSystem(TadaSuperSystem newSystem){

		this.system = newSystem;
	}

	// Getter and Setter
	public void setStoriesLocal(Stories newStories){
		this.storiesLocal = newStories;
	}

	public Stories GetStoriesLocal(){ return storiesLocal;}	// Ritorna la lista di storie salvate in locale

	public List<Story> GetStoriesUpdatable() { return storiesUpdatable;} // Ritorna la lista di storie da aggiornare

	public void AddStoryUpdatable(Story storyToUpdate){ //Aggiunge una storia alla lista di storie da aggiornare
		if (storiesUpdatable == null)
			storiesUpdatable = new List<Story>();
		storiesUpdatable.Add(storyToUpdate);
	}

	public void AddStoryLocal(Story story){ //Aggiunge una storia alla lista di storie locali
		storiesLocal.stories.Add(story);
	}


	// Verifica lo stato della storia 
	/*
	    valore -1: comingSoon
		valore 0 : purchasable
		valore 1 : downloadable
		valore 2 : readable
		valore 3 : updatable
	 */
	public int checkStory(string storyId){



		if (!FindStory(storyId).comingSoon)
		{
			if (system.GetTransactionHandler().checkTransaction(storyId) || (int)Mathf.Round(FindStory(storyId).price) == 0)
			{
				if (FileHandler.checkStoryDirectory(storyId)){
					if (CheckStoryUpdate(storyId)){
						return 3;
					}
					else 
						return 2;
				}
				else
					return 1;
			}
			else
				return 0;
		}
		else
			return -1;
		
	}

	// Cerca una storia in storiesLocal
	public Story FindStory(string storyId){

		for(int i=0; i < storiesLocal.stories.Count; i++){
			if (storiesLocal.stories.ToArray()[i].id.Equals(storyId))
				return storiesLocal.stories.ToArray()[i];
		}

		return null;
	}

    //Cerca storia tra aggiornabili
	public Story FindStoryUpdatable(string storyId)
    {

		for (int i = 0; i < storiesUpdatable.Count; i++)
        {
			if (storiesUpdatable.ToArray()[i].id.Equals(storyId))
				return storiesUpdatable.ToArray()[i];
        }

        return null;
    }

    //Pulisci storia aggiornata
	public void RemoveStoryUpdatable(string storyId)
    {

        for (int i = 0; i < storiesUpdatable.Count; i++)
        {
            if (storiesUpdatable.ToArray()[i].id.Equals(storyId))
            {
                storiesUpdatable.RemoveAt(i);
                i--;
            }
        }

    }

    //Rimpiazza storia con aggiornamento.
	public void ReplaceStory(Story story){
		for (int i = 0; i < storiesLocal.stories.Count; i++)
        {
			if (storiesLocal.stories.ToArray()[i].id.Equals(story.id))
			{
				storiesLocal.stories.RemoveAt(i);
				storiesLocal.stories.Insert(i, story);
				RemoveStoryUpdatable(story.id);
			}
        }
	}

    //Aggiungere update story in cui viene sostituito la storia updatable nel local
	public void UpdateStory(string storyId){
		ReplaceStory(FindStoryUpdatable(storyId));
		FileHandler.salvaFile(storiesLocal, true);
	}

	public bool FindStoryBool(string storyId){

		if(this.FindStory(storyId) != null)
			return true;

		return false;
	}

	public bool CheckStoryUpdate(string storyId){
		if (storiesUpdatable != null){
			for(int i=0; i < storiesUpdatable.Count; i++){
				if (storyId.Equals(storiesUpdatable.ToArray()[i].id))
					return true;
			}
		}

		return false;
	}

}
