package com.example.xnslq.novaworld.Request;

import com.android.volley.Response;
import com.android.volley.toolbox.StringRequest;

import java.util.HashMap;
import java.util.Map;

public class MyChatKey extends StringRequest {

    final static private String URL = "http://13.124.87.189/MyChatKey.php";   // 데이터를 보낼 URL
    private Map<String, String> parameters;



    public MyChatKey(String my_id ,Response.Listener<String> listener) {
        super (Method.POST, URL , listener, null);

        parameters = new HashMap<> ();
        parameters.put ("my_id", my_id);


    }

    @Override
    public Map<String, String> getParams() {
        return parameters;
    }
}
