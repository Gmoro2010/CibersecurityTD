using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnGroup
{
    [Header("Configuración del Enemigo")]
    public GameObject enemyPrefab;
    public int count = 5;
    
    [Header("Tiempos e Intervalos")]
    public float delayBeforeStart = 0f; // Espera antes de que empiece este grupo
    public float timeBetweenEnemies = 1f; // Cadencia de aparición

    [Header("Ruta (Opcional)")]
    public int spawnPointIndex = 0; // Por si tienes múltiples entradas en tu Tower Defense
}

[CreateAssetMenu(fileName = "NuevaOleada", menuName = "Tower Defense/Oleada")]
public class OleadaData : ScriptableObject
{
    public string waveName = "Oleada 1";
    public List<SpawnGroup> enemyGroups;
    public float timeAfterWave = 10f; // Tiempo de descanso antes de la siguiente oleada
}
