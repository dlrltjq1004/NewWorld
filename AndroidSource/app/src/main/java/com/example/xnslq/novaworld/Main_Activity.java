package com.example.xnslq.novaworld;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Debug;
import android.os.Handler;
import android.os.Message;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

//import com.Company.NovaWorld2.UnityPlayerActivity;
import com.Company.NovaWorld2.UnityPlayerActivity;
import com.bumptech.glide.Glide;
// import com.example.xnslq.novaworld.Chat.ChatActivity;
import com.example.xnslq.novaworld.Adapter.DTO.MsgDTO;
import com.example.xnslq.novaworld.Adapter.MsgAdapter;
import com.example.xnslq.novaworld.Friend.FriendListActivity;
import com.example.xnslq.novaworld.Kakao_login.KakaoSignupActivity;
import com.example.xnslq.novaworld.Login_Register.Login_Activity;
import com.example.xnslq.novaworld.Profile.UserProfileActivity;
import com.kakao.network.ErrorResult;
import com.kakao.usermgmt.UserManagement;
import com.kakao.usermgmt.callback.MeResponseCallback;
import com.kakao.usermgmt.response.model.UserProfile;
import com.kakao.util.helper.log.Logger;
import com.kakao.util.helper.log.Tag;
import com.unity3d.player.GoogleARCoreApi;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.util.ArrayList;

//import com.example.xnslq.novaworld.Kakao_login.SessionCallback;

public class Main_Activity extends AppCompatActivity {

    private ImageView main_profile_img , main_MainImg;

    SocketClient client ;
    ReceiveThread receive;
    SendThread send;
    Socket socket;
    Context context;
    Handler handler ;
    String worldchat = "false";

    String loginType = "" ;
    TextView MainWorldChat,mainTest_textview,main_GameSearch_tev;
    ImageView mainItem_FriendList_btn;
    Button btnConnect, btnSend ,main_GameStart_Btn;
    EditText editIp, editPort, editMessage;



    MsgAdapter msgAdapter;
    ArrayList<MsgDTO> msgDTOs = new ArrayList<> ();
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate (savedInstanceState);
        setContentView (R.layout.activity_main_);

//
//      Button  maintest_btn = (Button)findViewById (R.id.maintest_btn);
//        maintest_btn.setOnClickListener (new View.OnClickListener () {
//            @Override
//            public void onClick(View v) {
//                startActivity (new Intent (Main_Activity.this,SQLiteTestActivity.class));
//            }
//        });
        main_profile_img = (ImageView) findViewById (R.id.main_profile_img); // 프로필 이미지
        main_MainImg = (ImageView) findViewById (R.id.main_MainImg); // 이미지 클릭시 게임을 시작하는 이미지
        MainWorldChat = (TextView) findViewById (R.id.MainWorldChat);  // 메인로비 월드 채팅 텍스트뷰
        main_GameStart_Btn = (Button) findViewById(R.id.main_GameStart_btn);
        main_GameSearch_tev =(TextView) findViewById(R.id.main_GameStart_tev);

        context = Main_Activity.this;

//        mainTest_textview = (TextView) findViewById(R.id.mainTest_textview);

//        // 서버 접속
//        client = new SocketClient ("13.125.250.235","8888");
//        client.start();
//        receive = new ReceiveThread (socket);
//        receive.start ();
//
//        msgHandler = new Handler(){
//            // 백그라운드 스레드에서 받은 메시지를 처리
//            @Override
//            public void handleMessage(Message msg) {
//                super.handleMessage (msg);
//
//                if (msg.what == 1111){
//                    // 채팅서버로 부터 수신한 메시지를 텍스트뷰에 추가
//                    SharedPreferences spf = getSharedPreferences ("LoginId",Activity.MODE_PRIVATE);
//                    String[] msgtest = msg.obj.toString ().split (":");
//
//                    MainWorldChat.setText (msgtest[0]+":"+msgtest[1]);
//
//                }
//            }
//        };

        final Intent intent = getIntent ();
        loginType = intent.getStringExtra ("NovaWorld_login");

        if (loginType == null){
            loginType = "";
        }



        if (loginType.equals ("NovaWorld_login"))
        {
            main_profile_img.setImageResource (R.drawable.kakao_default_profile_image);
        } else
        {
            requestMe();       // 카카오 사용자 정보 요청

        }

        // 프로필 클릭 이벤트

        // ㄴ 프로필 액티비티로 이동
        main_profile_img.setOnClickListener (new View.OnClickListener () {
            @Override
            public void onClick(View v) {

                startActivity (new Intent (getApplicationContext (),UserProfileActivity.class));
                finish ();


            }
        });

        // 실시간 대전 클릭 이벤트

