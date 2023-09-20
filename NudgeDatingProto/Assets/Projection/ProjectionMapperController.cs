using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ProjectionMapperController : MonoBehaviour
{
    [System.Serializable]
    public class CornerPositions
    {
        public Vector2[] corners;
    }

    [Header("Settings")]
    [SerializeField] private Vector2[] corners;
    [SerializeField] private float z;
    [SerializeField] private Material mat;
    [SerializeField] private int resolution;

    [Header("Controls")]
    [SerializeField] private KeyCode[] activationKey;
    [SerializeField] private float startMoveStep;
    [SerializeField] private float moveStepIncrement;
    [SerializeField] private KeyCode testTextureKey;
    [SerializeField] private RenderTexture normalTexture;
    [SerializeField] private Vector2 normalTextureTiling;
    [SerializeField] private Texture2D testTexture;
    [SerializeField] private Vector2 testTextureTiling;
    [Space(40)]
    public float moveStep;

    [Header("Save Options")]
    [SerializeField] private string fileName;

    private MeshRenderer mr;
    private MeshFilter meshFilter;
    private Mesh mesh;
    private Vector3[] vertices;

    private void Start()
    {
        ReadoutPositions();

        Generate();

        moveStep = startMoveStep;
    }

    private void Update()
    {
        for (int i = 0; i < activationKey.Length; i++)
        {
            if (Input.GetKey(activationKey[i]))
            {
                if (Input.GetKey(KeyCode.LeftArrow)) { MoveCorner(i, Vector2.right * -moveStep); }
                if (Input.GetKey(KeyCode.RightArrow)) { MoveCorner(i, Vector2.right * moveStep); }
                if (Input.GetKey(KeyCode.DownArrow)) { MoveCorner(i, Vector2.up * -moveStep); }
                if (Input.GetKey(KeyCode.UpArrow)) { MoveCorner(i, Vector2.up * moveStep); }
            }
        }

        if (Input.GetKeyDown(KeyCode.RightBracket)) { moveStep += moveStepIncrement; }
        else if (Input.GetKeyDown(KeyCode.LeftBracket)) { moveStep -= moveStepIncrement; }

        if (Input.GetKeyDown(testTextureKey))
        {
            mat.mainTexture = testTexture;
            //mat.mainTextureScale = testTextureTiling;
            mat.SetVector("_Tiling", testTextureTiling);
        }
        else if (Input.GetKeyUp(testTextureKey))
        {
            mat.mainTexture = normalTexture;
            mat.SetVector("_Tiling", normalTextureTiling);
        }
    }

    private void MoveCorner(int corner, Vector2 moveVector)
    {
        corners[corner] += moveVector;

        //Vector3[] vertices = new Vector3[4]
        //{
        //    new Vector3(corners[0].x, corners[0].y, z),
        //    new Vector3(corners[1].x, corners[1].y, z),
        //    new Vector3(corners[2].x, corners[2].y, z),
        //    new Vector3(corners[3].x, corners[3].y, z)
        //};

        //mesh.vertices = vertices;

        ReDraw();
    }

    private void Generate()
    {
        mr = gameObject.AddComponent<MeshRenderer>();
        mr.sharedMaterial = mat;

        meshFilter = gameObject.AddComponent<MeshFilter>();

        mesh = new Mesh();

        meshFilter.mesh = mesh;

        ReDraw();
    }

    private void ReDraw()
    {
        vertices = new Vector3[(resolution + 1) * (resolution + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
        for (int i = 0, y = 0; y <= resolution; y++)
        {
            for (int x = 0; x <= resolution; x++, i++)
            {
                float xPos = Mathf.Lerp(Mathf.Lerp(corners[0].x, corners[2].x, ((float)y / ((float)(resolution + 1)))), Mathf.Lerp(corners[1].x, corners[3].x, ((float)y / ((float)(resolution + 1)))), ((float)x / ((float)(resolution + 1))));
                float yPos = Mathf.Lerp(Mathf.Lerp(corners[0].y, corners[1].y, ((float)x / ((float)(resolution + 1)))), Mathf.Lerp(corners[2].y, corners[3].y, ((float)x / ((float)(resolution + 1)))), ((float)y / ((float)(resolution + 1))));
                vertices[i] = new Vector3(xPos, yPos, z);

                uv[i] = new Vector2((float)x / (float)resolution, (float)y / (float)resolution);
                tangents[i] = tangent;
            }          
        }

        //Vector3[] vertices = new Vector3[4]
        //{
        //    new Vector3(corners[0].x, corners[0].y, z),
        //    new Vector3(corners[1].x, corners[1].y, z),
        //    new Vector3(corners[2].x, corners[2].y, z),
        //    new Vector3(corners[3].x, corners[3].y, z)
        //};

        mesh.vertices = vertices;

        //int[] tris = new int[6]
        //{
        //    //lower left triangle
        //    0,2,1,
        //    //upper right triangle
        //    2,3,1
        //};


        int[] tris = new int[resolution * resolution * 6];
        for (int ti = 0, vi = 0, y = 0; y < resolution; y++, vi++)
        {
            for (int x = 0; x < resolution; x++, ti+=6, vi++)
            {
                tris[ti] = vi;
                tris[ti + 3] = tris[ti + 2] = vi + 1;
                tris[ti + 4] = tris[ti + 1] = vi + resolution + 1;
                tris[ti + 5] = vi + resolution + 2;
                
            }
        }

        mesh.triangles = tris;
        mesh.RecalculateNormals();

        mesh.uv = uv;
        mesh.tangents = tangents;


        //Vector3[] normals = new Vector3[4]
        //{
        //    -Vector3.forward,
        //    -Vector3.forward,
        //    -Vector3.forward,
        //    -Vector3.forward
        //};

        //mesh.normals = normals;


        //Vector2[] uv = new Vector2[4]
        //{
        //    new Vector2(0,0),
        //    new Vector2(1,0),
        //    new Vector2(0,1),
        //    new Vector2(1,1)
        //};

        //mesh.uv = uv;

        SavePositions();
    }

    private void ReadoutPositions()
    {
        CornerPositions cornerPositions = new CornerPositions();
        string filePath = Path.Combine(Application.persistentDataPath, $"{fileName}.json");
        if (!File.Exists(filePath)) { return; }
        string jsonString = File.ReadAllText(filePath);
        cornerPositions = JsonUtility.FromJson<CornerPositions>(jsonString);
        corners = cornerPositions.corners;
    }

    private void SavePositions()
    {
        CornerPositions cornerPositions = new CornerPositions();
        cornerPositions.corners = corners;
        string filePath = Path.Combine(Application.persistentDataPath, $"{fileName}.json");
        File.WriteAllText(filePath, JsonUtility.ToJson(cornerPositions));
        Debug.Log($"Saved positions to JSON at: {filePath}");
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if(vertices == null) { return; }

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
    */
}
