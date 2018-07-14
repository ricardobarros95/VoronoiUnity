using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Point
{
    public double x;
    public double y;

    public Point(double x, double y)
    {
        this.x = x;
        this.y = y;
    }

    public static bool operator== (Point p1, Point p2)
    {
        return (p1.x == p2.x && p1.y == p2.y);
    }

    public static bool operator != (Point p1, Point p2)
    {
        return !(p1.x == p2.x && p1.y == p2.y);
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public class Edge
{
    public Point p1;
    public Point p2;

	public bool bad;

	public Edge(Point p1, Point p2)
	{
		this.p1 = p1;
		this.p2 = p2;
		this.bad = false;
	}

    public static bool operator== (Edge e1, Edge e2)
    {
        return (e1.p1 == e2.p1 && e1.p2 == e2.p2) || (e1.p1 == e2.p2 && e1.p2 == e2.p1);
    }

    public static bool operator != (Edge e1, Edge e2)
    {
        return !((e1.p1 == e2.p1 && e1.p2 == e2.p2) || (e1.p1 == e2.p2 && e1.p2 == e2.p1));
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

[System.Serializable]
public class Triangle
{
	public Point p1;
	public Point p2;
	public Point p3;

	public Edge e1;
	public Edge e2;
	public Edge e3;

	public double circleRadius;
	public Point circleCentre;

	public Triangle(Point p1, Point p2, Point p3)
	{
		this.p1 = p1;
		this.p2 = p2;
		this.p3 = p3;

		this.e1 = new Edge(p1, p2);
		this.e2 = new Edge(p2, p3);
		this.e3 = new Edge(p3, p1);

		SetTriangleCircle(this);
	}

	static Point GetCircumCentre(Triangle tri)
	{
		Point p1 = tri.p1;
		Point p2 = tri.p2;
		Point p3 = tri.p3;

		double offset = p2.x * p2.x + p2.y * p2.y;
		double bc = p1.x * p1.x + p1.y * p1.y - offset / 2.0f;
		double cd = (offset - p3.x * p3.x - p3.y * p3.y) / 2.0;
		double det = (p1.x - p2.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p2.y);

		double idet = 1 / det;

		double centrex = (bc * (p2.y - p3.y) - cd * (p1.y - p2.y)) * idet;
		double centrey = (cd * (p1.x - p2.x) - bc * (p2.x - p3.x)) * idet;

		//double radius = System.Math.Sqrt((p1.x - centrex) * (p1.x - centrex) + (p1.y - centrey) * (p1.y - centrey));

		return new Point(centrex, centrey);
	}

	private void SetTriangleCircle(Triangle tri)
	{
		Point p1 = tri.p1;
		Point p2 = tri.p2;
		Point p3 = tri.p3;

		double offset = p2.x * p2.x + p2.y * p2.y;
		double bc = p1.x * p1.x + p1.y * p1.y - offset / 2.0f;
		double cd = (offset - p3.x * p3.x - p3.y * p3.y) / 2.0;
		double det = (p1.x - p2.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p2.y);

		double idet = 1 / det;

		double centrex = (bc * (p2.y - p3.y) - cd * (p1.y - p2.y)) * idet;
		double centrey = (cd * (p1.x - p2.x) - bc * (p2.x - p3.x)) * idet;

		double radius = System.Math.Sqrt((p1.x - centrex) * (p1.x - centrex) + (p1.y - centrey) * (p1.y - centrey));

		circleCentre = new Point(centrex, centrey);
		circleRadius = radius;
	}

	public bool IsPointInsideCircle(Point p)
	{
		double x = p.x;
		double y = p.y;
		double cx = circleCentre.x;
		double a = (p.x - circleCentre.x);
		double b = (p.y - circleCentre.y);
		double r = circleRadius;
		return a * a + b * b - (r * r) <= 0;
	}

	public static bool IsInsideCircle(Point p, Point c, double r)
	{
		return (p.x - c.x) * (p.x - c.x) + (p.y - c.y) * (p.y - c.y) - (r * r) <= 0;
	}



	public static bool InsideCircumCircle(Triangle t, Point p)
	{
		double ax = t.p1.x;
		double ay = t.p1.y;
		double bx = t.p2.x;
		double by = t.p2.y;
		double cx = t.p3.x;
		double cy = t.p3.y;
		double dx = p.x;
		double dy = p.y;

		double ax_ = ax - dx;
		double ay_ = ay - dy;
		double bx_ = bx - dx;
		double by_ = by - dy;
		double cx_ = cx - dx;
		double cy_ = cy - dy;

		if ((bx - ax) * (cy - ay) - (cx - ax) * (by - ay) > 0)
		{
			return (
				(ax_ * ax_ + ay_ * ay_) * (bx_ * cy_ - cx_ * by_) -
				(bx_ * bx_ + by_ * by_) * (ax_ * cy_ - cx_ * ay_) +
				(cx_ * cx_ + cy_ * cy_) * (ax_ * by_ - bx_ * ay_)
			) >= 0;
		}
		else
		{
			return (
				(ax_ * ax_ + ay_ * ay_) * (bx_ * cy_ - cx_ * by_) -
				(bx_ * bx_ + by_ * by_) * (ax_ * cy_ - cx_ * ay_) +
				(cx_ * cx_ + cy_ * cy_) * (ax_ * by_ - bx_ * ay_)
			) < 0;
		}
	}

	public bool ContainsVertex(Triangle otherTri)
	{
		Point op1 = otherTri.p1;
		Point op2 = otherTri.p2;
		Point op3 = otherTri.p3;

		return p1 == op1 || p1 == op2 || p1 == op3 || p2 == op1 || p2 == op2 || p2 == op3 || p3 == op1 || p3 == op2 || p3 == op3;
	}

	public static bool operator== (Triangle t1, Triangle t2)
	{
		return (t1.e1 == t2.e1 && t1.e2 == t2.e2 && t1.e3 == t2.e3) || (t1.e2 == t2.e1 && t1.e2 == t2.e2 && t1.e3 == t2.e3) || (t1.e3 == t2.e1 && t1.e3 == t2.e2 && t1.e3 == t2.e3);
	}

	public static bool operator != (Triangle t1, Triangle t2)
	{
		return !((t1.e1 == t2.e1 && t1.e2 == t2.e2 && t1.e3 == t2.e3) || (t1.e2 == t2.e1 && t1.e2 == t2.e2 && t1.e3 == t2.e3) || (t1.e3 == t2.e1 && t1.e3 == t2.e2 && t1.e3 == t2.e3));
	}

	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}

public class Voronoi : MonoBehaviour {

	public List<Triangle> dTriangles;

	public List<Point> points = new List<Point>();

	public void Triangulate()
	{
		// Create super triangle
		if (points.Count == 0) return;

		foreach(Point p in points)
		{
			p.x = Random.Range(-20.0f, 20.0f);
			p.y = Random.Range(-20.0f, 20.0f);
		}

		double minX = points[0].x;
		double minY = points[0].y;
		double maxX = minX;
		double maxY = minY;

		for(int i = 0; i < points.Count; i++)
		{
			if (points[i].x < minX) minX = points[i].x;
			if (points[i].y < minY) minY = points[i].y;
			if (points[i].x > maxX) maxX = points[i].x;
			if (points[i].y > maxY) maxY = points[i].y;
		}
		double offset = 20.0f;
		double midX = (minX + maxX) / 2.0f;
		double midY = (minY + maxY) / 2.0f;
		double dx = (maxX - minX);
		double dy = (maxY - minY);
		double maxD = System.Math.Max(dx, dy);
		//
		//Point p1 = new Point(midX - maxD, midY - maxD); // left point
		//Point p2 = new Point(midX + maxD, midY - maxD); // right point
		//Point p3 = new Point(midX, midY + maxD);        // top point

		Point p1 = new Point((midX - maxD) * offset, midY - maxD); // left point
		Point p2 = new Point((midX + maxD) * offset, midY - maxD); // right point
		Point p3 = new Point(midX, offset * (midY + maxD));        // top point

		Triangle superTri = new Triangle(p1, p2, p3);


		//	function BowyerWatson(pointList)
		//  // pointList is a set of coordinates defining the points to be triangulated
		//triangulation:= empty triangle mesh data structure
		List<Triangle> tris = new List<Triangle>();
		dTriangles = new List<Triangle>();


		//  add super-triangle to triangulation // must be large enough to completely contain all the points in pointList
		tris.Add(superTri);

		//  for each point in pointList do // add all the points one at a time to the triangulation
		foreach (Point p in points)
		{
			List<Triangle> badTris = new List<Triangle>();
			for (int i = 0; i < tris.Count; i++)
			{
				//if (tris[i].IsPointInsideCircle(p))
				if(Triangle.InsideCircumCircle(tris[i], p))
				{
					badTris.Add(tris[i]);
				}
			}

			List<Edge> polygon = new List<Edge>();

			//for each triangle in badTriangles do 
			// find the boundary of the polygonal hole
			//    for each edge in triangle do
			//	      if edge is not shared by any other triangles in badTriangles
			//			  add edge to polygon

			//for (int i = 0; i < badTris.Count; ++i)
			//{
			//	for (int j = 0; j < badTris.Count; ++j)
			//	{
			//		if (i == j)
			//		{
			//			polygon.Add(badTris[i].e1);
			//			polygon.Add(badTris[i].e2);
			//			polygon.Add(badTris[i].e3);
			//		}
			//		else
			//		{
			//			if (badTris[i].e1 != badTris[j].e1 && badTris[i].e1 != badTris[j].e2 && badTris[i].e1 != badTris[j].e3)
			//			{
			//				polygon.Add(badTris[i].e1);
			//			}

			//			if (badTris[i].e2 != badTris[j].e1 && badTris[i].e2 != badTris[j].e2 && badTris[i].e2 != badTris[j].e3)
			//			{
			//				polygon.Add(badTris[i].e2);
			//			}

			//			if (badTris[i].e3 != badTris[j].e1 && badTris[i].e3 != badTris[j].e2 && badTris[i].e3 != badTris[j].e3)
			//			{
			//				polygon.Add(badTris[i].e3);
			//			}
			//		}
			//	}
			//}

			foreach (Triangle t in badTris)
			{
				polygon.Add(t.e1);
				polygon.Add(t.e2);
				polygon.Add(t.e3);
			}

			for(int i = 0; i < polygon.Count; ++i)
			{
				for(int j = 0; j < polygon.Count; ++j)
				{
					if (i == j) continue;

					if(polygon[i] == polygon[j])
					{
						polygon[j].bad = true;
					}
				}
			}

			polygon.RemoveAll(e => e.bad);

			//for each triangle in badTriangles do 
			//	remove triangle from triangulation
			for (int i = 0; i < badTris.Count; ++i)
			{
				for (int j = 0; j < tris.Count; ++j)
				{
					if (badTris[i] == tris[j])
					{
						tris.RemoveAt(j);
						break;
					}
				}
			}

			// for each edge in polygon do // re-triangulate the polygonal hole
			//	 newTri := form a triangle from edge to point
			//		Add newTri to triangulation
			for(int i = 0; i < polygon.Count; ++i)
			{
				tris.Add(new Triangle(polygon[i].p1, polygon[i].p2, p));
			}
		}

		tris.RemoveAll(t => t.ContainsVertex(superTri));

		dTriangles = tris;
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.K))
		{
			Triangulate();
		}
	}

	private void OnDrawGizmos()
	{
		if(points != null)
		{
			Gizmos.color = Color.red;

			for(int i =0; i < points.Count; i++)
			{
				Vector3 newP = new Vector3((float)points[i].x, 0, (float)points[i].y);

				Gizmos.DrawSphere(newP, 0.2f);
			}
		}

		if(dTriangles != null)
		{
			Gizmos.color = Color.white;

			for (int i = 0; i < dTriangles.Count; ++i)
			{
				Vector3 p1 = new Vector3((float)dTriangles[i].p1.x, 0.0f, (float)dTriangles[i].p1.y);
				Vector3 p2 = new Vector3((float)dTriangles[i].p2.x, 0.0f, (float)dTriangles[i].p2.y);
				Vector3 p3 = new Vector3((float)dTriangles[i].p3.x, 0.0f, (float)dTriangles[i].p3.y);

				Gizmos.DrawLine(p1, p2);
				Gizmos.DrawLine(p2, p3);
				Gizmos.DrawLine(p3, p1);
			}
		}

	}

	void BuildVoronoi()
	{

	}
}
