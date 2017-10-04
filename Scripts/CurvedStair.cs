using UnityEngine;

[ExecuteInEditMode]
public class CurvedStair : MonoBehaviour {

	public float innerRadius;
	public float stepHeight;
	public float stepWidth;
    public float angleOfCurve;
	public int numSteps;
	public float addToFirstStep;
	public bool counterClockwise;

    // Use this for initialization
    void Start () {

        buildMesh();

	}

	void OnValidate()
	{

        innerRadius = Mathf.Max(innerRadius, 0);
        stepHeight = Mathf.Max(stepHeight, 0.1f);
        stepWidth = Mathf.Max(stepWidth, 0.1f);
        numSteps = Mathf.Max(numSteps, 1);
        angleOfCurve = Mathf.Clamp(angleOfCurve, 1, 360);
        addToFirstStep = Mathf.Max(addToFirstStep, 0);

        if (GUI.changed) {

			buildMesh ();

		}
	}

    void Reset()
    {
        innerRadius = 2;
        stepHeight = 0.2f;
        stepWidth = 2;
        angleOfCurve = 90;
        numSteps = 10;
        addToFirstStep = 0;

        buildMesh();

    }

    void buildMesh() {
		int vertices_length = 20 * numSteps  + 4;
		int normals_length = 20 * numSteps + 4;
		int triangles_length = 30 * numSteps + 6;
		int uv_length = 20 * numSteps + 4;
		int step;

		Vector3[] vertices = new Vector3[vertices_length];
		int[] tri = new int[triangles_length];
		Vector3[] normals = new Vector3[normals_length];
		Vector2[] uv = new Vector2[uv_length];

		MeshFilter mf = GetComponent<MeshFilter>();
		Mesh mesh = new Mesh();
		mesh.Clear ();
		mf.mesh = mesh;

		Vector3 coord_0 = new Vector3(0, 0, 0);
		Vector3 coord_1 = new Vector3 (0, 0, 0);
		Vector3 coord_2 = new Vector3(0, 0, 0); 
		Vector3 coord_3 = new Vector3 (0, 0, 0);

		float degrees;
		Vector3 pos;
		Vector3 pos_start;
		Vector3 pos_end;
        float dist;
        float dist_inner;

		step = 20;
		for (int i = 0, count = 0; i < (numSteps * step) - 4; i += step, count++) {

			if (counterClockwise) {

				degrees = 180 + ((angleOfCurve / numSteps) * count);

			} else {

				degrees = 180 - ((angleOfCurve / numSteps) * count);

			}

			pos = getStartPosVectorFromAngle (degrees);
			pos_start = pos * innerRadius;
			pos_end = pos_start + (pos * stepWidth);

			if (counterClockwise) {

				coord_0 = new Vector3(pos_start.x, addToFirstStep + stepHeight + stepHeight * count, pos_start.z);
				coord_1 = new Vector3(pos_end.x, addToFirstStep + stepHeight + stepHeight * count, pos_end.z);

			} else {

				coord_0 = new Vector3(pos_end.x, addToFirstStep + stepHeight + stepHeight * count, pos_end.z);
				coord_1 = new Vector3(pos_start.x, addToFirstStep + stepHeight + stepHeight * count, pos_start.z);

			}

			vertices [i + 0] = coord_0;
			vertices [i + 1] = coord_1;

			if (counterClockwise) {

				degrees = 180 + ((angleOfCurve / numSteps) * (count + 1));

			} else {

				degrees = 180 - ((angleOfCurve / numSteps) * (count + 1));

			}

			pos = getStartPosVectorFromAngle (degrees);
			pos_start = pos * innerRadius;
			pos_end = pos_start + (pos * stepWidth);

			if (counterClockwise) {

				coord_2 = new Vector3 (pos_start.x, addToFirstStep + stepHeight + stepHeight * count, pos_start.z);
				coord_3 = new Vector3(pos_end.x, addToFirstStep + stepHeight + stepHeight * count, pos_end.z);

			} else {

				coord_2 = new Vector3(pos_end.x, addToFirstStep + stepHeight + stepHeight * count, pos_end.z);
				coord_3 = new Vector3 (pos_start.x, addToFirstStep + stepHeight + stepHeight * count, pos_start.z);

			}

			vertices [i + 2] = coord_2;
			vertices [i + 3] = coord_3;

			// front
			vertices [i + 4] = new Vector3 (coord_0.x, stepHeight * count, coord_0.z);
			vertices [i + 5] = new Vector3 (coord_1.x, stepHeight * count, coord_1.z);
			vertices [i + 6] = new Vector3 (coord_0.x, addToFirstStep + stepHeight + (stepHeight * count), coord_0.z);
			vertices [i + 7] = new Vector3 (coord_1.x, addToFirstStep + stepHeight + (stepHeight * count), coord_1.z);

			// right side
			vertices [i + 8] = new Vector3 (coord_1.x, 0, coord_1.z);
			vertices [i + 9] = new Vector3 (coord_3.x, 0, coord_3.z);
			vertices [i + 10] = new Vector3 (coord_1.x, coord_1.y, coord_1.z);
			vertices [i + 11] = new Vector3 (coord_3.x, coord_3.y, coord_3.z);

			// left side
			vertices [i + 12] = new Vector3 (coord_2.x, 0, coord_2.z);
			vertices [i + 13] = new Vector3 (coord_0.x, 0, coord_0.z);
			vertices [i + 14] = new Vector3 (coord_2.x, coord_2.y, coord_2.z);
			vertices [i + 15] = new Vector3 (coord_0.x, coord_0.y, coord_0.z);

			// bottom
			vertices [i + 16] = new Vector3 (coord_2.x, 0, coord_2.z);
			vertices [i + 17] = new Vector3 (coord_3.x, 0, coord_3.z);
			vertices [i + 18] = new Vector3 (coord_0.x, 0, coord_0.z);
			vertices [i + 19] = new Vector3 (coord_1.x, 0, coord_1.z);

		}

		// back
		vertices [vertices_length - 4] = new Vector3 (coord_3.x, 0, coord_3.z);
		vertices [vertices_length - 3] = new Vector3 (coord_2.x, 0, coord_2.z);
		vertices [vertices_length - 2] = new Vector3 (coord_3.x, coord_3.y, coord_3.z);
		vertices [vertices_length - 1] = new Vector3 (coord_2.x, coord_2.y, coord_2.z);

		// set triangle array
		step = 6;
		for (int i = 0, count = 0; i < tri.Length; i += step, count += 1) {

			//  Lower left triangle.
			tri[i] = 0 + (count * 4);
			tri[i+1] = 2 + (count * 4);
			tri[i+2] = 1 + (count * 4);

			//  Upper right triangle.  
			tri[i+3] = 2 + (count * 4);
			tri[i+4] = 3 + (count * 4);
			tri[i+5] = 1 + (count * 4);

		}

		step = 20;
		for (int i = 0; i < (normals.Length - 4); i += step) {

			// top
			normals [i+0] = -Vector3.down;
			normals [i+1] = -Vector3.down;
			normals [i+2] = -Vector3.down;
			normals [i+3] = -Vector3.down;

			// front
			normals [i+4] = -Vector3.forward;
			normals [i+5] = -Vector3.forward;
			normals [i+6] = -Vector3.forward;
			normals [i+7] = -Vector3.forward;

			// right
			normals [i+8] = Vector3.right;
			normals [i+9] = Vector3.right;
			normals [i+10] = Vector3.right;
			normals [i+11] = Vector3.right;

			// left
			normals [i + 12] = Vector3.left;
			normals [i + 13] = Vector3.left;
			normals [i + 14] = Vector3.left;
			normals [i + 15] = Vector3.left;

			// bottom
			normals [i + 16] = Vector3.down;
			normals [i + 17] = Vector3.down;
			normals [i + 18] = Vector3.down;
			normals [i + 19] = Vector3.down;

		}

		normals [normals_length-4] = -Vector3.forward;
		normals [normals_length-3] = -Vector3.forward;
		normals [normals_length-2] = -Vector3.forward;
		normals [normals_length-1] = -Vector3.forward;

		step = 20;
		for (int i = 0, count = 0; i < vertices.Length - 4; i += step, count += 1) {

            dist = Vector3.Distance(vertices[i], vertices[i + 2]);
            dist_inner = Vector3.Distance(vertices[i + 1], vertices[i + 3]);

            // top
            uv [0 + i] = new Vector2(vertices[0].x, vertices[0].z);
			uv [1 + i] = new Vector2(vertices[1].x, vertices[1].z + 0.25f);
            uv [2 + i] = new Vector2(vertices[0].x, vertices[2].z);
            uv [3 + i] = new Vector2(vertices[1].x, vertices[3].z + 0.25f);

            // front
            uv [4 + i] = new Vector2(0, 0);
            uv [5 + i] = new Vector2(stepWidth, 0);
            uv [6 + i] = new Vector2(0, addToFirstStep + stepHeight);
            uv [7 + i] = new Vector2(stepWidth, addToFirstStep + stepHeight);

            // right
            uv[8 + i] = new Vector2(0, vertices[i + 8].y);
            uv[9 + i] = new Vector2(dist_inner, vertices[i + 9].y);
            uv[10 + i] = new Vector2(0, vertices[i + 10].y);
            uv[11 + i] = new Vector2(dist_inner, vertices[i + 11].y);
           
            // left
            uv[12 + i] = new Vector2(dist, vertices[12 + i].y);
            uv[13 + i] = new Vector2(0, vertices[13 + i].y);
            uv[14 + i] = new Vector2(dist, vertices[14 + i].y);
            uv[15 + i] = new Vector2(0, vertices[15 + i].y);

            // bottom
            uv [16 + i] = new Vector2(vertices[0].x, vertices[0].z);
			uv [17 + i] = new Vector2(vertices[1].x, vertices[1].z + 0.25f);
			uv [18 + i] = new Vector2(vertices[0].x, vertices[2].z);
			uv [19 + i] = new Vector2(vertices[1].x, vertices[3].z + 0.25f);

		}

        dist = Vector3.Distance(vertices[vertices_length - 1], vertices[vertices_length - 2]);

        // back
        uv [uv_length - 4] = new Vector2(0, vertices[vertices_length - 4].y);
        uv [uv_length - 3] = new Vector2(dist, vertices[vertices_length - 3].y);
        uv [uv_length - 2] = new Vector2(0, vertices[vertices_length - 2].y);
        uv [uv_length - 1] = new Vector2(dist, vertices[vertices_length - 1].y);

        mesh.vertices = vertices;
		mesh.triangles = tri;
		mesh.normals = normals;
		mesh.RecalculateNormals ();
		mesh.RecalculateBounds ();
		mesh.RecalculateTangents ();
		mesh.uv = uv;

		GetComponent<MeshCollider>().sharedMesh = null;
		GetComponent<MeshCollider> ().sharedMesh = mf.sharedMesh;

	}

	private Vector3 getStartPosVectorFromAngle(float degrees) {
		float radians = degrees * Mathf.Deg2Rad;
		float x = Mathf.Cos(radians);
		float z = Mathf.Sin(radians);

		return new Vector3 (x, 0, z);
	}

}
