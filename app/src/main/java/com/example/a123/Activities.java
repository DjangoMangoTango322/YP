package com.example.a123;

import android.content.Intent;
import android.os.Bundle;
import android.widget.*;
import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.GridLayoutManager;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class Activities {

    // Main Activity
    public static class MainActivity extends AppCompatActivity {
        @Override
        protected void onCreate(Bundle savedInstanceState) {
            super.onCreate(savedInstanceState);
            setContentView(R.layout.activity_main);

            findViewById(R.id.loginButton).setOnClickListener(v ->
                    startActivity(new Intent(this, Activities.LoginActivity.class)));
            findViewById(R.id.registerButton).setOnClickListener(v ->
                    startActivity(new Intent(this, Activities.RegisterActivity.class)));
            findViewById(R.id.exploreButton).setOnClickListener(v ->
                    startActivity(new Intent(this, Activities.RestaurantListActivity.class)));
        }
    }

    // Login Activity
    public static class LoginActivity extends AppCompatActivity {
        private EditText emailEditText, passwordEditText;
        private ProgressBar progressBar;

        @Override
        protected void onCreate(Bundle savedInstanceState) {
            super.onCreate(savedInstanceState);
            setContentView(R.layout.activity_login);

            emailEditText = findViewById(R.id.emailEditText);
            passwordEditText = findViewById(R.id.passwordEditText);
            progressBar = findViewById(R.id.progressBar);
            Button loginButton = findViewById(R.id.loginButton);
            TextView registerLink = findViewById(R.id.registerLink);
            ImageButton backButton = findViewById(R.id.backButton);

            loginButton.setOnClickListener(v -> performLogin());
            registerLink.setOnClickListener(v ->
                    startActivity(new Intent(this, Activities.RegisterActivity.class)));
            backButton.setOnClickListener(v -> onBackPressed());
        }

        private void performLogin() {
            String email = emailEditText.getText().toString();
            String password = passwordEditText.getText().toString();

            if (email.isEmpty() || password.isEmpty()) {
                Utils.UI.showToast(this, "Заполните все поля");
                return;
            }

            progressBar.setVisibility(ProgressBar.VISIBLE);

            ApiClient.getApiService().login(new ApiClient.LoginRequest(email, password))
                    .enqueue(new Callback<ApiClient.ApiResponse<DataModels.User>>() {
                        @Override
                        public void onResponse(Call<ApiClient.ApiResponse<DataModels.User>> call, Response<ApiClient.ApiResponse<DataModels.User>> response) {
                            progressBar.setVisibility(ProgressBar.GONE);
                            if (response.isSuccessful() && response.body() != null && response.body().isSuccess()) {
                                DataModels.User user = response.body().getData();
                                new Utils.Prefs(LoginActivity.this).saveUserData(user.id, user.name, user.email, "token");
                                startActivity(new Intent(LoginActivity.this, Activities.RestaurantListActivity.class));
                                finish();
                            } else {
                                Utils.UI.showToast(LoginActivity.this, "Ошибка входа");
                            }
                        }

                        @Override
                        public void onFailure(Call<ApiClient.ApiResponse<DataModels.User>> call, Throwable t) {
                            progressBar.setVisibility(ProgressBar.GONE);
                            Utils.UI.showToast(LoginActivity.this, "Ошибка сети: " + t.getMessage());
                        }
                    });
        }
    }

    // Register Activity
    public static class RegisterActivity extends AppCompatActivity {
        private EditText nameEditText, emailEditText, passwordEditText, confirmPasswordEditText;
        private ProgressBar progressBar;

        @Override
        protected void onCreate(Bundle savedInstanceState) {
            super.onCreate(savedInstanceState);
            setContentView(R.layout.activity_register);

            nameEditText = findViewById(R.id.nameEditText);
            emailEditText = findViewById(R.id.emailEditText);
            passwordEditText = findViewById(R.id.passwordEditText);
            confirmPasswordEditText = findViewById(R.id.confirmPasswordEditText);
            progressBar = findViewById(R.id.progressBar);
            Button registerButton = findViewById(R.id.registerButton);
            TextView loginLink = findViewById(R.id.loginLink);
            ImageButton backButton = findViewById(R.id.backButton);

            registerButton.setOnClickListener(v -> performRegistration());
            loginLink.setOnClickListener(v -> {
                startActivity(new Intent(RegisterActivity.this, Activities.LoginActivity.class));
            });
            backButton.setOnClickListener(v -> onBackPressed());
        }

        private void performRegistration() {
            String name = nameEditText.getText().toString().trim();
            String email = emailEditText.getText().toString().trim();
            String password = passwordEditText.getText().toString();
            String confirmPassword = confirmPasswordEditText.getText().toString();

            // Валидация
            if (name.isEmpty() || email.isEmpty() || password.isEmpty() || confirmPassword.isEmpty()) {
                Utils.UI.showToast(this, "Заполните все поля");
                return;
            }

            if (!android.util.Patterns.EMAIL_ADDRESS.matcher(email).matches()) {
                Utils.UI.showToast(this, "Введите корректный email");
                return;
            }

            if (password.length() < 6) {
                Utils.UI.showToast(this, "Пароль должен содержать минимум 6 символов");
                return;
            }

            if (!password.equals(confirmPassword)) {
                Utils.UI.showToast(this, "Пароли не совпадают");
                return;
            }

            progressBar.setVisibility(ProgressBar.VISIBLE);
            Button registerButton = findViewById(R.id.registerButton);
            registerButton.setEnabled(false);

            ApiClient.RegisterRequest registerRequest = new ApiClient.RegisterRequest(name, email, password, "");

            ApiClient.getApiService().register(registerRequest).enqueue(new Callback<ApiClient.ApiResponse<DataModels.User>>() {
                @Override
                public void onResponse(Call<ApiClient.ApiResponse<DataModels.User>> call, Response<ApiClient.ApiResponse<DataModels.User>> response) {
                    progressBar.setVisibility(ProgressBar.GONE);
                    registerButton.setEnabled(true);

                    if (response.isSuccessful() && response.body() != null) {
                        ApiClient.ApiResponse<DataModels.User> apiResponse = response.body();

                        if (apiResponse.isSuccess()) {
                            DataModels.User user = apiResponse.getData();
                            new Utils.Prefs(RegisterActivity.this).saveUserData(user.id, user.name, user.email, "auth_token");
                            Utils.UI.showToast(RegisterActivity.this, "Регистрация успешна!");
                            startActivity(new Intent(RegisterActivity.this, Activities.RestaurantListActivity.class));
                            finish();
                        } else {
                            String errorMessage = apiResponse.getMessage() != null ?
                                    apiResponse.getMessage() : "Ошибка регистрации";
                            Utils.UI.showToast(RegisterActivity.this, errorMessage);
                        }
                    } else {
                        Utils.UI.showToast(RegisterActivity.this, "Ошибка сервера: " + response.code());
                    }
                }

                @Override
                public void onFailure(Call<ApiClient.ApiResponse<DataModels.User>> call, Throwable t) {
                    progressBar.setVisibility(ProgressBar.GONE);
                    registerButton.setEnabled(true);

                    if (!Utils.Network.isNetworkAvailable(RegisterActivity.this)) {
                        Utils.UI.showToast(RegisterActivity.this, "Отсутствует подключение к интернету");
                    } else {
                        Utils.UI.showToast(RegisterActivity.this, "Ошибка сети: " + t.getMessage());
                    }
                }
            });
        }
    }

    // Restaurant List Activity
    public static class RestaurantListActivity extends AppCompatActivity
            implements Adapters.RestaurantAdapter.OnRestaurantClickListener {

        private RecyclerView restaurantRecyclerView;
        private ProgressBar progressBar;
        private List<DataModels.Restaurant> restaurantList;
        private Adapters.RestaurantAdapter adapter;

        @Override
        protected void onCreate(Bundle savedInstanceState) {
            super.onCreate(savedInstanceState);
            setContentView(R.layout.activity_restaurant_list);

            restaurantRecyclerView = findViewById(R.id.restaurantRecyclerView);
            progressBar = findViewById(R.id.progressBar);
            ImageButton profileButton = findViewById(R.id.profileButton);
            ImageButton backButton = findViewById(R.id.backButton);

            restaurantList = new ArrayList<>();
            adapter = new Adapters.RestaurantAdapter(restaurantList, this);
            restaurantRecyclerView.setLayoutManager(new GridLayoutManager(this, 2));
            restaurantRecyclerView.setAdapter(adapter);

            loadRestaurantsFromApi();
            profileButton.setOnClickListener(v ->
                    startActivity(new Intent(this, Activities.ProfileActivity.class)));
            backButton.setOnClickListener(v -> onBackPressed());
        }

        @Override
        public void onRestaurantClick(DataModels.Restaurant restaurant) {
            Intent intent = new Intent(this, Activities.RestaurantDetailActivity.class);
            intent.putExtra("restaurant", restaurant);
            startActivity(intent);
        }

        private void loadRestaurantsFromApi() {
            progressBar.setVisibility(ProgressBar.VISIBLE);
            ApiClient.getApiService().getRestaurants().enqueue(new Callback<ApiClient.ApiResponse<List<DataModels.Restaurant>>>() {
                @Override
                public void onResponse(Call<ApiClient.ApiResponse<List<DataModels.Restaurant>>> call,
                                       Response<ApiClient.ApiResponse<List<DataModels.Restaurant>>> response) {
                    progressBar.setVisibility(ProgressBar.GONE);
                    if (response.isSuccessful() && response.body() != null && response.body().isSuccess()) {
                        restaurantList.clear();
                        restaurantList.addAll(response.body().getData());
                        adapter.notifyDataSetChanged();
                    } else {
                        Utils.UI.showToast(RestaurantListActivity.this, "Ошибка загрузки ресторанов");
                    }
                }

                @Override
                public void onFailure(Call<ApiClient.ApiResponse<List<DataModels.Restaurant>>> call, Throwable t) {
                    progressBar.setVisibility(ProgressBar.GONE);
                    Utils.UI.showToast(RestaurantListActivity.this, "Ошибка сети: " + t.getMessage());
                }
            });
        }
    }

    // Добавьте остальные активности по аналогии
    public static class RestaurantDetailActivity extends AppCompatActivity {
        // Код для деталей ресторана
    }

    public static class BookingActivity extends AppCompatActivity {
        // Код для бронирования
    }

    public static class BookingConfirmationActivity extends AppCompatActivity {
        // Код для подтверждения бронирования
    }

    public static class ProfileActivity extends AppCompatActivity {
        // Код для профиля
    }
}