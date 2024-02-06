using System;
using System.Collections.Generic;


// eyJhbGciOiJIUzUxMiJ9.eyJyb2xlIjoiUk9MRV9VTklUWSJ9.x2W1_eEMnpM0L-i7xppW9S_s5u0t64f6aBYgC_A_ChGNQk-z0eupwohQgIqVaOKndWldt80j5ENsgKc78_Apjw
namespace NetworkModel 
{
	[Serializable]
    public class NetworkTransaction
    {
        public string _id;
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
		public bool isGift;
		public bool isRefunded;
       
		public NetworkTransaction (string _id, string storyId, string userId, string type, string origin, string transactionId, float originalPrice, float discount, float finalPrice, long transactionDate, long refundDate, bool isGift, bool isRefunded)
    	{
    		this._id = _id;
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
    		this.isGift = isGift;
    		this.isRefunded = isRefunded;
    	}
			
	}

	[Serializable]
	public class NetworkTransactions
	{
		public List<NetworkTransaction> transactions;

		public NetworkTransactions(List<NetworkTransaction> transactions)
		{
			this.transactions = transactions;
		}
	}
}
