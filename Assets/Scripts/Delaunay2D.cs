using System;
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
        public bool IsValid {get; set;}

        public Triangle(Vector2 vertorX, Vector2 vertorY, Vector2 vertorZ)
        {
            VertorX = vertorX;
            VertorY = vertorY;
            VertorZ = vertorZ;
        }
        public Vector2 GetVectorX()
        {
            return VertorX;
        }
        public Vector2 GetVectorY()
        {
            return VertorY;
        }
        public Vector2 GetVectorZ()
        {
            return VertorZ;
        }
        public bool SharesVector(Triangle triangle)
        {
            if (Vector3.Distance(VertorX, triangle.GetVectorX()) < 0.01f)
                return true;
            if (Vector3.Distance(VertorY, triangle.GetVectorX()) < 0.01f)
                return true;
            if (Vector3.Distance(VertorZ, triangle.GetVectorX()) < 0.01f)
                return true;

            if (Vector3.Distance(VertorX, triangle.GetVectorY()) < 0.01f)
                return true;
            if (Vector3.Distance(VertorY, triangle.GetVectorY()) < 0.01f)
                return true;
            if (Vector3.Distance(VertorZ, triangle.GetVectorY()) < 0.01f)
                return true;

            if (Vector3.Distance(VertorX, triangle.GetVectorZ()) < 0.01f)
                return true;
            if (Vector3.Distance(VertorY, triangle.GetVectorZ()) < 0.01f)
                return true;
            if (Vector3.Distance(VertorZ, triangle.GetVectorZ()) < 0.01f)
                return true;

            return false;
        }
        public bool IsPointInCircumCircle(Vector2 v)
        {
            float ab = VertorX.sqrMagnitude;
            float cd = VertorY.sqrMagnitude;
            float ef = VertorZ.sqrMagnitude;

            float circumX = (ab * (VertorZ.y - VertorY.y) + cd * (VertorX.y - VertorZ.y) + ef * (VertorY.y - VertorX.y)) / (VertorX.x * (VertorZ.y - VertorY.y) + VertorY.x * (VertorX.y - VertorZ.y) + VertorZ.x * (VertorY.y - VertorX.y));
            float circumY = (ab * (VertorZ.x - VertorY.x) + cd * (VertorX.x - VertorZ.x) + ef * (VertorY.x - VertorX.x)) / (VertorX.y * (VertorZ.x - VertorY.x) + VertorY.y * (VertorX.x - VertorZ.x) + VertorZ.y * (VertorY.x - VertorX.x));

            Vector2 circum = new Vector2(circumX / 2f, circumY / 2f);
            float circumRadius = Vector2.SqrMagnitude(VertorX - circum);
            float dist = Vector2.SqrMagnitude(v - circum);
            return dist <= circumRadius;
        }
    }

    public class Line
    {
        private Vector2 VertorA;
        private Vector2 VertorB;
        public bool IsValid { get; set; }

        public Line(Vector2 vertorA, Vector2 vertorB)
        {
            VertorA = vertorA;
            VertorB = vertorB;
        }
        public Vector2 GetVectorA()
        {
            return VertorA;
        }
        public Vector2 GetVectorB()
        {
            return VertorB;
        }
        public bool Equals(Line Line)
        {
            if (VertorA == Line.GetVectorA() && VertorB == Line.GetVectorB())
                return true;
            if (VertorA == Line.GetVectorB() && VertorB == Line.GetVectorA())
                return true;

            return false;
        }
    }

    private List<Vector3> Vertices = new List<Vector3>();
    private List<Triangle> Triangles = new List<Triangle>();
    private List<Line> Lines = new List<Line>();
    private Triangle SuperTriangle = null;

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
        GenerateTriangles();
        GenerateLines();
    }

    private void initializeVertices(GameObject[,,] rooms)
    {
        foreach(GameObject room in rooms)
            if(room)
                Vertices.Add(room.transform.position);
    }
    
    //rework later
    private void CalculateSuperTriangle()
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

    private void GenerateTriangles()
    {
        foreach (Vector3 vertex in Vertices)
            AddPointToTriangulation(vertex);
        Triangles.RemoveAll((Triangle triangle) => triangle.SharesVector(SuperTriangle));
    }    

    private void AddPointToTriangulation(Vector3 vertex)
    {
        List<Line> line = new List<Line>();

        foreach (Triangle triangle in Triangles)
        {
            if (triangle.IsPointInCircumCircle(vertex))
            {
                triangle.IsValid = false;
                line.Add(new Line(triangle.GetVectorX(), triangle.GetVectorY()));
                line.Add(new Line(triangle.GetVectorY(), triangle.GetVectorZ()));
                line.Add(new Line(triangle.GetVectorZ(), triangle.GetVectorX()));
             }
        }

        Triangles.RemoveAll((Triangle triangle) => !triangle.IsValid);

        for (int i = 0; i < line.Count; i++)
        {
            for (int j = i + 1; j < line.Count; j++)
            {
                if (line[i].Equals(line[j]))
                {
                    line[i].IsValid = false;
                    line[j].IsValid = false;
                }
            }
        }

        line.RemoveAll((Line e) => e.IsValid);

        foreach (Line temp in line)
            Triangles.Add(new Triangle(temp.GetVectorA(), temp.GetVectorB(), vertex));
    }

    private void GenerateLines()
    {
        HashSet<Line> lines = new HashSet<Line>();

        foreach (Triangle triangle in Triangles)
        {
            Line line = new Line(triangle.GetVectorX(), triangle.GetVectorY());
            if (lines.Add(line))
                Lines.Add(line);

            line = new Line(triangle.GetVectorY(), triangle.GetVectorZ());
            if (lines.Add(line))
                Lines.Add(line);

            line = new Line(triangle.GetVectorZ(), triangle.GetVectorX());
            if (lines.Add(line))
                Lines.Add(line);
        }
    }
}