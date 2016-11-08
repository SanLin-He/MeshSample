using UnityEngine;
using System.Collections;
using System;

public class buildBlockMesh : MonoBehaviour {

	public GameObject newBlock;

    void Combine(GameObject block)
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];//CombineInstance
        Destroy(this.gameObject.GetComponent<MeshCollider>());

        int i = 0;

        while (i < meshFilters.Length) {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;//转换到世界坐标
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine,true);//合并网格

        transform.GetComponent<MeshFilter>().mesh.RecalculateBounds();
        transform.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        transform.GetComponent<MeshFilter>().mesh.Optimize();

        this.gameObject.AddComponent<MeshCollider>();
        transform.gameObject.SetActive(true);

        Destroy(block);

        //StaticBatchingUtility.Combine(gameObject);//设置网格可以被引擎静态合并
    }

    
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit, 1000.0f))
			{
                //generate new block
                Vector3 blockPos = hit.point + hit.normal/2.0f;//这是以0.5为间隔距离，因为是以原点为中心创建的跟之前的代码不同

                blockPos.x = (float) Math.Round(blockPos.x,MidpointRounding.AwayFromZero);
                blockPos.y = (float) Math.Round(blockPos.y,MidpointRounding.AwayFromZero);
                blockPos.z = (float) Math.Round(blockPos.z,MidpointRounding.AwayFromZero);
                       
                GameObject block = (GameObject) Instantiate(newBlock, blockPos, Quaternion.identity);
                block.transform.parent = this.transform;
                Combine(block);

			}
		}

	}
}

