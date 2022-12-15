using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Script takes care of drawing a path of an object
/// </summary>
public class PathHighlighter : MonoBehaviour
{
    /// <summary>
    /// The actual linerenderer
    /// </summary>
    public LineRenderer PathRenderer;

    /// <summary>
    /// Color 1 for the gradient
    /// </summary>
    public Color c1 = Color.green;

    /// <summary>
    /// Color 2 for the gradient
    /// </summary>
    public Color c2 = Color.green;

    /// <summary>
    /// Positions of all Lines
    /// </summary>
    public List<Vector3> LinePositions = new List<Vector3>();

    // Start is called before the first frame update
    void Awake()
    {
        PathRenderer = gameObject.AddComponent<LineRenderer>();
        PathRenderer.material = new Material(Shader.Find("Sprites/Default"));
        PathRenderer.widthMultiplier = 0.2f;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        PathRenderer.colorGradient = gradient;

        PathRenderer.enabled = false;

    }

    
    /// <summary>
    /// Create the line by adding the LinePositions to the path renderer
    /// </summary>
    public void CreateLine()
    {
        PathRenderer.positionCount = LinePositions.ToArray().Length;
        PathRenderer.SetPositions(LinePositions.ToArray());
    }

    /// <summary>
    /// Smoothes a curve.
    /// Method written by Jan Henry Belz & Mark Colley at Ulm University.
    /// </summary>
    /// <param name="arrayToCurve"></param>
    /// <param name="smoothness"></param>
    /// <returns></returns>
    [Obsolete("Currently unused because insanely resource-heavy")]
    public Vector3[] MakeSmoothCurve(Vector3[] arrayToCurve, float smoothness)
    {
        List<Vector3> points;
        List<Vector3> curvedPoints;
        int pointsLength = 0;
        int curvedLength = 0;

        if (smoothness < 1.0f) smoothness = 1.0f;

        pointsLength = arrayToCurve.Length;

        curvedLength = (pointsLength * Mathf.RoundToInt(smoothness)) - 1;
        curvedPoints = new List<Vector3>(curvedLength);

        float t = 0.0f;

        for (int pointInTimeOnCurve = 0; pointInTimeOnCurve < curvedLength + 1; pointInTimeOnCurve++)
        {
            t = Mathf.InverseLerp(0, curvedLength, pointInTimeOnCurve);

            points = new List<Vector3>(arrayToCurve);

            for (int j = pointsLength - 1; j > 0; j--)
            {
                for (int i = 0; i < j; i++)
                {
                    points[i] = (1 - t) * points[i] + t * points[i + 1];
                }
            }
            curvedPoints.Add(points[0]);
        }
        return (curvedPoints.ToArray());
    }

}
