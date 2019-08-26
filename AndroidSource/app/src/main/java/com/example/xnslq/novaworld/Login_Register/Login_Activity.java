package com.example.xnslq.novaworld.Login_Register;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.content.pm.Signature;
import android.graphics.Color;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Base64;
import android.util.Log;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.PopupWindow;
import android.widget.TextView;
import android.widget.Toast;

import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.toolbox.Volley;
import com.example.xnslq.novaworld.Kakao_login.KakaoSignupActivity;
//import com.example.xnslq.novaworld.Kakao_login.SessionCallback;
import com.example.xnslq.novaworld.Main_Activity;
import com.example.xnslq.novaworld.R;
import com.kakao.auth.AuthType;
import com.kakao.auth.ISessionCallback;
import com.kakao.auth.OAuthErrorCode;
import com.kakao.auth.Session;
import com.kakao.network.ErrorResult;
import com.kakao.usermgmt.LoginButton;
import com.kakao.usermgmt.UserManagement;
import com.kakao.usermgmt.callback.MeResponseCallback;
import com.kakao.usermgmt.response.model.UserProfile;
import com.kakao.util.exception.KakaoException;
import com.kakao.util.helper.log.Logger;

import org.json.JSONObject;

import java.security.MessageDigest;

public class Login_Activity extends AppCompatActivity {

    private static final String TAG = "KaKao";
    private SessionCallback sessionCallback;
    private LoginButton btn_kakao_login;
    private PopupWindow mPopupWindow ;
    private AlertDialog dialog,dialog2;
    private boolean validate = false;
    private boolean validate_approved = false;

    private EditText register_useremail,register_userpassword, register_userpassword2 , loginForm_Id, loginForm_Password ;
    private ImageView btn_custom_login , register_close_btn,loginForm_closebtn;
    private Button register_btn , register_validateButton ,register_registration_btn, login_btn ,loginForm_loginBtn ;
    private TextView register_validate_Tv;



    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate (savedInstanceState);
        setContentView (R.layout.activity_login);



        btn_custom_login = (ImageView) findViewById(R.id.btn_custom_login);  // 카카오톡 커스텀  로그인 이미지 버튼



        register_btn = (Button) findViewById (R.id.login_register_btn);            // 회원가입 버튼
        login_btn = (Button) findViewById (R.id.login_btn);                        // 로그인  버튼
        /** 카카오톡 로그인 키값 받는 코드 **/
try {
    PackageInfo info = getPackageManager ().getPackageInfo (this.getPackageName (),PackageManager.GET_SIGNATURES);
    for (Signature signature : info.signatures){
        MessageDigest messageDigest = MessageDigest.getInstance ("SHA");
        messageDigest.update (signature.toByteArray ());
        Log.d ("aaaa",Base64.encodeToString(messageDigest.digest(), Base64.DEFAULT));
    }
} catch (Exception e) {
    Log.d("error","PackageInfo error is" + e.toString ());
}

        sessionCallback = new SessionCallback ();
        Session.getCurrentSession().addCallback(sessionCallback);
        Session.getCurrentSession().checkAndImplicitOpen();

       btn_custom_login.setOnClickListener (new View.OnClickListener () {
           @Override
           public void onClick(View v) {
               btn_kakao_login.performClick ();
           }
       });
   btn_kakao_login = (LoginButton) findViewById (R.id.btn_kakao_login);



        /** 카카오 로그인 끝 **/





