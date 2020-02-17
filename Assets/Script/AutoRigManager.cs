using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRigManager : MonoBehaviour
{
    public GameObject spritePrefab;
    public GameObject model3D;
    GameObject currentSpriteGO;
    GameObject spriteHead;
    GameObject spriteEntreJambes;
    GameObject spriteGenoux1;
    GameObject spriteGenoux2;
    GameObject spriteCoude1;
    GameObject spriteCoude2;
    GameObject spritePoignet1;
    GameObject spritePoignet2;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    GameObject headGO;
    GameObject cuisseGaucheGO;
    GameObject tibiaGaucheGO;
    GameObject cuisseDroiteGO;
    GameObject tibiaDroiteGO;
    GameObject shoulderGaucheGO;
    GameObject avantBrasGaucheGO;
    GameObject poignetGaucheGO;
    GameObject shoulderDroiteGO;
    GameObject avantBrasDroiteGO;
    GameObject poignetDroiteGO;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = model3D.GetComponent<MeshFilter>();
        meshRenderer = model3D.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSpriteGO)
        {
            Vector3 spritePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spritePos.z += 1;

            if (currentSpriteGO == spriteGenoux1)
            {
                if (Input.mousePosition.x < Screen.width / 2)
                {
                    Vector3 spritePos2 = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width - Input.mousePosition.x, Input.mousePosition.y));
                    spritePos2.z += 1;
                    spriteGenoux2.transform.position = spritePos2;
                    currentSpriteGO.transform.position = spritePos;
                }
                else
                {
                    Vector3 spritePos2 = Camera.main.ScreenToWorldPoint(new Vector2((Screen.width / 2) - (Input.mousePosition.x - (Screen.width / 2)), Input.mousePosition.y));
                    spritePos2.z += 1;
                    spriteGenoux1.transform.position = spritePos2;
                    spriteGenoux2.transform.position = spritePos;
                }
            }
            else if (currentSpriteGO == spriteCoude1)
            {
                if (Input.mousePosition.x < Screen.width / 2)
                {
                    Vector3 spritePos2 = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width - Input.mousePosition.x, Input.mousePosition.y));
                    spritePos2.z += 1;
                    spriteCoude2.transform.position = spritePos2;
                    currentSpriteGO.transform.position = spritePos;
                }
                else
                {
                    Vector3 spritePos2 = Camera.main.ScreenToWorldPoint(new Vector2((Screen.width / 2) - (Input.mousePosition.x - (Screen.width / 2)), Input.mousePosition.y));
                    spritePos2.z += 1;
                    spriteCoude1.transform.position = spritePos2;
                    spriteCoude2.transform.position = spritePos;
                }
            }
            else if (currentSpriteGO == spritePoignet1)
            {
                if (Input.mousePosition.x < Screen.width / 2)
                {
                    Vector3 spritePos2 = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width - Input.mousePosition.x, Input.mousePosition.y));
                    spritePos2.z += 1;
                    spritePoignet2.transform.position = spritePos2;
                    currentSpriteGO.transform.position = spritePos;
                }
                else
                {
                    Vector3 spritePos2 = Camera.main.ScreenToWorldPoint(new Vector2((Screen.width / 2) - (Input.mousePosition.x - (Screen.width / 2)), Input.mousePosition.y));
                    spritePos2.z += 1;
                    spritePoignet1.transform.position = spritePos2;
                    spritePoignet2.transform.position = spritePos;
                }
            }
            else
            {
                spritePos.x = 0;
                currentSpriteGO.transform.position = spritePos;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                currentSpriteGO = null;
            }
        }

        if (spriteHead && spriteCoude1 && spriteEntreJambes && spritePoignet1 && spriteGenoux1 && Input.GetKeyDown(KeyCode.D))
        {
            //Découpe
            Mesh mesh = meshFilter.mesh;
            Vector3[] vertices = mesh.vertices;
            List<int> triangles = new List<int>();
            triangles.AddRange(mesh.triangles);
            List<Vector2> uvs = new List<Vector2>();
            uvs.AddRange(mesh.uv);

            //////////////////// Head ////////////////////
            Mesh meshHead = new Mesh();
            List<Vector3> verticesHead = new List<Vector3>();
            List<int> trianglesHead = new List<int>();
            List<Vector2> uvsHead = new List<Vector2>();

            float headY = spriteHead.transform.position.y;
            for (int i = 0; i < triangles.Count; i += 3)
            {
                Vector3 v1 = model3D.transform.TransformPoint(vertices[triangles[i]]);
                Vector3 v2 = model3D.transform.TransformPoint(vertices[triangles[i + 1]]);
                Vector3 v3 = model3D.transform.TransformPoint(vertices[triangles[i + 2]]);
                if (v1.y >= headY || v2.y >= headY || v3.y >= headY)
                {
                    verticesHead.Add(vertices[triangles[i]]);
                    verticesHead.Add(vertices[triangles[i + 1]]);
                    verticesHead.Add(vertices[triangles[i + 2]]);
                    trianglesHead.Add(verticesHead.Count - 3);
                    trianglesHead.Add(verticesHead.Count - 2);
                    trianglesHead.Add(verticesHead.Count - 1);
                    uvsHead.Add(uvs[triangles[i]]);
                    uvsHead.Add(uvs[triangles[i+1]]);
                    uvsHead.Add(uvs[triangles[i+2]]);

                    triangles.RemoveRange(i, 3);
                    i -= 3;
                }
            }
            meshHead.indexFormat = mesh.indexFormat;
            meshHead.SetVertices(verticesHead);
            meshHead.SetTriangles(trianglesHead, 0);
            meshHead.SetUVs(0, uvsHead);
            meshHead.RecalculateNormals();

            headGO = new GameObject("Head");
            headGO.transform.position = model3D.transform.position;
            headGO.transform.rotation = model3D.transform.rotation;
            headGO.transform.localScale = model3D.transform.localScale;
            MeshRenderer mr = headGO.AddComponent<MeshRenderer>();
            mr.material = meshRenderer.material;
            MeshFilter mf = headGO.AddComponent<MeshFilter>();
            mf.mesh = meshHead;

            ////////////////// Jambes /////////////////////
            Mesh meshCuisseGauche = new Mesh();
            List<Vector3> verticesCuisseGauche = new List<Vector3>();
            List<int> trianglesCuisseGauche = new List<int>();
            List<Vector2> uvsCuisseGauche = new List<Vector2>();

            Mesh meshTibiaGauche = new Mesh();
            List<Vector3> verticesTibiaGauche = new List<Vector3>();
            List<int> trianglesTibiaGauche = new List<int>();
            List<Vector2> uvsTibiaGauche = new List<Vector2>();

            Mesh meshCuisseDroite = new Mesh();
            List<Vector3> verticesCuisseDroite = new List<Vector3>();
            List<int> trianglesCuisseDroite = new List<int>();
            List<Vector2> uvsCuisseDroite = new List<Vector2>();

            Mesh meshTibiaDroite = new Mesh();
            List<Vector3> verticesTibiaDroite = new List<Vector3>();
            List<int> trianglesTibiaDroite = new List<int>();
            List<Vector2> uvsTibiaDroite = new List<Vector2>();

            float jambesY = spriteEntreJambes.transform.position.y;
            float genouxY = spriteGenoux1.transform.position.y;
            float minXShoulder = 0;
            float minXCoude = spriteCoude1.transform.position.x;
            for (int i = 0; i < triangles.Count; i += 3)
            {
                Vector3 v1 = model3D.transform.TransformPoint(vertices[triangles[i]]);
                Vector3 v2 = model3D.transform.TransformPoint(vertices[triangles[i + 1]]);
                Vector3 v3 = model3D.transform.TransformPoint(vertices[triangles[i + 2]]);
                if (v1.y < jambesY || v2.y < jambesY || v3.y < jambesY) // si sous Entre Jambes c'est les jambes
                {
                    if (v1.x < minXShoulder && v1.x > minXCoude) minXShoulder = v1.x; // help to find shoulder
                    if (v2.x < minXShoulder && v2.x > minXCoude) minXShoulder = v2.x;
                    if (v3.x < minXShoulder && v3.x > minXCoude) minXShoulder = v3.x;

                    if (v1.x < 0)
                    {
                        if (v1.y < genouxY || v2.y < genouxY || v3.y < genouxY) // si sous Genoux c'est le bas de la jambe
                        {
                            verticesTibiaDroite.Add(vertices[triangles[i]]);
                            verticesTibiaDroite.Add(vertices[triangles[i + 1]]);
                            verticesTibiaDroite.Add(vertices[triangles[i + 2]]);
                            trianglesTibiaDroite.Add(verticesTibiaDroite.Count - 3);
                            trianglesTibiaDroite.Add(verticesTibiaDroite.Count - 2);
                            trianglesTibiaDroite.Add(verticesTibiaDroite.Count - 1);
                            uvsTibiaDroite.Add(uvs[triangles[i]]);
                            uvsTibiaDroite.Add(uvs[triangles[i + 1]]);
                            uvsTibiaDroite.Add(uvs[triangles[i + 2]]);
                        }
                        else
                        {
                            verticesCuisseDroite.Add(vertices[triangles[i]]);
                            verticesCuisseDroite.Add(vertices[triangles[i + 1]]);
                            verticesCuisseDroite.Add(vertices[triangles[i + 2]]);
                            trianglesCuisseDroite.Add(verticesCuisseDroite.Count - 3);
                            trianglesCuisseDroite.Add(verticesCuisseDroite.Count - 2);
                            trianglesCuisseDroite.Add(verticesCuisseDroite.Count - 1);
                            uvsCuisseDroite.Add(uvs[triangles[i]]);
                            uvsCuisseDroite.Add(uvs[triangles[i + 1]]);
                            uvsCuisseDroite.Add(uvs[triangles[i + 2]]);
                        }

                    }
                    else
                    {
                        if (v1.y < genouxY || v2.y < genouxY || v3.y < genouxY) // si sous Genoux c'est le bas de la jambe
                        {

                            verticesTibiaGauche.Add(vertices[triangles[i]]);
                            verticesTibiaGauche.Add(vertices[triangles[i + 1]]);
                            verticesTibiaGauche.Add(vertices[triangles[i + 2]]);
                            trianglesTibiaGauche.Add(verticesTibiaGauche.Count - 3);
                            trianglesTibiaGauche.Add(verticesTibiaGauche.Count - 2);
                            trianglesTibiaGauche.Add(verticesTibiaGauche.Count - 1);
                            uvsTibiaGauche.Add(uvs[triangles[i]]);
                            uvsTibiaGauche.Add(uvs[triangles[i + 1]]);
                            uvsTibiaGauche.Add(uvs[triangles[i + 2]]);
                        }
                        else
                        {
                            verticesCuisseGauche.Add(vertices[triangles[i]]);
                            verticesCuisseGauche.Add(vertices[triangles[i + 1]]);
                            verticesCuisseGauche.Add(vertices[triangles[i + 2]]);
                            trianglesCuisseGauche.Add(verticesCuisseGauche.Count - 3);
                            trianglesCuisseGauche.Add(verticesCuisseGauche.Count - 2);
                            trianglesCuisseGauche.Add(verticesCuisseGauche.Count - 1);
                            uvsCuisseGauche.Add(uvs[triangles[i]]);
                            uvsCuisseGauche.Add(uvs[triangles[i + 1]]);
                            uvsCuisseGauche.Add(uvs[triangles[i + 2]]);
                        }
                    }

                    triangles.RemoveRange(i, 3);
                    i -= 3;
                }
            }
            meshCuisseGauche.indexFormat = mesh.indexFormat;
            meshCuisseGauche.SetVertices(verticesCuisseGauche);
            meshCuisseGauche.SetTriangles(trianglesCuisseGauche, 0);
            meshCuisseGauche.SetUVs(0, uvsCuisseGauche);
            meshCuisseGauche.RecalculateNormals();

            cuisseGaucheGO = new GameObject("CuisseGauche");
            cuisseGaucheGO.transform.position = model3D.transform.position;
            cuisseGaucheGO.transform.rotation = model3D.transform.rotation;
            cuisseGaucheGO.transform.localScale = model3D.transform.localScale;
            /*MeshRenderer*/
            mr = cuisseGaucheGO.AddComponent<MeshRenderer>();
            mr.material = meshRenderer.material;
            /*MeshFilter*/
            mf = cuisseGaucheGO.AddComponent<MeshFilter>();
            mf.mesh = meshCuisseGauche;

            meshTibiaGauche.indexFormat = mesh.indexFormat;
            meshTibiaGauche.SetVertices(verticesTibiaGauche);
            meshTibiaGauche.SetTriangles(trianglesTibiaGauche, 0);
            meshTibiaGauche.SetUVs(0, uvsTibiaGauche);
            meshTibiaGauche.RecalculateNormals();

            tibiaGaucheGO = new GameObject("TibiaGauche");
            tibiaGaucheGO.transform.position = model3D.transform.position;
            tibiaGaucheGO.transform.rotation = model3D.transform.rotation;
            tibiaGaucheGO.transform.localScale = model3D.transform.localScale;
            /*MeshRenderer*/
            mr = tibiaGaucheGO.AddComponent<MeshRenderer>();
            mr.material = meshRenderer.material;
            /*MeshFilter*/
            mf = tibiaGaucheGO.AddComponent<MeshFilter>();
            mf.mesh = meshTibiaGauche;

            ////////////////////// Droite /////////////////////////////
            meshCuisseDroite.indexFormat = mesh.indexFormat;
            meshCuisseDroite.SetVertices(verticesCuisseDroite);
            meshCuisseDroite.SetTriangles(trianglesCuisseDroite, 0);
            meshCuisseDroite.SetUVs(0, uvsCuisseDroite);
            meshCuisseDroite.RecalculateNormals();

            cuisseDroiteGO = new GameObject("CuisseDroite");
            cuisseDroiteGO.transform.position = model3D.transform.position;
            cuisseDroiteGO.transform.rotation = model3D.transform.rotation;
            cuisseDroiteGO.transform.localScale = model3D.transform.localScale;
            /*MeshRenderer*/
            mr = cuisseDroiteGO.AddComponent<MeshRenderer>();
            mr.material = meshRenderer.material;
            /*MeshFilter*/
            mf = cuisseDroiteGO.AddComponent<MeshFilter>();
            mf.mesh = meshCuisseDroite;

            meshTibiaDroite.indexFormat = mesh.indexFormat;
            meshTibiaDroite.SetVertices(verticesTibiaDroite);
            meshTibiaDroite.SetTriangles(trianglesTibiaDroite, 0);
            meshTibiaDroite.SetUVs(0, uvsTibiaDroite);
            meshTibiaDroite.RecalculateNormals();

            tibiaDroiteGO = new GameObject("TibiaDroite");
            tibiaDroiteGO.transform.position = model3D.transform.position;
            tibiaDroiteGO.transform.rotation = model3D.transform.rotation;
            tibiaDroiteGO.transform.localScale = model3D.transform.localScale;
            /*MeshRenderer*/
            mr = tibiaDroiteGO.AddComponent<MeshRenderer>();
            mr.material = meshRenderer.material;
            /*MeshFilter*/
            mf = tibiaDroiteGO.AddComponent<MeshFilter>();
            mf.mesh = meshTibiaDroite;

            /////////////////// Bras ///////////////////
            GameObject shoulderGSpriteGO = GameObject.Instantiate<GameObject>(spritePrefab);
            shoulderGSpriteGO.transform.position = new Vector3(minXShoulder, spriteCoude1.transform.position.y, -9);
            shoulderGSpriteGO.GetComponent<SpriteRenderer>().color = Color.red;

            Mesh meshShoulderGauche = new Mesh();
            List<Vector3> verticesShoulderGauche = new List<Vector3>();
            List<int> trianglesShoulderGauche = new List<int>();
            List<Vector2> uvsShoulderGauche = new List<Vector2>();

            Mesh meshAvantBrasGauche = new Mesh();
            List<Vector3> verticesAvantBrasGauche = new List<Vector3>();
            List<int> trianglesAvantBrasGauche = new List<int>();
            List<Vector2> uvsAvantBrasGauche = new List<Vector2>();

            Mesh meshPoignetGauche = new Mesh();
            List<Vector3> verticesPoignetGauche = new List<Vector3>();
            List<int> trianglesPoignetGauche = new List<int>();
            List<Vector2> uvsPoignetGauche = new List<Vector2>();

            Mesh meshShoulderDroite = new Mesh();
            List<Vector3> verticesShoulderDroite = new List<Vector3>();
            List<int> trianglesShoulderDroite = new List<int>();
            List<Vector2> uvsShoulderDroite = new List<Vector2>();

            Mesh meshAvantBrasDroite = new Mesh();
            List<Vector3> verticesAvantBrasDroite = new List<Vector3>();
            List<int> trianglesAvantBrasDroite = new List<int>();
            List<Vector2> uvsAvantBrasDroite = new List<Vector2>();

            Mesh meshPoignetDroite = new Mesh();
            List<Vector3> verticesPoignetDroite = new List<Vector3>();
            List<int> trianglesPoignetDroite = new List<int>();
            List<Vector2> uvsPoignetDroite = new List<Vector2>();

            Vector2 shoulderRPos = Camera.main.WorldToScreenPoint(shoulderGSpriteGO.transform.position);
            shoulderRPos.x = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - shoulderRPos.x, shoulderRPos.y, 0)).x;
            shoulderRPos.y = shoulderGSpriteGO.transform.position.y;
            GameObject shoulderRSpriteGO = GameObject.Instantiate<GameObject>(spritePrefab);
            shoulderRSpriteGO.transform.position = new Vector3(shoulderRPos.x, shoulderRPos.y, -9);
            shoulderRSpriteGO.GetComponent<SpriteRenderer>().color = Color.red;

            float maxXShoulder = shoulderRPos.x;
            float coudeX = spriteCoude1.transform.position.x;
            float poignetX = spritePoignet1.transform.position.x;
            float maxCoudeX = spriteCoude2.transform.position.x;
            float maxPoignetX = spritePoignet2.transform.position.x;
            for (int i = 0; i < triangles.Count; i += 3)
            {
                Vector3 v1 = model3D.transform.TransformPoint(vertices[triangles[i]]);
                Vector3 v2 = model3D.transform.TransformPoint(vertices[triangles[i + 1]]);
                Vector3 v3 = model3D.transform.TransformPoint(vertices[triangles[i + 2]]);
                if (v1.x < minXShoulder || v2.x < minXShoulder || v3.x < minXShoulder) // si apres shoulder c'est le bras droit
                {
                    if (v1.x < coudeX || v2.x < coudeX || v3.x < coudeX) // si apres coude c'est l'avant bras
                    {
                        if (v1.x < poignetX || v2.x < poignetX || v3.x < poignetX)
                        {
                            verticesPoignetDroite.Add(vertices[triangles[i]]);
                            verticesPoignetDroite.Add(vertices[triangles[i + 1]]);
                            verticesPoignetDroite.Add(vertices[triangles[i + 2]]);
                            trianglesPoignetDroite.Add(verticesPoignetDroite.Count - 3);
                            trianglesPoignetDroite.Add(verticesPoignetDroite.Count - 2);
                            trianglesPoignetDroite.Add(verticesPoignetDroite.Count - 1);
                            uvsPoignetDroite.Add(uvs[triangles[i]]);
                            uvsPoignetDroite.Add(uvs[triangles[i + 1]]);
                            uvsPoignetDroite.Add(uvs[triangles[i + 2]]);
                        }
                        else
                        {
                            verticesAvantBrasDroite.Add(vertices[triangles[i]]);
                            verticesAvantBrasDroite.Add(vertices[triangles[i + 1]]);
                            verticesAvantBrasDroite.Add(vertices[triangles[i + 2]]);
                            trianglesAvantBrasDroite.Add(verticesAvantBrasDroite.Count - 3);
                            trianglesAvantBrasDroite.Add(verticesAvantBrasDroite.Count - 2);
                            trianglesAvantBrasDroite.Add(verticesAvantBrasDroite.Count - 1);
                            uvsAvantBrasDroite.Add(uvs[triangles[i]]);
                            uvsAvantBrasDroite.Add(uvs[triangles[i + 1]]);
                            uvsAvantBrasDroite.Add(uvs[triangles[i + 2]]);
                        }
                    }
                    else
                    {
                        verticesShoulderDroite.Add(vertices[triangles[i]]);
                        verticesShoulderDroite.Add(vertices[triangles[i + 1]]);
                        verticesShoulderDroite.Add(vertices[triangles[i + 2]]);
                        trianglesShoulderDroite.Add(verticesShoulderDroite.Count - 3);
                        trianglesShoulderDroite.Add(verticesShoulderDroite.Count - 2);
                        trianglesShoulderDroite.Add(verticesShoulderDroite.Count - 1);
                        uvsShoulderDroite.Add(uvs[triangles[i]]);
                        uvsShoulderDroite.Add(uvs[triangles[i + 1]]);
                        uvsShoulderDroite.Add(uvs[triangles[i + 2]]);
                    }

                    triangles.RemoveRange(i, 3);
                    i -= 3;
                }
                else if(v1.x > maxXShoulder || v2.x > maxXShoulder || v3.x > maxXShoulder)
                {
                    if (v1.x > maxCoudeX || v2.x > maxCoudeX || v3.x > maxCoudeX) // si apres coude c'est l'avant bras
                    {
                        if (v1.x > maxPoignetX || v2.x > maxPoignetX || v3.x > maxPoignetX)
                        {
                            verticesPoignetGauche.Add(vertices[triangles[i]]);
                            verticesPoignetGauche.Add(vertices[triangles[i + 1]]);
                            verticesPoignetGauche.Add(vertices[triangles[i + 2]]);
                            trianglesPoignetGauche.Add(verticesPoignetGauche.Count - 3);
                            trianglesPoignetGauche.Add(verticesPoignetGauche.Count - 2);
                            trianglesPoignetGauche.Add(verticesPoignetGauche.Count - 1);
                            uvsPoignetGauche.Add(uvs[triangles[i]]);
                            uvsPoignetGauche.Add(uvs[triangles[i + 1]]);
                            uvsPoignetGauche.Add(uvs[triangles[i + 2]]);
                        }
                        else
                        {
                            verticesAvantBrasGauche.Add(vertices[triangles[i]]);
                            verticesAvantBrasGauche.Add(vertices[triangles[i + 1]]);
                            verticesAvantBrasGauche.Add(vertices[triangles[i + 2]]);
                            trianglesAvantBrasGauche.Add(verticesAvantBrasGauche.Count - 3);
                            trianglesAvantBrasGauche.Add(verticesAvantBrasGauche.Count - 2);
                            trianglesAvantBrasGauche.Add(verticesAvantBrasGauche.Count - 1);
                            uvsAvantBrasGauche.Add(uvs[triangles[i]]);
                            uvsAvantBrasGauche.Add(uvs[triangles[i + 1]]);
                            uvsAvantBrasGauche.Add(uvs[triangles[i + 2]]);
                        }
                    }
                    else
                    {
                        verticesShoulderGauche.Add(vertices[triangles[i]]);
                        verticesShoulderGauche.Add(vertices[triangles[i + 1]]);
                        verticesShoulderGauche.Add(vertices[triangles[i + 2]]);
                        trianglesShoulderGauche.Add(verticesShoulderGauche.Count - 3);
                        trianglesShoulderGauche.Add(verticesShoulderGauche.Count - 2);
                        trianglesShoulderGauche.Add(verticesShoulderGauche.Count - 1);
                        uvsShoulderGauche.Add(uvs[triangles[i]]);
                        uvsShoulderGauche.Add(uvs[triangles[i + 1]]);
                        uvsShoulderGauche.Add(uvs[triangles[i + 2]]);
                    }

                    triangles.RemoveRange(i, 3);
                    i -= 3;
                }
            }
            meshShoulderGauche.indexFormat = mesh.indexFormat;
            meshShoulderGauche.SetVertices(verticesShoulderGauche);
            meshShoulderGauche.SetTriangles(trianglesShoulderGauche, 0);
            meshShoulderGauche.SetUVs(0, uvsShoulderGauche);
            meshShoulderGauche.RecalculateNormals();

            shoulderGaucheGO = new GameObject("shoulderGauche");
            shoulderGaucheGO.transform.position = model3D.transform.position;
            shoulderGaucheGO.transform.rotation = model3D.transform.rotation;
            shoulderGaucheGO.transform.localScale = model3D.transform.localScale;
            /*MeshRenderer*/
            mr = shoulderGaucheGO.AddComponent<MeshRenderer>();
            mr.material = meshRenderer.material;
            /*MeshFilter*/
            mf = shoulderGaucheGO.AddComponent<MeshFilter>();
            mf.mesh = meshShoulderGauche;

            meshAvantBrasGauche.indexFormat = mesh.indexFormat;
            meshAvantBrasGauche.SetVertices(verticesAvantBrasGauche);
            meshAvantBrasGauche.SetTriangles(trianglesAvantBrasGauche, 0);
            meshAvantBrasGauche.SetUVs(0, uvsAvantBrasGauche);
            meshAvantBrasGauche.RecalculateNormals();

            avantBrasGaucheGO = new GameObject("AvantBrasGauche");
            avantBrasGaucheGO.transform.position = model3D.transform.position;
            avantBrasGaucheGO.transform.rotation = model3D.transform.rotation;
            avantBrasGaucheGO.transform.localScale = model3D.transform.localScale;
            /*MeshRenderer*/
            mr = avantBrasGaucheGO.AddComponent<MeshRenderer>();
            mr.material = meshRenderer.material;
            /*MeshFilter*/
            mf = avantBrasGaucheGO.AddComponent<MeshFilter>();
            mf.mesh = meshAvantBrasGauche;

            meshPoignetGauche.indexFormat = mesh.indexFormat;
            meshPoignetGauche.SetVertices(verticesPoignetGauche);
            meshPoignetGauche.SetTriangles(trianglesPoignetGauche, 0);
            meshPoignetGauche.SetUVs(0, uvsPoignetGauche);
            meshPoignetGauche.RecalculateNormals();

            poignetGaucheGO = new GameObject("PoignetGauche");
            poignetGaucheGO.transform.position = model3D.transform.position;
            poignetGaucheGO.transform.rotation = model3D.transform.rotation;
            poignetGaucheGO.transform.localScale = model3D.transform.localScale;
            /*MeshRenderer*/
            mr = poignetGaucheGO.AddComponent<MeshRenderer>();
            mr.material = meshRenderer.material;
            /*MeshFilter*/
            mf = poignetGaucheGO.AddComponent<MeshFilter>();
            mf.mesh = meshPoignetGauche;

            ////////////////////// Droite /////////////////////////////
            meshShoulderDroite.indexFormat = mesh.indexFormat;
            meshShoulderDroite.SetVertices(verticesShoulderDroite);
            meshShoulderDroite.SetTriangles(trianglesShoulderDroite, 0);
            meshShoulderDroite.SetUVs(0, uvsShoulderDroite);
            meshShoulderDroite.RecalculateNormals();

            shoulderDroiteGO = new GameObject("ShoulderDroite");
            shoulderDroiteGO.transform.position = model3D.transform.position;
            shoulderDroiteGO.transform.rotation = model3D.transform.rotation;
            shoulderDroiteGO.transform.localScale = model3D.transform.localScale;
            /*MeshRenderer*/
            mr = shoulderDroiteGO.AddComponent<MeshRenderer>();
            mr.material = meshRenderer.material;
            /*MeshFilter*/
            mf = shoulderDroiteGO.AddComponent<MeshFilter>();
            mf.mesh = meshShoulderDroite;

            meshAvantBrasDroite.indexFormat = mesh.indexFormat;
            meshAvantBrasDroite.SetVertices(verticesAvantBrasDroite);
            meshAvantBrasDroite.SetTriangles(trianglesAvantBrasDroite, 0);
            meshAvantBrasDroite.SetUVs(0, uvsAvantBrasDroite);
            meshAvantBrasDroite.RecalculateNormals();

            avantBrasDroiteGO = new GameObject("AvantBrasDroite");
            avantBrasDroiteGO.transform.position = model3D.transform.position;
            avantBrasDroiteGO.transform.rotation = model3D.transform.rotation;
            avantBrasDroiteGO.transform.localScale = model3D.transform.localScale;
            /*MeshRenderer*/
            mr = avantBrasDroiteGO.AddComponent<MeshRenderer>();
            mr.material = meshRenderer.material;
            /*MeshFilter*/
            mf = avantBrasDroiteGO.AddComponent<MeshFilter>();
            mf.mesh = meshAvantBrasDroite;

            meshPoignetDroite.indexFormat = mesh.indexFormat;
            meshPoignetDroite.SetVertices(verticesPoignetDroite);
            meshPoignetDroite.SetTriangles(trianglesPoignetDroite, 0);
            meshPoignetDroite.SetUVs(0, uvsPoignetDroite);
            meshPoignetDroite.RecalculateNormals();

            poignetDroiteGO = new GameObject("PoignetDroite");
            poignetDroiteGO.transform.position = model3D.transform.position;
            poignetDroiteGO.transform.rotation = model3D.transform.rotation;
            poignetDroiteGO.transform.localScale = model3D.transform.localScale;
            /*MeshRenderer*/
            mr = poignetDroiteGO.AddComponent<MeshRenderer>();
            mr.material = meshRenderer.material;
            /*MeshFilter*/
            mf = poignetDroiteGO.AddComponent<MeshFilter>();
            mf.mesh = meshPoignetDroite;

            /////////////////////////////////////////////////////

            mesh = new Mesh();
            mesh.indexFormat = meshHead.indexFormat;
            mesh.SetVertices(new List<Vector3>(vertices));
            mesh.SetTriangles(triangles, 0);
            mesh.SetUVs(0, uvs);
            mesh.RecalculateNormals();
            meshFilter.mesh = mesh;

            ShowSkeleton();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (spriteHead)
                currentSpriteGO = spriteHead;
            else
            {
                currentSpriteGO = GameObject.Instantiate<GameObject>(spritePrefab);
                spriteHead = currentSpriteGO;
                spriteHead.GetComponent<SpriteRenderer>().color = Color.cyan;
            }
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            if (spriteEntreJambes)
                currentSpriteGO = spriteEntreJambes;
            else
            {
                currentSpriteGO = GameObject.Instantiate<GameObject>(spritePrefab);
                spriteEntreJambes = currentSpriteGO;
                spriteEntreJambes.GetComponent<SpriteRenderer>().color = Color.magenta;
            }
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            if (spriteGenoux1)
                currentSpriteGO = spriteGenoux1;
            else
            {
                currentSpriteGO = GameObject.Instantiate<GameObject>(spritePrefab);
                spriteGenoux2 = GameObject.Instantiate<GameObject>(spritePrefab);
                spriteGenoux1 = currentSpriteGO;
                spriteGenoux1.GetComponent<SpriteRenderer>().color = new Color(1, 0.3f, 0);
                spriteGenoux2.GetComponent<SpriteRenderer>().color = new Color(1, 0.3f, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            if (spriteCoude1)
                currentSpriteGO = spriteCoude1;
            else
            {
                currentSpriteGO = GameObject.Instantiate<GameObject>(spritePrefab);
                spriteCoude2 = GameObject.Instantiate<GameObject>(spritePrefab);
                spriteCoude1 = currentSpriteGO;
                spriteCoude1.GetComponent<SpriteRenderer>().color = Color.yellow;
                spriteCoude2.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            if (spritePoignet1)
                currentSpriteGO = spritePoignet1;
            else
            {
                currentSpriteGO = GameObject.Instantiate<GameObject>(spritePrefab);
                spritePoignet2 = GameObject.Instantiate<GameObject>(spritePrefab);
                spritePoignet1 = currentSpriteGO;
                spritePoignet1.GetComponent<SpriteRenderer>().color = Color.green;
                spritePoignet2.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
    }

    Vector3 GetBarycentreOfGameObject(GameObject go)
    {
        return go.GetComponent<MeshRenderer>().bounds.center;
    }

    void ShowSkeleton()
    {
        Vector3 headPos = GetBarycentreOfGameObject(headGO);
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.position = headPos;
        go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }
}
