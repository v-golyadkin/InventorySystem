using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(fileName = "ItemParameter", menuName = "Scriptable Objects/ItemParameter")]
    public class ItemParameterSO : ScriptableObject
    {
        [field: SerializeField] public string ParameterName {  get; private set; }
    }
}