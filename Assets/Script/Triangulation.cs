using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Triangulation : MonoBehaviour
{
    struct Sommet
    {
        public Vector3 pos;

        public Sommet(Vector3 pos)
        {
            this.pos = pos;
        }

        public static implicit operator Sommet(Vector3 vec) => new Sommet(vec);
        public static implicit operator Vector3(Sommet s) => s.pos;
    }
    class Arète
    {
        public int s1;
        public int s2;
        public bool isInterior;

        public Arète(int s1, int s2)
        {
            this.s1 = s1;
            this.s2 = s2;
            isInterior = false;
        }

        public Vector3 GetCenter()
        {
            return (sommets[s1].pos + sommets[s2].pos)/2;
        }

        /*public Vector3 GetNormal()
        {
            Vector3 dir = sommets[s2].pos - sommets[s1].pos;
            Vector3 n = Vector3.Cross(dir, Vector3.back).normalized;
            return n;
            //return (new Vector3(sommets[s2].pos.y - sommets[s1].pos.y, sommets[s1].pos.x - sommets[s2].pos.x, dir.z)).normalized;
        }*/

        public Color GetColor()
        {
            if(isInterior)
            {
                return Color.green;
            }
            else
            {
                return Color.blue;
            }
        }
    }
    struct Triangle
    {
        public int a1;
        public int a2;
        public int a3;

        public Triangle(int a1, int a2, int a3)
        {
            this.a1 = a1;
            this.a2 = a2;
            this.a3 = a3;
        }
    }

    public Text text;
    public MeshFilter meshFilter;

    static List<Triangle> triangles;
    static List<Arète> arètes;
    static List<Sommet> sommets;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PointHandler.pointList.Count < 3)
            text.color = new Color(0.8f, 0, 0, 1);
        else
        {
            text.color = new Color(0.2f, 0.2f, 0.2f, 1);
            if (Input.GetKeyDown(KeyCode.T))
                StartCoroutine("StartTriangulation");
        }
    }
    
    IEnumerator StartTriangulation()
    {
        //if (PointHandler.pointList.Count < 3) return;

        //Tri du plus a gauche puis si égalité le plus en bas
        PointHandler.pointList.Sort(delegate(GameObject go1, GameObject go2)
        {
            if (go1.transform.position.x == go2.transform.position.x)
            {
                return go1.transform.position.y > go2.transform.position.y ? 1 : -1;
            }
            else
            {
                return go1.transform.position.x > go2.transform.position.x ? 1 : -1;
            }
        });

        triangles = new List<Triangle>();
        arètes = new List<Arète>();
        sommets = new List<Sommet>();

        Vector3 startDir = (PointHandler.pointList[1].transform.position - PointHandler.pointList[0].transform.position).normalized;
        Vector3 nextDir = (PointHandler.pointList[2].transform.position - PointHandler.pointList[1].transform.position).normalized;

        {
            Sommet s1 = new Sommet(PointHandler.pointList[0].transform.position);
            Sommet s2 = new Sommet(PointHandler.pointList[1].transform.position);
            Sommet s3 = new Sommet(PointHandler.pointList[2].transform.position);
            sommets.Add(s1);
            sommets.Add(s2);
            sommets.Add(s3);

            if (startDir == nextDir)
            {
                Sommet s4 = new Sommet(PointHandler.pointList[3].transform.position);
                sommets.Add(s4);

                Arète a1 = new Arète(0, 1);
                Arète a2 = new Arète(1, 3);
                Arète a3 = new Arète(3, 0);
                arètes.Add(a1);
                arètes.Add(a2);
                arètes.Add(a3);

                Triangle t1 = new Triangle(0,1,2);
                triangles.Add(t1);

                Arète a4 = new Arète(1, 2);
                arètes.Add(a4);
                Arète a5 = new Arète(2, 3);
                arètes.Add(a5);

                Triangle t2 = new Triangle(3,4,2);
                triangles.Add(t2);
            }
            else
            {
                Arète a1 = new Arète(0, 1);
                Arète a2 = new Arète(1, 2);
                Arète a3 = new Arète(2, 0);
                arètes.Add(a1);
                arètes.Add(a2);
                arètes.Add(a3);

                Triangle t1 = new Triangle(0, 1, 2);
                triangles.Add(t1);
            }
        }

        // Ajouter tout les autres points
        for (int p = sommets.Count; p < PointHandler.pointList.Count; ++p)
        {
            Sommet sommetToAdd = new Sommet(PointHandler.pointList[p].transform.position);
            sommets.Add(sommetToAdd);

            Vector2 sommet2 = new Vector2(sommetToAdd.pos.x, sommetToAdd.pos.y);

            foreach (Arète A in arètes)
            {
                //if (A.isInterior) continue;
                A.isInterior = false;
                Vector3 center = A.GetCenter();
                Vector2 centerA = new Vector2(center.x, center.y);

                foreach (Arète B in arètes)
                {
                    if (A == B) continue;
                    Vector2 pi = new Vector2(0,0);

                    if (LineSegmentsIntersection(centerA, sommet2, new Vector2(sommets[B.s1].pos.x, sommets[B.s1].pos.y), new Vector2(sommets[B.s2].pos.x, sommets[B.s2].pos.y), out pi))
                    {
                        A.isInterior = true;
                        break;
                    }
                }
            }

            int count = arètes.Count;
            for (int i = 0; i < count; ++i)
            {
                Arète A = arètes[i];
                if (!A.isInterior)
                {
                    Arète a1 = new Arète(A.s1, sommets.Count - 1);
                    arètes.Add(a1);
                    Arète a2 = new Arète(A.s2, sommets.Count - 1);
                    arètes.Add(a2);

                    Triangle tri = new Triangle(arètes.Count - 2, arètes.Count - 1, i);
                    triangles.Add(tri);
                }
            }
        }

        yield return 0;
    }

    void OnDrawGizmos()
    {
        if (triangles == null) return;
        
        foreach(Triangle triangle in triangles) {
            Gizmos.color = arètes[triangle.a1].GetColor();
            Gizmos.DrawLine(sommets[arètes[triangle.a1].s1], sommets[arètes[triangle.a1].s2]);
            Gizmos.color = arètes[triangle.a1].GetColor();
            Gizmos.DrawLine(sommets[arètes[triangle.a2].s1], sommets[arètes[triangle.a2].s2]);
            Gizmos.color = arètes[triangle.a1].GetColor();
            Gizmos.DrawLine(sommets[arètes[triangle.a3].s1], sommets[arètes[triangle.a3].s2]);
            
            /*Gizmos.color = arètes[triangle.a1].GetColor();
            Gizmos.DrawLine((sommets[arètes[triangle.a1].s1].pos + sommets[arètes[triangle.a1].s2].pos)/2, (sommets[arètes[triangle.a1].s1].pos + sommets[arètes[triangle.a1].s2].pos)/2 + arètes[triangle.a1].GetNormal());
            Gizmos.color = arètes[triangle.a2].GetColor();
            Gizmos.DrawLine((sommets[arètes[triangle.a2].s1].pos + sommets[arètes[triangle.a2].s2].pos)/2, (sommets[arètes[triangle.a2].s1].pos + sommets[arètes[triangle.a2].s2].pos)/2 + arètes[triangle.a2].GetNormal());
            Gizmos.color = arètes[triangle.a3].GetColor();
            Gizmos.DrawLine((sommets[arètes[triangle.a3].s1].pos + sommets[arètes[triangle.a3].s2].pos)/2, (sommets[arètes[triangle.a3].s1].pos + sommets[arètes[triangle.a3].s2].pos)/2 + arètes[triangle.a3].GetNormal());*/
        }
    }

    public static bool LineSegmentsIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 intersection)
    {
        intersection = Vector2.zero;

        var d = (p2.x - p1.x) * (p4.y - p3.y) - (p2.y - p1.y) * (p4.x - p3.x);

        if (d == 0.0f)
        {
            return false;
        }

        var u = ((p3.x - p1.x) * (p4.y - p3.y) - (p3.y - p1.y) * (p4.x - p3.x)) / d;
        var v = ((p3.x - p1.x) * (p2.y - p1.y) - (p3.y - p1.y) * (p2.x - p1.x)) / d;

        if (u < 0.0f || u > 1.0f || v < 0.0f || v > 1.0f)
        {
            return false;
        }

        intersection.x = p1.x + u * (p2.x - p1.x);
        intersection.y = p1.y + u * (p2.y - p1.y);

        return true;
    }
}
