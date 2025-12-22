package com.example.a123;

import android.os.Bundle;
import android.view.View;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.Toast;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.AppCompatButton;

import com.example.a123.DataModels.User;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class ProfileActivity extends AppCompatActivity {

    private EditText editFirstName, editLastName, editLogin, editPhone, editPassword;
    private ProgressBar progressBar;
    private AppCompatButton editProfileButton;
    private int userId;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        // убедись, что имя файла XML совпадает с этим названием:
        setContentView(R.layout.activity_edit_profile);

        // Подключение элементов интерфейса
        editFirstName = findViewById(R.id.editFirstName);
        editLastName = findViewById(R.id.editLastName);
        editLogin = findViewById(R.id.editLogin);
        editPhone = findViewById(R.id.editPhone);
        editPassword = findViewById(R.id.editPassword);
        progressBar = findViewById(R.id.progressBar);
        editProfileButton = findViewById(R.id.editProfileButton);

        userId = getIntent().getIntExtra("userId", 0);

        loadUserData();

        editProfileButton.setOnClickListener(v -> saveChanges());
    }

    private void loadUserData() {
        progressBar.setVisibility(View.VISIBLE);

        ApiClient.getApiService().getUserById(userId).enqueue(new Callback<User>() {
            @Override
            public void onResponse(Call<User> call, Response<User> response) {
                progressBar.setVisibility(View.GONE);
                if (response.isSuccessful() && response.body() != null) {
                    User user = response.body();
                    editFirstName.setText(user.First_Name);
                    editLastName.setText(user.Last_Name);
                    editLogin.setText(user.Login);
                    editPhone.setText(user.Phone);
                    editPassword.setText(user.Password);
                } else {
                    Toast.makeText(ProfileActivity.this, "Ошибка загрузки данных", Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<User> call, Throwable t) {
                progressBar.setVisibility(View.GONE);
                Toast.makeText(ProfileActivity.this, "Ошибка соединения", Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void saveChanges() {
        progressBar.setVisibility(View.VISIBLE);

        String firstName = editFirstName.getText().toString().trim();
        String lastName = editLastName.getText().toString().trim();
        String login = editLogin.getText().toString().trim();
        String phone = editPhone.getText().toString().trim();
        String password = editPassword.getText().toString().trim();

        ApiClient.getApiService().updateUser(userId, firstName, lastName, login, phone, password)
                .enqueue(new Callback<Void>() {
                    @Override
                    public void onResponse(Call<Void> call, Response<Void> response) {
                        progressBar.setVisibility(View.GONE);
                        if (response.isSuccessful()) {
                            Toast.makeText(ProfileActivity.this, "Изменения сохранены", Toast.LENGTH_SHORT).show();
                            finish();
                        } else {
                            Toast.makeText(ProfileActivity.this, "Ошибка при сохранении", Toast.LENGTH_SHORT).show();
                        }
                    }

                    @Override
                    public void onFailure(Call<Void> call, Throwable t) {
                        progressBar.setVisibility(View.GONE);
                        Toast.makeText(ProfileActivity.this, "Ошибка соединения", Toast.LENGTH_SHORT).show();
                    }
                });
    }
}
