using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NDimensionalMap))]
public class NDimensionalMapEditor : Editor
{
    private SerializedObject _editorInfo;//序列化
    private SerializedProperty _drawType, _edgeAmount, _postionList, _polygonColor, _drawLineFlag, _lineColor, _lineWidth;

    void OnEnable()
    {
        _editorInfo = new SerializedObject(target);
        _drawType = _editorInfo.FindProperty("DrawType");
        _edgeAmount = _editorInfo.FindProperty("EdgeAmount");
        _postionList = _editorInfo.FindProperty("Positions");
        _polygonColor = _editorInfo.FindProperty("PolygonColor");
        _drawLineFlag = _editorInfo.FindProperty("DrawLineFlag");
        _lineColor = _editorInfo.FindProperty("LineColor");
        _lineWidth = _editorInfo.FindProperty("LineWidth");
    }
    public override void OnInspectorGUI()
    {
        _editorInfo.Update();

        EditorGUILayout.PropertyField(_drawType);
        EditorGUILayout.PropertyField(_polygonColor);
        if (_drawType.enumValueIndex == 1)
        {
            EditorGUILayout.PropertyField(_postionList, true);
        }
        else
        {
            EditorGUILayout.PropertyField(_edgeAmount);
        }
        EditorGUILayout.PropertyField(_drawLineFlag);
        if (_drawLineFlag.boolValue == true)
        {
            EditorGUILayout.PropertyField(_lineColor);
            EditorGUILayout.PropertyField(_lineWidth);
        }
        _editorInfo.ApplyModifiedProperties();//应用
    }
}
