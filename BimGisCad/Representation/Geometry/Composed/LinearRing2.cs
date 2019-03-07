using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BimGisCad.Representation.Geometry.Elementary;
using static BimGisCad.Representation.Geometry.Elementary.Common;

namespace BimGisCad.Representation.Geometry.Composed
{
    /// <summary>
    /// 2D Linearring (geschlossener Linienzug)
    /// </summary>
    public class LinearRing2 : IEnumerable<Point2>
    {
        private readonly List<Point2> _points;
        private bool? _isClean;
        private bool? _isCCW;


        private LinearRing2()
        { 
            _points = new List<Point2>();
        }

        private LinearRing2(IEnumerable<Point2> points)
        {
            _points = new List<Point2>(points);
        }

        private void reset()
        {
            _isClean = null;
            _isCCW = null;
        }


        /// <summary>
        /// Erzeugt Linearring aus gegebenen Punkten (Achtung Anfang und Ende müssen verschieden sein!)
        /// </summary>
        /// <param name="points">Punkte</param>
        /// <returns></returns>
        public static LinearRing2 Create(params Point2[] points) => Create(points);

        /// <summary>
        /// Erzeugt Linearring aus gegebenen Punkten (Achtung Anfang und Ende müssen verschieden sein!)
        /// </summary>
        /// <param name="points">Punkte</param>
        /// <returns></returns>
        public static LinearRing2 Create(IEnumerable<Point2> points) => new LinearRing2(points);

        public IEnumerator<Point2> GetEnumerator()
        {
            return _points.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Ist Linienzug gegen Uhrzeigersinn orientiert? (Achtung doppelte Punkte oder "gerade" Knicke können Ergebnis verfälschen!)
        /// </summary>
        public bool IsCCW {
            get
            {
                if (!_isCCW.HasValue)
                {
                    //TODO: Anpassen an ohne Endpunkt
                    //    // der LU Punkt muss ein konvexer Knick sein
                    //    // besser als gesamte Fläche zu berechnen
                    //    int last = _points.Count - 1;
                    //    int lui = 0;
                    //    var lup = _points[0];

                    //    for (int i = 1; i < last; i++)
                    //    {
                    //        if (_points[i].Y < lup.Y || (_points[i].Y == lup.Y && _points[i].X < lup.X))
                    //        {
                    //            lui = i;
                    //            lup = _points[i];
                    //        }
                    //    }
                    //    var prev = _points[lui == 0 ? last - 1 : lui - 1];
                    //    _isCCW = Vector2.Det(lup - prev, _points[lui + 1] - prev) > 0.0;
                    //
                }
                return _isCCW.Value;
            }
        }


        /// Indexer
        public Point2 this[int i] {
            get { return _points[i]; }
            set
            {
                reset();
                _points[i] = value;
            }
        }

        /// <summary>
        /// Fügt neuen Punkt hinzu
        /// </summary>
        /// <param name="point">Neuer Punkt</param>
        public void Add(Point2 point)
        {
            reset();
            _points.Add(point);
        }

        /// <summary>
        /// Entfernt zu kurze Seiten und zu "gerade" Knicke
        /// </summary>
        /// <param name="lring">Eingabering</param>
        /// <param name="mindist">Mindestabstand unterschiedlicher Punkte</param>
        /// <returns>Neuen Linearring</returns>
        public static LinearRing2 CleanGeometry(LinearRing2 lring, double mindist = MINDIST)
        {

        }
    }
}
