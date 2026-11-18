using System.Collections;
using UnityEngine;

public class EnemigoGenerador : MonoBehaviour
{
    [Header("Configuración de Invocación")]
    [SerializeField] private GameObject prefabEnemigoHijo; // Prefab del enemigo que va a generar
    [SerializeField] private float intervaloGeneracion = 5f; // Tiempo entre cada invocación

    private SeguirCamino miSeguirCamino;

    void Start()
    {
        miSeguirCamino = GetComponent<SeguirCamino>();
        StartCoroutine(RutinaGenerarEnemigos());
    }

    IEnumerator RutinaGenerarEnemigos()
    {
        yield return new WaitForSeconds(intervaloGeneracion);

        while (true)
        {
            GenerarEnemigo();
            yield return new WaitForSeconds(intervaloGeneracion);
        }
    }

    void GenerarEnemigo()
    {
        if (prefabEnemigoHijo == null || miSeguirCamino == null ||
            miSeguirCamino.PuntosDelCamino == null) return;

        // Instanciamos al enemigo hijo exactamente en la posición actual del jefe
        GameObject nuevoHijo = Instantiate(prefabEnemigoHijo, transform.position, Quaternion.identity);

        SeguirCamino scriptCaminoHijo = nuevoHijo.GetComponent<SeguirCamino>();
        
        if (scriptCaminoHijo != null)
        {
            scriptCaminoHijo.SetCamino(miSeguirCamino.PuntosDelCamino,
                miSeguirCamino.IndiceSiguientePunto, miSeguirCamino.Oleada);
        }
    }
}
