// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ByteFileAssetDrawer.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using UnityEditor;
using UnityEngine;

namespace NetCoreServer
{
	[CanEditMultipleObjects]
	[CustomPropertyDrawer(typeof(ByteFileAsset))]
	public class ByteFileAssetDrawer : PropertyDrawer
	{
		SerializedProperty guidProperty;
		SerializedProperty bytesProperty;
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			{
				guidProperty = property.FindPropertyRelative("guid");
				bytesProperty = property.FindPropertyRelative("bytes");
				var guid = guidProperty.stringValue;

				Object asset = null;
				if (!string.IsNullOrEmpty(guid))
				{
					var path = AssetDatabase.GUIDToAssetPath(guid);
					asset = AssetDatabase.LoadAssetAtPath<Object>(path);
				}

				EditorGUI.BeginChangeCheck();
				position.height = EditorGUIUtility.singleLineHeight;
				asset = EditorGUI.ObjectField(position, label, asset, typeof(Object), false);

				if (EditorGUI.EndChangeCheck())
				{
					UpdateData(asset);
				}

				position.y += EditorGUIUtility.singleLineHeight;
				EditorGUI.HelpBox(position, $"Byte Array size: {bytesProperty.arraySize}.", MessageType.None);
			}
			EditorGUI.EndProperty();
		}

		private void UpdateData(Object asset)
		{
			string guid;
			var path = AssetDatabase.GetAssetPath(asset);
			if (string.IsNullOrEmpty(path))
			{
				guidProperty.stringValue = null;
				bytesProperty.ClearArray();
				return;
			}

			guid = AssetDatabase.AssetPathToGUID(path);
			guidProperty.stringValue = guid;
			var bytes = File.ReadAllBytes(path);
			bytesProperty.arraySize = bytes.Length;
			for (int i = 0; i < bytes.Length; i++)
			{
				bytesProperty.GetArrayElementAtIndex(i).intValue = bytes[i];
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return 2 * EditorGUIUtility.singleLineHeight;
		}
	}
}
