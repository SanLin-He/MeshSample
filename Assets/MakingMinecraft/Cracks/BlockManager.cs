using UnityEngine;
using System.Collections;

public class BlockManager : MonoBehaviour {

	public Texture[] cracks;
    public Texture noCrack;
	int numHits = 0;
	float lastHitTime;
	float hitTimeThreshold = 0.5f;

	public bool RegisterHit()
	{
	    bool destroy = false;
		//if short enough time between hits then register as another hit
		if(hitTimeThreshold > Time.time - lastHitTime)
		{
			numHits++;
			CancelInvoke();
		    if (numHits < cracks.Length)
		        this.GetComponent<Renderer>().material.SetTexture("_DetailMask", cracks[numHits]);
		    else
		    {
				//Destroy(this.gameObject);
		        destroy = true;
		    }

			Invoke("Heal",2f);
		}

		lastHitTime = Time.time;

	    return destroy;
	}

	void Heal()
	{
		numHits = 0;
		this.GetComponent<Renderer>().material.SetTexture("_DetailMask", noCrack);
	}
}
