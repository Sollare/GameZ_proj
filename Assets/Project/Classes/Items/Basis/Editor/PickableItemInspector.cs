using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(PickableItem))]
public class PickableItemInspector : Editor
{
    [SerializeField] private PickableItem _pickableItem;

    void OnEnable()
    {
        _pickableItem = target as PickableItem;
    }

	// Use this for initialization
    public override void OnInspectorGUI()
    {
#pragma warning disable 618
        //EditorGUILayout.LabelField("Item class: ", "");
        if (_pickableItem.GetItemSource() == null)
        {
            if(Application.isEditor)
            _pickableItem.SetItemSource(EditorGUILayout.ObjectField(label: "Item class:", obj: _pickableItem.GetItemSource(), objType: typeof (UnityEngine.Object)));
        }
        else
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.ObjectField("Item class:", _pickableItem.GetItemSource(),
                typeof (UnityEngine.Object));

            if (GUILayout.Button("Remove"))
            {
                _pickableItem.ClearItemSource();  
                return;
            }
            GUILayout.EndHorizontal();
        }

        if (_pickableItem.HoldedItem != null)
        {
            EditorGUILayout.HelpBox(string.Concat("Предмет: ", _pickableItem.HoldedItem.LocalizedName), MessageType.Info);

            _pickableItem.InspectableItem.OnInspectorGUI();
        }

        if (GUI.changed)
        {
            HandleUtility.Repaint();
            EditorUtility.SetDirty(_pickableItem);
        }
    }
#pragma warning restore 618
}
