using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDataEditor;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System;

public class Main : MonoBehaviour {

	List<GDEStockData> allStocks;
	Dictionary<string,GDEStockData> cachedIdDic = new Dictionary<string, GDEStockData> ();
	Dictionary<string,GDEStockData> cachedNameDic = new Dictionary<string, GDEStockData> ();

	List<GDENewDataData> allNewDatas;
	// Use this for initialization
	void Start () {
		GDEDataManager.Init ("gde_data");
		allStocks = GDEDataManager.GetAllItems<GDEStockData> ();
		allNewDatas = GDEDataManager.GetAllItems<GDENewDataData> ();
	
		BuildDataDic_And_CheckOldData ();
		CalculateAndLogResultToScreen (true);
		BuildEBK ();
		BuildTS_txt ();
	}

	//根据历史数据，生成缓存字典，用来后期查询使用
	void BuildDataDic_And_CheckOldData()
	{
		
		//检查是否有名字相同但股票代号不同的错误，主要检测人为手动输错股票代号。
		foreach (var stock in allStocks) {
			if (cachedNameDic.ContainsKey (stock.name)) {
				if(cachedNameDic[stock.name].id != stock.id)
					Debug.LogError ("股票代号错误，股票名称" + stock.name + "  其中一个股票行号：" +stock.Key + "  第二个行号：" + cachedNameDic[stock.name].Key);

			} else {
				cachedNameDic.Add (stock.name, stock);
			}
		}

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


	//根据缓存字典，对比第二个sheet "NewData", 打印出需要新加的股票
	void CalculateAndLogResultToScreen(bool logSeparate)
	{
		List<GDENewDataData> noDataList = new List<GDENewDataData> ();
		List<GDEStockData> stockList = new List<GDEStockData> ();
		foreach (var newData in allNewDatas) {
			if (cachedIdDic.ContainsKey (newData.id)) {
				var stock = cachedIdDic[newData.id];
				stockList.Add (stock);
				if (!logSeparate) {
					LogStockInfo (stock.id, stock.name, stock.minute, stock.lotnumber, stock.repeatnumber);				
				}
			} else {
				noDataList.Add (newData);

				if (!logSeparate) {
					LogStockInfo (newData.id,newData.name,0,0,0);
				}
			}
		}
		Debug.Log ("<color=#00ff00> <=================================== 有数据股票总览 ===================================> </color>");

		foreach (var stock in stockList) {
			LogStockInfo (stock.id, stock.name, stock.minute, stock.lotnumber, stock.repeatnumber);
		}
		Debug.Log ("<color=#ff4500> <=================================== 空数据股票总览 ===================================> </color>");

		foreach (var newData in noDataList) {
			LogStockInfo (newData.id,newData.name,0,0,0);
		}
	}

	//创建金阳光自选股文件
	void BuildEBK()
	{
		string path = Application.dataPath  + "/预警股池.EBK";
		StreamWriter sw = new StreamWriter (path,false);
		foreach (var newData in allNewDatas) {
			//沪市股票，股票代码前 + 1
			if (newData.id.StartsWith ("6")) {
				sw.WriteLine ("1" + newData.id);
			}
			//其他市场股票，股票代码前 + 0
			else {
				sw.WriteLine ("0" + newData.id);
			}

		}
		sw.Close ();

		Debug.Log ("<color=#ffff00>通达信</color>  <color=#ffffff>自选股文件已经输出，文件位置:</color>   <color=#00ff00>" + path + "</color>");
	}
		
	//创建金阳光自选股文件
	void BuildTS_txt()
	{
		string path = Application.dataPath  + "/TS股票文件.txt";
		StreamWriter sw = new StreamWriter (path,false);
		foreach (var newData in allNewDatas) {

			if (cachedIdDic.ContainsKey (newData.id)) {
				var stock = cachedIdDic[newData.id];
//				if (newData.id.StartsWith ("6")) {
//					sw.WriteLine (newData.id + ".SH" + "," + stock.minute.ToString() + "," + stock.lotnumber.ToString() + "," + stock.repeatnumber.ToString());
//				}
//
//				else {
//					sw.WriteLine (newData.id + ".SZ" + "," + stock.minute.ToString() + "," + stock.lotnumber.ToString() + "," + stock.repeatnumber.ToString());
//				}

				sw.WriteLine (newData.id + "," + stock.minute.ToString() + "," + stock.lotnumber.ToString() + "," + stock.repeatnumber.ToString());
			} 


		}
		sw.Close ();

		Debug.Log ("<color=#ffff00>通达信</color>  <color=#ffffff>自选股文件已经输出，文件位置:</color>   <color=#00ff00>" + path + "</color>");
	}


	void LogStockInfo(string id,string name,int minute,int lot,int repeat)
	{
		//用来对其文字，股票有3个字的，有4个字的
		string nameGap = "";
		if (name.Length == 3) {
			nameGap = "   ";
		}

		//用来对其手数文字
		string lotGap = "";
		if (lot < 10) {
			lotGap = "         ";
		}
		else if (lot < 100) {
			lotGap = "        ";
		}
		else if (lot < 1000) {
			lotGap = "     ";
		}
		else if (lot < 10000) {
			lotGap = "   ";
		}
		else if (lot < 100000) {
			lotGap = "  ";
		}
		else if (lot < 1000000) {
			lotGap = " ";
		}

		//没有数据
		if (minute == 0) {
			Debug.Log (
				"<color=#f0e68c>" + id + "</color>"
				+ "   <color=#f0e68c>" + name + nameGap + "</color>  ");
				//+ "   <color=#ff4500>无数据</color>  " );
		} else {
			string minuteColor = "<color=#00ff00>";
			if (minute != 2) {
				minuteColor = "<color=#ff0000>";
			}
			Debug.Log (
				"<color=#00ffff>" + id + "</color>"  
				+ "   <color=#ff8c00>" + name + nameGap + "</color>  " 
				+ "   "+ minuteColor + minute + "</color>"  + " <color=ffffff>分钟</color>" 
				+ "        <color=#00ff00>" + lot + "</color>"  + " <color=ffffff>手</color>" +  lotGap 
				+ "   <color=#00ff00>" + repeat + "</color>" + " <color=ffffff>次</color>" );
		}
	}

}
