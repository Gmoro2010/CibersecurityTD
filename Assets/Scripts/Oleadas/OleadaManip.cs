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
    private int activeEnemiesCount = 0;
    private bool isSpawning = false;

    // Suscríbete a estos eventos desde tu interfaz (UI) si lo necesitas
    public System.Action<string> OnWaveChanged;
    public System.Action OnAllWavesCompleted;

    void Update()
    {
        // Esta es la forma correcta de detectar la Barra Espaciadora en el nuevo Input System
        if (UnityEngine.InputSystem.Keyboard.current != null && 
            UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StartNextWave();
        }
    }

    public void StartNextWave()
    {
        if (isSpawning || activeEnemiesCount > 0) return; // No iniciar si ya hay una activa o enemigos vivos

        if (currentWaveIndex < levelWaves.Count)
        {
            StartCoroutine(SpawnWaveRoutine(levelWaves[currentWaveIndex]));
        }
        else
        {
            Debug.Log("¡Felicidades! Has defendido todas las oleadas.");
            OnAllWavesCompleted?.Invoke();
        }
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

            // Validar que el índice de camino configurado en el Scriptable Object exista
            int caminoIndex = Mathf.Clamp(group.spawnPointIndex, 0, caminosDelNivel.Length - 1);
            
            // Si el camino no tiene puntos configurados, saltamos el grupo para evitar errores
            if (caminosDelNivel[caminoIndex].puntos == null || caminosDelNivel[caminoIndex].puntos.Length == 0)
            {
                Debug.LogError($"El camino en el índice {caminoIndex} no tiene puntos asignados.");
                continue;
            }

            // El punto de aparición (Spawn) siempre será el primer punto (índice 0) del camino seleccionado
            Transform spawnPoint = caminosDelNivel[caminoIndex].puntos[0];

            // Aparecer la cantidad de enemigos de este bloque
            for (int i = 0; i < group.count; i++)
            {
                GameObject enemy = Instantiate(group.enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                activeEnemiesCount++;

                // PASAR EL CAMINO AL ENEMIGO: Le inyectamos la ruta asignada de forma automática
                if (enemy.TryGetComponent<SeguirCamino>(out var scriptMovimiento))
                {
                    scriptMovimiento.SetCamino(caminosDelNivel[caminoIndex].puntos);
                }
                else
                {
                    Debug.LogWarning($"El prefab {enemy.name} no tiene el script 'SeguirCamino' adjunto.");
                }

                yield return new WaitForSeconds(group.timeBetweenEnemies);
            }
        }

        isSpawning = false;
        currentWaveIndex++;
    }

    // Llama a esta función desde el script de tus enemigos cuando mueran o lleguen a la base
    public void RegisterEnemyDeath()
    {
        activeEnemiesCount--;

        if (activeEnemiesCount <= 0 && !isSpawning)
        {
            Debug.Log("Oleada limpia. Listo para la siguiente.");
            // Aquí puedes habilitar el botón de "Siguiente Oleada" en tu UI si tienes uno
        }
    }
}