        main_GameStart_Btn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
// Tcp 시작


                main_GameSearch_tev.setVisibility(View.VISIBLE);
                // 게임 서버에 접속
                client = new SocketClient ("13.124.87.189","8888");
                client.start();

                handler = new Handler(){
                    public Socket socket;

                    // 백그라운드 스레드에서 받은 메시지를 처리
                    @Override
                    public void handleMessage(Message msg) {
                        super.handleMessage (msg);
                            String camp;
                        if (msg.what == 1111){
                            // 서버로 부터 수신한 메시지를 텍스트뷰에 추가
                            SharedPreferences spf = getSharedPreferences ("LoginId",Activity.MODE_PRIVATE);


                            int image = R.drawable.kakao_default_profile_image;

                            String loginId = spf.getString ("LoginId","");

//                            mainTest_textview.setText(msg.obj.toString());
                          String[] success = msg.obj.toString().split(",");
                            Log.i("success value:", msg.obj.toString() );
                             if (msg.obj.toString().equals("success"))
                             {
//
                                 //    유니티 프로젝트로 데이터 전달 (방 Key,유저 인원,레드진영 블루진영 나눈 정보,)

//                                 Intent intent1 = new Intent(Main_Activity.this , UnityPlayerActivity.class);
//
//                                 intent1.putExtra("i",loginId);
//                                 intent1.putExtra("key",msg.obj.toString());
                                 main_GameSearch_tev.setVisibility(View.INVISIBLE);
                                 Intent intent1 = new Intent(Main_Activity.this , UnityPlayerActivity.class);
                                 Log.i("Unity:",loginId);
                                 intent1.putExtra("i",loginId);
                                 startActivity(intent1);
//                               finish();

//                                 Toast.makeText(Main_Activity.this,"success : "+ msg.obj.toString(),Toast.LENGTH_SHORT).show();
                             } else {
//                                 Toast.makeText(Main_Activity.this,"fail",Toast.LENGTH_SHORT).show();
                             }

                        }
                    }
                };






            }
        });




        // 서버에 접속 버튼
