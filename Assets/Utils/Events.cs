using UnityEngine.Events;
// ReSharper disable All

public static class Events
{
    public static readonly UnityEvent OnLanguageChanged = new UnityEvent();
    public static readonly UnityEvent<string> OnUserConnected = new UnityEvent<string>();
    public static readonly UnityEvent<User> OnUserUpdate = new UnityEvent<User>();
    public static readonly UnityEvent<Item> OnItemAdded = new UnityEvent<Item>();
    public static readonly UnityEvent<Item> OnItemRemoved = new UnityEvent<Item>();
    public static readonly UnityEvent<Item> OnItemEquipped = new UnityEvent<Item>();
    public static readonly UnityEvent<Item> OnItemUnequipped = new UnityEvent<Item>();
    public static readonly UnityEvent<Drop> OnEnemyKilled = new UnityEvent<Drop>();
    public static readonly UnityEvent<int> OnExperienceGained = new UnityEvent<int>();
    public static readonly UnityEvent<int> OnLevelUp = new UnityEvent<int>();
    public static readonly UnityEvent<Item, int> OnItemSold = new UnityEvent<Item, int>();
    public static readonly UnityEvent<Item, int> OnItemBuy = new UnityEvent<Item, int>();
    public static readonly UnityEvent OnUserDisconnected = new UnityEvent();
    public static readonly UnityEvent<int> OnLifeLost = new UnityEvent<int>();
    public static readonly UnityEvent<int> OnLifeGained = new UnityEvent<int>();
}
