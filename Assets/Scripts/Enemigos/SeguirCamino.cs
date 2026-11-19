using UnityEngine;

public class SeguirCamino : MonoBehaviour
{
    public float velocidad = 3f;
    public float dano = 25f;

    private Transform[] puntosDelCamino;
    private int indiceSiguientePunto;
    private bool eliminado;
    private float[] distanciasDesdePunto;

    public Transform[] PuntosDelCamino => puntosDelCamino;
    public int IndiceSiguientePunto => indiceSiguientePunto;
    public OleadaManip Oleada { get; private set; }
    public float DistanciaRestante => puntosDelCamino != null && indiceSiguientePunto < puntosDelCamino.Length
        ? Vector2.Distance(transform.position, puntosDelCamino[indiceSiguientePunto].position) +
            distanciasDesdePunto[indiceSiguientePunto]
        : Mathf.Infinity;

    public void SetCamino(Transform[] nuevoCamino, int indiceInicial = 0, OleadaManip oleada = null)
    {
        if (nuevoCamino == null || nuevoCamino.Length == 0) return;
        puntosDelCamino = nuevoCamino;
        distanciasDesdePunto = new float[puntosDelCamino.Length];
        for (int indice = puntosDelCamino.Length - 2; indice >= 0; indice--)
            distanciasDesdePunto[indice] = distanciasDesdePunto[indice + 1] +
                Vector2.Distance(puntosDelCamino[indice].position, puntosDelCamino[indice + 1].position);
        indiceSiguientePunto = Mathf.Clamp(indiceInicial, 0, puntosDelCamino.Length - 1);
        Oleada = oleada;
        Oleada?.RegisterEnemy(gameObject);
    }

    public Vector2 PredecirPosicion(float segundos)
    {
        Vector2 posicion = transform.position;
        if (puntosDelCamino == null) return posicion;

        float distanciaDisponible = velocidad * segundos;
        for (int indice = indiceSiguientePunto; indice < puntosDelCamino.Length; indice++)
        {
            Vector2 destino = puntosDelCamino[indice].position;
            float distancia = Vector2.Distance(posicion, destino);
            if (distancia >= distanciaDisponible)
                return Vector2.MoveTowards(posicion, destino, distanciaDisponible);

            posicion = destino;
            distanciaDisponible -= distancia;
        }
        return posicion;
    }

    private void Update()
    {
        if (eliminado || Time.timeScale == 0f || puntosDelCamino == null || puntosDelCamino.Length == 0)
            return;

        float distanciaDisponible = velocidad * Time.deltaTime;
        while (indiceSiguientePunto < puntosDelCamino.Length)
        {
            Transform puntoObjetivo = puntosDelCamino[indiceSiguientePunto];
            float distancia = Vector2.Distance(transform.position, puntoObjetivo.position);
            transform.position = Vector2.MoveTowards(transform.position, puntoObjetivo.position, distanciaDisponible);
            if (distancia > distanciaDisponible) return;

            distanciaDisponible -= distancia;
            indiceSiguientePunto++;
            if (indiceSiguientePunto != puntosDelCamino.Length) continue;

            puntoObjetivo.TryGetComponent<NucleoVida>(out var nucleo);
            Retirar(nucleo);
        }
    }

    public void Retirar() => Retirar(null);

    private void Retirar(NucleoVida nucleo)
    {
        if (eliminado) return;
        eliminado = true;
        gameObject.SetActive(false);
        // Resolver el impacto antes de que el gestor pueda cerrar la oleada.
        if (nucleo != null) nucleo.DanarNucleo(dano);
        Oleada?.RegisterEnemyDeath(gameObject);
        Destroy(gameObject);
    }
}
