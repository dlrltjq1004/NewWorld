package com.example.xnslq.novaworld.Login_Register;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;

import com.example.xnslq.novaworld.R;

public class RegisterActivity extends AppCompatActivity {

    ImageView register_close_btn ;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate (savedInstanceState);
        setContentView (R.layout.activity_register);

        register_close_btn = (ImageView) findViewById (R.id.register_close_btn);





    }
}
