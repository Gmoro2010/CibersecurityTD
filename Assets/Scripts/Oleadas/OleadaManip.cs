using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Estructura para agrupar y ordenar todos los puntos de un camino específico
[Unity.Properties.GeneratePropertyBag]
[System.Serializable]
public class RutaDeCamino
{
    public string nombreDelCamino;
    [Tooltip("Arrastra en orden todos los puntos de la ruta. El primero será el Spawn y el último la base.")]
    public Transform[] puntos; 
}

public class OleadaManip : MonoBehaviour
{
    [Header("Configuración de Rutas")]
    [Tooltip("Crea aquí tus caminos y asigna los puntos correspondientes de la escena")]
    public RutaDeCamino[] caminosDelNivel; 

    [Header("Lista de Oleadas del Nivel")]
    public List<OleadaData> levelWaves;

    private int currentWaveIndex = 0;
    private readonly HashSet<GameObject> activeEnemies = new HashSet<GameObject>();
    private bool isSpawning = false;
    private bool allWavesCompleted;
    private bool partidaTerminada;

    public int OleadaActual => currentWaveIndex;
    public int TotalOleadas => levelWaves?.Count ?? 0;
    public bool PuedeEmpezarOleada =>
        !partidaTerminada && Time.timeScale > 0f && !isSpawning &&
        activeEnemies.Count == 0 && currentWaveIndex < TotalOleadas;

    // Suscríbete a estos eventos desde tu interfaz (UI) si lo necesitas
    public System.Action<string> OnWaveChanged;
    public System.Action OnAllWavesCompleted;

    void Update()
    {
        if (!PuedeEmpezarOleada) return;

        var teclado = UnityEngine.InputSystem.Keyboard.current;
        if (teclado != null && teclado.spaceKey.wasPressedThisFrame)
            StartNextWave();
    }

    public void StartNextWave()
    {
        if (!PuedeEmpezarOleada) return;

        OleadaData wave = levelWaves[currentWaveIndex];
        currentWaveIndex++;
        StartCoroutine(SpawnWaveRoutine(wave));
    }

    private IEnumerator SpawnWaveRoutine(OleadaData wave)
    {
        isSpawning = true;
        OnWaveChanged?.Invoke(wave.waveName);

        // Procesamos cada grupo de enemigos configurado en la oleada
        foreach (SpawnGroup group in wave.enemyGroups)
        {
            // Esperar el retraso inicial del grupo si se especificó
            if (group.delayBeforeStart > 0)
            {
                yield return new WaitForSeconds(group.delayBeforeStart);
            }

            if (caminosDelNivel == null || caminosDelNivel.Length == 0 || group.enemyPrefab == null)
            {
                Debug.LogError("Falta configurar una ruta o el prefab del enemigo.");
                continue;
            }

            int caminoIndex = Mathf.Clamp(group.spawnPointIndex, 0, caminosDelNivel.Length - 1);
            Transform[] puntos = caminosDelNivel[caminoIndex].puntos;
            if (puntos == null || puntos.Length == 0)
            {
                Debug.LogError($"El camino en el índice {caminoIndex} no tiene puntos asignados.");
                continue;
            }

            // El punto de aparición (Spawn) siempre será el primer punto (índice 0) del camino seleccionado
            Transform spawnPoint = puntos[0];

            // Aparecer la cantidad de enemigos de este bloque
            for (int i = 0; i < group.count; i++)
            {
                GameObject enemy = Instantiate(group.enemyPrefab, spawnPoint.position, spawnPoint.rotation);

                // PASAR EL CAMINO AL ENEMIGO: Le inyectamos la ruta asignada de forma automática
                if (enemy.TryGetComponent<SeguirCamino>(out var scriptMovimiento))
                {
                    scriptMovimiento.SetCamino(puntos, oleada: this);
                }
                else
                {
                    Debug.LogWarning($"El prefab {enemy.name} no tiene el script 'SeguirCamino' adjunto.");
                    Destroy(enemy);
                }

                yield return new WaitForSeconds(group.timeBetweenEnemies);
            }
        }

        isSpawning = false;
        ComprobarFinDeOleada();
    }

    internal void RegisterEnemy(GameObject enemy)
    {
        activeEnemies.Add(enemy);
    }

    public void RegisterEnemyDeath(GameObject enemy)
    {
        if (activeEnemies.Remove(enemy))
            ComprobarFinDeOleada();
    }

    public void DetenerPartida()
    {
        partidaTerminada = true;
        StopAllCoroutines();
    }

    private void ComprobarFinDeOleada()
    {
        if (partidaTerminada || allWavesCompleted || isSpawning ||
            activeEnemies.Count > 0 || currentWaveIndex < TotalOleadas)
            return;

        allWavesCompleted = true;
        Debug.Log("¡Felicidades! Has defendido todas las oleadas.");
        OnAllWavesCompleted?.Invoke();
    }
}
