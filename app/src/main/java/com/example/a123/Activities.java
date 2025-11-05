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
                    startActivity(new Intent(this, LoginActivity.class)));
            findViewById(R.id.registerButton).setOnClickListener(v ->
                    startActivity(new Intent(this, RegisterActivity.class)));
            findViewById(R.id.exploreButton).setOnClickListener(v ->
                    startActivity(new Intent(this, RestaurantListActivity.class)));
        }
    }

    // Login Activity
    public static class LoginActivity extends AppCompatActivity {
        private EditText emailEditText, passwordEditText;

        @Override
        protected void onCreate(Bundle savedInstanceState) {
            super.onCreate(savedInstanceState);
            setContentView(R.layout.activity_login);

            emailEditText = findViewById(R.id.emailEditText);
            passwordEditText = findViewById(R.id.passwordEditText);
            Button loginButton = findViewById(R.id.loginButton);
            TextView registerLink = findViewById(R.id.registerLink);
            ImageButton backButton = findViewById(R.id.backButton);

            loginButton.setOnClickListener(v -> performLogin());
            registerLink.setOnClickListener(v ->
                    startActivity(new Intent(this, RegisterActivity.class)));
            backButton.setOnClickListener(v -> onBackPressed());
        }

        private void performLogin() {
            String email = emailEditText.getText().toString();
            String password = passwordEditText.getText().toString();

            if (email.isEmpty() || password.isEmpty()) {
                Utils.UI.showToast(this, "Заполните все поля");
                return;
            }

            ProgressBar progressBar = findViewById(R.id.progressBar);
            progressBar.setVisibility(ProgressBar.VISIBLE);

            ApiClient.getApiService().login(new LoginRequest(email, password))
                    .enqueue(new Callback<ApiResponse<DataModels.User>>() {
                        @Override
                        public void onResponse(Call<ApiResponse<DataModels.User>> call, Response<ApiResponse<DataModels.User>> response) {
                            progressBar.setVisibility(ProgressBar.GONE);
                            if (response.isSuccessful() && response.body() != null && response.body().isSuccess()) {
                                DataModels.User user = response.body().getData();
                                new Utils.Prefs(LoginActivity.this).saveUserData(user.id, user.name, user.email, "token");
                                startActivity(new Intent(LoginActivity.this, RestaurantListActivity.class));
                                finish();
                            } else {
                                Utils.UI.showToast(LoginActivity.this, "Ошибка входа");
                            }
                        }

                        @Override
                        public void onFailure(Call<ApiResponse<DataModels.User>> call, Throwable t) {
                            progressBar.setVisibility(ProgressBar.GONE);
                            Utils.UI.showToast(LoginActivity.this, "Ошибка сети: " + t.getMessage());
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
                    startActivity(new Intent(this, ProfileActivity.class)));
            backButton.setOnClickListener(v -> onBackPressed());
        }

        @Override
        public void onRestaurantClick(DataModels.Restaurant restaurant) {
            Intent intent = new Intent(this, RestaurantDetailActivity.class);
            intent.putExtra("restaurant", restaurant);
            startActivity(intent);
        }

        private void loadRestaurantsFromApi() {
            progressBar.setVisibility(ProgressBar.VISIBLE);
            ApiClient.getApiService().getRestaurants().enqueue(new Callback<ApiResponse<List<DataModels.Restaurant>>>() {
                @Override
                public void onResponse(Call<ApiResponse<List<DataModels.Restaurant>>> call,
                                       Response<ApiResponse<List<DataModels.Restaurant>>> response) {
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
                public void onFailure(Call<ApiResponse<List<DataModels.Restaurant>>> call, Throwable t) {
                    progressBar.setVisibility(ProgressBar.GONE);
                    Utils.UI.showToast(RestaurantListActivity.this, "Ошибка сети: " + t.getMessage());
                }
            });
        }
    }
}
