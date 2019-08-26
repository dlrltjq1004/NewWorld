package com.example.xnslq.novaworld.Login_Register;

import com.android.volley.Response;
import com.android.volley.toolbox.StringRequest;

import java.util.HashMap;
import java.util.Map;

/**
 * Created by xnslq on 2018-05-24.
 */

public class RegisterRequest extends StringRequest {

    final static private String URL = "http://13.124.87.189/UserRegister.php";   // 데이터를 보낼 URL
    private Map<String, String> parameters;


    public RegisterRequest(String user_Email, String user_Password, Response.Listener<String> listener) {
        super (Method.POST, URL , listener, null);

        parameters = new HashMap <> ();
        parameters.put ("UserId", user_Email);
        parameters.put ("user_Password", user_Password);

    }

    @Override
    public Map<String, String> getParams() {
        return parameters;
    }
}
