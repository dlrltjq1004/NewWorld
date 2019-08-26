package com.example.xnslq.novaworld.Fragment;

import com.android.volley.Response;
import com.android.volley.toolbox.StringRequest;

import java.util.HashMap;
import java.util.Map;

public class AddFriendSearch extends StringRequest {


    final static private String URL = "http://13.124.87.189/AddFriend_Search.php";   // 데이터를 보낼 URL
    private Map<String, String> parameters;



    public AddFriendSearch(String SearchId,Response.Listener<String> listener) {
        super (Method.POST, URL , listener, null);

        parameters = new HashMap<> ();
        parameters.put ("SearchId", SearchId);


    }

    @Override
    public Map<String, String> getParams() {
        return parameters;
    }
}



