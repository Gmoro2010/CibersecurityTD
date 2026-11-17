using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public sealed class CifrasPixel : MaskableGraphic
{
    // El atlas tiene una fila con doce celdas de 8 × 12 px.
    private const string Caracteres = "0123456789/.";
    private const float AnchoCelda = 8f;
    private const float AltoCelda = 12f;
    private const float Separacion = 1f;

    [SerializeField] private Texture2D atlas;
    [SerializeField] private string texto = "";

    public override Texture mainTexture => atlas != null ? atlas : base.mainTexture;

    public void Mostrar(string valor)
    {
        if (texto == valor) return;

        texto = valor;
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vertices)
    {
        vertices.Clear();
        if (atlas == null || string.IsNullOrEmpty(texto)) return;

        Rect rect = rectTransform.rect;
        float entreCifras = Separacion * (texto.Length - 1);
        float ancho = Mathf.Min(AnchoCelda, Mathf.Floor((rect.width - entreCifras) / texto.Length));
        if (ancho <= 0f) return;

        float alto = ancho * AltoCelda / AnchoCelda;
        float x = Mathf.Floor(rect.center.x - (texto.Length * ancho + entreCifras) / 2f);
        float y = Mathf.Floor(rect.center.y - alto / 2f);

        foreach (char caracter in texto)
        {
            int indice = Caracteres.IndexOf(caracter);
            if (indice >= 0)
            {
                float uMin = (float)indice / Caracteres.Length;
                float uMax = (float)(indice + 1) / Caracteres.Length;
                int inicio = vertices.currentVertCount;
                vertices.AddVert(new Vector3(x, y), color, new Vector2(uMin, 0));
                vertices.AddVert(new Vector3(x, y + alto), color, new Vector2(uMin, 1));
                vertices.AddVert(new Vector3(x + ancho, y + alto), color, new Vector2(uMax, 1));
                vertices.AddVert(new Vector3(x + ancho, y), color, new Vector2(uMax, 0));
                vertices.AddTriangle(inicio, inicio + 1, inicio + 2);
                vertices.AddTriangle(inicio, inicio + 2, inicio + 3);
            }
            x += ancho + Separacion;
        }
    }
}
