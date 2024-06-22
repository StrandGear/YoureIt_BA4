/*using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(TransparentLayer))]
[CanEditMultipleObjects]
public class TransparentLayerEditor : Editor
{
    SerializedProperty transparencyProperty;
    SerializedProperty rendererProperty;

    private void OnEnable()
    {
        transparencyProperty = serializedObject.FindProperty("_transparency");

        rendererProperty = serializedObject.FindProperty("_renderer");
    }

    public override void OnInspectorGUI()
    {
        //get defaults values
        serializedObject.Update();

        //snapped transparency property 
        EditorGUILayout.LabelField("Transparency");
        float snappedTransparency = SnapValue(EditorGUILayout.Slider(transparencyProperty.floatValue, 0f, 1f), new float[] { 0f, 0.5f, 1f });
        if (snappedTransparency != transparencyProperty.floatValue)
        {
            transparencyProperty.floatValue = snappedTransparency;
        }

        //assigning renderer in GUI
        if (rendererProperty.objectReferenceValue == null)
        {
            if (GUILayout.Button("Assign Default Renderer"))
            {
                foreach (Object targetObject in targets)
                {
                    TransparentLayer layer = (TransparentLayer)targetObject;
                    if (layer.TryGetComponent<MeshRenderer>(out var renderer))
                    {
                        rendererProperty.objectReferenceValue = renderer;
                    }
                }
            }
        }
        else
            EditorGUILayout.PropertyField(rendererProperty, new GUIContent("Renderer"));


        serializedObject.ApplyModifiedProperties();
    }
    private float SnapValue(float value, float[] snapPoints)
    {
        float closest = snapPoints[0];
        float minDifference = value - closest;

        foreach (float snapPoint in snapPoints)
        {
            float difference = value - snapPoint;

            if (difference < minDifference)
            {
                closest = snapPoint;
                minDifference = difference;
            }
        }
        return closest;
    }
}
*/