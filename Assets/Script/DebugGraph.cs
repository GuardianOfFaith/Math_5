using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnveloppeConvexeIncrementale;
using Random = UnityEngine.Random;

public class DebugGraph : MonoBehaviour
{
    public Material mat;

    public static Polyhèdre polyhèdre;

    List<Color> SToAColor;
    List<Color> AToFColor;

    bool isHide = false;

    // Start is called before the first frame update
    void Start()
    {
        SToAColor = new List<Color>();
        AToFColor = new List<Color>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            SToAColor.Clear();
            AToFColor.Clear();
        }
        if (Input.GetKeyDown(KeyCode.H)) isHide = !isHide;
    }

    void OnPostRender()
    {
        if (isHide) return;
        if(!mat)
        {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }
        if (polyhèdre == null) return;

        //if(SToA.Count < polyhèdre.arètes.Count*2)
        for(int i=SToAColor.Count-1; i<polyhèdre.arètes.Count*2; ++i)
        {
            SToAColor.Add(Random.ColorHSV(0f,1f,0f,1f,0f,1f,1f,1f));
        }
        for (int i = AToFColor.Count - 1; i < polyhèdre.faces.Count*3; ++i)
        {
            AToFColor.Add(Random.ColorHSV(0f, 1f, 0f, 1f, 0f, 1f, 1f, 1f));
        }

        GL.PushMatrix();
        mat.SetPass(0);
        GL.LoadOrtho();

        GL.Begin(GL.QUADS);
        Sommet som;
        for (int i=0; i<polyhèdre.sommets.Count; ++i)
        {
            som = polyhèdre.sommets[i];
            GL.Color(som.GetColor());
            GL.Vertex3(0.05f*i, 1-0, 0);
            GL.Vertex3(0.05f*i+0.045f, 1-0, 0);
            GL.Vertex3(0.05f*i+0.045f, 1-0.05f, 0);
            GL.Vertex3(0.05f*i, 1-0.05f, 0);
        }
        GL.End();

        Arète arète;
        for (int i = 0; i < polyhèdre.arètes.Count; ++i)
        {
            arète = polyhèdre.arètes[i];
            GL.Begin(GL.QUADS);        
            GL.Color(arète.GetColor());
            GL.Vertex3(0.05f * i, 1 - 0.15f, 0);
            GL.Vertex3(0.05f * i + 0.045f, 1 - 0.15f, 0);
            GL.Vertex3(0.05f * i + 0.045f, 1 - 0.20f, 0);
            GL.Vertex3(0.05f * i, 1 - 0.20f, 0);
            GL.End();

            if (arète.A != null)
            {
                int iA = polyhèdre.sommets.FindIndex(x => x == arète.A);
                GL.Begin(GL.LINES);
                GL.Color(SToAColor[i]);
                GL.Vertex3(0.05f * iA + 0.025f, 1 - 0.05f, 0);
                GL.Vertex3(0.05f * i + 0.0125f, 1 - 0.15f, 0);
                GL.End();

                GL.Begin(GL.TRIANGLES);
                GL.Color(Color.white);
                GL.Vertex3(0.05f * i + 0.0125f, 1 - 0.15f, 0);
                GL.Vertex3(0.05f * i + 0.0125f - 0.002f, 1 - (0.15f + 0.005f), 0);
                GL.Vertex3(0.05f * i + 0.0125f + 0.002f, 1 - (0.15f + 0.005f), 0);
                GL.End();
            }

            if (arète.B != null)
            {
                int iB = polyhèdre.sommets.FindIndex(x => x == arète.B);
                GL.Begin(GL.LINES);
                GL.Color(SToAColor[polyhèdre.arètes.Count + i]);
                GL.Vertex3(0.05f * iB + 0.025f, 1 - 0.05f, 0);
                GL.Vertex3(0.05f * i + 0.0375f, 1 - 0.15f, 0);
                GL.End();

                GL.Begin(GL.TRIANGLES);
                GL.Color(Color.white);
                GL.Vertex3(0.05f * i + 0.0375f, 1 - 0.15f, 0);
                GL.Vertex3(0.05f * i + 0.0375f - 0.002f, 1 - (0.15f + 0.005f), 0);
                GL.Vertex3(0.05f * i + 0.0375f + 0.002f, 1 - (0.15f + 0.005f), 0);
                GL.End();
            }
        }

        Face face;
        for (int i = 0; i < polyhèdre.faces.Count; ++i)
        {
            face = polyhèdre.faces[i];
            GL.Begin(GL.QUADS);
            GL.Color(face.GetColor());
            GL.Vertex3(0.05f * i, 1 - 0.30f, 0);
            GL.Vertex3(0.05f * i + 0.045f, 1 - 0.30f, 0);
            GL.Vertex3(0.05f * i + 0.045f, 1 - 0.35f, 0);
            GL.Vertex3(0.05f * i, 1 - 0.35f, 0);
            GL.End();

            if (face.A != null)
            {
                int iA = polyhèdre.arètes.FindIndex(x => x == face.A);
                GL.Begin(GL.LINES);
                GL.Color(AToFColor[i]);
                GL.Vertex3(0.05f * iA + 0.025f, 1 - 0.20f, 0);
                GL.Vertex3(0.05f * i + 0.0125f, 1 - 0.30f, 0);
                GL.End();

                GL.Begin(GL.TRIANGLES);
                GL.Color(Color.white);
                GL.Vertex3(0.05f * i + 0.0125f, 1 - 0.30f, 0);
                GL.Vertex3(0.05f * i + 0.0125f - 0.002f, 1 - (0.30f + 0.005f), 0);
                GL.Vertex3(0.05f * i + 0.0125f + 0.002f, 1 - (0.30f + 0.005f), 0);
                GL.End();
            }

            if (face.B != null)
            {
                int iB = polyhèdre.arètes.FindIndex(x => x == face.B);
                GL.Begin(GL.LINES);
                GL.Color(AToFColor[polyhèdre.faces.Count + i]);
                GL.Vertex3(0.05f * iB + 0.025f, 1 - 0.20f, 0);
                GL.Vertex3(0.05f * i + 0.025f, 1 - 0.30f, 0);
                GL.End();

                GL.Begin(GL.TRIANGLES);
                GL.Color(Color.white);
                GL.Vertex3(0.05f * i + 0.025f, 1 - 0.30f, 0);
                GL.Vertex3(0.05f * i + 0.025f - 0.002f, 1 - (0.30f + 0.005f), 0);
                GL.Vertex3(0.05f * i + 0.025f + 0.002f, 1 - (0.30f + 0.005f), 0);
                GL.End();
            }


            if (face.C != null)
            {
                int iB = polyhèdre.arètes.FindIndex(x => x == face.C);
                GL.Begin(GL.LINES);
                GL.Color(AToFColor[polyhèdre.faces.Count*2 + i]);
                GL.Vertex3(0.05f * iB + 0.025f, 1 - 0.20f, 0);
                GL.Vertex3(0.05f * i + 0.0375f, 1 - 0.30f, 0);
                GL.End();

                GL.Begin(GL.TRIANGLES);
                GL.Color(Color.white);
                GL.Vertex3(0.05f * i + 0.0375f, 1 - 0.30f, 0);
                GL.Vertex3(0.05f * i + 0.0375f - 0.002f, 1 - (0.30f + 0.005f), 0);
                GL.Vertex3(0.05f * i + 0.0375f + 0.002f, 1 - (0.30f + 0.005f), 0);
                GL.End();
            }
        }

        GL.PopMatrix();
    }
}
