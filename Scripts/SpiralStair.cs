using UnityEngine;

[ExecuteInEditMode]
public class SpiralStair : MonoBehaviour {

	public float innerRadius;
	public float stepWidth;
	public float stepHeight;
	public float stepThickness;
	public int numStepsPer360;
	public int numSteps;
	public bool slopedCeiling;
	public bool slopedFloor;
	public bool counterClockwise;

	// Use this for initialization
	void Start () {

		buildMesh ();

	}

	void OnValidate()
	{

        innerRadius = Mathf.Max(innerRadius, 0);
        stepWidth = Mathf.Max(stepWidth, 0.1f);
        stepHeight = Mathf.Max(stepHeight, 0);
        stepThickness = Mathf.Max(stepThickness, 0.1f);
        numStepsPer360 = Mathf.Max(numStepsPer360, 1);
        numSteps = Mathf.Max(numSteps, 1);

        if (GUI.changed) {

			buildMesh ();

		}
	}

    void Reset()
    {
        innerRadius = 1;
        stepWidth = 2;
        stepHeight = 0.2f;
        stepThickness = 0.5f;
        numStepsPer360 = 16;
        numSteps = 16;
        slopedCeiling = false;
        slopedFloor = false;
        counterClockwise = false;

        buildMesh();

    }