        /** 계정 등록 이벤트 **/
        // 로그인 화면에서 회원가입 버튼 클릭시 팝업창 출력
        register_btn.setOnClickListener (new View.OnClickListener () {
            @Override
            public void onClick(View v) {



                AlertDialog.Builder builder = new AlertDialog.Builder (Login_Activity.this);
                final View view = getLayoutInflater ().inflate (R.layout.activity_register,null);

                register_close_btn = (ImageView) view.findViewById (R.id.register_close_btn);
                register_useremail = (EditText) view.findViewById (R.id.register_useremail);
                register_userpassword = (EditText) view.findViewById (R.id.register_userpassword);
                register_userpassword2 = (EditText) view.findViewById (R.id.register_userpassword2);
                register_validateButton = (Button) view.findViewById (R.id.register_validateButton);
                register_registration_btn = (Button) view.findViewById (R.id.register_registration_btn);
                register_validate_Tv = (TextView) view.findViewById (R.id.register_validate_Tv);



                builder.setView(view);

                final AlertDialog dialog1 = builder.create ();

                final WindowManager.LayoutParams params = new WindowManager.LayoutParams ();
                params.copyFrom (dialog1.getWindow ().getAttributes ());
                params.width = 1850;
                params.height = 1000;
                dialog1.getWindow ().setAttributes ((android.view.WindowManager.LayoutParams) params);
                dialog1.show ();
                final Window window = dialog1.getWindow ();
                window.setAttributes (params);


               // 이메일 중복체크

                register_validateButton.setOnClickListener (new View.OnClickListener () {
                   @Override
                   public void onClick(View v) {

                       final String user_Email = register_useremail.getText ().toString ();

                       // 중복체크가 트루면 버튼 비 활성화
//                       if (validate)
//                       {
//                           return;
//                       }

                       // 아이디 중복체크시 공백 일시 확인 메시지
                       if (user_Email.equals (""))
                       {
                           final AlertDialog.Builder builder = new AlertDialog.Builder(Login_Activity.this);
                           dialog = builder.setMessage ("아이디를 입력 해주세요.")
                                   .setPositiveButton ("확인",null)
                                   .create ();

                           // 다이얼로그 확인 버튼 컬러 바꾸기 이벤트
                           dialog.setOnShowListener (new DialogInterface.OnShowListener () {
                               @Override
                               public void onShow(DialogInterface arg0) {
                                   dialog.getButton (AlertDialog.BUTTON_POSITIVE).setTextColor(Color.BLACK);
                               }
                           });

                           dialog.show ();
                           return;
                       }
                       // 아이디 입력란이 공백이 아니라면 DB에 중복 되는 이메일이 있는지 체크
                       Response.Listener<String> responseListener = new Response.Listener<String> () {

                           @SuppressLint("ResourceType")
                           @Override
                           public void onResponse(String response) {
                           try {

                               JSONObject jsonResponse = new JSONObject (response);
                               boolean success = jsonResponse.getBoolean("success");

                               // 중복되는 이메일이 없다면 다이얼로그로 메시지 출력
                               if (success) {
                                   AlertDialog.Builder builder = new AlertDialog.Builder(Login_Activity.this);
                                   dialog = builder.setMessage ("사용할 수 있는 아이디 입니다.")
                                           .setPositiveButton ("확인",null)
                                           .create ();
                                   // 다이얼로그 확인 버튼 컬러 바꾸기 이벤트
                                   dialog.setOnShowListener (new DialogInterface.OnShowListener () {
                                       @Override
                                       public void onShow(DialogInterface arg0) {
                                           dialog.getButton (AlertDialog.BUTTON_POSITIVE).setTextColor(Color.BLACK);
                                       }
                                   });
                                   dialog.show ();

//                                   register_useremail.setEnabled (false);
                                   validate = true ;
                                   register_useremail.setBackgroundColor (getResources ().getColor (R.color.colorEmail));
                                   register_validateButton.setBackgroundColor (getResources ().getColor (R.color.colorEmail));
                                   register_validate_Tv.setVisibility (View.VISIBLE);
                               }
                               // 이미 가입한 이메일 이라면 다이얼로그로 메시지 출력
                               else {
                                   AlertDialog.Builder builder = new AlertDialog.Builder(Login_Activity.this);
                                   dialog = builder.setMessage ("이미 가입된 아이디 입니다.")
                                           .setPositiveButton ("확인",null)
                                           .create ();
                                   // 다이얼로그 확인 버튼 컬러 바꾸기 이벤트
                                   dialog.setOnShowListener (new DialogInterface.OnShowListener () {
                                       @Override
                                       public void onShow(DialogInterface arg0) {
                                           dialog.getButton (AlertDialog.BUTTON_POSITIVE).setTextColor(Color.BLACK);
                                       }
                                   });
                                   dialog.show ();
                                   register_useremail.setBackground (getResources ().getDrawable (R.drawable.back));
                                   register_validateButton.setBackground (getResources ().getDrawable (R.drawable.back));
                                   validate_approved = true;

                               }

                           } catch (Exception e)  {

                                e.printStackTrace ();

                           }

                           }
                       };
                           ValidateRequest validateRequest = new ValidateRequest (user_Email, responseListener);
                       RequestQueue queue = Volley.newRequestQueue (Login_Activity.this);
                       queue.add (validateRequest);
                   }
               });

                /** 계정 등록 버튼 클릭 이벤트 **/

                register_registration_btn.setOnClickListener (new View.OnClickListener () {
                    @Override
                    public void onClick(View v) {
                        String user_Email = register_useremail.getText ().toString ();
                        String user_Password = register_userpassword.getText ().toString ();
                        String user_Password2 = register_userpassword2.getText ().toString ();




//                         아이디 중복 체크를 하지 않았다면 확인 메시지
                        if (!validate)
                        {
                            AlertDialog.Builder builder = new AlertDialog.Builder (Login_Activity.this);
                            dialog = builder.setMessage("아이디 중복 체크를 해주세요")
                                    .setNegativeButton ("확인",null)
                                    .create ();
                            // 다이얼로그 확인 버튼 컬러 바꾸기 이벤트
                            dialog.setOnShowListener (new DialogInterface.OnShowListener () {
                                @Override
                                public void onShow(DialogInterface arg0) {
                                    dialog.getButton (AlertDialog.BUTTON_NEGATIVE).setTextColor(Color.BLACK);
                                }
                            });
                            dialog.show ();
                            return;

                        }

                        // 공백이 있다면 확인 메시지
                        if (user_Email.equals ("") || user_Password.equals ("") || user_Password2.equals (""))
                        {
                            AlertDialog.Builder builder = new AlertDialog.Builder (Login_Activity.this);
                            dialog = builder.setMessage("빈 칸 없이 입력 해주세요")
                                    .setNegativeButton ("확인",null)
                                    .create ();
                            // 다이얼로그 확인 버튼 컬러 바꾸기 이벤트
                            dialog.setOnShowListener (new DialogInterface.OnShowListener () {
                                @Override
                                public void onShow(DialogInterface arg0) {
                                    dialog.getButton (AlertDialog.BUTTON_NEGATIVE).setTextColor(Color.BLACK);
                                }
                            });
                            dialog.show ();

                            return;
                        }

                        // 입력한 패스워드가 패스워드 확인과 일치 하지 않는다면 확인 메시지
                        if (user_Password.equals(user_Password2)) {

                        } else {
                            AlertDialog.Builder builder = new AlertDialog.Builder (Login_Activity.this);
                            dialog = builder.setMessage("패스워드를 확인해 주세요")
                                    .setNegativeButton ("확인",null)
                                    .create ();
                            // 다이얼로그 확인 버튼 컬러 바꾸기 이벤트
                            dialog.setOnShowListener (new DialogInterface.OnShowListener () {
                                @Override
                                public void onShow(DialogInterface arg0) {
                                    dialog.getButton (AlertDialog.BUTTON_NEGATIVE).setTextColor(Color.BLACK);
                                }
                            });
                            dialog.show ();
                            return;
                        }

                        // 계정 등록 버튼 이벤트
                        // 회원가입 성공 실패 여부 다이얼로그로 출력

                        Response.Listener<String> responseListener = new Response.Listener<String> () {

                            @Override
                            public void onResponse(String response) {



                                try {


                                    JSONObject jsonResponse = new JSONObject (response);
                                    boolean success = jsonResponse.getBoolean("success");
                                    if (success)
                                    {
                                        AlertDialog.Builder builder = new AlertDialog.Builder(Login_Activity.this);
                                        dialog = builder.setMessage ("계정 등록에 성공했습니다.")
                                                .setPositiveButton ("확인",null)
                                                .create ();
                                        // 다이얼로그 확인 버튼 컬러 바꾸기 이벤트
                                        dialog.setOnShowListener (new DialogInterface.OnShowListener () {
                                            @Override
                                            public void onShow(DialogInterface arg0) {
                                                dialog.getButton (AlertDialog.BUTTON_POSITIVE).setTextColor(Color.BLACK);
                                            }
                                        });
                                        dialog.show ();
                                        dialog1.dismiss ();


                                    } else

                                    {
                                        AlertDialog.Builder builder = new AlertDialog.Builder(Login_Activity.this);
                                        dialog = builder.setMessage ("계정 등록에 실패했습니다.")
                                                .setPositiveButton ("확인",null)
                                                .create ();
                                        // 다이얼로그 확인 버튼 컬러 바꾸기 이벤트
                                        dialog.setOnShowListener (new DialogInterface.OnShowListener () {
                                            @Override
                                            public void onShow(DialogInterface arg0) {
                                                dialog.getButton (AlertDialog.BUTTON_POSITIVE).setTextColor(Color.BLACK);
                                            }
                                        });
                                        dialog.show ();

                                    }

                                } catch (Exception e)
                                {

                                    e.printStackTrace ();

                                }

                            }
                        };
                        RegisterRequest registerRequest = new RegisterRequest (user_Email,user_Password, responseListener);
                        RequestQueue queue = Volley.newRequestQueue (Login_Activity.this);
                        queue.add (registerRequest);

                    }
                });


                    // 팝업창의 X 버튼을 클릭했을때 창 닫기
                        register_close_btn.setOnClickListener (new View.OnClickListener () {
                    @Override
                    public void onClick(View v) {

                        dialog1.dismiss ();


                    }
                });

            }
        });
        /** 계정 등록 이벤트 끝 **/





