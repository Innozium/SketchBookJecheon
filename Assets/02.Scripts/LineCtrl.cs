using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCtrl : MonoBehaviour {

    public int touchId = 0;

    public Material material;
    public Color color;
    public LineRenderer lineRenderer;
    public SpriteRenderer spriteRenderer;
    public float startWidth = 0.025f;
    public float endWidth = 0.025f;
    public float offsetBetweenPoints = 0.5f;
    public float pointZPosition;
    public int sortingOrder = 0;
    private List<Vector3> points = new List<Vector3>();

    private Transform mainTr;

    public void Create()
    {
        mainTr = GetComponent<Transform>();

        color = Color.black;

        if (material == null)
        {
            material = new Material(Shader.Find("Sprites/Default"));
        }

        points = new List<Vector3>();
        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer != null)
        {
            lineRenderer.material = material;
            lineRenderer.startWidth = startWidth;
            lineRenderer.endWidth = endWidth;
            //lineRenderer.SetWidth(startWidth, endWidth); 5.5 이전용
        }

        SetColor(color);

        //lineRenderer.numCapVertices = 5;
        //lineRenderer.numCornerVertices = 5;

    }

    public void SpriteCreate()
    {
        mainTr = GetComponent<Transform>();

        color = Color.white;

        if (material == null)
        {
            material = new Material(Shader.Find("Sprites/Default"));
        }

        spriteRenderer = GetComponent<SpriteRenderer>();


        if (spriteRenderer != null)
        {
            spriteRenderer.material = material;
        }
    }

    public void SpriteImgSetUp(Sprite sprite)
    {
        
        this.spriteRenderer.sprite = sprite;
    }

    public void AddPoint(Vector3 point)
    {
        if (lineRenderer != null)
        {
            point.z = pointZPosition;
            if (points.Count > 1)
            {
                if (Vector3.Distance(point, points[points.Count - 1]) < offsetBetweenPoints)
                {
                    return;
                }
            }

            points.Add(point);
            //lineRenderer.SetVertexCount(points.Count);
            lineRenderer.numPositions = (points.Count); //5.5버전
            lineRenderer.SetPosition(points.Count - 1, point);
            
        }

    }

    public void SpritePos(Vector3 point)
    {
        if (spriteRenderer != null)
        {
            point.z = pointZPosition;
            mainTr.position = point;
        }
    }

    public void SetMaterial(Material material)
    {
        this.material = material;
        if (lineRenderer != null)
        {
            lineRenderer.material = this.material;
        }
    }

    public void SetWidth(float startWidth, float endWidth)
    {
        if (lineRenderer != null)
        {
            lineRenderer.startWidth = startWidth;
            lineRenderer.endWidth = endWidth;
            //lineRenderer.SetWidth(startWidth, endWidth);
        }
        this.startWidth = startWidth;
        this.endWidth = endWidth;

    }

    public void SetColor(Color value)
    {
        if (lineRenderer != null)
        {
            print(value);
            lineRenderer.startColor = value;
            lineRenderer.endColor = value;
            //lineRenderer.SetColors(value, value);
        }
        color = value;
    }

    public void SetSortingOrder(int sorting)
    {
        if (lineRenderer != null)
        {
            lineRenderer.sortingOrder = sorting;
        }
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = sorting;
        }

        //Input.acceleration

        sortingOrder = sorting;
    }

    private void SetLinePoints(List<Vector3> drawingPoints)
    {
        lineRenderer.numPositions = (drawingPoints.Count);
        //lineRenderer.SetVertexCount(drawingPoints.Count);
    }

}
