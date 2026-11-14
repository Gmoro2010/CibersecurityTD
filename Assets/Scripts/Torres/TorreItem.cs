using UnityEngine;

[CreateAssetMenu(fileName = "NuevaTorre", menuName = "Tower Defense/Torre Item")]
public class TorreItem : ScriptableObject
{
    public string nombre = "Torre Básica";
    public int costo = 100;
    public GameObject prefabTorre;
    public Sprite iconoUI;
}