package com.example.xnslq.novaworld.Login_Register;

import com.android.volley.Response;
import com.android.volley.toolbox.StringRequest;

import java.util.HashMap;
import java.util.Map;

/**
 * Created by xnslq on 2018-05-25.
 */

public class LoginRequest extends StringRequest{

    final static private String URL = "http://13.124.87.189/UserLogin.php";   // 데이터를 보낼 URL
    private Map<String, String> parameters;


    public LoginRequest(String userID, String userPassword, Response.Listener<String> listener) {
        super (Method.POST, URL , listener, null);

        parameters = new HashMap<> ();
        parameters.put ("UserId", userID);
        parameters.put ("userPassword", userPassword);

    }

    @Override
    public Map<String, String> getParams() {
        return parameters;
    }
}
