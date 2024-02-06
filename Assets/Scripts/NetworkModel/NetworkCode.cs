using System;
using System.Collections.Generic;

namespace NetworkModel 
{
	[Serializable]
	public class NetworkCode
	{
		public string _id;
		public string storyId;
		public string addonId;
		public string userId;
		public string code;
		public string type;
		public bool redeemed;
		public long purchaseData;
		public long redeemData;
		public string redeemedBy;
		public float Price;

		public NetworkCode( string _id,
			 string storyId,
			 string addonId,
			 string userId,
			 string code,
			 string type,
			 bool redeemed,
			 long purchaseData,
			 long redeemData,
			 string redeemedBy,
			 float Price)
		{
			this._id = _id;
			this.storyId = storyId;
			this.addonId = addonId;
			this.userId = userId;
			this.code = code;
			this.type = type;
			this.redeemed = redeemed;
			this.purchaseData = purchaseData;
			this.redeemData = redeemData;
			this.redeemedBy = redeemedBy;
			this.Price = Price;
		}

	}

	[Serializable]
	public class NetworkCodes
	{
		public List<NetworkCode> codes;

		public NetworkCodes(List<NetworkCode> codes)
		{
			this.codes = codes;
		}
	}

}
