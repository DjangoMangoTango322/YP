package com.example.a123;

import android.os.Bundle;
import android.view.View;
import android.widget.*;
import androidx.appcompat.app.AppCompatActivity;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class EditProfileActivity extends AppCompatActivity {

    private EditText editFirstName, editLastName, editLogin, editPhone, editPassword;
    private ProgressBar editProgressBar;
    private Button saveEditButton;
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
        editProgressBar = findViewById(R.id.progressBar);

        saveEditButton = findViewById(R.id.editProfileButton);


        userId = getIntent().getIntExtra("userId", -1);
        if (userId == -1) userId = new Utils.Prefs(this).getUserId();

        loadUserData();
        saveEditButton.setOnClickListener(v -> saveChanges());
    }

    private void loadUserData() {
        editProgressBar.setVisibility(View.VISIBLE);
        ApiClient.getApiService().getUserById(userId).enqueue(new Callback<DataModels.User>() {
            @Override
            public void onResponse(Call<DataModels.User> call, Response<DataModels.User> response) {
                editProgressBar.setVisibility(View.GONE);
                if (response.isSuccessful() && response.body() != null) {
                    DataModels.User user = response.body();
                    editFirstName.setText(user.firstName != null ? user.firstName : "");
                    editLastName.setText(user.lastName != null ? user.lastName : "");
                    editLogin.setText(user.login != null ? user.login : "");
                    editPhone.setText(user.phone != null ? user.phone : "");
                    editPassword.setText(user.password != null ? user.password : "");
                } else {
                    Utils.UI.showToast(EditProfileActivity.this, "Ошибка загрузки данных");
                }
            }
            @Override public void onFailure(Call<DataModels.User> call, Throwable t) {
                editProgressBar.setVisibility(View.GONE);
                Utils.UI.showToast(EditProfileActivity.this, "Ошибка соединения");
            }
        });
    }

    private void saveChanges() {
        editProgressBar.setVisibility(View.VISIBLE);
        String firstName = editFirstName.getText().toString().trim();
        String lastName = editLastName.getText().toString().trim();
        String login = editLogin.getText().toString().trim();
        String phone = editPhone.getText().toString().trim();
        String password = editPassword.getText().toString().trim();

        ApiClient.getApiService().updateUser(userId, firstName, lastName, login, phone, password)
                .enqueue(new Callback<Void>() {
                    @Override
                    public void onResponse(Call<Void> call, Response<Void> response) {
                        editProgressBar.setVisibility(View.GONE);
                        if (response.isSuccessful()) {
                            Utils.UI.showToast(EditProfileActivity.this, "Изменения сохранены");
                            finish();
                        } else {
                            Utils.UI.showToast(EditProfileActivity.this, "Ошибка при сохранении");
                        }
                    }
                    @Override public void onFailure(Call<Void> call, Throwable t) {
                        editProgressBar.setVisibility(View.GONE);
                        Utils.UI.showToast(EditProfileActivity.this, "Ошибка соединения");
                    }
                });
    }
}
