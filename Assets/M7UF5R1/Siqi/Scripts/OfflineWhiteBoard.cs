using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfflineWhiteBoard : MonoBehaviour
{
    private Texture2D texture;
    private Texture2D backgroundTexture;
    private List<Vector2Int> pixelBuffer = new List<Vector2Int>();

    public Color paintColor = Color.blue;
    public int brushSizeValue = 5;

    public Vector2? previousDot = null;
    public float interval = 0.5f;
    public bool isSending = false;

    [SerializeField]
    private MeshRenderer boardMaterial;

    [SerializeField]
    private Slider brushSize;

    void Start()
    {
        InitTexture();
        brushSize.onValueChanged.AddListener(value => brushSizeValue = (int)value);
    }

    private void InitTexture()
    {
        texture = new Texture2D(1920, 1080, TextureFormat.RGBA32, false);
        backgroundTexture = new Texture2D(texture.width, texture.height);

        for (int i = 0; i < backgroundTexture.width; i++)
        {
            for (int j = 0; j < backgroundTexture.height; j++)
            {
                backgroundTexture.SetPixel(i, j, Color.white);
            }
        }

        texture.SetPixels32(backgroundTexture.GetPixels32());
        texture.Apply();
        boardMaterial.material.mainTexture = texture;
    }

    public void PaintOnTexture(Vector2 uv)
    {
        if (texture == null) return;

        if (previousDot != null)
        {
            Vector2 start = previousDot.Value;
            Vector2 end = uv;

            float distance = Vector2.Distance(start, end);
            int steps = Mathf.CeilToInt(distance * texture.width);

            for (int step = 0; step <= steps; step++)
            {
                float t = (float)step / steps;
                Vector2 interpolatedUV = Vector2.Lerp(start, end, t);
                DrawPoint(interpolatedUV, paintColor, brushSizeValue);
            }
        }

        previousDot = uv;
    }

    public void DrawPoint(Vector2 uv, Color color, int brushSize)
    {
        int x = (int)(uv.x * texture.width);
        int y = (int)(uv.y * texture.height);

        pixelBuffer.Add(new Vector2Int(x, y));

        if (!isSending)
            StartCoroutine(SendPixelsCoroutine());
    }

    private IEnumerator SendPixelsCoroutine()
    {
        isSending = true;

        while (pixelBuffer.Count > 0)
        {
            yield return new WaitForSeconds(interval);

            foreach (var pixel in pixelBuffer)
            {
                for (int i = -brushSizeValue; i <= brushSizeValue; i++)
                {
                    for (int j = -brushSizeValue; j <= brushSizeValue; j++)
                    {
                        float distanceToCenter = Vector2.Distance(new Vector2(pixel.x, pixel.y), new Vector2(pixel.x + i, pixel.y + j));
                        int px = Mathf.Clamp(pixel.x + i, 0, texture.width - 1);
                        int py = Mathf.Clamp(pixel.y + j, 0, texture.height - 1);

                        if (distanceToCenter < brushSizeValue)
                        {
                            texture.SetPixel(px, py, paintColor);
                        }
                    }
                }
            }

            texture.Apply();
            pixelBuffer.Clear();
        }

        isSending = false;
    }

    public void EraseBoard()
    {
        texture.SetPixels32(backgroundTexture.GetPixels32());
        texture.Apply();
    }

    public void SetBrushColor(Color color)
    {
        paintColor = color;
    }
}