        /** 로그인 이벤트 시작 **/

        login_btn.setOnClickListener (new View.OnClickListener () {
            @Override
            public void onClick(View v) {

                AlertDialog.Builder builder = new AlertDialog.Builder (Login_Activity.this);
                View view = getLayoutInflater ().inflate (R.layout.activity_login_form,null);

                loginForm_Id = (EditText) view.findViewById (R.id.loginForm_userID);             // 로그인 폼 아이디 에디트
                loginForm_Password = (EditText) view.findViewById (R.id.loginForm_userPassword); // 로그인 폼 패스워드 에디트
                loginForm_loginBtn = (Button) view.findViewById (R.id.loginForm_loginBtn);      // 로그인폼 로그인 버튼
                loginForm_closebtn = (ImageView) view.findViewById (R.id.loginForm_close_btn);     // 로그인 닫기  x 버튼


                builder.setView(view);

                final AlertDialog dialog2 = builder.create ();

                final WindowManager.LayoutParams params = new WindowManager.LayoutParams ();
                params.copyFrom (dialog2.getWindow ().getAttributes ());
                params.width = 1850;
                params.height = 1000;
                dialog2.getWindow ().setAttributes ((android.view.WindowManager.LayoutParams) params);
                dialog2.show ();
                final Window window = dialog2.getWindow ();
                window.setAttributes (params);


                loginForm_loginBtn.setOnClickListener (new View.OnClickListener () {
                    @Override
                    public void onClick(View v) {

                        final String userID = loginForm_Id.getText ().toString ();
                        String userPassword = loginForm_Password.getText ().toString ();


                        if (userID.equals ("") || userPassword.equals (""))
                        {
                            AlertDialog.Builder builder = new AlertDialog.Builder (Login_Activity.this);
                            dialog = builder.setMessage ("아이디와 패스워드를 입력해주세요.")
                                    .setNegativeButton ("확인",null)
                                    .create ();

                            // 다이얼로그 확인 버튼 컬러 바꾸기 이벤트
                            dialog.setOnShowListener (new DialogInterface.OnShowListener () {
                                @Override
                                public void onShow(DialogInterface arg0) {
                                    dialog.getButton (AlertDialog.BUTTON_NEGATIVE).setTextColor(Color.BLACK);
                                }
                            });
                            dialog.show ();
                            return;
                        }


                        Response.Listener<String> responseLister = new Response.Listener<String> (){


                            @Override
                            public void onResponse(String response) {
                                try
                                {

                                     JSONObject jsonObject = new JSONObject (response);
                                     boolean success = jsonObject.getBoolean ("success");

                                     // 로그인 성공시 메인 화면으로 이동

                                     if (success)
                                     {
                                         String loginId = userID;
                                         SharedPreferences spf = getSharedPreferences ("LoginId",Activity.MODE_PRIVATE);
                                         SharedPreferences.Editor editor = spf.edit ();
                                         editor.putString ("LoginId",loginId);
                                         Log.d ("LoginId",loginId);
                                         editor.commit();

                                         Intent intent = new Intent (getApplicationContext (), Main_Activity.class);
                                         intent.putExtra ("NovaWorld_login","NovaWorld_login");
                                         startActivity (intent);
                                         finish ();
                                     } else

                                         // 로그인에 실패 했다면

                                     {
                                         AlertDialog.Builder builder = new AlertDialog.Builder (Login_Activity.this);
                                         dialog = builder.setMessage ("등록되지 않은 아이디 거나 비밀번호가 일치하지 않습니다..")
                                                 .setNegativeButton ("확인",null)
                                                 .create ();

                                         // 다이얼로그 확인 버튼 컬러 바꾸기 이벤트
                                         dialog.setOnShowListener (new DialogInterface.OnShowListener () {
                                             @Override
                                             public void onShow(DialogInterface arg0) {
                                                 dialog.getButton (AlertDialog.BUTTON_NEGATIVE).setTextColor(Color.BLACK);
                                             }
                                         });
                                         dialog.show ();
                                     }

                                } catch (Exception e)
                                {
                                    e.printStackTrace ();
                                }

                            }
                        };
                            LoginRequest loginRequest = new LoginRequest (userID , userPassword, responseLister);
                            RequestQueue queue = Volley.newRequestQueue (Login_Activity.this);
                            queue.add (loginRequest);
                    }



                });

                loginForm_closebtn.setOnClickListener (new View.OnClickListener () {
                    @Override
                    public void onClick(View v) {
                        dialog2.dismiss ();
                    }
                });
            }
        });

    }

   @Override
    protected void onStop() {

       super.onStop ();
       if (dialog != null) {
           dialog.dismiss ();
           dialog = null;
       }


   }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (Session.getCurrentSession().handleActivityResult(requestCode, resultCode, data)) {
            return;
        }
        super.onActivityResult(requestCode, resultCode, data);
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        Session.getCurrentSession().removeCallback(sessionCallback);
    }

    private class SessionCallback implements ISessionCallback {

        @Override
        public void onSessionOpened() {
            redirectSignupActivity();  // 세션 연결성공 시 redirectSignupActivity() 호출
        }

        @Override
        public void onSessionOpenFailed(KakaoException exception) {
            if(exception != null) {
                Logger.e(exception);
            }
            setContentView(R.layout.activity_login); // 세션 연결이 실패했을때
        }                                            // 로그인화면을 다시 불러옴
    }

    public void redirectSignupActivity() {       //세션 연결 성공 시 SignupActivity로 넘김
        final Intent intent = new Intent(this, KakaoSignupActivity.class);
        intent.setFlags(Intent.FLAG_ACTIVITY_NO_ANIMATION);
        startActivity(intent);
        finish();
    }














}
