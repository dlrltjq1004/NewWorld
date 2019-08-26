package com.example.xnslq.novaworld.Chat;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;

import com.example.xnslq.novaworld.Main_Activity;
import com.example.xnslq.novaworld.R;

public class ServerActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate (savedInstanceState);
        setContentView (R.layout.activity_server);

        Button butPc = findViewById (R.id.butPc);
        Button butAndroid = findViewById (R.id.butAndroid);

        butPc.setOnClickListener (new View.OnClickListener () {
            @Override
            public void onClick(View v) {
                startActivity (new Intent (ServerActivity.this, Main_Activity.class));
            }
        });

        butAndroid.setOnClickListener (new View.OnClickListener () {
            @Override
            public void onClick(View v) {

            }
        });



    }
}
