// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultAssetPathDrawer.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NetCoreServer
{
	[CanEditMultipleObjects]
	[CustomPropertyDrawer(typeof(StreamingAssetPath))]
	public class StreamingAssetPathDrawer : PropertyDrawer
	{
		private const float HelpboxHeight = 36f;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			position.height = HelpboxHeight;
			EditorGUI.HelpBox(position, "Only Assets in Assets/StreamingAssets are allowed", MessageType.Info);
			position.y += HelpboxHeight;
			position.height = EditorGUIUtility.singleLineHeight;

			EditorGUI.BeginProperty(position, label, property);
			{
				var pathProperty = property.FindPropertyRelative("path");
				var asset = string.IsNullOrEmpty(pathProperty.stringValue) ? null : AssetDatabase.LoadAssetAtPath<Object>("Assets/StreamingAssets/" + pathProperty.stringValue);

				EditorGUI.BeginChangeCheck();
				asset = EditorGUI.ObjectField(position, label, asset, typeof(Object), false);

				if (EditorGUI.EndChangeCheck())
				{
					var path = AssetDatabase.GetAssetPath(asset);
					if (string.IsNullOrEmpty(path))
					{
						pathProperty.stringValue = null;
						return;
					}
					if (!path.StartsWith("Assets/StreamingAssets/"))
					{
						Debug.LogError("Only asssets in the StreamingAssets folder are allowed");
						pathProperty.stringValue = null;
						return;
					}
					pathProperty.stringValue = path.Substring("Assets/StreamingAssets/".Length);
				}
			}
			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return HelpboxHeight + EditorGUIUtility.singleLineHeight;
		}
	}
}
