package com.example.xnslq.novaworld.SQLite;

import android.content.Context;
import android.database.DatabaseErrorHandler;
import android.database.SQLException;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;

public class MyDatabaseOpenHelper extends SQLiteOpenHelper {

    public static final String tableName = "container";

    public MyDatabaseOpenHelper(Context context,String name,SQLiteDatabase.CursorFactory factory,int version) {
        super (context,name,factory,version);
    }


    @Override
    public void onCreate(SQLiteDatabase db) {
             createTable(db);

    }

    @Override
    public void onUpgrade(SQLiteDatabase db,int oldVersion,int newVersion) {

    }
    private void createTable(SQLiteDatabase db) {
      String sql = "CREATE TABLE " + tableName + "(name text)";
      try
      {
          db.execSQL (sql);
      }catch (SQLException e)
      {
          e.printStackTrace ();
      }
    }


    public void inserName(SQLiteDatabase db, String name)
    {
      db.beginTransaction ();
      try
      {
          String sql = "insert into " + tableName + "(name)" + "values('" + name + "')";
          db.execSQL (sql);
          db.setTransactionSuccessful ();
      } catch (Exception e)
      {
          e.printStackTrace ();
      }
      finally
      {
          db.endTransaction ();
      }
    }


}
