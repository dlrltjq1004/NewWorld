package com.example.xnslq.novaworld.SQLite;

import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import com.example.xnslq.novaworld.R;

public class SQLiteTestActivity extends AppCompatActivity {

    Button btn_add;
    Button btn_winner;
    EditText et_name;
    TextView tv_people;
    String sql;
    StringBuffer sb;
    String participants[] = new String[100];
    Cursor cursor;

    int count = 0;
    int version = 1;
    MyDatabaseOpenHelper helper;
    SQLiteDatabase database;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate (savedInstanceState);
        setContentView (R.layout.activity_sqlite_test);

      setUp();
      nameList();

    }

    private void setUp()
    {
        btn_add = (Button) findViewById (R.id.test_addbtn);
        btn_winner = (Button)findViewById (R.id.test_btn2);
        et_name = (EditText)findViewById (R.id.test_edit);
        tv_people = (TextView)findViewById (R.id.test_testview2);
        btn_add.setOnClickListener (myListener);
        btn_winner.setOnClickListener (myListener);

        helper = new MyDatabaseOpenHelper (SQLiteTestActivity.this,MyDatabaseOpenHelper.tableName,null,version);
        database = helper.getWritableDatabase ();
        sb = new StringBuffer ();
    }

    View.OnClickListener myListener = new View.OnClickListener () {
        @Override
        public void onClick(View v) {
            switch (v.getId ())
            {
                case R.id.test_addbtn:
                     sb.setLength (0);
                     helper.inserName (database,(et_name.getText ().toString ()));
                     nameList();
                    break;

                    case R.id.test_btn2:



                        break;
            }
        }
    };

    private void nameList()
    {
        sql = "select name from " + helper.tableName;
        cursor = database.rawQuery (sql,null);
        if (cursor != null)
        {
            count = cursor.getCount ();
            for (int i = 0; i < count; i++)
            {
                cursor.moveToNext ();
                String participant = cursor.getString(0);
                participants[i] = participant;
                sb.append (participants[i] + " ");
            }
            tv_people.setText ("" + sb);
            cursor.close ();
        }

    }
}
