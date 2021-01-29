using BimGisCad.Representation.Geometry.Elementary;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BimGisCad.Representation.Geometry.Composed
{
    /// <summary>
    /// Klasse für TIN Objekte, Achtung Dreiecksindizes müssen gegen Uhrzeigersinn geordnet sein!
    /// </summary>
    public readonly struct Tin
    {
        /// <summary>
        /// Punkte des TIN
        /// </summary>
        public IReadOnlyList<Point3> Points { get; }

        /// <summary>
        /// Dreiecke, Indizes der Vertizes (gegen Uhrzeigersinn ist positive Fläche!)
        /// </summary>
        public IReadOnlyList<int> Triangles { get; }

        /// <summary>
        /// Indizes der Nachbardreiecke (an Kante gegenüber Vertex, wenn negativ dann ohne Nachbar)
        /// </summary>
        public IReadOnlyList<int> Neighbours { get; }

        /// <summary>
        /// Markierte Kanten (in der Regel Bruchkante, im Dreieck die Kante gegenüber Vertex)
        /// </summary>
        public BitArray MarkedEdges { get; }

        /// <summary>
        /// Sind markierte Kanten (Bruchlinien) vorhanden ?
        /// </summary>
        public bool HasMarkedLines => MarkedEdges.Length > 0;

        /// <summary>
        /// Sind Nachbardreiecke definiert ?
        /// </summary>
        public bool HasNeighBours => Neighbours.Count > 0;

        /// <summary>
        /// Anzahl der Dreiecke
        /// </summary>
        public int NumTriangles => Triangles.Count / 3;

        /// <summary>
        /// Konstruktor 
        /// </summary>
        /// <param name="points">Punkte des TIN</param>
        /// <param name="triangles">Dreiecke des TIN</param>
        /// <param name="neighbours">Nachbardreiecke</param>
        /// <param name="markedEdges">Markierte Kanten</param>
        public Tin(in Point3[] points, in int[] triangles, in int[] neighbours, in BitArray markedEdges)
        {
            Points = points;
            Triangles = triangles;
            Neighbours = neighbours;
            MarkedEdges = markedEdges;
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="points">Punkte des TIN</param>
        /// <param name="triangles">Dreiecke des TIN</param>
        /// <param name="neighbours">Nachbardreiecke</param>
        /// <param name="markedEdges">Markierte Kanten</param>
        public Tin(Point3[] points, int[] triangles, int[] neighbours, bool[] markedEdges)
        {
            Points = points;
            Triangles = triangles;
            Neighbours = neighbours;
            MarkedEdges = new BitArray(markedEdges);
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="points">Punkte des TIN</param>
        /// <param name="triangles">Dreiecke des TIN</param>
        /// <param name="neighbours">Nachbardreiecke</param>
        public Tin(Point3[] points, int[] triangles, int[] neighbours)
        {
            Points = points;
            Triangles = triangles;
            Neighbours = neighbours;
            MarkedEdges = new BitArray(0);
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="points">Punkte des TIN</param>
        /// <param name="triangles">Dreiecke des TIN</param>
        /// <param name="markedEdges">Markierte Kanten</param>
        public Tin(Point3[] points, int[] triangles, BitArray markedEdges)
        {
            Points = points;
            Triangles = triangles;
            Neighbours = Array.Empty<int>();
            MarkedEdges = markedEdges;
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="points">Punkte des TIN</param>
        /// <param name="triangles">Dreiecke des TIN</param>
        /// <param name="markedEdges">Markierte Kanten</param>
        public Tin(Point3[] points, int[] triangles, bool[] markedEdges)
        {
            Points = points;
            Triangles = triangles;
            Neighbours = Array.Empty<int>();
            MarkedEdges = new BitArray(markedEdges);
        }

        /// <summary>
        /// Konstrukor
        /// </summary>
        /// <param name="points">Punkte des TIN</param>
        /// <param name="triangles">Dreiecke des TIN</param>
        public Tin(Point3[] points, int[] triangles)
        {
            Points = points;
            Triangles = triangles;
            Neighbours = Array.Empty<int>();
            MarkedEdges = new BitArray(0);
        }

        /// <summary>
        /// Punktindizes der Dreiecksvertizes
        /// </summary>
        /// <param name="triangleIndex">Index des Dreiecks</param>
        /// <param name="pointIndexVertexA">Punktindex des ersten Vertex</param>
        /// <param name="pointIndexVertexB">Punktindex des zweiten Vertex</param>
        /// <param name="pointIndexVertexC">Punktindex des dritten Vertex</param>
        public void TriangleVertexPointIndizesAt(int triangleIndex, out int pointIndexVertexA, out int pointIndexVertexB, out int pointIndexVertexC)
        {
            int i0 = triangleIndex * 3;
            pointIndexVertexA = Triangles[i0];
            pointIndexVertexB = Triangles[i0 + 1];
            pointIndexVertexC = Triangles[i0 + 2];
        }

        /// <summary>
        /// Vertizes des Dreiecks
        /// </summary>
        /// <param name="triangleIndex">Index des Dreiecks</param>
        /// <param name="vertexA">Erster Vertex</param>
        /// <param name="vertexB">Zweiter Vertex</param>
        /// <param name="vertexC">Dritter Vertex</param>
        public void TriangleVertizesAt(int triangleIndex, out Point3 vertexA, out Point3 vertexB, out Point3 vertexC)
        {
            int i0 = triangleIndex * 3;
            vertexA = Points[Triangles[i0]];
            vertexB = Points[Triangles[i0 + 1]];
            vertexC = Points[Triangles[i0 + 2]];
        }

        /// <summary>
        /// Indizes der Nachbardreiecke eines Dreiecksv
        /// </summary>
        /// <param name="triangleIndex">Index des Dreiecks</param>
        /// <param name="neighbourIndexA">Index des Nachbardreieckes genüber des ersten Vertex</param>
        /// <param name="neighbourIndexB">Index des Nachbardreieckes genüber des zweiten Vertex</param>
        /// <param name="neighbourIndexC">Index des Nachbardreieckes genüber des dritten Vertex</param>
        public void TriangleNeighbourIndizesAt(int triangleIndex, out int neighbourIndexA, out int neighbourIndexB, out int neighbourIndexC)
        {
            int i0 = triangleIndex * 3;
            neighbourIndexA = Neighbours[i0];
            neighbourIndexB = Neighbours[i0 + 1];
            neighbourIndexC = Neighbours[i0 + 2];
        }

        /// <summary>
        /// Aufzählung aller Dreiecke (Punktindizes der Vertizes)
        /// </summary>
        public IEnumerable<int[]> TriangleVertexPointIndizes()
        {
            for (int i = 0; i < Triangles.Count;)
            {
                yield return new int[] { Triangles[i++], Triangles[i++], Triangles[i++] };
            }
        }

        /// <summary>
        /// Aufzählung aller Dreiecke (Vertizes)
        /// </summary>
        public IEnumerable<Point3[]> TriangleVertizes()
        {
            for (int i = 0; i < Triangles.Count;)
            {
                yield return new Point3[] { Points[Triangles[i++]], Points[Triangles[i++]], Points[Triangles[i++]] };
            }
        }

        /// <summary>
        /// Aufzählung aller Nachbardreiecksindizes
        /// </summary>
        public IEnumerable<int[]> TriangleNeighbourIndizes()
        {
            for (int i = 0; i < Neighbours.Count;)
            {
                yield return new int[] { Neighbours[i++], Neighbours[i++], Neighbours[i++] };
            }
        }

        /// <summary>
        /// Aufzählung aller markierten Kanten (Punktindizes)
        /// </summary>
        public IEnumerable<int[]> MarkedEdgePointIndizes()
        {
            for (int i = 0; i < MarkedEdges.Length;i++)
            {
                if(MarkedEdges[i])
                {
                    yield return new[] { Triangles[i + (i + 1) % 3], Triangles[i + (i + 2) % 3] };
                }
            }
        }

        /// <summary>
        /// Aufzählung aller markierten Kanten (Punktindizes)
        /// </summary>
        public IEnumerable<Point3[]> MarkedEdgeVertizes()
        {
            for (int i = 0; i < MarkedEdges.Length; i++)
            {
                if (MarkedEdges[i])
                {
                    yield return new[] { Points[Triangles[i + (i + 1) % 3]], Points[Triangles[i + (i + 2) % 3]] };
                }
            }
        }

        /// <summary>
        /// Erzeugt leeren Builder
        /// </summary>
        /// <returns></returns>
        public static Tin.Builder CreateBuilder() => new Tin.Builder();

        /// <summary>
        /// Klasse zum Erzeugen eines Tin durch schrittweises Hinzufügen von Elementen
        /// </summary>
        public class Builder
        {
            private readonly List<Point3> _points;
            private readonly List<int> _triangles;
            private readonly List<int> _neighbours;
            private readonly List<int> _markedEdges;


            internal Builder()
            {
                _points = new List<Point3>();
                _triangles = new List<int>();
                _neighbours = new List<int>();
                _markedEdges = new List<int>();
            }

            /// <summary>
            /// Fügt Punkt dem TIN hinzu
            /// </summary>
            /// <param name="point"></param>
            public void AddPoint(in Point3 point)
            {
                _points.Add(point);
            }

            /// <summary>
            /// Fügt Punkt dem TIN hinzu
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="z"></param>
            public void AddPoint(in double x, in double y, in double z)
            {
                AddPoint(Point3.Create(x, y, z));
            }

            /// <summary>
            /// Fügt Dreieck dem TIN hinzu
            /// </summary>
            /// <param name="pointIndexVertexA">Punktindex des ersten Vertex</param>
            /// <param name="pointIndexVertexB">Punktindex des zweiten Vertex</param>
            /// <param name="pointIndexVertexC">Punktindex des dritten Vertex</param>
            /// <param name="triangleIndexNeighbourA">Index des Nachbardreieckes gegenüber des ersten Vertex oder <code>null</code></param>
            /// <param name="triangleIndexNeighbourB">Index des Nachbardreieckes gegenüber des zweiten Vertex oder <code>null</code></param>
            /// <param name="triangleIndexNeighbourC">Index des Nachbardreieckes gegenüber des dritten Vertex oder <code>null</code></param>
            /// <param name="isMarkedEdgeA">Kante gegenüber des ersten Vertex ist markiert (Bruchkante)</param>
            /// <param name="isMarkedEdgeB">Kante gegenüber des zweiten Vertex ist markiert (Bruchkante)</param>
            /// <param name="isMarkedEdgeC">Kante gegenüber des dritten Vertex ist markiert (Bruchkante)</param>
            /// <param name="clockwise">Hier <code>true</code> setzen wenn Reihenfolge der Vertizes im Uhrzeigersinn, sonst weglassen oder <code>false</code></param>
            public void AddTriangle(int pointIndexVertexA, int pointIndexVertexB, int pointIndexVertexC,
                int? triangleIndexNeighbourA, int? triangleIndexNeighbourB, int? triangleIndexNeighbourC,
                bool isMarkedEdgeA, bool isMarkedEdgeB, bool isMarkedEdgeC,
                bool clockwise = false)
            {
                _triangles.AddRange(new[] { pointIndexVertexA, pointIndexVertexB, pointIndexVertexC });
                _triangles.AddRange(new[]{ triangleIndexNeighbourA ?? -1,
                    triangleIndexNeighbourB ?? -1, triangleIndexNeighbourC ?? -1});
                _markedEdges.Add((isMarkedEdgeA ? 1 : 0) + (isMarkedEdgeB ? 2 : 0) + (isMarkedEdgeC ? 4 : 0));
            }

            /// <summary>
            /// Fügt Dreieck dem TIN hinzu
            /// </summary>
            /// <param name="pointIndexVertexA">Punktindex des ersten Vertex</param>
            /// <param name="pointIndexVertexB">Punktindex des zweiten Vertex</param>
            /// <param name="pointIndexVertexC">Punktindex des dritten Vertex</param>
            /// <param name="triangleIndexNeighbourA">Index des Nachbardreieckes gegenüber des ersten Vertex oder <code>null</code></param>
            /// <param name="triangleIndexNeighbourB">Index des Nachbardreieckes gegenüber des zweiten Vertex oder <code>null</code></param>
            /// <param name="triangleIndexNeighbourC">Index des Nachbardreieckes gegenüber des dritten Vertex oder <code>null</code></param>
            /// <param name="clockwise">Hier <code>true</code> setzen wenn Reihenfolge der Vertizes im Uhrzeigersinn, sonst weglassen oder <code>false</code></param>
            public void AddTriangle(int pointIndexVertexA, int pointIndexVertexB, int pointIndexVertexC,
                int? triangleIndexNeighbourA, int? triangleIndexNeighbourB, int? triangleIndexNeighbourC,
                bool clockwise = false)
            {
                _triangles.AddRange(new[] { pointIndexVertexA, pointIndexVertexB, pointIndexVertexC });
                _triangles.AddRange(new[]{ triangleIndexNeighbourA ?? -1,
                    triangleIndexNeighbourB ?? -1, triangleIndexNeighbourC ?? -1});
            }

            /// <summary>
            /// Fügt Dreieck dem TIN hinzu
            /// </summary>
            /// <param name="pointIndexVertexA">Punktindex des ersten Vertex</param>
            /// <param name="pointIndexVertexB">Punktindex des zweiten Vertex</param>
            /// <param name="pointIndexVertexC">Punktindex des dritten Vertex</param>
            /// <param name="isMarkedEdgeA">Kante gegenüber des ersten Vertex ist markiert (Bruchkante)</param>
            /// <param name="isMarkedEdgeB">Kante gegenüber des zweiten Vertex ist markiert (Bruchkante)</param>
            /// <param name="isMarkedEdgeC">Kante gegenüber des dritten Vertex ist markiert (Bruchkante)</param>
            /// <param name="clockwise">Hier <code>true</code> setzen wenn Reihenfolge der Vertizes im Uhrzeigersinn, sonst weglassen oder <code>false</code></param>
            public void AddTriangle(int pointIndexVertexA, int pointIndexVertexB, int pointIndexVertexC,
                bool isMarkedEdgeA, bool isMarkedEdgeB, bool isMarkedEdgeC,
                bool clockwise = false)
            {
                _triangles.AddRange(new[] { pointIndexVertexA, pointIndexVertexB, pointIndexVertexC });
                _markedEdges.Add((isMarkedEdgeA ? 1 : 0) + (isMarkedEdgeB ? 2 : 0) + (isMarkedEdgeC ? 4 : 0));
            }

            /// <summary>
            /// Erzeugt <see cref="Tin"/> aus diesem <see cref="Builder"/>
            /// </summary>
            /// <returns>Neues <see cref="Tin"/></returns>
            public Tin ToTin()
            {
                var hasNeighbours = _triangles.Count == _neighbours.Count;
                var markedEdges = new BitArray(_triangles.Count == _markedEdges.Count * 3 ? _triangles.Count : 0);
                if (markedEdges.Length > 0)
                {
                    for (int i = 0; i < _markedEdges.Count; i++)
                    {
                        var val = _markedEdges[i];
                        int i0 = i * 3;
                        markedEdges.Set(i0, (val & 1) == 1);
                        markedEdges.Set(i0 + 1, (val & 2) == 2);
                        markedEdges.Set(i0 + 2, (val & 4) == 4);
                    }
                }
                return new Tin(_points.ToArray(), _triangles.ToArray(), 
                    _triangles.Count == _neighbours.Count ? _neighbours.ToArray() : Array.Empty<int>(), markedEdges);
            }

        }
    }
}
