//package com.example.xnslq.novaworld.Chat;
//
//import android.app.Activity;
//import android.content.Context;
//import android.content.SharedPreferences;
//import android.net.wifi.WifiInfo;
//import android.net.wifi.WifiManager;
//import android.os.Handler;
//import android.os.Message;
//import android.support.v7.app.AppCompatActivity;
//import android.os.Bundle;
//import android.view.View;
//import android.widget.Button;
//import android.widget.EditText;
//import android.widget.TextView;
//
//import com.example.xnslq.novaworld.R;
//
//import java.io.BufferedReader;
//import java.io.DataInputStream;
//import java.io.DataOutputStream;
//import java.io.OutputStream;
//import java.net.Socket;
//
//public class ChatActivity extends AppCompatActivity {
//
//    TextView txtMessage;
//    Button btnConnect, btnSend ;
//    EditText editIp, editPort, editMessage;
//    Handler msgHandler ;
//    SocketClient client;
//    ReceiveThread receive;
//    SendThread send;
//    Socket socket;
//    String mac;
//    Context context;
//
//
//    @Override
//    protected void onCreate(Bundle savedInstanceState) {
//        super.onCreate (savedInstanceState);
//        setContentView (R.layout.activity_chat);
//
//        context = this;
////        editIp = (EditText) findViewById (R.id.editIp);
////        editPort = (EditText) findViewById (R.id.editPort);
//        editMessage = (EditText) findViewById (R.id.editMessage);
////        btnConnect = (Button) findViewById (R.id.btnConnect);
//        btnSend = (Button) findViewById (R.id.btnSend);
//        txtMessage = (TextView) findViewById (R.id.txtMessage);
//
//
//        msgHandler = new Handler(){
//            // 백그라운드 스레드에서 받은 메시지를 처리
//            @Override
//            public void handleMessage(Message msg) {
//                super.handleMessage (msg);
//
//                if (msg.what == 1111){
//                    // 채팅서버로 부터 수신한 메시지를 텍스트뷰에 추가
//                    txtMessage.append (msg.obj.toString ()+"\n");
//                }
//            }
//        };
//
//        client = new SocketClient("13.125.250.235","8888");
//        client.start();
//
//        // 서버에 접속 버튼
////        btnConnect.setOnClickListener (new View.OnClickListener () {
////            @Override
////            public void onClick(View v) {
////
////            }
////        });
//
//        //메시지 전송 버튼
//        btnSend.setOnClickListener (new View.OnClickListener () {
//            @Override
//            public void onClick(View v) {
//                String message = editMessage.getText ().toString ();
//                if (message != null || !message.equals ("")){
//                   send = new SendThread(socket);
//                   send.start();
//                   editMessage.setText ("");
//                }
//            }
//        });
//
//    }
//
//
//    // 내부 클래스
//  public class SocketClient extends Thread {
//        boolean threadAlive;
//        String ip;
//        String port;
//
//        OutputStream outputStream = null;
//     //   BufferedReader br = null;
//        DataOutputStream output = null;
//        public  SocketClient(String ip, String port){
//            threadAlive = true;
//            this.ip = ip;
//            this.port = port;
//        }
//        public void run(){
//            try {
//                //채팅서버에 접속
//                socket = new Socket(ip, Integer.parseInt (port));
//                //서버에 메시지를 전달하기 위한 스트림 생성
//                output = new DataOutputStream (socket.getOutputStream ());
//                //메시지 수신용 스레드 생성
//                receive = new ReceiveThread(socket);
//                receive.start();
//                //와이파이 정보 관리자 객체로부터 폰의 mac address를 가져와서
//                //채팅서버에 전달
//
//                /* 와이파이 매니저 */
//
//                //  WifiManager mng = (WifiManager)context.getSystemService (
//                //        WIFI_SERVICE);
//
//             //   WifiInfo info = mng.getConnectionInfo ();
//             //   mac = info.getMacAddress ();
//
//                SharedPreferences spf = getSharedPreferences ("LoginId",Activity.MODE_PRIVATE);
//               String loginId = spf.getString ("LoginId","");
//                output.writeUTF (loginId); //한글 처리를 위해 utf
//
//            } catch (Exception e){
//                e.printStackTrace ();
//            }
//        }
//
//    } // end of SocketClient
//    //내부 클래스
//    public class ReceiveThread extends Thread {
//        Socket socket = null;
//        DataInputStream input = null;
//        public ReceiveThread(Socket socket){
//            this.socket = socket;
//            try{
//                // 채팅서버로부터 메시지를 받기 위한 스트림 생성
//                input = new DataInputStream (socket.getInputStream ());
//            } catch(Exception e){
//                e.printStackTrace ();
//            }
//        }
//      public void run(){
//            try{
//                while( input != null){
//                    String msg = input.readUTF ();
//                    if (msg != null){
//                        //핸들러에게 전달할 메시지 객체
//                        Message hdmsg = msgHandler.obtainMessage ();
//                        hdmsg.what=1111; // 메시지 식별자
//                        hdmsg.obj = msg; // 메시지의 본문
//                        // 핸들러에게 메시지 전달 (화면 변경 요청)
//                        msgHandler.sendMessage (hdmsg);
//
//                    }
//                }
//            }catch (Exception e){
//                e.printStackTrace ();
//            }
//      }
//    }// end of ReceiveThread
//     // 내부 클래스
//     public class SendThread extends Thread {
//        Socket socket;
//        String sendmsg = editMessage.getText ().toString (); // 사용자가 작성한 메시지
//        DataOutputStream output;
//        public SendThread(Socket socket){
//            this.socket = socket;
//            try{
//                // 채팅서버로 메시지를 보내기 위한 스트림 생성
//                output = new DataOutputStream (socket.getOutputStream ());
//
//            }catch (Exception e){
//                e.printStackTrace ();
//            }
//        }
//        public void run() {
//            try {
//               if (output != null){
//                   if (sendmsg != null){
//                       //채팅서버에 메시지 전달
//                       SharedPreferences spf = getSharedPreferences ("LoginId",Activity.MODE_PRIVATE);
//                       String loginId = spf.getString ("LoginId","");
//                       output.writeUTF (loginId+":"+sendmsg);
//                   }
//               }
//
//            }catch (Exception e){
//                e.printStackTrace ();
//            }
//        }
//
//     }
//}
