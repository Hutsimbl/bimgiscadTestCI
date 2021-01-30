using BimGisCad.Representation.Geometry.Composed;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"C:\Users\romanschek\Downloads\B200310-DGM.out";

            var tinB = Tin.CreateBuilder(true);

            if (File.Exists(path))
            {
                foreach (var line in File.ReadAllLines(path))
                {
                    var values = line.Split(new[] { ',' });
                    if (line.StartsWith("PK") && values.Length > 4 
                        && int.TryParse(values[0].Substring(2, values[0].IndexOf(':') - 2), out int pn)
                        && double.TryParse(values[2], NumberStyles.Float, CultureInfo.InvariantCulture, out double x)
                        && double.TryParse(values[3], NumberStyles.Float, CultureInfo.InvariantCulture, out double y)
                        && double.TryParse(values[4], NumberStyles.Float, CultureInfo.InvariantCulture, out double z))
                    {
                        tinB.AddPoint(pn, x, y, z);
                    }
                    if (line.StartsWith("DG") && values.Length > 9
                        && int.TryParse(values[0].Substring(2, values[0].IndexOf(':') - 2), out int tn)
                        && int.TryParse(values[1].Substring(3), out int va)
                        && int.TryParse(values[2].Substring(3), out int vb)
                        && int.TryParse(values[3].Substring(3), out int vc))
                    {
                        int? na = !string.IsNullOrEmpty(values[4]) && int.TryParse(values[4], out int n) ? n : (int?)null;
                        int? nb = !string.IsNullOrEmpty(values[5]) && int.TryParse(values[5], out n) ? n : (int?)null;
                        int? nc = !string.IsNullOrEmpty(values[6]) && int.TryParse(values[6], out n) ? n : (int?)null;
                        bool ea = !string.IsNullOrEmpty(values[7]);
                        bool eb = !string.IsNullOrEmpty(values[8]);
                        bool ec = !string.IsNullOrEmpty(values[9]);
                        tinB.AddTriangle(tn, va, vb, vc, na, nb, nc, ea, eb, ec, true);
                    }
                }
                var tin = tinB.ToTin(out var pointIndex2NumberMap, out var triangleIndex2NumberMap);

                int pos = 0;
                foreach (var tri in tin.TriangleVertexPointIndizes())
                {
                    Console.WriteLine("{0,-5:D}: {1,5:D}-{2,5:D}-{3,5:D}", 
                        triangleIndex2NumberMap[pos++], 
                        pointIndex2NumberMap[tri[0]], 
                        pointIndex2NumberMap[tri[1]], 
                        pointIndex2NumberMap[tri[2]]);
                }

                Console.ReadKey(true);

            }
        }
    }
}
