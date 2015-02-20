//  Copyright 2014 Invex Games http://invexgames.com
//	Licensed under the Apache License, Version 2.0 (the "License");
//	you may not use this file except in compliance with the License.
//	You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//	Unless required by applicable law or agreed to in writing, software
//	distributed under the License is distributed on an "AS IS" BASIS,
//	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//	See the License for the specific language governing permissions and
//	limitations under the License.

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MaterialUIforGUI
{
	public class ShadowConfig : MonoBehaviour
	{
		public ShadowAnim[] shadows;
		[Range(0,3)]
		public int shadowNormalSize = 1;
		[Range(0, 3)]
		public int shadowActiveSize = 2;
		
		public enum ShadowsActive
		{
			Hovered,
			Clicked
		}

		public ShadowsActive shadowsActiveWhen = ShadowsActive.Hovered;

		public bool isEnabled = true;
		
		public void OnPress (bool pressed)
		{
			if (pressed)
			{
				if (shadowsActiveWhen == ShadowsActive.Clicked)
					SetShadows(shadowActiveSize);
			}
			else
			{
				if (shadowsActiveWhen == ShadowsActive.Clicked)
					SetShadows(shadowNormalSize);
			}
		}

		public void OnHover (bool hovered)
		{
			if (hovered)
			{
				if (shadowsActiveWhen == ShadowsActive.Hovered)
					SetShadows(shadowActiveSize);
			}
			else
			{
				SetShadows(shadowNormalSize);
			}
		}

		public void OnDragOut ()
		{
			SetShadows(shadowNormalSize);
		}

		public void SetShadows (int shadowOn)
		{
			if (isEnabled)
			{
				foreach (ShadowAnim shadow in shadows)
				{
					shadow.SetShadow(false);
				}
				
				if (shadowOn - 1 >= 0)
				{
					shadows [shadowOn - 1].SetShadow (true);
				}
			}
		}
	}
}