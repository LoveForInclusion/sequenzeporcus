using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using RESTClient.Request;

public class TransactionHandler{

    //Instance Variables
	private Transactions transactionsLocal = null;
	private TadaSuperSystem system;


	//Constructors
	public TransactionHandler(){}

	public TransactionHandler(TadaSuperSystem system) {
		//SuperSystem chiama caricaFile da FileHandler e carica le transitions
        
        this.transactionsLocal = new Transactions(new List<Transaction>());
		this.system = system;

	}


    //Getters & Setters
	public Transactions GetTransactions(){ return transactionsLocal; }

	public void SetTransactions(Transactions transactions){ this.transactionsLocal = transactions; }


    //System Methods
	public bool checkTransaction(string id){

		Transaction t = findTransaction(id);

		if (t == null) return false;

		return !t.gift && !t.refunded;

	}

	public void refundTransaction(string id){
		
		Transaction t = findTransaction(id);
		if (!t.gift && !t.refunded && !t.origin.Equals("web") && (t.origin.Equals(SystemInfo.operatingSystem)))
        {
            t.refunded = true;
            TransactionRequest.makeRefund(system, t,

			delegate (Transaction transaction)
			{
				Debug.Log("Refund completato");
				//Notifica l'avvenuto aggiornamento al supersystem
				system.ChangeStoryStatus(system.findToken(id));
			},
            delegate (string code, string error)
            {
                Debug.Log("La transazione non è stata rimborsata: " + code + "---" + error);
				//Notifica l'errore al supersystem
				system.ErrorWrapper(error);
            });
        }

	}

	public void createTransaction(Transaction t){
		TransactionRequest.createTransaction(system, t,
                        delegate (Transaction obj2)
                        {
                            Debug.Log(JsonUtility.ToJson(obj2));
							this.transactionsLocal.transactions.Add(obj2);
							FileHandler.salvaFile(this.transactionsLocal, false);
							//Notifica avvenuta transazione al supersystem
							system.ChangeStoryStatus(obj2.storyId);
                        },
                        delegate (string arg1, string arg2)
                        {
							Debug.Log(arg1 + " - " + arg2);
							//Notifica errore al supersystem;
							system.ErrorWrapper("Possono esserci problemi con la finalizzazione dell'acquisto, " +
			                                    "si consiglia di controllare la connessione e/o riavviare l'applicazione.\n" +
			                                    "Se il problema persiste contattare support@tadabook.it.");
                        }
                    );
	}


    //Utility Methods
	private Transaction findTransaction(string id){
		Debug.Log(transactionsLocal.transactions.Count);
		for (int i = 0; i < transactionsLocal.transactions.Count; i++)
        {
			Debug.Log("Check Transaction: " + transactionsLocal.transactions.ToArray()[i].storyId);
			if (transactionsLocal.transactions.ToArray()[i].storyId.Equals(id) && !transactionsLocal.transactions.ToArray()[i].refunded)
			{
				//Debug.Log("Trovata.");
				return transactionsLocal.transactions.ToArray()[i];
			}
        }
		//Debug.Log("Non Trovata.");
		return null;
	}

	public bool findTransactionBool(string id){
		for (int i = 0; i < transactionsLocal.transactions.Count; i++)
        {
			if (transactionsLocal.transactions.ToArray()[i].storyId.Equals(id))
				return true;
        }
		return false;
	}

}
