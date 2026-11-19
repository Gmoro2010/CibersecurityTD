using UnityEngine;

[RequireComponent(typeof(SeguirCamino))]
public sealed class JefeLaser : MonoBehaviour
{
    [SerializeField] private Material materialRayo;
    [SerializeField, Min(0.1f)] private float intervalo = 7.5f;
    [SerializeField, Min(0.1f)] private float duracion = 5f;

    private BaseTorre objetivo;
    private LineRenderer halo;
    private LineRenderer centro;
    private double siguienteDisparo;
    private double finRayo;

    private void Start()
    {
        siguienteDisparo = Time.timeAsDouble + intervalo;
        halo = CrearLinea("Halo laser", 0.045f, new Color(1f, 0.2f, 0.56f, 0.8f), 8);
        centro = CrearLinea("Rayo laser", 0.016f, new Color32(255, 235, 252, 255), 9);
    }

    private void LateUpdate()
    {
        double ahora = Time.timeAsDouble;
        if (Time.timeScale > 0f && ahora >= siguienteDisparo)
        {
            siguienteDisparo += intervalo;
            Disparar(ahora);
        }

        bool visible = objetivo != null && ahora < finRayo;
        if (halo == null || centro == null) return;
        halo.enabled = centro.enabled = visible;
        if (!visible) return;

        // El pivote de ambos sprites está en el centro del ojo.
        Vector3 origen = transform.position;
        Vector3 destino = objetivo.transform.position;
        halo.SetPosition(0, origen);
        halo.SetPosition(1, destino);
        centro.SetPosition(0, origen);
        centro.SetPosition(1, destino);
    }

    private void Disparar(double ahora)
    {
        float distanciaMinima = Mathf.Infinity;
        objetivo = null;
        foreach (var baseTorre in FindObjectsByType<BaseTorre>())
        {
            if (baseTorre.torreEnBase == null || !baseTorre.torreEnBase.gameObject.activeInHierarchy) continue;
            float distancia = ((Vector2)(baseTorre.transform.position - transform.position)).sqrMagnitude;
            if (distancia >= distanciaMinima) continue;
            distanciaMinima = distancia;
            objetivo = baseTorre;
        }

        if (objetivo == null) return;
        objetivo.Inhabilitar(duracion);
        finRayo = ahora + duracion;
    }

    private LineRenderer CrearLinea(string nombre, float ancho, Color color, int orden)
    {
        if (materialRayo == null) return null;
        var linea = new GameObject(nombre).AddComponent<LineRenderer>();
        linea.transform.SetParent(transform, false);
        linea.sharedMaterial = materialRayo;
        linea.positionCount = 2;
        linea.startWidth = linea.endWidth = ancho;
        linea.startColor = linea.endColor = color;
        linea.sortingOrder = orden;
        linea.enabled = false;
        return linea;
    }
}
