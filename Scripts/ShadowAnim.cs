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
using UnityEngine.UI;
using System.Collections;

namespace MaterialUIforGUI
{
	public class ShadowAnim : MonoBehaviour
	{
		public bool isOn;
		public bool anim;
		private UISprite thisSprite;

		void Awake ()
		{
			thisSprite = gameObject.GetComponent<UISprite>();
		}
		
		void Update ()
		{
			if (!anim) return;

			if (isOn)
			{
				if (thisSprite.alpha < 1f)
				{
					thisSprite.alpha = Mathf.Lerp(thisSprite.alpha, 1.1f, Time.deltaTime * 6);
				}
				else
				{
					thisSprite.alpha = 1f;
					anim = false;
				}
			}
			else
			{
				if (thisSprite.alpha > 0f)
				{
					thisSprite.alpha = Mathf.Lerp(thisSprite.alpha, -0.1f, Time.deltaTime * 6);
				}
				else
				{
					thisSprite.alpha = 0f;
					anim = false;
					thisSprite.enabled = false;
				}
			}
		}

		public void SetShadow (bool set)
		{
			isOn = set;
			anim = true;
			thisSprite.enabled = true;
		}
	}
}
