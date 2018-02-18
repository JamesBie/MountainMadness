using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(HexCoordinates))]

public class HexCoordinatesDrawer : PropertyDrawer {


	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
		HexCoordinates coordinates = new HexCoordinates(
			property.FindPropertyRelative("x").intValue, //extracts x value from the properties to use
			property.FindPropertyRelative("z").intValue
		);
		position = EditorGUI.PrefixLabel(position, label);// shifts the position of the labels. 
		GUI.Label(position, coordinates.ToString()); //draw gui at specific position with the coordinates


	}

}