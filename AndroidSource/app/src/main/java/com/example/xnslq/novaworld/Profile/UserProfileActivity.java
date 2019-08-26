package com.example.xnslq.novaworld.Profile;

import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.graphics.Bitmap;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;

import com.bumptech.glide.Glide;
import com.example.xnslq.novaworld.Chat.ServerActivity;
import com.example.xnslq.novaworld.Kakao_login.KakaoSignupActivity;
import com.example.xnslq.novaworld.Login_Register.Login_Activity;
import com.example.xnslq.novaworld.Main_Activity;
import com.example.xnslq.novaworld.R;
import com.kakao.network.ErrorResult;
import com.kakao.usermgmt.UserManagement;
import com.kakao.usermgmt.callback.MeResponseCallback;
import com.kakao.usermgmt.callback.UnLinkResponseCallback;
import com.kakao.usermgmt.response.model.UserProfile;
import com.kakao.util.helper.log.Logger;

public class UserProfileActivity extends AppCompatActivity {

    private KakaoSignupActivity kakaoSignupActivity = new KakaoSignupActivity();
    private Login_Activity login_activity = new Login_Activity ();
    private Button kakao_logout, profile_back_btn , chatServer;
    private AlertDialog dialog;
    private ImageView profile_kakaoprofile_img;
    private Bitmap bitmap;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate (savedInstanceState);
        setContentView (R.layout.activity_user_profile);

        kakao_logout = (Button) findViewById (R.id.profile_kakaologout_btn);
        profile_back_btn = (Button) findViewById (R.id.profile_back_btn);
        profile_kakaoprofile_img = (ImageView) findViewById (R.id.profile_kakoprofile_img);
        chatServer = (Button) findViewById (R.id.chatServer);
        requestMe();

//        UserManagement.getInstance ().requestMe ();



        kakao_logout.setOnClickListener (new View.OnClickListener () {
            @Override
            public void onClick(View v) {
                onClickUnlink ();

            }
        });
        profile_back_btn.setOnClickListener (new View.OnClickListener () {
            @Override
            public void onClick(View v) {
                redirectMainActivity ();
            }
        });
        chatServer.setOnClickListener (new View.OnClickListener () {
            @Override
            public void onClick(View v) {
                startActivity (new Intent (UserProfileActivity.this,ServerActivity.class));
            }
        });
    }

    private void onClickUnlink() {
        final String appendMessage = getString(R.string.com_kakao_confirm_unlink);
        new AlertDialog.Builder(this)
                .setMessage(appendMessage)
                .setPositiveButton(getString(R.string.com_kakao_ok_button),
                        new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int which) {

                                UserManagement.getInstance().requestUnlink(new UnLinkResponseCallback () {
                                    @Override
                                    public void onFailure(ErrorResult errorResult) {
                                        Logger.e(errorResult.toString());
                                    }

                                    @Override
                                    public void onSessionClosed(ErrorResult errorResult) {
                                        redirectLoginActivity ();
                                    }

                                    @Override
                                    public void onNotSignedUp() {
                                        redirectSignupActivity();
                                    }

                                    @Override
                                    public void onSuccess(Long userId) {
                                       redirectLoginActivity();
//                                          startActivity (new Intent (UserProfileActivity.this, Login_Activity.class));
//                                          finish ();

                                    }

                                });

                                dialog.dismiss();
                            }

                })

                .setNegativeButton(getString(R.string.com_kakao_cancel_button),
                        new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int which) {

                                dialog.dismiss();
                            }

                        }).show();



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

                Log.d ("a","UserProfile : " + userProfile);
                Log.d ("b","UserProfile_img : " + id);



                Glide.with(getApplicationContext ()).load (thumnailPath).into (profile_kakaoprofile_img);

            }

            @Override
            public void onNotSignedUp() {

            }
        });

    }
}
