using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// eyJhbGciOiJIUzUxMiJ9.eyJyb2xlIjoiUk9MRV9VTklUWSJ9.x2W1_eEMnpM0L-i7xppW9S_s5u0t64f6aBYgC_A_ChGNQk-z0eupwohQgIqVaOKndWldt80j5ENsgKc78_Apjw
namespace Model 
{
	[Serializable]
    public class Transaction
    {
        public string id;
		public string storyId;
		public string userId;
		public string type;
		public string origin;
		public string transactionId;
		public float originalPrice;
		public float discount;
		public float finalPrice;
		public long transactionDate;
		public long refundDate;
		public bool gift;
		public bool refunded;
       
		public Transaction (string id, string storyId, string userId, string type, string origin, string transactionId, float originalPrice, float discount, float finalPrice, long transactionDate, long refundDate, bool gift, bool refunded)
    	{
    		this.id = id;
    		this.storyId = storyId;
    		this.userId = userId;
    		this.type = type;
    		this.origin = origin;
			this.transactionId = transactionId;
    		this.originalPrice = originalPrice;
    		this.discount = discount;
    		this.finalPrice = finalPrice;
    		this.transactionDate = transactionDate;
    		this.refundDate = refundDate;
    		this.gift = gift;
    		this.refunded = refunded;
    	}
			
	}

	[Serializable]
	public class Transactions
	{
		public List<Transaction> transactions;

		public Transactions(List<Transaction> transactions)
		{
			this.transactions = transactions;
		}
	}
}
