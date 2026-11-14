using System.Collections.Generic;
using UnityEngine;

public class BalaArea : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    [SerializeField] private float velocidad = 10f;
    [SerializeField] private float tiempoDeVidaMaximo = 5f;

    [Header("Ajustes de Daño en Área (AoE)")]
    [SerializeField] private float radioExplosion = 3.5f;
    [SerializeField] private float danoDeAtaque = 25f;
    [SerializeField] private string tagEnemigo = "Enemigo";

    private bool haExplotado;
    private Vector3 posicionTorre;
    private float rangoMaximo = Mathf.Infinity;
    private bool tieneRestriccionDeRango;

    public void ConfigurarRango(Vector3 origenTorre, float rangoTorre)
    {
        posicionTorre = origenTorre;
        rangoMaximo = rangoTorre;
        tieneRestriccionDeRango = true;
    }

    private void Start()
    {
        Destroy(gameObject, tiempoDeVidaMaximo);
    }

    private void Update()
    {
        if (haExplotado) return;

        transform.Translate(Vector3.right * velocidad * Time.deltaTime);

        if (tieneRestriccionDeRango && Vector3.Distance(transform.position, posicionTorre) > rangoMaximo)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (haExplotado) return;

        VidaObj enemigoImpactado = BuscarEnemigo(other);
        if (enemigoImpactado == null) return;

        Explotar(other.ClosestPoint(transform.position), enemigoImpactado);
    }

    private VidaObj BuscarEnemigo(Collider2D collider)
    {
        VidaObj vida = collider.GetComponentInParent<VidaObj>();
        if (vida != null && (vida.CompareTag(tagEnemigo) || collider.CompareTag(tagEnemigo)))
        {
            return vida;
        }

        return null;
    }

    private void Explotar(Vector2 puntoImpacto, VidaObj enemigoImpactado)
    {
        haExplotado = true;

        // Incluir todas las capas y los triggers, porque los enemigos usan Ignore Raycast.
        var filtro = new ContactFilter2D { useTriggers = true };
        var colliders = new List<Collider2D>();
        Physics2D.OverlapCircle(puntoImpacto, radioExplosion, filtro, colliders);

        var enemigos = new HashSet<VidaObj> { enemigoImpactado };
        foreach (Collider2D collider in colliders)
        {
            VidaObj enemigo = BuscarEnemigo(collider);
            if (enemigo != null)
            {
                enemigos.Add(enemigo);
            }
        }

        foreach (VidaObj enemigo in enemigos)
        {
            enemigo.RecibirDano(danoDeAtaque);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radioExplosion);
    }
}
