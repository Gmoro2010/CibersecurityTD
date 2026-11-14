using UnityEngine;

[CreateAssetMenu(fileName = "BaseDeDatosCombinaciones", menuName = "Tower Defense/Base de Datos Combinaciones")]
public class BaseDeDatosCombinaciones : ScriptableObject
{
    [System.Serializable]
    public struct Receta
    {
        [Tooltip("ID de la primera torre")]
        public string idTorre1;
        [Tooltip("ID de la segunda torre")]
        public string idTorre2;
        [Tooltip("Prefab de la torre resultante al combinarlas")]
        public GameObject prefabResultado;
    }

    [Header("Lista de Combinaciones Válidas")]
    public Receta[] recetas;

    public GameObject BuscarCombinacion(string id1, string id2)
    {
        foreach (var receta in recetas)
        {
            // Comprobamos en ambos sentidos (A + B o B + A) para que el orden no importe
            bool matchDirecto = (receta.idTorre1 == id1 && receta.idTorre2 == id2);
            bool matchInverso = (receta.idTorre1 == id2 && receta.idTorre2 == id1);

            if (matchDirecto || matchInverso)
            {
                return receta.prefabResultado;
            }
        }
        return null; // Retorna nulo si no existe combinación
    }
}