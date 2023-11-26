using UnityEngine;

[CreateAssetMenu(menuName = "Roguelike/CollectableSO")]
public class CollectableSO : ScriptableObject
{
    public ItemType itemType;
    public Vector3 LocalCollectedPos = new Vector3(0, 1, 0);
}