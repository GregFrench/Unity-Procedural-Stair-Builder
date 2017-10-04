using UnityEngine;
using UnityEditor;

public class MenuItems : MonoBehaviour {
	[MenuItem("GameObject/3D Object/Linear Stair")]
	private static void LinearStairOption()
	{
		GameObject obj = new GameObject ();

        obj.transform.position = SceneView.lastActiveSceneView.camera.transform.TransformPoint(Vector3.forward * 10f);

		obj.name = "Linear Stair";

		obj.AddComponent<MeshFilter> ();
		obj.AddComponent<MeshCollider> ();
		obj.AddComponent<MeshRenderer> ();
		obj.AddComponent<LinearStair> ();

		obj.GetComponent<MeshRenderer> ().sharedMaterial = new Material(Shader.Find("Standard"));

		LinearStair script = obj.GetComponent<LinearStair> ();
		script.stepLength = 0.3f;
		script.stepHeight = 0.2f;
		script.stepWidth = 2f;
		script.stepCount = 10;

		Selection.activeGameObject = obj.gameObject;
	}

	[MenuItem("GameObject/3D Object/Curved Stair")]
	private static void CurvedStairOption()
	{
		GameObject obj = new GameObject ();

        obj.transform.position = SceneView.lastActiveSceneView.camera.transform.TransformPoint(Vector3.forward * 10f);

        obj.name = "Curved Stair";

		obj.AddComponent<MeshFilter> ();
        obj.AddComponent<MeshCollider>();
        obj.AddComponent<MeshRenderer> ();
		obj.AddComponent<CurvedStair> ();

		obj.GetComponent<MeshRenderer> ().sharedMaterial = new Material(Shader.Find("Standard"));

        CurvedStair script = obj.GetComponent<CurvedStair> ();
		script.innerRadius = 2;
		script.stepHeight = 0.2f;
		script.stepWidth = 2;
		script.angleOfCurve = 90;
		script.numSteps = 10;
		script.addToFirstStep = 0;

		Selection.activeGameObject = obj.gameObject;

	}

	[MenuItem("GameObject/3D Object/Spiral Stair")]
	private static void SpiralStairOption()
	{
		GameObject obj = new GameObject ();

        obj.transform.position = SceneView.lastActiveSceneView.camera.transform.TransformPoint(Vector3.forward * 10f);

        obj.name = "Spiral Stair";

		obj.AddComponent<MeshFilter> ();
		obj.AddComponent<MeshCollider> ();
		obj.AddComponent<MeshRenderer> ();
		obj.AddComponent<SpiralStair> ();

		obj.GetComponent<MeshRenderer> ().sharedMaterial = new Material(Shader.Find("Standard"));

		SpiralStair script = obj.GetComponent<SpiralStair> ();
		script.innerRadius = 1;
		script.stepWidth = 2;
		script.stepHeight = 0.2f;
		script.stepThickness = 0.5f;
		script.numStepsPer360 = 16;
		script.numSteps = 16;
		script.slopedCeiling = false;
		script.slopedFloor = false;
		script.counterClockwise = false;

		Selection.activeGameObject = obj.gameObject;

	}
}
