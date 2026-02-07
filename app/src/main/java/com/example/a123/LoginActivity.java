package com.example.a123;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;


import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class LoginActivity extends AppCompatActivity {
    private EditText loginEditText, passwordEditText;
    private ProgressBar progressBar;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        loginEditText = findViewById(R.id.loginEditText);
        passwordEditText = findViewById(R.id.passwordEditText);
        progressBar = findViewById(R.id.progressBar);
        Button loginButton = findViewById(R.id.loginButton);
        TextView registerLink = findViewById(R.id.registerLink);

        loginButton.setOnClickListener(v -> loginUser());
        registerLink.setOnClickListener(v -> startActivity(new Intent(this, RegisterActivity.class)));
    }

    private void loginUser() {
        String enteredLogin = loginEditText.getText().toString().trim();
        String enteredPassword = passwordEditText.getText().toString().trim();

        if (enteredLogin.isEmpty() || enteredPassword.isEmpty()) {
            Toast.makeText(this, "Введите логин и пароль", Toast.LENGTH_SHORT).show();
            return;
        }

        progressBar.setVisibility(View.VISIBLE);

        ApiClient.getApiService().getAllUsers()
                .enqueue(new Callback<List<UserIdResponse>>() {
                    @Override
                    public void onResponse(Call<List<UserIdResponse>> call,
                                           Response<List<UserIdResponse>> response) {
                        progressBar.setVisibility(View.GONE);

                        if (!response.isSuccessful() || response.body() == null) {
                            Toast.makeText(LoginActivity.this, "Ошибка сервера", Toast.LENGTH_SHORT).show();
                            return;
                        }

                        UserIdResponse foundUser = null;

                        for (UserIdResponse user : response.body()) {
                            if (enteredLogin.equals(user.login)
                                    && enteredPassword.equals(user.password)) {
                                foundUser = user;
                                break;
                            }
                        }

                        if (foundUser != null) {
                            saveUserId(foundUser.id);

                            Toast.makeText(
                                    LoginActivity.this,
                                    "Добро пожаловать, " + foundUser.first_Name,
                                    Toast.LENGTH_SHORT
                            ).show();

                            startActivity(
                                    new Intent(LoginActivity.this, RestaurantListActivity.class)
                            );
                            finish();

                        } else {
                            Toast.makeText(
                                    LoginActivity.this,
                                    "Неверный логин или пароль",
                                    Toast.LENGTH_SHORT
                            ).show();
                        }
                    }

                    @Override
                    public void onFailure(Call<List<UserIdResponse>> call, Throwable t) {
                        progressBar.setVisibility(View.GONE);
                        Toast.makeText(LoginActivity.this, "Ошибка сети", Toast.LENGTH_SHORT).show();
                    }
                });
    }
    private void saveUserId(int userId) {
        getSharedPreferences("app_prefs", MODE_PRIVATE)
                .edit()
                .putInt("user_id", userId)
                .apply();
    }
}