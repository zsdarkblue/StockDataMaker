using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDataEditor;
public class QueryItem : MonoBehaviour {

	public UILabel Code;
	public UILabel Name;
	public UILabel Min;
	public UILabel Lot;
	public UILabel Repeat;

	public GameObject HideRoot;

	// Use this for initialization
	void Start () {
		
	}


	public void Refresh(GDEStockData data,int index)
	{
		bool isActive = (null != data);
		HideRoot.SetActive (isActive);

		if (!isActive)
			return;

		string a = data.id.Substring (0, index);
		string b = data.id.Substring (index);
		Code.text = "[bcab29]" + a + "[-]" + b;

		Name.text = data.name;
		Min.text = data.minute.ToString();
		Lot.text = data.lotnumber.ToString();
		Repeat.text = data.repeatnumber.ToString();
	}
}
