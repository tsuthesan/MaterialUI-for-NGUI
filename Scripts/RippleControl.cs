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

namespace MaterialUIforNGUI
{
	public static class RippleControl
	{
		static GameObject ripplePrefab;
		static GameObject currentRipple;
		static GameObject currentPanel;

		public static void Initialize ()
		{
			if (ripplePrefab == null)
				ripplePrefab = Resources.Load ("Ripple_NGUI", typeof(GameObject)) as GameObject;
		}

		public static GameObject MakeRipple (Camera camera, Vector3 position, Transform parent, int size, Color color)
		{
			currentPanel = GameObject.Instantiate(ripplePrefab) as GameObject;

			Transform panelTransform = currentPanel.GetComponent<Transform>();

			panelTransform.SetParent(parent);

			panelTransform.position = parent.position;

			panelTransform.rotation = parent.rotation;

			panelTransform.localScale = new Vector3(1f, 1f, 1f);

			currentPanel.GetComponent<UIPanel>().baseClipRegion = new Vector4(0f, 0f, parent.GetComponent<UISprite>().width, parent.GetComponent<UISprite>().height);

			currentRipple = currentPanel.GetComponentInChildren<RippleAnim>().gameObject;

			Transform rippleTransform = currentRipple.GetComponent<Transform>();

			rippleTransform.position = camera.ScreenToWorldPoint(position);

			rippleTransform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
			
			currentRipple.GetComponent<RippleAnim> ().MakeRipple (size, 6f, 0.5f, 0.3f, color, new Vector3 (0, 0, 0));
			
			return currentRipple;
		}

		public static GameObject MakeRipple(Camera camera, Vector3 position, Transform parent, int size, float animSpeed, float startAlpha, float endAlpha, Color color)
		{
			currentPanel = GameObject.Instantiate(ripplePrefab) as GameObject;

			Transform panelTransform = currentPanel.GetComponent<Transform>();

			panelTransform.SetParent(parent);

			panelTransform.position = parent.position;

			panelTransform.rotation = parent.rotation;

			panelTransform.localScale = new Vector3(1f, 1f, 1f);

			currentPanel.GetComponent<UIPanel>().baseClipRegion = new Vector4(0f, 0f, parent.GetComponent<UISprite>().width, parent.GetComponent<UISprite>().height);

			currentRipple = currentPanel.GetComponentInChildren<RippleAnim>().gameObject;

			Transform rippleTransform = currentRipple.GetComponent<Transform>();

			rippleTransform.position = camera.ScreenToWorldPoint(position);

			rippleTransform.localRotation = new Quaternion(0f, 0f, 0f, 0f);

			currentRipple.GetComponent<RippleAnim> ().MakeRipple (size, animSpeed, startAlpha, endAlpha, color, new Vector3 (0, 0, 0));

			return currentRipple;
		}

		public static GameObject MakeRipple(Camera camera, Vector3 position, Transform parent, int size, float animSpeed, float startAlpha, float endAlpha, Color color, Vector3 endPosition)
		{
			currentPanel = GameObject.Instantiate(ripplePrefab) as GameObject;

			Transform panelTransform = currentPanel.GetComponent<Transform>();

			panelTransform.SetParent(parent);

			panelTransform.position = parent.position;

			panelTransform.rotation = parent.rotation;

			panelTransform.localScale = new Vector3(1f, 1f, 1f);

			currentPanel.GetComponent<UIPanel>().baseClipRegion = new Vector4(0f, 0f, parent.GetComponent<UISprite>().width, parent.GetComponent<UISprite>().height);

			currentRipple = currentPanel.GetComponentInChildren<RippleAnim>().gameObject;

			Transform rippleTransform = currentRipple.GetComponent<Transform>();

			rippleTransform.position = camera.ScreenToWorldPoint(position);

			rippleTransform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
			
			currentRipple.GetComponent<RippleAnim> ().MakeRipple (size, animSpeed, startAlpha, endAlpha, color, endPosition);
			
			return currentRipple;
		}
	}
}