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

namespace MaterialUIforNGUI
{
	public class RippleConfig : MonoBehaviour
	{
		[HideInInspector()]
		public Camera uiCamera;
		[HideInInspector()]
		public bool autoSize = true;
		[HideInInspector()]
		public float sizePercentage = 75f;
		[HideInInspector()]
		public int rippleSize = 0;
		[HideInInspector()]
		public float rippleSpeed = 6f;
		[HideInInspector()]
		public Color rippleColor = Color.black;
		[HideInInspector()]
		public float rippleStartAlpha = 0.5f;
		[HideInInspector()]
		public float rippleEndAlpha = 0.3f;

		[SerializeField()]
		public enum HighlightActive
		{
			Never,
			Hovered,
			Clicked
		}

		[SerializeField()]
		[HideInInspector()]
		public HighlightActive highlightWhen = HighlightActive.Clicked;

		[HideInInspector()]
		public bool moveTowardCenter = false;

		private RippleAnim currentRippleAnim;
		private UISprite thisSprite;
		private bool worldSpace;

		private Color normalColor;
		private Color highlightColor;

		private Color tempColor;
		private Color currentColor;

		private int state;
		private float animStartTime;
		private float animDeltaTime;
		private float animationDuration;

		public void Setup()
		{
			thisSprite = gameObject.GetComponent<UISprite>();
		}

		void Awake()
		{
			RippleControl.Initialize();
			thisSprite = gameObject.GetComponent<UISprite>();
		}

		void Start()
		{
			Refresh();
		}

		public void Refresh()
		{
			if (autoSize)
			{
				if (thisSprite.width > thisSprite.height)
				{
					rippleSize = Mathf.RoundToInt(thisSprite.width);
				}
				else
				{
					rippleSize = Mathf.RoundToInt(thisSprite.height);
				}

				rippleSize = Mathf.RoundToInt(rippleSize * sizePercentage / 100f);
			}

			normalColor = thisSprite.color;

			if (highlightWhen != HighlightActive.Never)
			{
				highlightColor = rippleColor;

				HSBColor highlightColorHSB = HSBColor.FromColor(highlightColor);
				HSBColor normalColorHSB = HSBColor.FromColor(normalColor);

				if (highlightColorHSB.s <= 0.05f)
				{
					if (highlightColorHSB.b > 0.5f)
					{
						if (normalColorHSB.b > 0.9f)
						{
							highlightColorHSB.h = normalColorHSB.h;
							highlightColorHSB.s = normalColorHSB.s - 0.1f;
							highlightColorHSB.b = normalColorHSB.b + 0.2f;
						}
						else
						{
							highlightColorHSB.h = normalColorHSB.h;
							highlightColorHSB.s = normalColorHSB.s;
							highlightColorHSB.b = normalColorHSB.b + 0.2f;
						}
						
					}
					else
					{
						highlightColorHSB.h = normalColorHSB.h;
						highlightColorHSB.s = normalColorHSB.s;
						highlightColorHSB.b = normalColorHSB.b - 0.15f;
					}

					highlightColor = HSBColor.ToColor(highlightColorHSB);
					highlightColor.a = normalColor.a;
				}
				else
				{
					highlightColor.r = Anim.Linear(normalColor.r, highlightColor.r, 0.2f, 1f);
					highlightColor.g = Anim.Linear(normalColor.g, highlightColor.g, 0.2f, 1f);
					highlightColor.b = Anim.Linear(normalColor.b, highlightColor.b, 0.2f, 1f);
					highlightColor.a = Anim.Linear(normalColor.a, highlightColor.a, 0.2f, 1f);
				}
			}

			animationDuration = 4 / rippleSpeed;
		}

		void Update()
		{
			if (state == 1)
			{
				animDeltaTime = Time.realtimeSinceStartup - animStartTime;

				if (animDeltaTime < animationDuration)
				{
					thisSprite.color = Anim.Quint.Out(currentColor, highlightColor, animDeltaTime, animationDuration);
				}
				else
				{
					thisSprite.color = highlightColor;
					state = 0;
				}
			}
			else if (state == 2)
			{
				animDeltaTime = Time.realtimeSinceStartup - animStartTime;

				if (animDeltaTime < animationDuration)
				{
					thisSprite.color = Anim.Quint.Out(currentColor, normalColor, animDeltaTime, animationDuration);
				}
				else
				{
					thisSprite.color = normalColor;
					state = 0;
				}
			}
		}

		public void OnHover(bool isOver)
		{
			if (isOver)
			{
				if (highlightWhen == HighlightActive.Hovered)
				{
					currentColor = thisSprite.color;
					animStartTime = Time.realtimeSinceStartup;
					state = 1;
				}
			}
			else
			{
				if (currentRippleAnim)
				{
					currentRippleAnim.ClearRipple();
				}

				currentRippleAnim = null;

				if (highlightWhen != HighlightActive.Never)
				{
					currentColor = thisSprite.color;
					animStartTime = Time.realtimeSinceStartup;
					state = 2;
				}
			}
		}

		public void OnDragOut()
		{
			if (currentRippleAnim)
			{
				currentRippleAnim.ClearRipple();
			}

			currentRippleAnim = null;
			
			if (highlightWhen != HighlightActive.Never)
			{
				currentColor = thisSprite.color;
				animStartTime = Time.realtimeSinceStartup;
				state = 2;
			}
		}

		public void OnPress (bool pressed)
		{
			if (pressed)
			{
				MakeInkBlot(Input.mousePosition);

				if (highlightWhen == HighlightActive.Clicked)
				{
					currentColor = thisSprite.color;
					animStartTime = Time.realtimeSinceStartup;
					state = 1;
				}
			}
			else
			{
				if (currentRippleAnim)
				{
					currentRippleAnim.ClearRipple();
				}

				currentRippleAnim = null;

				if (highlightWhen != HighlightActive.Never)
				{
					currentColor = thisSprite.color;
					animStartTime = Time.realtimeSinceStartup;
					state = 2;
				}
			}
		}

		private void MakeInkBlot (Vector3 pos)
		{
			if (currentRippleAnim)
			{
				currentRippleAnim.ClearRipple ();
			}

			if (moveTowardCenter)
				currentRippleAnim = RippleControl.MakeRipple (uiCamera, pos, transform, rippleSize, rippleSpeed, rippleStartAlpha, rippleEndAlpha, rippleColor, gameObject.GetComponent<Transform>().position).GetComponent<RippleAnim>();
			else
				currentRippleAnim = RippleControl.MakeRipple(uiCamera, pos, transform, rippleSize, rippleSpeed, rippleStartAlpha, rippleEndAlpha, rippleColor).GetComponent<RippleAnim>();
		}
	}
}
