using UnityEngine;


[CreateAssetMenu(fileName="New Upgrade", menuName = "Game/Upgrades")]
public class UpgradeContainers : ScriptableObject
{
    public int id;
    public string upgradeName;
    public string description;
    public Sprite icon;
}
