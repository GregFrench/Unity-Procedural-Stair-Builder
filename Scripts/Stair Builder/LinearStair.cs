using UnityEngine;
using System;

[ExecuteInEditMode]
public class LinearStair : MonoBehaviour {
    public float stepLength;
    public float stepHeight;
    public float stepWidth;
    public int stepCount;

    void Start() {
        buildMesh ();
        Undo.undoRedoPerformed += UndoRedoCallback;
    }

    void UndoRedoCallback() {
        buildMesh();
    }

    void OnValidate() {
        stepLength = Mathf.Max(stepLength, 0.1f);
        stepHeight = Mathf.Max(stepHeight, 0.1f);
        stepWidth = Mathf.Max(stepWidth, 0.1f);
        stepCount = Mathf.Max(stepCount, 1);

        if (GUI.changed) {
            buildMesh ();
        }
    }

    void Reset() {
        stepLength = 0.3f;
        stepHeight = 0.2f;
        stepWidth = 2f;
        stepCount = 10;

        buildMesh();
    }

    void buildMesh() {
        int vertices_length = 16 * (stepCount + 2);
        int normals_length = 16 * (stepCount + 2);
        int triangles_length = 24 * (stepCount + 2);
        int uv_length = 16 * (stepCount + 2);

        Vector3[] vertices = new Vector3[vertices_length];
        int[] tri = new int[triangles_length];
        Vector3[] normals = new Vector3[normals_length];
        Vector2[] uv = new Vector2[uv_length];

        int step = 16;
        for (int i = 0; i < (stepCount * step); i += step) {
            // front
            vertices [i+0] = new Vector3 (0, (i * stepHeight) / step, (i * stepLength) / step);
            vertices [i+1] = new Vector3 (stepWidth, (i * stepHeight) / step, (i * stepLength) / step);
            vertices [i+2] = new Vector3 (0, stepHeight + (i * stepHeight) / step, (i * stepLength) / step);
            vertices [i+3] = new Vector3 (stepWidth, stepHeight + (i * stepHeight) / step, (i * stepLength) / step);

            // right
            vertices [i+4] = new Vector3 (stepWidth, (i * stepHeight) / step, (i * stepLength) / step);
            vertices [i+5] = new Vector3 (stepWidth, (i * stepHeight) / step, stepLength * stepCount);
            vertices [i+6] = new Vector3 (stepWidth, stepHeight + (i * stepHeight) / step, (i * stepLength) / step);
            vertices [i+7] = new Vector3 (stepWidth, stepHeight + (i * stepHeight) / step, stepLength * stepCount);

            // left
            vertices [i+8] = new Vector3 (0, (i * stepHeight) / step, stepLength * stepCount);
            vertices [i+9] = new Vector3 (0, (i * stepHeight) / step, (i * stepLength) / step);
            vertices [i+10] = new Vector3 (0, stepHeight + (i * stepHeight) / step, stepLength * stepCount);
            vertices [i+11] = new Vector3 (0, stepHeight + (i * stepHeight) / step, (i * stepLength) / step);

            // top
            vertices [i+12] = new Vector3(0, stepHeight + (i * stepHeight) / step, (i * stepLength) / step);
            vertices [i+13] = new Vector3(stepWidth, stepHeight + (i * stepHeight) / step, (i * stepLength) / step);
            vertices [i+14] = new Vector3(0, stepHeight + (i * stepHeight) / step, stepLength + (i * stepLength) / step);
            vertices [i+15] = new Vector3(stepWidth, stepHeight + (i * stepHeight) / step, stepLength + (i * stepLength) / step);
        }

        // bottom
        vertices[vertices_length-8] = new Vector3 (stepWidth, 0, 0);
        vertices [vertices_length - 7] = new Vector3 (0, 0, 0);
        vertices [vertices_length - 6] = new Vector3 (stepWidth, 0, stepLength * stepCount);
        vertices [vertices_length - 5] = new Vector3 (0, 0, stepLength * stepCount);

        // back
        vertices[vertices_length-4] = new Vector3(stepWidth, 0, stepLength * stepCount);
        vertices [vertices_length - 3] = new Vector3(0, 0, stepLength * stepCount);
        vertices [vertices_length - 2] = new Vector3(stepWidth, stepHeight * stepCount, stepLength * stepCount);
        vertices [vertices_length - 1] = new Vector3 (0, stepHeight * stepCount, stepLength * stepCount);

        // set triangle array
        step = 6;
        for (int i = 0; i < tri.Length; i += step) {
            //  Lower left triangle.
            tri[i] = 0 + (i * 4) / step;
            tri[i+1] = 2 + (i * 4) / step;
            tri[i+2] = 1 + (i * 4) / step;

            //  Upper right triangle.
            tri[i+3] = 2 + (i * 4) / step;
            tri[i+4] = 3 + (i * 4) / step;
            tri[i+5] = 1 + (i * 4) / step;
        }

        step = 16;
        for (int i = 0; i < normals.Length; i += step) {
            normals [i+0] = -Vector3.forward;
            normals [i+1] = -Vector3.forward;
            normals [i+2] = -Vector3.forward;
            normals [i+3] = -Vector3.forward;

            normals [i+4] = Vector3.right;
            normals [i+5] = Vector3.right;
            normals [i+6] = Vector3.right;
            normals [i+7] = Vector3.right;

            normals [i+8] = Vector3.left;
            normals [i+9] = Vector3.left;
            normals [i+10] = Vector3.left;
            normals [i+11] = Vector3.left;

            normals [i+12] = -Vector3.down;
            normals [i+13] = -Vector3.down;
            normals [i+14] = -Vector3.down;
            normals [i+15] = -Vector3.down;
        }

        normals [normals_length-8] = -Vector3.up;
        normals [normals_length-7] = -Vector3.up;
        normals [normals_length-6] = -Vector3.up;
        normals [normals_length-5] = -Vector3.up;

        normals [normals_length-4] = -Vector3.back;
        normals [normals_length-3] = -Vector3.back;
        normals [normals_length-2] = -Vector3.back;
        normals [normals_length-1] = -Vector3.back;

        step = 16;
        for (int i = 0, count = 0; i < (stepCount * step); i += step, count += 1) {
            // front
            uv[0 + i] = new Vector2(0, 0);
            uv[1 + i] = new Vector2(stepWidth, 0);
            uv[2 + i] = new Vector2(0, stepHeight);
            uv[3 + i] = new Vector2(stepWidth, stepHeight);

            // right
            uv[4 + i] = new Vector2(vertices[i + 4].z, vertices[i + 4].y);
            uv[5 + i] = new Vector2(vertices[i + 5].z, vertices[i + 5].y);
            uv[6 + i] = new Vector2(vertices[i + 6].z, vertices[i + 6].y);
            uv[7 + i] = new Vector2(vertices[i + 7].z, vertices[i + 7].y);

            // left
            uv[8 + i] = new Vector2(vertices[i + 8].z, vertices[i + 8].y);
            uv[9 + i] = new Vector2(vertices[i + 9].z, vertices[i + 9].y);
            uv[10 + i] = new Vector2(vertices[i + 10].z, vertices[i + 10].y);
            uv[11 + i] = new Vector2(vertices[i + 11].z, vertices[i + 11].y);

            // top
            uv[12 + i] = new Vector2(0, 0);
            uv[13 + i] = new Vector2(stepWidth, 0);
            uv[14 + i] = new Vector2(0, stepLength);
            uv[15 + i] = new Vector2(stepWidth, stepLength);
        }

        // bottom
        uv[uv_length - 8] = new Vector2(vertices[vertices_length - 8].x, vertices[vertices_length - 8].z);
        uv[uv_length - 7] = new Vector2(vertices[vertices_length - 7].x, vertices[vertices_length - 7].z);
        uv[uv_length - 6] = new Vector2(vertices[vertices_length - 6].x, vertices[vertices_length - 6].z);
        uv[uv_length - 5] = new Vector2(vertices[vertices_length - 5].x, vertices[vertices_length - 5].z);

        // back
        uv[uv_length - 4] = new Vector2(vertices[vertices_length - 4].x, vertices[vertices_length - 4].y);
        uv[uv_length - 3] = new Vector2(vertices[vertices_length - 3].x, vertices[vertices_length - 3].y);
        uv[uv_length - 2] = new Vector2(vertices[vertices_length - 2].x, vertices[vertices_length - 2].y);
        uv[uv_length - 1] = new Vector2(vertices[vertices_length - 1].x, vertices[vertices_length - 1].y);

        MeshFilter mf = gameObject.GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mesh.Clear ();
        mf.mesh = mesh;
        mesh.vertices = vertices;
        mesh.triangles = tri;
        mesh.normals = normals;
        mesh.RecalculateNormals ();
        mesh.RecalculateBounds ();
        mesh.RecalculateTangents ();
        mesh.uv = uv;

        GetComponent<MeshCollider>().sharedMesh = null;
        GetComponent<MeshCollider>().sharedMesh = mf.sharedMesh;
    }
}
