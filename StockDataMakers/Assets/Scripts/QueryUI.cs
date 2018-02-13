using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDataEditor;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System;

public class QueryUI : MonoBehaviour {

	public UILabel MainCodeLable;
	public GameObject GridRoot;
	public GameObject QueryItem;

	public GameObject Hint;

	List<QueryItem> QueryItemList = new List<QueryItem>();

	string CurrentInput = "";

	enum InputState
	{
		Query,
		Locked,
	}

	List<GDEStockData> allStocks;
	Dictionary<string,GDEStockData> cachedIdDic = new Dictionary<string, GDEStockData> ();

	InputState state = InputState.Query;
	// Use this for initialization
	void Start () {

		for(int i = 0; i < 10; i++)
		{
			var go = GameObject.Instantiate (QueryItem) as GameObject;
			go.transform.parent = GridRoot.transform;
			go.transform.localScale = Vector3.one;
			go.SetActive (true);
			var queryScript = go.GetComponent<QueryItem> ();
			queryScript.Refresh (null, 0);
			QueryItemList.Add (queryScript);
		}

		GridRoot.GetComponent<UIGrid> ().Reposition ();


		GDEDataManager.Init ("gde_data");
		allStocks = GDEDataManager.GetAllItems<GDEStockData> ();

		BuildDataDic_And_CheckOldData ();
	}

	//根据历史数据，生成缓存字典，用来后期查询使用
	void BuildDataDic_And_CheckOldData()
	{
		//检测是否有股票代码相同，但是数据不同的重复数据
		foreach (var stock in allStocks) {
			if (cachedIdDic.ContainsKey (stock.id)) {
				if(cachedIdDic[stock.id].minute == stock.minute && cachedIdDic[stock.id].lotnumber == stock.lotnumber && cachedIdDic[stock.id].repeatnumber == stock.repeatnumber)
					Debug.Log ("重复股票数据：" + stock.Key + "  " +stock.name);
				else 
					Debug.LogError ("发现相同股票的不同历史数据：" + stock.Key + "  " +stock.name);
			} else {
				cachedIdDic.Add (stock.id, stock);
			}
		}
	}
		
	void InputPreCheck()
	{
		if (state == InputState.Locked) {
			CurrentInput = "";
			state = InputState.Query;
		}
		refreshUI = true;
	}
	bool refreshUI = false;
	// Update is called once per frame
	void Update () {
		refreshUI = false;

		if (Input.GetKeyDown (KeyCode.Alpha0) || Input.GetKeyDown (KeyCode.Keypad0)) {
			InputPreCheck ();
			CurrentInput = CurrentInput.Insert (CurrentInput.Length,"0");
		}
		if (Input.GetKeyDown (KeyCode.Alpha1) || Input.GetKeyDown (KeyCode.Keypad1)) {
			InputPreCheck ();
			CurrentInput = CurrentInput.Insert (CurrentInput.Length,"1");
		}
		if (Input.GetKeyDown (KeyCode.Alpha2) || Input.GetKeyDown (KeyCode.Keypad2)) {
			InputPreCheck ();
			CurrentInput = CurrentInput.Insert (CurrentInput.Length,"2");
		}
		if (Input.GetKeyDown (KeyCode.Alpha3) || Input.GetKeyDown (KeyCode.Keypad3)) {
			InputPreCheck ();
			CurrentInput = CurrentInput.Insert (CurrentInput.Length,"3");
		}
		if (Input.GetKeyDown (KeyCode.Alpha4) || Input.GetKeyDown (KeyCode.Keypad4)) {
			InputPreCheck ();
			CurrentInput = CurrentInput.Insert (CurrentInput.Length,"4");
		}
		if (Input.GetKeyDown (KeyCode.Alpha5) || Input.GetKeyDown (KeyCode.Keypad5)) {
			InputPreCheck ();
			CurrentInput = CurrentInput.Insert (CurrentInput.Length,"5");
		}
		if (Input.GetKeyDown (KeyCode.Alpha6) || Input.GetKeyDown (KeyCode.Keypad6)) {
			InputPreCheck ();
			CurrentInput = CurrentInput.Insert (CurrentInput.Length,"6");
		}
		if (Input.GetKeyDown (KeyCode.Alpha7) || Input.GetKeyDown (KeyCode.Keypad7)) {
			InputPreCheck ();
			CurrentInput = CurrentInput.Insert (CurrentInput.Length,"7");
		}
		if (Input.GetKeyDown (KeyCode.Alpha8) || Input.GetKeyDown (KeyCode.Keypad8)) {
			InputPreCheck ();
			CurrentInput = CurrentInput.Insert (CurrentInput.Length,"8");
		}
		if (Input.GetKeyDown (KeyCode.Alpha9) || Input.GetKeyDown (KeyCode.Keypad9)) {
			InputPreCheck ();
			CurrentInput = CurrentInput.Insert (CurrentInput.Length,"9");
		}

		if (Input.GetKeyDown (KeyCode.Backspace) || Input.GetKeyDown (KeyCode.Delete)) {
			InputPreCheck ();
			if (CurrentInput.Length > 0) {
				CurrentInput = CurrentInput.Remove (CurrentInput.Length - 1);
				refreshUI = true;
			}

		}

		if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.KeypadEnter)) {
			if (state == InputState.Locked) {
			
			} else {
				InputPreCheck ();
				state = InputState.Locked;

				foreach (var stockId in cachedIdDic.Keys) {
					if (stockId.StartsWith (CurrentInput)) {
						CurrentInput = stockId;
					}
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			InputPreCheck ();
			CurrentInput = "";

		}

//		foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode))) {
//			if (key == KeyCode.Mouse0 ||
//				key == KeyCode.Mouse1 ||
//				key == KeyCode.Mouse2 ||
//				key == KeyCode.Mouse3 ||
//				key == KeyCode.Mouse4 ||
//				key == KeyCode.Mouse5 ||
//				key == KeyCode.Mouse6 ) {
//				continue;
//			}
//
//			if (Input.GetKeyDown (key)) {
//				Debug.Log (key);
//			}
//		}

		if (refreshUI) {
			RefreshUI ();
		}

	}

	void RefreshUI()
	{
		
		MainCodeLable.text = CurrentInput;

		MainCodeLable.color = Color.yellow;
		int index = 0;

		if (string.IsNullOrEmpty (CurrentInput)) {
			for (int i = index; i < QueryItemList.Count; ++i) {
				QueryItemList [i].Refresh (null,0);
			}

			Hint.SetActive (true);
			return;
		}


		foreach (var stock in cachedIdDic.Values) {
			if (stock.id.StartsWith (CurrentInput)) {
				if (index < QueryItemList.Count) {
					QueryItemList [index].Refresh (stock,CurrentInput.Length);
					index++;
				}
			}
		}

		for (int i = index; i < QueryItemList.Count; ++i) {
			QueryItemList [i].Refresh (null,0);
		}

		if (index > 0) {
			MainCodeLable.color = Color.yellow;
		} else {
			MainCodeLable.color = Color.red;
		}

		Hint.SetActive (index == 0);
	}
}
