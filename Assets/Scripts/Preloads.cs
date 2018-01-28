using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preloads : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);

        StartCoroutine(loadData());
    }

    IEnumerator loadData()
    {
        yield return StartCoroutine(EffectMgr.Instance.LoadData());

        UnityEngine.SceneManagement.SceneManager.LoadScene("login");
    }


	
	// Update is called once per frame
	void Update () {
		
	}


}