//        btnConnect.setOnClickListener (new View.OnClickListener () {
//            @Override
//            public void onClick(View v) {
//
//            }
//        });

        WorldChat(); // 월드 채팅 클릭 이벤트

        mainItem_FriendList_btn = (ImageView)findViewById (R.id.mainItem_FriendList_btn);
        mainItem_FriendList_btn.setOnClickListener (new View.OnClickListener () {
            @Override
            public void onClick(View v) {
                startActivity (new Intent (Main_Activity.this,FriendListActivity.class));
            }
        });




    }


    protected void redirectMainActivity() {
        startActivity(new Intent (this, Main_Activity.class));
        finish();
    }

    protected void redirectLoginActivity() {
        final Intent intent = new Intent(this, Login_Activity.class);
        intent.setFlags(Intent.FLAG_ACTIVITY_NO_ANIMATION);
        startActivity(intent);
        finish();
    }
    protected void redirectSignupActivity() {       //세션 연결 성공 시 SignupActivity로 넘김
        final Intent intent = new Intent(this, KakaoSignupActivity.class);
        intent.setFlags(Intent.FLAG_ACTIVITY_NO_ANIMATION);
        startActivity(intent);
        finish();
    }

    private void requestMe() {
        UserManagement.getInstance ().requestMe (new MeResponseCallback () {
            @Override
            public void onFailure(ErrorResult errorResult) {
                String message = "failed to get user info. msg=" + errorResult;
                Logger.d (message);

                redirectLoginActivity ();
            }

            @Override
            public void onSessionClosed(ErrorResult errorResult) {
                redirectLoginActivity ();
            }

            @Override
            public void onSuccess(UserProfile userProfile) {


                String nickname = userProfile.getNickname ();

                String email = userProfile.getEmail ();

                String profileImagePath = userProfile.getProfileImagePath ();

                String thumnailPath =  userProfile.getThumbnailImagePath ();

                String UUID = userProfile.getUUID ();

                long id = userProfile.getId ();

                SharedPreferences spf = getSharedPreferences ("LoginId",Activity.MODE_PRIVATE);
                SharedPreferences.Editor editor = spf.edit ();
                editor.putString ("LoginId", nickname);
                editor.commit ();

               Log.d ("profile","LoginId" +nickname);

                Glide.with(getApplicationContext ()).load (thumnailPath).into (main_profile_img);

            }

            @Override
            public void onNotSignedUp() {

            }
        });

    }

    // 월드 채팅 클릭 이벤트
    public void WorldChat(){

        MainWorldChat.setOnClickListener (new View.OnClickListener () {
            @SuppressLint("HandlerLeak")
            @Override
            public void onClick(View v) {
                AlertDialog.Builder builder = new AlertDialog.Builder (Main_Activity.this);
                final View view = getLayoutInflater ().inflate (R.layout.activity_chat,null);

                builder.setView(view);

                final AlertDialog dialog1 = builder.create ();

                final WindowManager.LayoutParams params = new WindowManager.LayoutParams ();
                params.copyFrom (dialog1.getWindow ().getAttributes ());
                params.width = 1850;
                params.height = 1050;
                dialog1.getWindow ().setAttributes ((android.view.WindowManager.LayoutParams) params);
                dialog1.show ();
                final Window window = dialog1.getWindow ();
                window.setAttributes (params);

                editMessage = (EditText) view.findViewById (R.id.editMessage);
                btnSend = (Button) view.findViewById (R.id.btnSend);

                final RecyclerView worldChatRecyclerView = (RecyclerView) view.findViewById (R.id.worldChat_recyclerview);
                RecyclerView.LayoutManager layoutManager = new LinearLayoutManager (Main_Activity.this);


                final MsgAdapter msgAdapter = new MsgAdapter (getApplicationContext (),msgDTOs);
                worldChatRecyclerView.setLayoutManager (layoutManager);
                // 채팅서버 접속
                client = new SocketClient ("13.124.87.189","7777");
                client.start();

                //메시지 전송 버튼
                btnSend.setOnClickListener (new View.OnClickListener () {
                    @Override
                    public void onClick(View v) {
                        String message = editMessage.getText ().toString ();
                        if (message != null || !message.equals ("")){
                            worldchat = "true";
                            send = new SendThread(socket,worldchat);
                            send.start();
                            editMessage.setText ("");

                        }
                    }
                });


                worldChatRecyclerView.setAdapter (msgAdapter);


                handler = new Handler(){
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
                            Log.d ("msg","username :" + msgtest[0] +"msg"+msgtest[1]);

                            MsgDTO msgDTO = new MsgDTO (image,msgtest[0],msgtest[1]);

                            msgDTOs.add (msgDTO);
                            msgAdapter.notifyDataSetChanged ();

                            msgAdapter.onDetachedFromRecyclerView (worldChatRecyclerView);

                            MainWorldChat.setText (msg.obj.toString ());


                        }
                    }
                };



            }
        });
    }



    class SocketClient extends Thread {
        boolean threadAlive;
        String ip;
        String port;

        OutputStream outputStream = null;
        //   BufferedReader br = null;
        DataOutputStream output = null;
        public  SocketClient(String ip, String port){
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
                receive = new Main_Activity.ReceiveThread (socket);
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
     class ReceiveThread extends Thread {
        Socket socket = null;
        DataInputStream input = null;
        public ReceiveThread(Socket socket){
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
                        Message hdmsg = handler.obtainMessage ();
                        hdmsg.what=1111; // 메시지 식별자
                        hdmsg.obj = msg; // 메시지의 본문
                        // 핸들러에게 메시지 전달 (화면 변경 요청)
                        handler.sendMessage (hdmsg);

                    }
                }
            }catch (Exception e){
                e.printStackTrace ();
            }
        }
    }// end of ReceiveThread
    // 내부 클래스
     class SendThread extends Thread {
        Socket socket;
        String key;
//        String sendmsg = editMessage.getText ().toString (); // 사용자가 작성한 메시지
        DataOutputStream output;
        public SendThread(Socket socket,String msg){
            this.socket = socket;
            this.key = msg;
            SharedPreferences spf = getSharedPreferences ("LoginId",Activity.MODE_PRIVATE);
            SharedPreferences.Editor editor = spf.edit();
//            editor.putString("key",key);
            try{
                // 채팅서버로 메시지를 보내기 위한 스트림 생성
                output = new DataOutputStream (socket.getOutputStream ());

            }catch (Exception e){
                e.printStackTrace ();
            }
        }
        public void run() {
            try {
                if (output != null) {
//                    if (sendmsg != null){

                        //채팅서버에 메시지 전달


                        SharedPreferences spf = getSharedPreferences ("LoginId",Activity.MODE_PRIVATE);
                        String sendId = spf.getString ("LoginId","");
//                        String key = spf.getString("key","");
// 월드 채팅 이라면
if (worldchat.equals("true")) {
    String sendmsg = editMessage.getText ().toString (); // 사용자가 작성한 메시지
    output.writeUTF (sendId+": "+sendmsg);
} else {

    Log.d("Gamekey",key);

    //   output.writeUTF (sendmsg);
    output.writeUTF (sendId+": "+key);
}


//                    }
                }

            }catch (Exception e){
                e.printStackTrace ();
            }
        }

    }
}


