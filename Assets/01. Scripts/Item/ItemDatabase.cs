using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] Item[] items;

    private void OnValidate()
    {
        LoadItems();
    }

    private void OnEnable()
    {
        EditorApplication.projectChanged -= LoadItems;
        EditorApplication.projectChanged += LoadItems;
    }
    private void OnDisable()
    {
        EditorApplication.projectChanged -= LoadItems;
    }

    private void LoadItems()
    {
        items = FindAssetByType<Item>("Assets/Use/Items");
    }

    public static T[] FindAssetByType<T>(params string[] folders) where T : Object
    {
        string type = typeof(T).Name;

        string[] guids;
        if (folders == null || folders.Length == 0)
        {
            guids = AssetDatabase.FindAssets($"t:{type}");
        }
        else
        {
            guids = AssetDatabase.FindAssets($"t:{type}", folders);
        }

        T[] assets = new T[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            assets[i] = AssetDatabase.LoadAssetAtPath<T>(assetPath);
        }

        return assets;
    }

    public string GetItemId(string name)
    {
        string id = null;
        foreach (var item in items)
        {
            if (item.name.Equals(name))
            {
                id = item.ID;
            }
        }
        if(id == null)
        {
            Debug.LogError($"Cant Found {name}'s ID");
        }

        return id;
    }
}