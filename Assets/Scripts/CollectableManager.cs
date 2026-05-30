using UnityEngine;

[System.Serializable]
public class Item
{
    public string Name;
    public int Level;
    public int Position;
    public bool Collected;
}

[System.Serializable]
public class CollectableRoot
{
    public Item[] Collectables;

}

public class CollectableManager : MonoBehaviour
{
    
    private TextAsset jsonFile;
    private CollectableRoot root;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake(){
        jsonFile = Resources.Load<TextAsset>("JSONs/Collectable");
        Debug.Log(jsonFile == null ? "NICHT gefunden" : "Gefunden!");
        root = JsonUtility.FromJson<CollectableRoot>(jsonFile.text);
    }

    public Item GetCollectable(int level, int position)
    {   
        foreach (Item item in root.Collectables)
        {
            if (item.Level == level && item.Position == position)
            {
                return item;
            }
        }
        return null;
    }
    public void setCollected(Item item)
    {
        item.Collected = true;
        Debug.Log(item.Name + " collected!");
    }
    public void setUncollected(Item item)
    {
        item.Collected = false;
        Debug.Log(item.Name + " uncollected!");
    }
    public bool isCollected(Item item)
    {
        return item.Collected;
    }


}
