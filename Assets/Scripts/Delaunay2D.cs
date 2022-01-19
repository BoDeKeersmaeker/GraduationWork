using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delaunay2D : BaseAlgorithm
{
    public class Triangle
    {
        private Vector2 VertorX;
        private Vector2 VertorY;
        private Vector2 VertorZ;

        public Triangle(Vector2 vertorX, Vector2 vertorY, Vector2 vertorZ)
        {
            VertorX = vertorX;
            VertorY = vertorY;
            VertorZ = vertorZ;
        }
    }

    private List<Vector3> Vertices = null;
    private List<Triangle> Triangles = null;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Generate(GameObject[,,] rooms)
    {
        initializeVertices(rooms);
        CalculateSuperTriangle();
    }

    public void initializeVertices(GameObject[,,] rooms)
    {
        foreach(GameObject ro in rooms)
            Vertices.Add(ro.transform.position);
    }

    public void CalculateSuperTriangle()
    {
        float minX = Vertices[0].x;
        float minY = Vertices[0].z;
        float maxX = minX;
        float maxY = minY;

        foreach (var vertex in Vertices)
        {
            if (vertex.x < minX) 
                minX = vertex.x;
            if (vertex.x > maxX) 
                maxX = vertex.x;
            if (vertex.y < minY) 
                minY = vertex.y;
            if (vertex.y > maxY) 
                maxY = vertex.y;
        }

        float dx = maxX - minX;
        float dy = maxY - minY;
        float deltaMax = Mathf.Max(dx, dy) * 2;

        Vector2 VertorX = new Vector2( minX - 1, minY - 1);
        Vector2 VertorY = new Vector2(minX - 1, maxY + deltaMax);
        Vector2 VertorZ = new Vector2(maxX + deltaMax, minY - 1);

        Triangles.Add(new Triangle(VertorX, VertorY, VertorZ));
    }
}