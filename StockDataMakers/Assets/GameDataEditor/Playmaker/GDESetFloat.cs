/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using System.Collections.Generic;
using GameDataEditor;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip(GDMConstants.SetFloatActionTooltip)]
	public class GDESetFloat : GDEActionBase
	{   
		[UIHint(UIHint.FsmFloat)]
		public FsmFloat FloatValue;
		
		public override void Reset()
		{
			base.Reset();
			FloatValue = null;
		}
		
		public override void OnEnter()
		{
			try
			{
				GDEDataManager.SetFloat(ItemName.Value, FieldName.Value, FloatValue.Value);
			}
			catch(UnityException ex)
			{
				LogError(string.Format(GDMConstants.ErrorSettingValue, GDMConstants.FloatType, ItemName.Value, FieldName.Value));
				LogError(ex.ToString());
			}
			finally
			{
				Finish();
			}
		}
	}
}

#endif

