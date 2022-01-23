using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
public class Delaunay2D : BaseAlgorithm
{
    public class Triangle
    {
        private Vector2 VertorX;
        private Vector2 VertorY;
        private Vector2 VertorZ;
        public bool IsValid { get; set; }

        public Triangle(Vector2 vertorX, Vector2 vertorY, Vector2 vertorZ)
        {
            VertorX = vertorX;
            VertorY = vertorY;
            VertorZ = vertorZ;
            IsValid = true;
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
            IsValid = true;
        }
        public Vector2 GetVectorA()
        {
            return VertorA;
        }
        public Vector2 GetVectorB()
        {
            return VertorB;
        }
        public bool AlmostEqual(Line Line)
        {
            //if (VertorA == Line.GetVectorA() && VertorB == Line.GetVectorB())
            //    return true;
            //if (VertorA == Line.GetVectorB() && VertorB == Line.GetVectorA())
            //    return true;

            //return false;

            if (AlmostEqual(VertorA, Line.GetVectorA()) && AlmostEqual(VertorB, Line.GetVectorB()))
                return true;
            if (AlmostEqual(VertorA, Line.GetVectorB()) && AlmostEqual(VertorB, Line.GetVectorA()))
                return true;
            return false;
        }
        public bool AlmostEqual(Vector2 left, Vector2 right)
        {
            return AlmostEqual(left.x, right.x) && AlmostEqual(left.y, right.y);
        }
        public bool AlmostEqual(float x, float y)
        {
            return Mathf.Abs(x - y) <= float.Epsilon * Mathf.Abs(x + y) * 2
            || Mathf.Abs(x - y) < float.MinValue;
        }
    }

    private List<Vector3> Vertices = new List<Vector3>();
    private List<Triangle> Triangles = new List<Triangle>();
    private List<Line> Lines = new List<Line>();
    //private Triangle SuperTriangle = null;

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
        print("Starting delaunay triangulation");

        initializeVertices(rooms);
        CalculateSuperTriangle();
        GenerateTriangles();
        GenerateLines();

        print("Delaunay triangulation completed");
    }

    private void initializeVertices(GameObject[,,] rooms)
    {
        print("Start adding vertices");

        foreach (GameObject room in rooms)
        {
            if (room)
            {
                Vector3 temp = room.transform.position;
                Vertices.Add(temp);
                print("Vertex added");
            }
        }

        print("Finished adding vertices");
    }
    
    private void CalculateSuperTriangle()
    {
        print("Starting SuperTriangle creation");

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

        Vector2 VertorX = new Vector2(minX - 1, minY - 1);
        Vector2 VertorY = new Vector2(minX - 1, maxY + deltaMax);
        Vector2 VertorZ = new Vector2(maxX + deltaMax, minY - 1);

        Triangles.Add(new Triangle(VertorX, VertorY, VertorZ));
        //SuperTriangle = new Triangle(VertorX, VertorY, VertorZ);

        print("SuperTriangle created");
    }

    private void GenerateTriangles()
    {
        print("Start generation of triangles");

        foreach (Vector3 vertex in Vertices)
            AddPointToTriangulation(vertex);
        Triangles.RemoveAll((Triangle triangle) => triangle.SharesVector(Triangles[0]));

        print("Completed triangle generation");
    }    

    private void AddPointToTriangulation(Vector3 vertex)
    {
        print("Adding point to triangulation");

        List<Line> lines = new List<Line>();

        foreach (Triangle triangle in Triangles)
        {
            if (triangle.IsPointInCircumCircle(vertex))
            {
                triangle.IsValid = false;
                lines.Add(new Line(triangle.GetVectorX(), triangle.GetVectorY()));
                lines.Add(new Line(triangle.GetVectorY(), triangle.GetVectorZ()));
                lines.Add(new Line(triangle.GetVectorZ(), triangle.GetVectorX()));
                print("Triangle removed");
            }
        }

        Triangles.RemoveAll((Triangle triangle) => !triangle.IsValid);

        for (int i = 0; i < lines.Count; i++)
        {
            for (int j = i + 1; j < lines.Count; j++)
            {
                if (lines[i].AlmostEqual(lines[j]))
                {
                    lines[i].IsValid = false;
                    lines[j].IsValid = false;
                    print("2 lines removed");
                }
            }
        }

        lines.RemoveAll((Line line) => !line.IsValid);

        foreach (Line line in lines)
        {
            Triangles.Add(new Triangle(line.GetVectorA(), line.GetVectorB(), new Vector2(vertex.x, vertex.z)));
            print("Triangle added");
        }
    }

    private void GenerateLines()
    {
        print("Start generation of lines");

        HashSet<Line> lines = new HashSet<Line>();

        foreach (Triangle triangle in Triangles)
        {
            Line line = new Line(triangle.GetVectorX(), triangle.GetVectorY());
            if (lines.Add(line))
            {
                Lines.Add(line);
                print("Line added");
            }

            line = new Line(triangle.GetVectorY(), triangle.GetVectorZ());
            if (lines.Add(line))
            {
                Lines.Add(line);
                print("Line added");
            }

            line = new Line(triangle.GetVectorZ(), triangle.GetVectorX());
            if (lines.Add(line))
            {
                Lines.Add(line);
                print("Line added");
            }
        }

        print("Completed line generation");
    }

    public override void TempDebug()
    {
        //debug
        foreach (Line line in Lines)
        {
            print("Vector A: " + line.GetVectorA() + "Vector B: " + line.GetVectorB());
            //Debug.DrawLine(new Vector3(line.GetVectorA().x, line.GetVectorA().y, 0f), new Vector3(line.GetVectorB().x, line.GetVectorB().y, 0f), new Color(1f, 1f, 1f, 1f), 1000f);
            DrawLine(new Vector3(line.GetVectorA().x, 0f, line.GetVectorA().y), new Vector3(line.GetVectorB().x, 0f, line.GetVectorB().y), new Color(1f, 1f, 1f, 1f), 1000f);
        
        }
    }

    private void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        //lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }
}