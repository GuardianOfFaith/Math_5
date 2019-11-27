using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnveloppeConvexeIncrementale : MonoBehaviour
{
    public enum EPolyColor
    {
        Rouge,
        Bleu,
        Violet
    }
    public class Sommet
    {
        Vector3 pos;
        public EPolyColor color;
        public Sommet(Vector3 pos)
        {
            this.pos = pos;
        }
        public static implicit operator Sommet(Vector3 vec) => new Sommet(vec);
        public static implicit operator Vector3(Sommet s) => s.pos;
        public Color GetColor()
        {
            return GetColorFromEPolyColor(color);
        }
        public static Sommet Moyenne(List<Sommet> sommets)
        {
            Vector3 res = Vector3.zero;
            foreach (Vector3 v in sommets)
                res += v;
            return res / sommets.Count;
        }
    }
    public class Arète
    {
        public Sommet A, B;
        public EPolyColor color;
        public Arète(Sommet A, Sommet B)
        {
            this.A = A;
            this.B = B;
        }
        public Color GetColor()
        {
            return GetColorFromEPolyColor(color);
        }
    }
    public class Face
    {
        public Arète A,B,C;
        public EPolyColor color;
        public Face(Arète A, Arète B, Arète C)
        {
            this.A = A;
            this.B = B;
            this.C = C;
        }
        public Color GetColor()
        {
            return GetColorFromEPolyColor(color);
        }
        public Sommet[] GetSommets()
        {
            Vector3 v1 = A.A;
            Vector3 v2 = B.A;
            Vector3 v3 = C.A;
            if (v1.Equals(v2))
            {
                if (v3.Equals(A.B))
                    v2 = B.B;
                else
                    v1 = A.B;
            }
            else if (v2.Equals(v3))
            {
                if (v1.Equals(C.B))
                    v2 = B.B;
                else
                    v3 = C.B;
            }
            else if (v1.Equals(v3))
            {
                if (v2.Equals(A.B))
                    v3 = C.B;
                else
                    v1 = A.B;
            }
            Sommet[] som = new Sommet[3];
            som[0] = v1;
            som[1] = v2;
            som[2] = v3;
            return som;
        }
        public Vector3 GetNormal(Vector3 center)
        {
            Sommet[] som = GetSommets();
            Vector3 v1 = som[0];
            Vector3 v2 = som[1];
            Vector3 v3 = som[2];

            // Centre du triangle
            Vector3 c = (v1 + v2 + v3) / 3;

            // Calcul normal du triangle
            Vector3 n = Vector3.Cross(v2 - v1, v3 - v1).normalized;

            if (Vector3.Dot((c - center).normalized, n) >= 0)
                return n;
            else return -n;
        }
    }
    public class Polyhèdre
    {
        public List<Sommet> sommets;
        public List<Arète> arètes;
        public List<Face> faces;

        public Polyhèdre(ref Vector3[] firstPoints)
        {
            sommets = new List<Sommet>();
            arètes = new List<Arète>();
            faces = new List<Face>();

            foreach(Vector3 v in firstPoints)
                sommets.Add(v);

            Vector3 center = Sommet.Moyenne(sommets);

            AddNewArètesAndFace(0, 1, 2);
            AddNewArètesAndFace(1, 2, 3);
            AddNewArètesAndFace(3, 0, 1);
            AddNewArètesAndFace(0, 2, 3);
        }

        Arète FindOrCreateArète(Sommet a, Sommet b)
        {
            foreach(Arète arète in arètes) {
                if (arète.A == a && arète.B == b || arète.B == a && arète.A == b)
                    return arète;
            }
            Arète ar = new Arète(a, b);
            arètes.Add(ar);
            return ar;
        }

        void AddNewArètesAndFace(int i1, int i2, int i3)
        {
            Vector3 v1 = PointHandler.pointList[i1].transform.position;
            Vector3 v2 = PointHandler.pointList[i2].transform.position;
            Vector3 v3 = PointHandler.pointList[i3].transform.position;

            Arète a1 = FindOrCreateArète(sommets[i1], sommets[i3]);
            Arète a2 = FindOrCreateArète(sommets[i3], sommets[i2]);
            Arète a3 = FindOrCreateArète(sommets[i2], sommets[i1]);
            faces.Add(new Face(a1, a2, a3));
        }

        public Mesh ToMesh()
        {
            Mesh mesh = new Mesh();

            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            Vector3 center = Sommet.Moyenne(sommets);

            foreach (Face f in faces)
            {
                Sommet[] som = f.GetSommets();
                Vector3 v1 = som[0];
                Vector3 v2 = som[1];
                Vector3 v3 = som[2];

                // Centre du triangle
                Vector3 c = (v1 + v2 + v3) / 3;

                // Calcul normal du triangle
                Vector3 n = Vector3.Cross(v2 - v1, v3 - v1).normalized;

                triangles.Add(vertices.Count);
                vertices.Add(v1);

                if (Vector3.Dot((c - center).normalized, n) <= 0)
                {
                    triangles.Add(vertices.Count);
                    vertices.Add(v3);

                    triangles.Add(vertices.Count);
                    vertices.Add(v2);
                }
                else
                {
                    triangles.Add(vertices.Count);
                    vertices.Add(v2);

                    triangles.Add(vertices.Count);
                    vertices.Add(v3);
                }
            }

            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();

            return mesh;
        }

        public bool MarquageFacesHasBlue(Vector3 Pq1)
        {
            bool hasBlue = false;
            foreach (Face f in faces)
            {
                Vector3 n = f.GetNormal(GetCenter());

                Vector3 c = Vector3.zero;
                Sommet[] som = f.GetSommets();
                foreach (Vector3 v in som)
                {
                    c += v;
                }
                c /= som.Length;

                // rouge si derriere sinon bleu
                if (Vector3.Dot(n, (Pq1 - c).normalized) < 0)
                {
                    f.color = EPolyColor.Rouge;
                }
                else
                {
                    hasBlue = true;
                    f.color = EPolyColor.Bleu;
                }
            }

            return hasBlue;
        }

        public void MarquageArètes()
        {
            foreach(Arète a in arètes)
            {
                int numFace = 0;
                int numRouge = 0;
                int numBleu = 0;
                foreach(Face f in faces)
                {
                    if(f.A == a || f.B == a || f.C == a)
                    {
                        ++numFace;
                        if (f.color == EPolyColor.Bleu) ++numBleu;
                        else ++numRouge;
                    }
                    if (numFace >= 2) break;
                }
                if (numRouge == 2) a.color = EPolyColor.Rouge;
                else if (numBleu == 2) a.color = EPolyColor.Bleu;
                else a.color = EPolyColor.Violet;
            }
        }

        public void MarquageSommets()
        {
            foreach (Arète a in arètes)
            {
                if(a.color == EPolyColor.Rouge)
                {
                    a.A.color = EPolyColor.Rouge;
                    a.B.color = EPolyColor.Rouge;
                }
            }

            foreach (Arète a in arètes)
            {
                if (a.color == EPolyColor.Bleu)
                {
                    a.A.color = EPolyColor.Bleu;
                    a.B.color = EPolyColor.Bleu;
                }
            }

            foreach (Arète a in arètes)
            {
                if (a.color == EPolyColor.Violet)
                {
                    a.A.color = EPolyColor.Violet;
                    a.B.color = EPolyColor.Violet;
                }
            }
        }

        public Vector3 GetCenter()
        {
            return Sommet.Moyenne(sommets);
        }

        public void RemoveBlues() {
            List<Face> fs = new List<Face>();
            fs.AddRange(faces);
            foreach(Face f in fs)
            {
                if(f.color == EPolyColor.Bleu)
                {
                    faces.Remove(f);
                }
            }
            List<Arète> ars = new List<Arète>();
            ars.AddRange(arètes);
            foreach(Arète ar in ars)
            {
                if (ar.color == EPolyColor.Bleu)
                {
                    arètes.Remove(ar);
                }
            }
            List<Sommet> ss = new List<Sommet>();
            ss.AddRange(sommets);
            foreach (Sommet s in ss)
            {
                if (s.color == EPolyColor.Bleu)
                {
                    sommets.Remove(s);
                }
            }
        }

        public void AddPq1(Vector3 v)
        {
            Sommet Pq1 = new Sommet(v);
            sommets.Add(Pq1);

            Arète[] ars = arètes.ToArray();

            foreach(Arète a in ars)
            {
                if (a.color != EPolyColor.Violet) continue;

                Arète a1 = FindOrCreateArète(a.A, Pq1);
                Arète a2 = FindOrCreateArète(a.B, Pq1);
                Face f = new Face(a1, a2, a);
                faces.Add(f);
            }
        }
    }

    public Text text;
    public MeshFilter meshFilter;

    Polyhèdre polyhèdre;

    public float delaySecond = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PointHandler.pointList.Count < 4)
            text.color = new Color(0.8f, 0, 0, 1);
        else
        {
            text.color = new Color(0.2f, 0.2f, 0.2f, 1);
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine("enveloppeConvexeIncrementale");
                //enveloppeConvexeIncrementale();
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = Time.timeScale > 0 ? 0 : 1;
        }
    }

    IEnumerator/*void*/ enveloppeConvexeIncrementale()
    {
        //1
        // Make Tetraède C4
        Vector3[] fourFirstPoint = { PointHandler.pointList[0].transform.position, PointHandler.pointList[1].transform.position, PointHandler.pointList[2].transform.position, PointHandler.pointList[3].transform.position };
        polyhèdre = new Polyhèdre(ref fourFirstPoint);
        DebugGraph.polyhèdre = polyhèdre;
        int q = 4;

        if (delaySecond > 0)
        {
            meshFilter.mesh = polyhèdre.ToMesh();
            yield return new WaitForSeconds(1);
        }

        //2
        for (; q < PointHandler.pointList.Count; ++q)
        { 
            Vector3 Pq1 = PointHandler.pointList[q].transform.position; //Pq+1 point start at 0
            if (polyhèdre.MarquageFacesHasBlue(Pq1))
            {
                //Pq+1 est en dehors de Cq
                if (delaySecond > 0) yield return new WaitForSeconds(1);
                polyhèdre.MarquageArètes();

                if (delaySecond > 0) yield return new WaitForSeconds(1);
                polyhèdre.MarquageSommets();

                if (delaySecond > 0) yield return new WaitForSeconds(1);
                polyhèdre.RemoveBlues();

                if (delaySecond > 0) yield return new WaitForSeconds(1);
                polyhèdre.AddPq1(Pq1);

                if (delaySecond > 0)
                {
                    meshFilter.mesh = polyhèdre.ToMesh();
                    yield return new WaitForSeconds(1);
                }
            }
        }

        meshFilter.mesh = polyhèdre.ToMesh();

        print("Nb Sommets: "+polyhèdre.sommets.Count+" Nb d'Arètes: "+polyhèdre.arètes.Count+" Nb Faces: "+polyhèdre.faces.Count);

        yield return 0;
    }

    static Color GetColorFromEPolyColor(EPolyColor color)
    {
        switch(color)
        {
            case EPolyColor.Rouge:
                return Color.red;
            case EPolyColor.Bleu:
                return Color.blue;
            case EPolyColor.Violet:
                return new Color(0.5f, 0, 0.5f);
            default:
                return Color.white;
        }
    }

    void OnDrawGizmos()
    {
        if (polyhèdre == null) return;

        Vector3 center = polyhèdre.GetCenter();

        foreach (Face f in polyhèdre.faces)
        {
            Gizmos.color = f.A.GetColor();
            Gizmos.DrawLine(f.A.A, f.A.B);
            Gizmos.color = f.B.GetColor();
            Gizmos.DrawLine(f.B.A, f.B.B);
            Gizmos.color = f.C.GetColor();
            Gizmos.DrawLine(f.C.A, f.C.B);

            Gizmos.color = f.A.A.GetColor();
            Gizmos.DrawWireSphere(f.A.A, 0.5f);
            Gizmos.color = f.A.B.GetColor();
            Gizmos.DrawWireSphere(f.A.B, 0.5f);
            Gizmos.color = f.B.A.GetColor();
            Gizmos.DrawWireSphere(f.B.A, 0.5f);
            Gizmos.color = f.B.B.GetColor();
            Gizmos.DrawWireSphere(f.B.B, 0.5f);
            Gizmos.color = f.C.A.GetColor();
            Gizmos.DrawWireSphere(f.C.A, 0.5f);
            Gizmos.color = f.C.B.GetColor();
            Gizmos.DrawWireSphere(f.C.B, 0.5f);

            Gizmos.color = Color.blue;
            Vector3 c = Vector3.zero;
            Sommet[] som = f.GetSommets();
            foreach (Vector3 v in som)
            {
                c += v;
            }
            c /= som.Length;
            Gizmos.DrawLine(c, c+(f.GetNormal(center)*0.5f));
        }
    }
}