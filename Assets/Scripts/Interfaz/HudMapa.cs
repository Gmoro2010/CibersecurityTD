using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public sealed class HudMapa : MonoBehaviour
{
    // El tablero de 4 unidades ocupa 400 px en el diseño, bajo la barra de 60 px.
    private const float AnchoDiseno = 480f;
    private const float AltoDiseno = 272f;
    private const float PixelesPorUnidad = 100f;

    [SerializeField] private Camera camaraMapa;
    [SerializeField] private RectTransform marco;
    [SerializeField] private BuildManager construccion;
    [SerializeField] private OleadaManip oleadas;
    [SerializeField] private CifrasPixel saldo;
    [SerializeField] private CifrasPixel contador;
    [SerializeField] private NucleoVida nucleo;
    [SerializeField] private CifrasPixel contadorVida;
    [SerializeField] private Button empezar;
    [SerializeField] private TorreItem[] torres;
    [SerializeField] private CifrasPixel[] precios;

    private Vector2Int resolucion;
    private int? ultimoSaldo;
    private Vector2Int ultimasOleadas = new Vector2Int(-1, -1);
    private Vector2Int ultimaVida = new Vector2Int(-1, -1);

    private void Start()
    {
        for (int i = 0; i < torres.Length; i++)
            precios[i].Mostrar(torres[i].costo.ToString(CultureInfo.InvariantCulture));

        LateUpdate();
    }

    private void LateUpdate()
    {
        var actual = new Vector2Int(Screen.width, Screen.height);
        if (actual != resolucion && actual.x > 0 && actual.y > 0)
        {
            resolucion = actual;
            float escala = Mathf.Min(actual.x / AnchoDiseno, actual.y / AltoDiseno);
            marco.localScale = new Vector3(escala, escala, 1f);
            camaraMapa.orthographicSize = actual.y / (2f * escala * PixelesPorUnidad);
        }

        int dinero = construccion.DineroActual;
        if (dinero != ultimoSaldo)
        {
            ultimoSaldo = dinero;
            saldo.Mostrar(dinero.ToString("N0", CultureInfo.InvariantCulture).Replace(',', '.'));
        }

        var cuenta = new Vector2Int(oleadas.OleadaActual, oleadas.TotalOleadas);
        if (cuenta != ultimasOleadas)
        {
            ultimasOleadas = cuenta;
            contador.Mostrar(FormattableString.Invariant($"{cuenta.x}/{cuenta.y}"));
        }

        if (nucleo != null && contadorVida != null)
        {
            var vida = new Vector2Int(
                Mathf.CeilToInt(nucleo.VidaActual),
                Mathf.CeilToInt(nucleo.VidaMaxima));
            if (vida != ultimaVida)
            {
                ultimaVida = vida;
                contadorVida.Mostrar(FormattableString.Invariant($"{vida.x}/{vida.y}"));
            }
        }

        empezar.interactable = oleadas.PuedeEmpezarOleada;
    }
}
