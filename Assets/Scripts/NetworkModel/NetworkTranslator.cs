using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

namespace NetworkModel 
{
	public class NetworkTranslator : MonoBehaviour {

		public static Transactions translateTransactions(NetworkTransactions nt){
			List<Transaction> tr = new List<Transaction>();
			foreach (NetworkTransaction t in nt.transactions)
			{
				tr.Add(new Transaction(t._id, t.storyId, t.userId, t.type, t.origin, t.transactionId, t.originalPrice, t.discount, t.finalPrice, t.transactionDate, t.refundDate, t.isGift, t.isRefunded ));
			}
			return new Transactions(tr);
		}

		public static Stories translateStories(NetworkStories ns){
			//Debug.Log(ns.stories[0]._id);
			List<Story> ls = new List<Story>();
			foreach (NetworkStory s in ns.stories)
			{
				List<Addon> la = new List<Addon>();
				foreach (NetworkAddon na in s.addons)
				{
					la.Add(new Addon(na._id, na.storyId, na.created, na.lastUpdate, na.webserverUrl, na.Category, na.Price, na.Age, na.ComingSoon, na.ShelfCover, na.Data, na.Title));
				}
				//Debug.Log("Local: " + s._id);
				ls.Add(new Story(s._id, s.androidData, s.iosData, s.macData, s.winData, s.linuxData, la, s.created, s.lastUpdate, s.webserverUrl, s.activities, s.ComingSoon, s.Cover, s.ShelfCover, s.category, s.Price, s.Age, s.Author, s.Description, s.Title, s.isDownloaded));
			}

			Debug.Log("Local: " + ls[0].id);
			return new Stories(ls);

		}

		public static Story translateStory(NetworkStory s){
			List<Addon> la = new List<Addon>();
            foreach (NetworkAddon na in s.addons)
            {
                la.Add(new Addon(na._id, na.storyId, na.created, na.lastUpdate, na.webserverUrl, na.Category, na.Price, na.Age, na.ComingSoon, na.ShelfCover, na.Data, na.Title));
            }
            Debug.Log("Local: " + s._id);
			return new Story(s._id, s.androidData, s.iosData, s.macData, s.winData, s.linuxData, la, s.created, s.lastUpdate, s.webserverUrl, s.activities, s.ComingSoon, s.Cover, s.ShelfCover, s.category, s.Price, s.Age, s.Author, s.Description, s.Title, s.isDownloaded);
		}

		public static Codes translateCodes(NetworkCodes nc){
			List<Code> ls = new List<Code>();
			foreach (NetworkCode c in nc.codes)
			{
				ls.Add(new Code(c._id, c.storyId, c.addonId, c.userId, c.code, c.type, c.redeemed, c.purchaseData, c.redeemData, c.redeemedBy, c.Price));
			}
			return new Codes(ls);
		}

		public static Transaction translateTransaction(NetworkTransaction t){
			
			return new Transaction(t._id, t.storyId, t.userId, t.type, t.origin, t.transactionId, t.originalPrice, t.discount, t.finalPrice, t.transactionDate, t.refundDate, t.isGift, t.isRefunded);

		}

		public static NetworkTransaction translateNetworkTransaction(Transaction t){
			
			return new NetworkTransaction(t.id, t.storyId, t.userId, t.type, t.origin, t.transactionId, t.originalPrice, t.discount, t.finalPrice, t.transactionDate, t.refundDate, t.gift, t.refunded);

		}

        
		public static User translateNetworkUser(User u)
        {
			return new User(u._id, u.email, u.password, u.firstName, u.lastName, u.role);
        }
		
	}
}