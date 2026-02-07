package com.example.a123;

import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.example.a123.ApiClient;
import com.example.a123.R;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class RegisterActivity extends AppCompatActivity {
    private EditText firstNameEdit, lastNameEdit, loginEdit, phoneEdit, passwordEdit;
    private ProgressBar progressBar;
    private Button registerButton;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_register);

        firstNameEdit = findViewById(R.id.nameEditText);
        lastNameEdit = findViewById(R.id.lastNameEditText);
        loginEdit = findViewById(R.id.emailEditText);
        phoneEdit = findViewById(R.id.phoneEditText);
        passwordEdit = findViewById(R.id.passwordEditText);
        progressBar = findViewById(R.id.progressBar);
        registerButton = findViewById(R.id.registerButton);

        registerButton.setOnClickListener(v -> registerUser());
    }

    private void registerUser() {
        String f = firstNameEdit.getText().toString().trim();
        String l = lastNameEdit.getText().toString().trim();
        String log = loginEdit.getText().toString().trim();
        String ph = phoneEdit.getText().toString().trim();
        String pw = passwordEdit.getText().toString().trim();

        if (f.isEmpty() || l.isEmpty() || log.isEmpty() || ph.isEmpty() || pw.isEmpty()) {
            Toast.makeText(this, "Заполните все поля", Toast.LENGTH_SHORT).show();
            return;
        }

        UserRequest user = new UserRequest();
        user.first_Name = f;
        user.last_Name = l;
        user.login = log;
        user.phone = ph;
        user.password = pw;

        progressBar.setVisibility(View.VISIBLE);
        ApiClient.getApiService().register(user).enqueue(new Callback<Void>() {
            @Override
            public void onResponse(Call<Void> call, Response<Void> response) {
                progressBar.setVisibility(View.GONE);
                if (response.isSuccessful()) {
                    Toast.makeText(RegisterActivity.this, "Регистрация успешна", Toast.LENGTH_SHORT).show();
                    finish();
                } else {
                    Toast.makeText(RegisterActivity.this, "Ошибка регистрации", Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<Void> call, Throwable t) {
                progressBar.setVisibility(View.GONE);
                Toast.makeText(RegisterActivity.this, "Ошибка сети", Toast.LENGTH_SHORT).show();
            }
        });
    }
}