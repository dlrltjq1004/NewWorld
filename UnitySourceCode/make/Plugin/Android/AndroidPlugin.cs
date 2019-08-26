using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidPlugin : MonoBehaviour {

    AndroidJavaClass UnityPlayer;
    AndroidJavaObject currentActivity;
    public string androidIntentGetString;
    public string text;

    private void Awake()
    {
        //AndroidLoginId();
        AndroidPluginGetIdCall();
    }

    void Start () {

        
    }
	
	
	void Update () {

        AndroidJavaObject getIntent = currentActivity.Call<AndroidJavaObject>("getIntent");
        //AndroidJavaObject setIntent = currentActivity.Call<AndroidJavaObject>("putIntent");
        bool hasExtra = getIntent.Call<bool>("hasExtra", "i");

        if (hasExtra)
        {
            AndroidJavaObject extras = getIntent.Call<AndroidJavaObject>("getExtras");
            androidIntentGetString = extras.Call<string>("getstring", "i");
        }

        //if()
        //{

        //}
    }

    public void AndroidLoginId()
    {
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity.player.UnityPlayer");
        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("cureentActivity");

       

        //if (hasExtra)
        //{
        //    AndroidJavaObject extras = intent.Call<AndroidJavaObject>("getExtras");
        //    androidIntentGetString = extras.Call<string>("getstring", "i");
        //}
    }

    public void AndroidPluginGetIdCall()
    {
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");

        text = intent.Call<string>("getStringExtra", "i");
    }

    public void AndroidIdPlugin()
    {
        using (AndroidJavaClass clss = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj = clss.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                obj.Call(
                    "runOnUiThread",
                    new AndroidJavaRunnable(
                        () =>
                        {
                            AndroidJavaObject extras = obj.Call<AndroidJavaObject>("getExtras");
                            androidIntentGetString = extras.Call<string>("getString", "i");
                        }
                        ));
            }
        }

       
    }

    }

