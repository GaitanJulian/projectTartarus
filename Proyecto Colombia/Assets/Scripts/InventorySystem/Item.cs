
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Scriptable objects/Item")]
public class Item : ScriptableObject
{
    public string _name = "New Item";
    public Sprite _icon = null;
}