    void buildMesh() {
		int vertices_length = 24 * numSteps;
		int normals_length = 24 * numSteps;
		int triangles_length = 36 * numSteps;
		int uv_length = 24 * numSteps;
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

        step = 24;
		for (int i = 0, count = 0; i < (numSteps * step); i += step, count++) {

			if (counterClockwise) {

				degrees = 180 + ((360f / (float)numStepsPer360) * count);

			} else {

				degrees = 180 - ((360f / (float)numStepsPer360) * count);

			}

			pos = getStartPosVectorFromAngle (degrees);
			pos_start = pos * innerRadius;
			pos_end = pos_start + (pos * stepWidth);

			if (counterClockwise) {

                if (slopedFloor)
                {
                    float jump = (stepThickness + stepHeight * (count + 1)) - (stepThickness + stepHeight * count);
                    coord_0 = new Vector3(pos_start.x, (stepThickness + stepHeight * count) - jump, pos_start.z);
                    coord_1 = new Vector3(pos_end.x, (stepThickness + stepHeight * count) - jump, pos_end.z);
                }
                else
                {
                    coord_0 = new Vector3(pos_start.x, stepThickness + stepHeight * count, pos_start.z);
                    coord_1 = new Vector3(pos_end.x, stepThickness + stepHeight * count, pos_end.z);
                }
                

			} else {

                if (slopedFloor)
                {
                    float jump = (stepThickness + stepHeight * (count + 1)) - (stepThickness + stepHeight * count);
                    coord_0 = new Vector3(pos_end.x, (stepThickness + stepHeight * count) - jump, pos_end.z);
                    coord_1 = new Vector3(pos_start.x, (stepThickness + stepHeight * count) - jump, pos_start.z);
                }
                else
                {
                    coord_0 = new Vector3(pos_end.x, stepThickness + stepHeight * count, pos_end.z);
                    coord_1 = new Vector3(pos_start.x, stepThickness + stepHeight * count, pos_start.z);
                }

			}

			if (counterClockwise) {

				degrees = 180 + ((360f / (float)numStepsPer360) * (count + 1));

			} else {

				degrees = 180 - ((360f / (float)numStepsPer360) * (count + 1));

			}

			pos = getStartPosVectorFromAngle (degrees);
			pos_start = pos * innerRadius;
			pos_end = pos_start + (pos * stepWidth);

			if (counterClockwise) {

				coord_2 = new Vector3 (pos_start.x, stepThickness + stepHeight * count, pos_start.z);
				coord_3 = new Vector3(pos_end.x, stepThickness + stepHeight * count, pos_end.z);

            } else {

				coord_2 = new Vector3(pos_end.x, stepThickness + stepHeight * count, pos_end.z);
				coord_3 = new Vector3 (pos_start.x, stepThickness + stepHeight * count, pos_start.z);

			}

            // top
            vertices[i + 0] = coord_0;
            vertices[i + 1] = coord_1;
            vertices [i + 2] = coord_2;
			vertices [i + 3] = coord_3;

			// front
			vertices [i + 4] = new Vector3 (coord_0.x, stepHeight * count, coord_0.z);
			vertices [i + 5] = new Vector3 (coord_1.x, stepHeight * count, coord_1.z);
			vertices [i + 6] = new Vector3 (coord_0.x, coord_0.y, coord_0.z);
			vertices [i + 7] = new Vector3 (coord_1.x, coord_1.y, coord_1.z);

			// right side
			vertices [i + 8] = new Vector3 (coord_1.x, stepHeight * count, coord_1.z);
			if (slopedCeiling) {
				vertices [i + 9] = new Vector3 (coord_3.x, stepHeight * (count + 1), coord_3.z);
			} else {
				vertices [i + 9] = new Vector3 (coord_3.x, stepHeight * count, coord_3.z);
			}
			vertices [i + 10] = new Vector3 (coord_1.x, coord_1.y, coord_1.z);
			vertices [i + 11] = new Vector3 (coord_3.x, coord_3.y, coord_3.z);

			// left side
			if (slopedCeiling) {
				vertices [i + 12] = new Vector3 (coord_2.x, stepHeight * (count + 1), coord_2.z);
			} else {
				vertices [i + 12] = new Vector3 (coord_2.x, stepHeight * count, coord_2.z);
			}
			vertices [i + 13] = new Vector3 (coord_0.x, stepHeight * count, coord_0.z);
			vertices [i + 14] = new Vector3 (coord_2.x, coord_2.y, coord_2.z);
			vertices [i + 15] = new Vector3 (coord_0.x, coord_0.y, coord_0.z);

			// bottom
			if (slopedCeiling) {
				vertices [i + 16] = new Vector3 (coord_2.x, stepHeight * (count + 1), coord_2.z);
				vertices [i + 17] = new Vector3 (coord_3.x, stepHeight * (count + 1), coord_3.z);
			} else {
				vertices [i + 16] = new Vector3 (coord_2.x, stepHeight * count, coord_2.z);
				vertices [i + 17] = new Vector3 (coord_3.x, stepHeight * count, coord_3.z);
			}
			vertices [i + 18] = new Vector3 (coord_0.x, stepHeight * count, coord_0.z);
			vertices [i + 19] = new Vector3 (coord_1.x, stepHeight * count, coord_1.z);

			// back
			if (slopedCeiling) {
				vertices [i + 20] = new Vector3 (coord_3.x, stepHeight * (count + 1), coord_3.z);
				vertices [i + 21] = new Vector3 (coord_2.x, stepHeight * (count + 1), coord_2.z);
			} else {
				vertices [i + 20] = new Vector3 (coord_3.x, stepHeight * count, coord_3.z);
				vertices [i + 21] = new Vector3 (coord_2.x, stepHeight * count, coord_2.z);
			}
			vertices [i + 22] = new Vector3 (coord_3.x, coord_2.y, coord_3.z);
			vertices [i + 23] = new Vector3 (coord_2.x, coord_3.y, coord_2.z);

		}

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

		step = 24;
		for (int i = 0; i < normals.Length; i += step) {

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

			// back
			normals [i + 20] = -Vector3.forward;
			normals [i + 21] = -Vector3.forward;
			normals [i + 22] = -Vector3.forward;
			normals [i + 23] = -Vector3.forward;

		}

        step = 24;
        for (int i = 0, count = 0; i < vertices.Length; i += step, count += 1)
        {

            dist = Vector3.Distance(vertices[i], vertices[i + 2]);
            dist_inner = Vector3.Distance(vertices[i + 1], vertices[i + 3]);

            // top
            uv[0 + i] = new Vector2(vertices[0].x, vertices[0].z);
            uv[1 + i] = new Vector2(vertices[1].x, vertices[1].z + 0.25f);
            uv[2 + i] = new Vector2(vertices[0].x, vertices[2].z);
            uv[3 + i] = new Vector2(vertices[1].x, vertices[3].z + 0.25f);

            // front
            uv[4 + i] = new Vector2(0, 0);
            uv[5 + i] = new Vector2(stepWidth, 0);
            uv[6 + i] = new Vector2(0, stepThickness);
            uv[7 + i] = new Vector2(stepWidth, stepThickness);

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
            uv[16 + i] = new Vector2(vertices[0].x, vertices[0].z);
            uv[17 + i] = new Vector2(vertices[1].x, vertices[1].z + 0.25f);
            uv[18 + i] = new Vector2(vertices[0].x, vertices[2].z);
            uv[19 + i] = new Vector2(vertices[1].x, vertices[3].z + 0.25f);

            // back
            uv[20 + i] = new Vector2(0, 0);
            uv[21 + i] = new Vector2(stepWidth, 0);
            uv[22 + i] = new Vector2(0, stepThickness);
            uv[23 + i] = new Vector2(stepWidth, stepThickness);

        }

        mesh.vertices = vertices;
		mesh.triangles = tri;
		mesh.normals = normals;
		mesh.RecalculateNormals ();
		mesh.RecalculateBounds ();
		mesh.RecalculateTangents ();
		mesh.uv = uv;
	}

	private Vector3 getStartPosVectorFromAngle(float degrees) {
		float radians = degrees * Mathf.Deg2Rad;
		float x = Mathf.Cos(radians);
		float z = Mathf.Sin(radians);

		return new Vector3 (x, 0, z);
	}

}
