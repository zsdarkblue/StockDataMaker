// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by the Game Data Editor.
//
//      Changes to this file will be lost if the code is regenerated.
//
//      This file was generated from this data file:
//      C:\Projects\StockDataMaker\Assets/GameDataEditor/Resources/gde_data.txt
//  </autogenerated>
// ------------------------------------------------------------------------------
using UnityEngine;
using System;
using System.Collections.Generic;

using GameDataEditor;

namespace GameDataEditor
{
    public class GDEStockData : IGDEData
    {
        static string minuteKey = "minute";
		int _minute;
        public int minute
        {
            get { return _minute; }
            set {
                if (_minute != value)
                {
                    _minute = value;
					GDEDataManager.SetInt(_key, minuteKey, _minute);
                }
            }
        }

        static string lotnumberKey = "lotnumber";
		int _lotnumber;
        public int lotnumber
        {
            get { return _lotnumber; }
            set {
                if (_lotnumber != value)
                {
                    _lotnumber = value;
					GDEDataManager.SetInt(_key, lotnumberKey, _lotnumber);
                }
            }
        }

        static string repeatnumberKey = "repeatnumber";
		int _repeatnumber;
        public int repeatnumber
        {
            get { return _repeatnumber; }
            set {
                if (_repeatnumber != value)
                {
                    _repeatnumber = value;
					GDEDataManager.SetInt(_key, repeatnumberKey, _repeatnumber);
                }
            }
        }

        static string idKey = "id";
		string _id;
        public string id
        {
            get { return _id; }
            set {
                if (_id != value)
                {
                    _id = value;
					GDEDataManager.SetString(_key, idKey, _id);
                }
            }
        }

        static string nameKey = "name";
		string _name;
        public string name
        {
            get { return _name; }
            set {
                if (_name != value)
                {
                    _name = value;
					GDEDataManager.SetString(_key, nameKey, _name);
                }
            }
        }

        public GDEStockData(string key) : base(key)
        {
            GDEDataManager.RegisterItem(this.SchemaName(), key);
        }
        public override Dictionary<string, object> SaveToDict()
		{
			var dict = new Dictionary<string, object>();
			dict.Add(GDMConstants.SchemaKey, "Stock");
			
            dict.Merge(true, minute.ToGDEDict(minuteKey));
            dict.Merge(true, lotnumber.ToGDEDict(lotnumberKey));
            dict.Merge(true, repeatnumber.ToGDEDict(repeatnumberKey));
            dict.Merge(true, id.ToGDEDict(idKey));
            dict.Merge(true, name.ToGDEDict(nameKey));
            return dict;
		}

        public override void UpdateCustomItems(bool rebuildKeyList)
        {
        }

        public override void LoadFromDict(string dataKey, Dictionary<string, object> dict)
        {
            _key = dataKey;

			if (dict == null)
				LoadFromSavedData(dataKey);
			else
			{
                dict.TryGetInt(minuteKey, out _minute);
                dict.TryGetInt(lotnumberKey, out _lotnumber);
                dict.TryGetInt(repeatnumberKey, out _repeatnumber);
                dict.TryGetString(idKey, out _id);
                dict.TryGetString(nameKey, out _name);
                LoadFromSavedData(dataKey);
			}
		}

        public override void LoadFromSavedData(string dataKey)
		{
			_key = dataKey;
			
            _minute = GDEDataManager.GetInt(_key, minuteKey, _minute);
            _lotnumber = GDEDataManager.GetInt(_key, lotnumberKey, _lotnumber);
            _repeatnumber = GDEDataManager.GetInt(_key, repeatnumberKey, _repeatnumber);
            _id = GDEDataManager.GetString(_key, idKey, _id);
            _name = GDEDataManager.GetString(_key, nameKey, _name);
        }

        public GDEStockData ShallowClone()
		{
			string newKey = Guid.NewGuid().ToString();
			GDEStockData newClone = new GDEStockData(newKey);

            newClone.minute = minute;
            newClone.lotnumber = lotnumber;
            newClone.repeatnumber = repeatnumber;
            newClone.id = id;
            newClone.name = name;

            return newClone;
		}

        public GDEStockData DeepClone()
		{
			GDEStockData newClone = ShallowClone();
            return newClone;
		}

        public void Reset_minute()
        {
            GDEDataManager.ResetToDefault(_key, minuteKey);

            Dictionary<string, object> dict;
            GDEDataManager.Get(_key, out dict);
            dict.TryGetInt(minuteKey, out _minute);
        }

        public void Reset_lotnumber()
        {
            GDEDataManager.ResetToDefault(_key, lotnumberKey);

            Dictionary<string, object> dict;
            GDEDataManager.Get(_key, out dict);
            dict.TryGetInt(lotnumberKey, out _lotnumber);
        }

        public void Reset_repeatnumber()
        {
            GDEDataManager.ResetToDefault(_key, repeatnumberKey);

            Dictionary<string, object> dict;
            GDEDataManager.Get(_key, out dict);
            dict.TryGetInt(repeatnumberKey, out _repeatnumber);
        }

        public void Reset_id()
        {
            GDEDataManager.ResetToDefault(_key, idKey);

            Dictionary<string, object> dict;
            GDEDataManager.Get(_key, out dict);
            dict.TryGetString(idKey, out _id);
        }

        public void Reset_name()
        {
            GDEDataManager.ResetToDefault(_key, nameKey);

            Dictionary<string, object> dict;
            GDEDataManager.Get(_key, out dict);
            dict.TryGetString(nameKey, out _name);
        }

        public void ResetAll()
        {
            GDEDataManager.ResetToDefault(_key, idKey);
            GDEDataManager.ResetToDefault(_key, nameKey);
            GDEDataManager.ResetToDefault(_key, minuteKey);
            GDEDataManager.ResetToDefault(_key, lotnumberKey);
            GDEDataManager.ResetToDefault(_key, repeatnumberKey);


            Dictionary<string, object> dict;
            GDEDataManager.Get(_key, out dict);
            LoadFromDict(_key, dict);
        }
    }
}