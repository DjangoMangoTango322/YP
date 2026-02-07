package com.example.a123;

import android.os.Bundle;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class EditProfileActivity extends AppCompatActivity {

    private EditText editFirstName, editLastName, editLogin, editPhone, editPassword;
    private ProgressBar progressBar;
    private Button saveButton;
    private int userId;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_edit_profile);

        editFirstName = findViewById(R.id.editFirstName);
        editLastName = findViewById(R.id.editLastName);
        editLogin = findViewById(R.id.editLogin);
        editPhone = findViewById(R.id.editPhone);
        editPassword = findViewById(R.id.editPassword);
        progressBar = findViewById(R.id.progressBar);
        saveButton = findViewById(R.id.editProfileButton);

        userId = getIntent().getIntExtra("userId", 0);
        if (userId == 0) {
            Toast.makeText(this, "Ошибка: не найден пользователь", Toast.LENGTH_SHORT).show();
            finish();
            return;
        }

        loadUserData();

        saveButton.setOnClickListener(v -> saveChanges());
    }

    private void loadUserData() {
        progressBar.setVisibility(ProgressBar.VISIBLE);

        ApiClient.getApiService().getUserById(userId).enqueue(new Callback<UserRequest>() {
            @Override
            public void onResponse(Call<UserRequest> call, Response<UserRequest> response) {
                progressBar.setVisibility(ProgressBar.GONE);
                if (response.isSuccessful() && response.body() != null) {
                    UserRequest user = response.body();
                    editFirstName.setText(user.first_Name);
                    editLastName.setText(user.last_Name);
                    editLogin.setText(user.login);
                    editPhone.setText(user.phone);
                    editPassword.setText(user.password);
                } else {
                    Toast.makeText(EditProfileActivity.this, "Ошибка загрузки данных", Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<UserRequest> call, Throwable t) {
                progressBar.setVisibility(ProgressBar.GONE);
                Toast.makeText(EditProfileActivity.this, "Ошибка сети", Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void saveChanges() {
        UserRequest user = new UserRequest();
        user.id = userId;
        user.first_Name = editFirstName.getText().toString().trim();
        user.last_Name = editLastName.getText().toString().trim();
        user.login = editLogin.getText().toString().trim();
        user.phone = editPhone.getText().toString().trim();
        user.password = editPassword.getText().toString().trim();

        ApiClient.getApiService().updateUser(user).enqueue(new Callback<Void>() {
            @Override
            public void onResponse(Call<Void> call, Response<Void> response) {
                progressBar.setVisibility(ProgressBar.GONE);

                if (response.isSuccessful()) {
                    Toast.makeText(EditProfileActivity.this, "Профиль успешно обновлён!", Toast.LENGTH_SHORT).show();
                    setResult(RESULT_OK);
                    finish();
                } else {
                    Toast.makeText(EditProfileActivity.this, "Ошибка при сохранении изменений", Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<Void> call, Throwable t) {
                progressBar.setVisibility(ProgressBar.GONE);
                Toast.makeText(EditProfileActivity.this, "Ошибка сети: " + t.getMessage(), Toast.LENGTH_SHORT).show();
            }
        });
    }
}
