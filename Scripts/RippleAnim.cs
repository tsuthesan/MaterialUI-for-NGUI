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

namespace MaterialUIforNGUI
{
	public class RippleAnim : MonoBehaviour
	{
		private UISprite thisSprite;
		private Transform thisTransform;

		private float animationSpeed;

		private float startColorAlpha;
		private float endColorAlpha;

		private float animStartTime;
		private float animFirstStartTime;
		private float animDeltaTime;
		private float animFirstDeltaTime;

		private float clearInkSize;
		private float clearInkAlpha;

		private Vector3 startPos;
		private Vector3 endPos;
		private bool moveTowardCenter;
		private float endScale;

		private Color tempColor;
		private Vector3 tempVector3;

		private int state;

		public void StartRipple()
		{
			MakeRipple(128, 6f, 0.5f, 0.3f, Color.black, Vector3.zero);
		}

		public void MakeRipple(int size, float animSpeed, float startAlpha, float endAlpha, Color color, Vector3 endPosition)
		{
	//		Get references to components
			thisSprite = gameObject.GetComponent<UISprite>();
			thisTransform = gameObject.GetComponent<Transform>();

			thisTransform.localScale = Vector3.zero;
			thisSprite.width = size;
			thisSprite.height = size;

			tempColor = color;
			tempColor.a = startAlpha;
			thisSprite.color = tempColor;

			if (endPosition != new Vector3(0, 0, 0))
			{
				moveTowardCenter = true;
				endPos = endPosition;
			}

			startPos = thisTransform.position;

			startColorAlpha = startAlpha;
			endColorAlpha = endAlpha;
			animationSpeed = animSpeed;

			state = 1;
			animStartTime = Time.realtimeSinceStartup;
			animFirstStartTime = animStartTime;
		}

		public void ClearRipple()
		{
			state = 2;
			animStartTime = Time.realtimeSinceStartup;
			clearInkSize = thisTransform.localScale.x;
			clearInkAlpha = thisSprite.color.a;

			if (moveTowardCenter)
				endScale = 1f;
			else
				endScale = 1.21f;
		}

		void Update()
		{
			animDeltaTime = Time.realtimeSinceStartup - animStartTime;

			if (state == 1)    // After initiated
			{
				if (thisTransform.localScale.x < 1f)
				{
	//				Expand
					tempVector3 = thisTransform.localScale;
					tempVector3.x = Anim.Quint.Out(0f, 1f, animDeltaTime, 4 / animationSpeed);
					tempVector3.y = tempVector3.x;
					tempVector3.z = 1f;
					thisTransform.localScale = tempVector3;

	//				Fade
					tempColor = thisSprite.color;
					tempColor.a = Anim.Quint.Out(startColorAlpha, endColorAlpha, animDeltaTime, 4 / animationSpeed);
					thisSprite.color = tempColor;

	//				Move toward center of parent
					if (moveTowardCenter)
					{
						tempVector3 = thisTransform.position;
						tempVector3.x = Anim.Quint.Out(startPos.x, endPos.x, animDeltaTime, 4 / animationSpeed);
						tempVector3.y = Anim.Quint.Out(startPos.y, endPos.y, animDeltaTime, 4 / animationSpeed);
						thisTransform.position = tempVector3;
					}
				}
				else
				{
					tempColor = thisSprite.color;
					tempColor.a = endColorAlpha;
					thisSprite.color = tempColor;
				}
			}
			else if (state == 2)    // After released
			{
				if (animDeltaTime < 6 / animationSpeed)
				{
					animFirstDeltaTime = Time.realtimeSinceStartup - animFirstStartTime;

	//				Expand
					tempVector3 = thisTransform.localScale;
					tempVector3.x = Anim.Quint.Out(clearInkSize, endScale, animDeltaTime, 6 / animationSpeed);
					tempVector3.y = tempVector3.x;
					tempVector3.z = 1f;
					thisTransform.localScale = tempVector3;

	//				Fade
					tempColor = thisSprite.color;
					tempColor.a = Anim.Quint.Out(clearInkAlpha, 0f, animDeltaTime, 6 / animationSpeed);
					thisSprite.color = tempColor;

	//				Move toward center of parent
					if (moveTowardCenter)
					{
						Vector3 tempVec3 = thisTransform.position;
						tempVec3.x = Anim.Quint.Out(startPos.x, endPos.x, animFirstDeltaTime, 4 / animationSpeed);
						tempVec3.y = Anim.Quint.Out(startPos.y, endPos.y, animFirstDeltaTime, 4 / animationSpeed);
						thisTransform.position = tempVec3;
					}
				}
				else
				{
					Destroy(gameObject.transform.parent.gameObject);
				}
			}
		}
	}
}
