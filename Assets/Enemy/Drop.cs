[System.Serializable]
public class  Drop
{
    public Item[] items;
    public int experience;
    public int gold;

    public Item DropRandomItem()
    {
        if(items == null)
            return null;
        if(items.Length == 1)
            return items[0];

        int randomIndex = UnityEngine.Random.Range(0, items.Length);
        return items[randomIndex];
    }
}