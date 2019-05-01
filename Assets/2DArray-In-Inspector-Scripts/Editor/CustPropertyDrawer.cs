using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
[CustomPropertyDrawer(typeof(ArrayLayout))]
public class CustPropertyDrawer : PropertyDrawer {


	public override void OnGUI(Rect position,SerializedProperty property,GUIContent label){
		EditorGUI.PrefixLabel(position,label);
		Rect newposition = position;
		newposition.y += 36f;
		SerializedProperty data = property.FindPropertyRelative("rows");
		//data.rows[0][]
		for(int j=0;j<7;j++){
			SerializedProperty row = data.GetArrayElementAtIndex(j).FindPropertyRelative("row");
			newposition.height = 36f;
			if(row.arraySize != 7)
				row.arraySize = 7; 
			newposition.width = position.width/7;
			for(int i=0;i<7;i++){
                //SerializedProperty enumProperty = property.FindPropertyRelative(SpaceTypePicker.spacetypeVarName);
                //EditorGUI.PropertyField(newposition, enumProperty, GUIContent.none);
                //chosen property field at 18 above
                //newposition.y += 18f;
                //SpaceType chosen = (SpaceType)enumProperty.enumValueIndex;
                //SerializedProperty selection = row.GetArrayElementAtIndex(i).FindPropertyRelative(chosen.ToString());
                //EditorGUI.PropertyField(newposition, selection, GUIContent.none);
                //enumProperty.enumValueIndex = (int)(SpaceType)EditorGUILayout.EnumPopup("My Enum:", (SpaceType)Enum.GetValues(typeof(SpaceType)).GetValue(enumProperty.enumValueIndex));

                EditorGUI.PropertyField(newposition, row.GetArrayElementAtIndex(i), GUIContent.none);
                //newposition.y -= 18f;
                newposition.x += newposition.width;
			}

			newposition.x = position.x;
			newposition.y += 36f;
		}
	}

	public override float GetPropertyHeight(SerializedProperty property,GUIContent label){
		return 18f * 8;
	}
}
