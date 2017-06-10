﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;

namespace RevitReactionImporter
{
    public class AnalyticalModel
    {
        public ProjectInformation ProjectInfo { get; set; }
        public Members StructuralMembers { get; set; }
        public GridData GridData { get; set; }
        public Dictionary<string, double> LevelInfo { get; set; }
        public double[] ReferencePointDataTransfer { get; set; }
        public AnalyticalModel()
        {
            ProjectInfo = new ProjectInformation();
            StructuralMembers = new Members();
            GridData = new GridData();
            LevelInfo = new Dictionary<string, double>();
        }


        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            if (ProjectInfo == null)
            {
                ProjectInfo = new ProjectInformation();
            }
            if (StructuralMembers == null)
            {
                StructuralMembers = new Members();
            }
            if (StructuralMembers.Beams == null)
            {
                StructuralMembers.Beams = new List<Beam>();
            }
            if (StructuralMembers.Columns == null)
            {
                StructuralMembers.Columns = new List<Column>();
            }
        }
    }

    public class ProjectInformation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string City { get; set; }
        public string Units { get; set; }
        public ProjectInformation()
        {
        }
    }

    public class Members
    {
        public List<Column> Columns { get; set; }
        public List<Beam> Beams { get; set; }

        public Members()
        {
            Columns = new List<Column>();
            Beams = new List<Beam>();
        }
    }

    public class Grid
    {
        public string Name { get; set; }
        public Vector Direction { get; set; }
        public double[] Origin { get; set; }

    }

    public class GridData
    {
        public List<Grid> Grids { get; set; }
        public int VerticalGridCount { get; set; }
        public int HorizontalGridCount { get; set; }
        public Dictionary<string, double> VerticalGridSpacings { get; set; }
        public Dictionary<string, double> HorizontalGridSpacings { get; set; }


        public GridData()
        {
            Grids = new List<Grid>();
            VerticalGridSpacings = new Dictionary<string, double>();
            HorizontalGridSpacings = new Dictionary<string, double>();
        }
    }

    public enum MemberType
    {
        Beam,
        Column
    }

    public class WideFlange
    {
        public string StructuralMaterial { get; set; }
        public string Id { get; set; }
        public int ElementId { get; set; }
        public string ElementLevel { get; set; }
        public string Symbol { get; set; }
        public double Width { get; set; }
        public double Depth { get; set; }
        public double WebThickness { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public double WeightPerLF { get; set; }
        public double CrossSectionalArea { get; set; }
        public double TopFlangeThickness { get; set; }
        public double BottomFlangeThickness { get; set; }

        // The Length property represents the overall length of the member (including the 
        // length of cut backs for beams).
        public double Length { get; set; }

        public string Section { get { return Symbol; } }
        public string FamilySymbol { get; set; }

        public WideFlange()
        {
        }


    }

    public class Column : WideFlange
    {
        //public double Length { get; set; }
        //public string BaseConnection { get; set; }
        //public string TopConnection { get; set; }
        public string BaseConnectionParameter { get; set; }
        public string TopConnectionParameter { get; set; }
        public double[] BasePoint { get; set; }
        public double[] TopPoint { get; set; }
        public double[] FacingOrientation { get; set; }
        public double Rotation { get; set; }
        public string TopLevelName { get; set; }
        public string BaseLevelName { get; set; }

        public Column()
        {
        }

    }

    public class Beam : WideFlange
    {
        public double EndLevelOffset { get; set; }
        public string StartConnectionParameter { get; set; }
        public string EndConnectionParameter { get; set; }
        public string StructuralUsage { get; set; }
        public double[] StartPoint { get; set; }
        public double[] EndPoint { get; set; }
        public string StartReactionTotal { get; set; }
        public string EndReactionTotal { get; set; }

        // The CutLength property is the reduced length of beams after subtracting the 
        // cut backs.
        public double CutLength { get; set; }
        public double[] TopFlangeNormal { get { return new double[] { 0, 0, 1.0 }; } }


        public Beam()
        {
        }
    }

    public class Vector
    {
        public Vector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public void Normalize()
        {
            var distance = Math.Sqrt(X * X + Y * Y + Z * Z);
            X /= distance;
            Y /= distance;
            Z /= distance;
        }

        public static Vector Cross(Vector a, Vector b)
        {
            var x = a.Y * b.Z - a.Z * b.Y;
            var y = a.Z * b.X - a.X * b.Z;
            var z = a.X * b.Y - a.Y * b.X;

            return new Vector(x, y, z);
        }

        public static Vector FromTo(double[] a, double[] b)
        {
            var x = b[0] - a[0];
            var y = b[1] - a[1];
            var z = b[2] - a[2];

            return new Vector(x, y, z);
        }
    }

    public class CoordinateSystem
    {
        public CoordinateSystem(double[] o, Vector x, Vector y, Vector z)
        {
            Origin = o;
            XAxis = x;
            YAxis = y;
            ZAxis = z;
        }

        public Vector XAxis { get; set; }
        public Vector YAxis { get; set; }
        public Vector ZAxis { get; set; }
        public double[] Origin { get; set; }
    }
}