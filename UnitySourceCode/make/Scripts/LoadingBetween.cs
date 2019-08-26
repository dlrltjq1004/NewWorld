using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class LoadingBetween : MonoBehaviour {

    public Image progressBar ;
    bool IsDone = false;
    float fTime = 0f;
    AsyncOperation async_operation;



   
   
    // Use this for initialization
    void Start()
    {
        

        StartCoroutine(StartLoad("NovaWorldProject"));
    
    }
        // Update is called once per frame
        void Update() {

        fTime += 0.01f;
         
        progressBar.fillAmount = fTime;
      
       // Debug.Log("b" + fTime);

        if (fTime >= 3)
        {
            async_operation.allowSceneActivation = true;
        }
    }

    public IEnumerator StartLoad(string strSceneName)
    {
        async_operation = SceneManager.LoadSceneAsync(strSceneName);
     //   Debug.Log(async_operation);
        async_operation.allowSceneActivation = false;
   //     Debug.Log("로드 불러옴");
        if (IsDone == false)
        {

            IsDone = true;
        //    Debug.Log("isDone :"+IsDone);
            while (async_operation.progress < 0.9f)
            {
                progressBar.fillAmount = async_operation.progress;

                yield return true;
            }
        }
    }
    
}
