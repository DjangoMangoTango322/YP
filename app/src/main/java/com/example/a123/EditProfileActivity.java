package com.example.a123;

import android.os.Bundle;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.example.a123.DataModels.User;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class EditProfileActivity extends AppCompatActivity {

    private EditText editFirstName;
    private EditText editLastName;
    private EditText editLogin;
    private EditText editPhone;
    private EditText editPassword;

    private ProgressBar editProgressBar;
    private Button saveEditButton;

    private int userId;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_edit_profile);

        // Инициализация элементов
        editFirstName = findViewById(R.id.editFirstName);
        editLastName = findViewById(R.id.editLastName);
        editLogin = findViewById(R.id.editLogin);
        editPhone = findViewById(R.id.editPhone);
        editPassword = findViewById(R.id.editPassword);
        editProgressBar = findViewById(R.id.progressBar);
        saveEditButton = findViewById(R.id.editProfileButton);

        // Получаем userId
        userId = getIntent().getIntExtra("userId", -1);
        if (userId == -1 || userId == 0) {
            Toast.makeText(this, "Ошибка: не передан ID пользователя", Toast.LENGTH_SHORT).show();
            finish();
            return;
        }

        loadUserData();

        saveEditButton.setOnClickListener(v -> saveChanges());
    }

    private void loadUserData() {
        editProgressBar.setVisibility(ProgressBar.VISIBLE);

        ApiClient.getApiService().getUserById(userId).enqueue(new Callback<User>() {
            @Override
            public void onResponse(Call<User> call, Response<User> response) {
                editProgressBar.setVisibility(ProgressBar.GONE);

                if (response.isSuccessful() && response.body() != null) {
                    User user = response.body();

                    editFirstName.setText(user.First_Name != null ? user.First_Name : "");
                    editLastName.setText(user.Last_Name != null ? user.Last_Name : "");
                    editLogin.setText(user.Login != null ? user.Login : "");
                    editPhone.setText(user.Phone != null ? user.Phone : "");
                    editPassword.setText(user.Password != null ? user.Password : "");
                } else {
                    Toast.makeText(EditProfileActivity.this, "Не удалось загрузить данные профиля", Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<User> call, Throwable t) {
                editProgressBar.setVisibility(ProgressBar.GONE);
                Toast.makeText(EditProfileActivity.this, "Ошибка сети: " + t.getMessage(), Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void saveChanges() {
        String firstName = editFirstName.getText().toString().trim();
        String lastName = editLastName.getText().toString().trim();
        String login = editLogin.getText().toString().trim();
        String phone = editPhone.getText().toString().trim();
        String password = editPassword.getText().toString().trim();

        if (firstName.isEmpty() || lastName.isEmpty() || login.isEmpty() || phone.isEmpty()) {
            Toast.makeText(this, "Заполните все обязательные поля", Toast.LENGTH_SHORT).show();
            return;
        }

        editProgressBar.setVisibility(ProgressBar.VISIBLE);

        ApiClient.getApiService().updateUser(userId, firstName, lastName, login, phone, password)
                .enqueue(new Callback<Void>() {
                    @Override
                    public void onResponse(Call<Void> call, Response<Void> response) {
                        editProgressBar.setVisibility(ProgressBar.GONE);

                        if (response.isSuccessful()) {
                            Toast.makeText(EditProfileActivity.this, "Профиль успешно обновлён!", Toast.LENGTH_SHORT).show();
                            setResult(RESULT_OK); // Опционально — чтобы обновить профиль
                            finish();
                        } else {
                            Toast.makeText(EditProfileActivity.this, "Ошибка при сохранении изменений", Toast.LENGTH_SHORT).show();
                        }
                    }

                    @Override
                    public void onFailure(Call<Void> call, Throwable t) {
                        editProgressBar.setVisibility(ProgressBar.GONE);
                        Toast.makeText(EditProfileActivity.this, "Ошибка сети: " + t.getMessage(), Toast.LENGTH_SHORT).show();
                    }
                });
    }
}