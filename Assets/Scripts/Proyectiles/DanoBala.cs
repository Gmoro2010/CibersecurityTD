using UnityEngine;

public class DanoBala : MonoBehaviour
{
    [SerializeField] private float danoDeAtaque = 25f;
    private bool consumida;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (consumida) return;
        VidaObj sistemaVidaEnemigo = other.GetComponent<VidaObj>();

        if (sistemaVidaEnemigo != null)
        {
            consumida = true;
            sistemaVidaEnemigo.RecibirDano(danoDeAtaque);
            Destroy(gameObject);
        }
    }
}
