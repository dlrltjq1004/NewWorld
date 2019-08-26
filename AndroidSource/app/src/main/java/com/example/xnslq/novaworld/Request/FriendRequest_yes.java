package com.example.xnslq.novaworld.Request;

import com.android.volley.Response;
import com.android.volley.toolbox.StringRequest;

import java.util.HashMap;
import java.util.Map;

public class FriendRequest_yes extends StringRequest {

    final static private String URL = "http://13.124.87.189/FriendRequest_yes.php";   // 데이터를 보낼 URL
    private Map<String, String> parameters;



    public FriendRequest_yes(String my_id ,String friend_id,Response.Listener<String> listener) {
        super (Method.POST, URL , listener, null);

        parameters = new HashMap<> ();
        parameters.put ("my_id", my_id);
        parameters.put ("friend_id", friend_id);


    }

    @Override
    public Map<String, String> getParams() {
        return parameters;
    }
}
