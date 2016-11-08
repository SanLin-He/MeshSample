using UnityEngine;
using System.Collections;

public class SetUVs : MonoBehaviour {

	public float pixelSize = 16;
	public float tileX = 1;
	public float tileY = 1;

	// Use this for initialization
	void Start () 
	{

		float tilePerc = 1/pixelSize;

		float umin = tilePerc * tileX;
		float umax = tilePerc * (tileX+1);
		float vmin = tilePerc * tileY;
		float vmax = tilePerc * (tileY+1);
		
		Vector2[] blockUVs = new Vector2[24];

		blockUVs[0] = new Vector2(umin, vmin); 
		blockUVs[1] = new Vector2(umax, vmin); 
		blockUVs[2] = new Vector2(umin, vmax); 
		blockUVs[3] = new Vector2(umax, vmax); 
		blockUVs[4] = new Vector2(umin, vmax); 
		blockUVs[5] = new Vector2(umax, vmax); 
		blockUVs[6] = new Vector2(umin, vmax); 
		blockUVs[7] = new Vector2(umax, vmax); 
		blockUVs[8] = new Vector2(umin, vmin); 
		blockUVs[9] = new Vector2(umax, vmin); 
		blockUVs[10] = new Vector2(umin, vmin); 
		blockUVs[11] = new Vector2(umax, vmin); 
		blockUVs[12] = new Vector2(umin, vmin); 
		blockUVs[13] = new Vector2(umin, vmax); 
		blockUVs[14] = new Vector2(umax, vmax); 
		blockUVs[15] = new Vector2(umax, vmin); 
		blockUVs[16] = new Vector2(umin, vmin); 
		blockUVs[17] = new Vector2(umin, vmax); 
		blockUVs[18] = new Vector2(umax, vmax); 
		blockUVs[19] = new Vector2(umax, vmin); 
		blockUVs[20] = new Vector2(umin, vmin); 
		blockUVs[21] = new Vector2(umin, vmax); 
		blockUVs[22] = new Vector2(umax, vmax); 
		blockUVs[23] = new Vector2(umax, vmin);

		Mesh mesh = this.GetComponent<MeshFilter>().mesh;
	    mesh.uv = blockUVs;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}


//Default Unity Cube UVs
		/*blockUVs[0] = new Vector2(0.0f, 0.0f); 
		blockUVs[1] = new Vector2(1.0f, 0.0f); 
		blockUVs[2] = new Vector2(0.0f, 1.0f); 
		blockUVs[3] = new Vector2(1.0f, 1.0f); 
		blockUVs[4] = new Vector2(0.0f, 1.0f); 
		blockUVs[5] = new Vector2(1.0f, 1.0f); 
		blockUVs[6] = new Vector2(0.0f, 1.0f); 
		blockUVs[7] = new Vector2(1.0f, 1.0f); 
		blockUVs[8] = new Vector2(0.0f, 0.0f); 
		blockUVs[9] = new Vector2(1.0f, 0.0f); 
		blockUVs[10] = new Vector2(0.0f, 0.0f); 
		blockUVs[11] = new Vector2(1.0f, 0.0f); 
		blockUVs[12] = new Vector2(0.0f, 0.0f); 
		blockUVs[13] = new Vector2(0.0f, 1.0f); 
		blockUVs[14] = new Vector2(1.0f, 1.0f); 
		blockUVs[15] = new Vector2(1.0f, 0.0f); 
		blockUVs[16] = new Vector2(0.0f, 0.0f); 
		blockUVs[17] = new Vector2(0.0f, 1.0f); 
		blockUVs[18] = new Vector2(1.0f, 1.0f); 
		blockUVs[19] = new Vector2(1.0f, 0.0f); 
		blockUVs[20] = new Vector2(0.0f, 0.0f); 
		blockUVs[21] = new Vector2(0.0f, 1.0f); 
		blockUVs[22] = new Vector2(1.0f, 1.0f); 
		blockUVs[23] = new Vector2(1.0f, 0.0f);*/
