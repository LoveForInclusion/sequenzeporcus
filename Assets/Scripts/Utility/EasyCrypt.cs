using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyCrypt {

	public static string encrypt(string toEnc){
		if (toEnc.Equals(""))
			return "";
		string enc = "";

		char[] chars =  toEnc.ToCharArray();
        


		for(int i = 0; i<toEnc.Length; i++){

			chars[i] = (char)(chars[i] + (42) - i);

		}

		System.Text.StringBuilder sb = new System.Text.StringBuilder("", toEnc.Length);

		sb.Append(chars);

		//Debug.Log(sb.ToString());

		return sb.ToString();
	}

	public static string decrypt(string toDec){

		if (toDec.Equals(""))
            return "";

		char[] chars = toDec.ToCharArray();



        for (int i = 0; i < toDec.Length; i++)
        {

            chars[i] = (char)(chars[i] - (42) + i);

        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder("", toDec.Length);

        sb.Append(chars);

        //Debug.Log(sb.ToString());

        return sb.ToString();
	}

}
