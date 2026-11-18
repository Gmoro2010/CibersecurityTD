using UnityEngine;

public class SeguirCamino : MonoBehaviour
{
    public float velocidad = 3f;
    public float dano = 25f;

    private Transform[] puntosDelCamino;
    private int indiceSiguientePunto;
    private bool eliminado;

    public Transform[] PuntosDelCamino => puntosDelCamino;
    public int IndiceSiguientePunto => indiceSiguientePunto;
    public OleadaManip Oleada { get; private set; }

    public void SetCamino(Transform[] nuevoCamino, int indiceInicial = 0, OleadaManip oleada = null)
    {
        puntosDelCamino = nuevoCamino;
        indiceSiguientePunto = Mathf.Clamp(indiceInicial, 0, puntosDelCamino.Length - 1);
        Oleada = oleada;
        Oleada?.RegisterEnemy(gameObject);
    }

    private void Update()
    {
        if (eliminado || Time.timeScale == 0f || puntosDelCamino == null || puntosDelCamino.Length == 0)
            return;

        Transform puntoObjetivo = puntosDelCamino[indiceSiguientePunto];
        transform.position = Vector2.MoveTowards(
            transform.position, puntoObjetivo.position, velocidad * Time.deltaTime);
        if (Vector2.Distance(transform.position, puntoObjetivo.position) >= 0.01f) return;

        indiceSiguientePunto++;
        if (indiceSiguientePunto < puntosDelCamino.Length) return;

        if (puntoObjetivo.TryGetComponent<NucleoVida>(out var nucleo))
            nucleo.DanarNucleo(dano);

        Retirar();
    }

    public void Retirar()
    {
        if (eliminado) return;
        eliminado = true;
        gameObject.SetActive(false);
        Oleada?.RegisterEnemyDeath(gameObject);
        Destroy(gameObject);
    }
}
