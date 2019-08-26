package com.example.xnslq.novaworld.Chat;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Handler;
import android.os.Message;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.example.xnslq.novaworld.Adapter.DTO.ChatRoom_MsgDTO;
import com.example.xnslq.novaworld.Adapter.DTO.MsgDTO;
import com.example.xnslq.novaworld.Main_Activity;
import com.example.xnslq.novaworld.R;

import java.net.Socket;
import android.annotation.SuppressLint;
import android.app.Activity;

import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;

import com.example.xnslq.novaworld.Adapter.MsgAdapter;


import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.util.ArrayList;


public class FriendList_ChatRoomActivity extends AppCompatActivity {


   Room_SocketClient client ;
   Room_ReceiveThread receive;
   Room_SendThread send;

    Socket socket;
    Context context;
    Handler room_msgHandler ;

    EditText room_EditMessage;
    Button room_SendBtn;
    RecyclerView chatRoom_RecyclerView;
    ArrayList<MsgDTO> chatRoom_msgDTOs = new ArrayList <> ();
    MsgAdapter msgAdapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate (savedInstanceState);
        setContentView (R.layout.activity_friend_list__chat_room);

        room_EditMessage = (EditText) findViewById (R.id.room_editMessage);
        room_SendBtn = (Button)findViewById (R.id.room_btnSend);
        chatRoom_RecyclerView = (RecyclerView) findViewById (R.id.chatRoom_recyclerview);
        RecyclerView.LayoutManager layoutManager = new LinearLayoutManager (getApplicationContext ());
        msgAdapter = new MsgAdapter (getApplicationContext (),chatRoom_msgDTOs);
        chatRoom_RecyclerView.setLayoutManager (layoutManager);
        chatRoom_RecyclerView.setAdapter (msgAdapter);

//        chatRoom_msgDTOs.add (new MsgDTO (R.drawable.kakao_default_profile_image,"노바","안녕하세요"));
//        msgAdapter.notifyDataSetChanged ();



//        Toast.makeText (this,"친구아이디: " + friendId + "key: "+ chat_key,Toast.LENGTH_LONG).show ();

        client = new Room_SocketClient ("13.125.250.235","7777");
        client.start();

        room_SendBtn.setOnClickListener (new View.OnClickListener () {
            @Override
            public void onClick(View v) {
                String message = room_EditMessage.getText ().toString ();
                if (message != null || !message.equals ("")) {
                    send = new Room_SendThread (socket);
                    send.start ();
                    room_EditMessage.setText ("");
                }
            }
        });


    }




  Handler msgHandler = new Handler(){
        // 백그라운드 스레드에서 받은 메시지를 처리
        @Override
        public void handleMessage(Message msg) {
            super.handleMessage (msg);

            if (msg.what == 1111){
                // 채팅서버로 부터 수신한 메시지를 텍스트뷰에 추가
                SharedPreferences spf = getSharedPreferences ("LoginId",Activity.MODE_PRIVATE);


                int image = R.drawable.kakao_default_profile_image;

                String loginId = spf.getString ("LoginId","");

                String[] msgtest = msg.obj.toString ().split (":");
//                Log.d ("msg","username :" + msgtest[0] +"msg"+msgtest[1] +msgtest[2]);
                Intent intent = getIntent ();
                String friendId = intent.getStringExtra ("friendId");
                String chat_key =  intent.getStringExtra ("chat_key");

                if (chat_key.equals (msgtest[2]))
                {
                    chatRoom_msgDTOs.add (new MsgDTO (image,msgtest[0],msgtest[1]));
                    msgAdapter.notifyDataSetChanged ();
                    msgAdapter.onDetachedFromRecyclerView (chatRoom_RecyclerView);
                } else
                {

                }



//                MainWorldChat.setText (msg.obj.toString ()); // 채팅방 리스트 대화 내용 표시 부분

            }
        }
    };

    class Room_SocketClient extends Thread {
        boolean threadAlive;
        String ip;
        String port;

        OutputStream outputStream = null;
        //   BufferedReader br = null;
        DataOutputStream output = null;
        public  Room_SocketClient(String ip, String port){
            threadAlive = true;
            this.ip = ip;
            this.port = port;
        }
        public void run(){
            try {
                //채팅서버에 접속

                socket = new Socket(ip, Integer.parseInt (port));
                //서버에 메시지를 전달하기 위한 스트림 생성
                output = new DataOutputStream (socket.getOutputStream ());
                //메시지 수신용 스레드 생성
                receive = new Room_ReceiveThread (socket);
                receive.start();
                //와이파이 정보 관리자 객체로부터 폰의 mac address를 가져와서
                //채팅서버에 전달

                /* 와이파이 매니저 */

                //  WifiManager mng = (WifiManager)context.getSystemService (
                //        WIFI_SERVICE);

                //   WifiInfo info = mng.getConnectionInfo ();
                //   mac = info.getMacAddress ();

                SharedPreferences spf = getSharedPreferences ("LoginId",Activity.MODE_PRIVATE);
                String mac = spf.getString ("LoginId","");
                output.writeUTF (mac); //한글 처리를 위해 utf

            } catch (Exception e){
                e.printStackTrace ();
            }
        }

    } // end of SocketClient
    //내부 클래스
    class Room_ReceiveThread extends Thread {
        Socket socket = null;
        DataInputStream input = null;
        public Room_ReceiveThread(Socket socket){
            this.socket = socket;
            try{
                // 채팅서버로부터 메시지를 받기 위한 스트림 생성
                input = new DataInputStream (socket.getInputStream ());
            } catch(Exception e){
                e.printStackTrace ();
            }
        }
        public void run(){
            try{
                while( input != null){
                    // 채팅 서버로 부터 받은 메시지
                    String msg = input.readUTF ();

                    if (msg != null){
                        //핸들러에게 전달할 메시지 객체
                        Message hdmsg = msgHandler.obtainMessage ();
                        hdmsg.what=1111; // 메시지 식별자
                        hdmsg.obj = msg; // 메시지의 본문
                        // 핸들러에게 메시지 전달 (화면 변경 요청)
                        msgHandler.sendMessage (hdmsg);

                    }
                }
            }catch (Exception e){
                e.printStackTrace ();
            }
        }
    }// end of ReceiveThread
    // 내부 클래스
    class Room_SendThread extends Thread {
        Socket socket;

        String sendmsg = room_EditMessage.getText ().toString (); // 사용자가 작성한 메시지
        DataOutputStream output;
        public Room_SendThread(Socket socket){
            this.socket = socket;
            try{
                // 채팅서버로 메시지를 보내기 위한 스트림 생성
                output = new DataOutputStream (socket.getOutputStream ());

            }catch (Exception e){
                e.printStackTrace ();
            }
        }
        public void run() {
            try {
                if (output != null){
                    if (sendmsg != null){

                        //채팅서버에 메시지 전달
                        Intent intent = getIntent ();
                        String friendId = intent.getStringExtra ("friendId");
                        String chat_key =  intent.getStringExtra ("chat_key");

                        SharedPreferences spf = getSharedPreferences ("LoginId",Activity.MODE_PRIVATE);
                        String sendId = spf.getString ("LoginId","");
                        //   output.writeUTF (sendmsg);
                        output.writeUTF (sendId+": "+sendmsg+ ":"+chat_key);
                    }
                }

            }catch (Exception e){
                e.printStackTrace ();
            }
        }

    }

}
