using UnityEngine;

namespace Dzajna
{
    public class Item : ScriptableObject
    {
        [Header("Item Information")] 
        public Sprite ItemIcon;
        public string ItemName;
    }
}