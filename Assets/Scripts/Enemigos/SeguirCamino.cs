using UnityEngine;

public class SeguirCamino : MonoBehaviour
{
    [Header("Configuración del Camino")]
    private Transform[] puntosDelCamino; 
    public float velocidad = 3f;
    public float dano = 25f;
    
    private int indiceSiguientePunto = 0; 
    private float distanciaMargen = 0.01f;

    public Transform[] PuntosDelCamino => puntosDelCamino;
    public int IndiceSiguientePunto => indiceSiguientePunto;
    
    public void SetCamino(Transform[] nuevoCamino, int indiceInicial = 0)
    {
        puntosDelCamino = nuevoCamino;
        indiceSiguientePunto = Mathf.Clamp(indiceInicial, 0, puntosDelCamino.Length - 1); 
    }

    void Update()
    {
        if (puntosDelCamino == null || puntosDelCamino.Length == 0) return;

        Transform puntoObjetivo = puntosDelCamino[indiceSiguientePunto];

        transform.position = Vector2.MoveTowards(
            transform.position, 
            puntoObjetivo.position, 
            velocidad * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, puntoObjetivo.position) < distanciaMargen)
        {
            indiceSiguientePunto++;

            if (indiceSiguientePunto >= puntosDelCamino.Length)
            {
                Transform ultimoPunto = puntosDelCamino[puntosDelCamino.Length - 1];
                NucleoVida nucleo = ultimoPunto.GetComponent<NucleoVida>();

                if (nucleo != null)
                {
                    nucleo.DanarNucleo(dano); 
                }

                FindAnyObjectByType<OleadaManip>()?.RegisterEnemyDeath(gameObject);

                Destroy(gameObject); 
            }
        }
    }
}
