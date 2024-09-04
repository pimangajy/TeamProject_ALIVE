using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DragItem))]
public class OnInspectorGUI_Item : Editor
{

    public override void OnInspectorGUI()
    {
        DragItem myinspector = (DragItem)target;

        myinspector.itemType = (ItemType)EditorGUILayout.EnumPopup("Select Type", myinspector.itemType);

        switch(myinspector.itemType)
        {
            case ItemType.Weapon:
                myinspector.itemName = EditorGUILayout.TextField("Item Name", myinspector.itemName);
                myinspector.model = (GameObject)EditorGUILayout.ObjectField("Item Model", myinspector.model, typeof(GameObject), allowSceneObjects: false);
                myinspector.numbering = EditorGUILayout.IntField("Item Numbering", myinspector.numbering);
                myinspector.sale_price = EditorGUILayout.IntField("판매가", myinspector.sale_price);
                myinspector.itemIcon = (Sprite)EditorGUILayout.ObjectField("Item Icon", myinspector.itemIcon, typeof(Sprite), allowSceneObjects: false);
                myinspector.Weapon_Img = (Sprite)EditorGUILayout.ObjectField("Weapon Image", myinspector.Weapon_Img, typeof(Sprite), allowSceneObjects: false);
                myinspector.ATK = EditorGUILayout.IntField("ATK", myinspector.ATK);
                break;

            case ItemType.miscellaneous:
                myinspector.itemName = EditorGUILayout.TextField("Item Name", myinspector.itemName);
                myinspector.model = (GameObject)EditorGUILayout.ObjectField("Item Model", myinspector.model, typeof(GameObject), allowSceneObjects: false);
                myinspector.numbering = EditorGUILayout.IntField("Item Numbering", myinspector.numbering);
                myinspector.sale_price = EditorGUILayout.IntField("판매가", myinspector.sale_price);
                myinspector.itemIcon = (Sprite)EditorGUILayout.ObjectField("Item Icon", myinspector.itemIcon, typeof(Sprite), allowSceneObjects: false);
                break;

            case ItemType.Hunger:
                myinspector.itemName = EditorGUILayout.TextField("Item Name", myinspector.itemName);
                myinspector.model = (GameObject)EditorGUILayout.ObjectField("Item Model", myinspector.model, typeof(GameObject), allowSceneObjects: false);
                myinspector.numbering = EditorGUILayout.IntField("Item Numbering", myinspector.numbering);
                myinspector.sale_price = EditorGUILayout.IntField("판매가", myinspector.sale_price);
                myinspector.itemIcon = (Sprite)EditorGUILayout.ObjectField("Item Icon", myinspector.itemIcon, typeof(Sprite), allowSceneObjects: false);
                myinspector.fullness = EditorGUILayout.FloatField("포만감", myinspector.fullness);
                break;

            case ItemType.thirst:
                myinspector.itemName = EditorGUILayout.TextField("Item Name", myinspector.itemName);
                myinspector.model = (GameObject)EditorGUILayout.ObjectField("Item Model", myinspector.model, typeof(GameObject), allowSceneObjects: false);
                myinspector.numbering = EditorGUILayout.IntField("Item Numbering", myinspector.numbering);
                myinspector.sale_price = EditorGUILayout.IntField("판매가", myinspector.sale_price);
                myinspector.itemIcon = (Sprite)EditorGUILayout.ObjectField("Item Icon", myinspector.itemIcon, typeof(Sprite), allowSceneObjects: false);
                myinspector.hydration = EditorGUILayout.FloatField("갈증", myinspector.hydration);
                break;

            case ItemType.recovery:
                myinspector.itemName = EditorGUILayout.TextField("Item Name", myinspector.itemName);
                myinspector.model = (GameObject)EditorGUILayout.ObjectField("Item Model", myinspector.model, typeof(GameObject), allowSceneObjects: false);
                myinspector.numbering = EditorGUILayout.IntField("Item Numbering", myinspector.numbering);
                myinspector.sale_price = EditorGUILayout.IntField("판매가", myinspector.sale_price);
                myinspector.itemIcon = (Sprite)EditorGUILayout.ObjectField("Item Icon", myinspector.itemIcon, typeof(Sprite), allowSceneObjects: false);
                myinspector.recovery = EditorGUILayout.FloatField("회복량", myinspector.recovery);
                break;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(myinspector);
        }
    }
}
