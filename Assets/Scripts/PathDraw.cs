using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDraw : MonoBehaviour
{
    public List<Hextile> tilesToDraw; // Contains all tiles for path to be drawn between

    public Color c1; // Gradient between c1 and c2
    public Color c2;

    private const int MAXLEN = 1000; // TODO: should be set to width*height of map
    private LineRenderer lineRenderer;
    private Vector3[] points = new Vector3[MAXLEN]; // Positions for all tiles
    private float lineHeight = 0.8f;

    void Start()
    {
        makeLr();
    }

    private void makeLr()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.1f;
        lineRenderer.positionCount = MAXLEN;
        lineRenderer.useWorldSpace = false;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 0.5f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;
    }

    void Update()
    {
        drawPoints();
    }

    public void EmptyList()
    {
        //TODO: reset the list so that it may be repopulated.
    }

    // Draws all paths that have been added.
    public void drawPoints()
    {
        for (int i = 0; i < tilesToDraw.Count; i++)
        {
            points[i].x = tilesToDraw[i].getPosition().x - transform.position.x;
            points[i].z = tilesToDraw[i].getPosition().z - transform.position.z;
            points[i].y = lineHeight;
        }
        lineRenderer.SetPositions(points);
    }

    // Tells PathDraw to add another node at position of Hextile h to be drawn.
    public void addNodeToPath(Hextile h)
    {
        tilesToDraw.Insert(0, h);
        //tilesToDraw.Add(h);
    }
}
