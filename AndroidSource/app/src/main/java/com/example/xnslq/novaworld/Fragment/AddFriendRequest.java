package com.example.xnslq.novaworld.Fragment;

import android.util.Log;

import com.android.volley.Response;
import com.android.volley.toolbox.StringRequest;

import java.util.HashMap;
import java.util.Map;

public class AddFriendRequest extends StringRequest{

    final static private String URL = "http://13.124.87.189/Applyforlist.php";   // 데이터를 보낼 URL
    private Map<String, String> parameters;



    public AddFriendRequest(String Request_id ,int Request_profileimage , String answer_id , Response.Listener<String> listener) {
        super (Method.POST, URL , listener, null);

        String request_ProfileImage = String.valueOf (Request_profileimage);
        Log.d ("파싱한로그인아이디",request_ProfileImage);
        Log.d ("로그인 아이디",Request_id);
        parameters = new HashMap<> ();
        parameters.put ("Request_id", Request_id);
        parameters.put ("Request_profileimage", request_ProfileImage);
        parameters.put ("answer_id", answer_id);



    }

    @Override
    public Map<String, String> getParams() {
        return parameters;
    }
}


