package com.example.a123;

import android.app.DatePickerDialog;
import android.app.TimePickerDialog;
import android.widget.ProgressBar;
import android.widget.Toast;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.*;
import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.RecyclerView;
import androidx.recyclerview.widget.LinearLayoutManager;
import com.example.a123.Adapters.RestaurantAdapter;


import java.text.SimpleDateFormat;
import java.util.*;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class Activities {

    // ---------- ЛОГИН ----------
    public static class LoginActivity extends AppCompatActivity {
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
            String login = loginEditText.getText().toString().trim();
            String password = passwordEditText.getText().toString().trim();
            if (login.isEmpty() || password.isEmpty()) {
                Toast.makeText(this, "Введите логин и пароль", Toast.LENGTH_SHORT).show();
                return;
            }

            progressBar.setVisibility(View.VISIBLE);
            ApiClient.getApiService().login(login, password).enqueue(new Callback<ApiClient.LoginResult>() {
                @Override
                public void onResponse(Call<ApiClient.LoginResult> call, Response<ApiClient.LoginResult> response) {
                    progressBar.setVisibility(View.GONE);
                    if (response.isSuccessful() && response.body() != null) {
                        int userId = response.body().getUserId();
                        Intent i = new Intent(LoginActivity.this, RestaurantListActivity.class);
                        i.putExtra("userId", userId);
                        startActivity(i);
                        finish();
                    } else
                        Toast.makeText(LoginActivity.this, "Неверные данные", Toast.LENGTH_SHORT).show();
                }

                @Override
                public void onFailure(Call<ApiClient.LoginResult> call, Throwable t) {
                    progressBar.setVisibility(View.GONE);
                    Toast.makeText(LoginActivity.this, "Ошибка сети", Toast.LENGTH_SHORT).show();
                }
            });
        }
    }

    // ---------- РЕГИСТРАЦИЯ ----------
    public static class RegisterActivity extends AppCompatActivity {
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

            progressBar.setVisibility(View.VISIBLE);
            ApiClient.getApiService().register(f, l, log, ph, pw).enqueue(new Callback<Void>() {
                @Override
                public void onResponse(Call<Void> call, Response<Void> response) {
                    progressBar.setVisibility(View.GONE);
                    if (response.isSuccessful()) {
                        Toast.makeText(RegisterActivity.this, "Регистрация успешна", Toast.LENGTH_SHORT).show();
                        finish();
                    } else
                        Toast.makeText(RegisterActivity.this, "Ошибка регистрации", Toast.LENGTH_SHORT).show();
                }

                @Override
                public void onFailure(Call<Void> call, Throwable t) {
                    progressBar.setVisibility(View.GONE);
                    Toast.makeText(RegisterActivity.this, "Ошибка сети", Toast.LENGTH_SHORT).show();
                }
            });
        }
    }

    // ---------- СПИСОК РЕСТОРАНОВ ----------
     public static class RestaurantListActivity extends AppCompatActivity {

        private RecyclerView restaurantRecyclerView;
        private ProgressBar progressBar;

        @Override
        protected void onCreate(Bundle savedInstanceState) {
            super.onCreate(savedInstanceState);
            setContentView(R.layout.activity_restaurant_list); // твой XML с RecyclerView

            restaurantRecyclerView = findViewById(R.id.restaurantRecyclerView);
            progressBar = findViewById(R.id.progressBar);

            restaurantRecyclerView.setLayoutManager(new LinearLayoutManager(this));

            loadRestaurants();
        }

        private void loadRestaurants() {
            progressBar.setVisibility(View.VISIBLE);

            ApiClient.getApiService().getRestaurants().enqueue(new Callback<List<DataModels.Restaurant>>() {
                @Override
                public void onResponse(Call<List<DataModels.Restaurant>> call, Response<List<DataModels.Restaurant>> response) {
                    progressBar.setVisibility(View.GONE);
                    if (response.isSuccessful() && response.body() != null) {
                        List<DataModels.Restaurant> list = response.body();

                        // подключаем твой адаптер
                        Adapters.RestaurantAdapter adapter = new Adapters.RestaurantAdapter(list, restaurant -> {
                            // открываем страницу деталей ресторана
                            Intent intent = new Intent(RestaurantListActivity.this, RestaurantDetailActivity.class);
                            intent.putExtra("restaurantId", restaurant.id);
                            startActivity(intent);
                        });

                        restaurantRecyclerView.setAdapter(adapter);

                    } else {
                        Toast.makeText(RestaurantListActivity.this, "Ошибка загрузки ресторанов", Toast.LENGTH_SHORT).show();
                    }
                }

                @Override
                public void onFailure(Call<List<DataModels.Restaurant>> call, Throwable t) {
                    progressBar.setVisibility(View.GONE);
                    Toast.makeText(RestaurantListActivity.this, "Ошибка сети: " + t.getMessage(), Toast.LENGTH_SHORT).show();
                }
            });
        }
    }
    public static class RestaurantDetailActivity extends AppCompatActivity {

        private TextView restaurantNameTextView, restaurantAddressTextView, ratingTextView, cuisineTextView, openingHoursTextView;
        private RecyclerView menuRecyclerView;
        private ProgressBar progressBar;
        private Button bookTableButton;
        private int restaurantId;

        @Override
        protected void onCreate(Bundle savedInstanceState) {
            super.onCreate(savedInstanceState);
            setContentView(R.layout.activity_restaurant_detail);

            // Инициализация элементов интерфейса
            restaurantNameTextView = findViewById(R.id.restaurantName);
            restaurantAddressTextView = findViewById(R.id.restaurantAddressTextView);
            ratingTextView = findViewById(R.id.ratingTextView);
            cuisineTextView = findViewById(R.id.cuisineTextView);
            openingHoursTextView = findViewById(R.id.openingHoursTextView);
            progressBar = findViewById(R.id.progressBar);
            menuRecyclerView = findViewById(R.id.menuRecyclerView);
            bookTableButton = findViewById(R.id.bookTableButton);

            // Настраиваем RecyclerView для меню
            menuRecyclerView.setLayoutManager(new LinearLayoutManager(this));

            // Получаем ID ресторана
            restaurantId = getIntent().getIntExtra("restaurantId", 0);

            if (restaurantId == 0) {
                Toast.makeText(this, "Ошибка: ресторан не найден", Toast.LENGTH_SHORT).show();
                finish();
                return;
            }

            loadRestaurantDetails();
            loadMenu();

            // Кнопка "Забронировать стол"
            bookTableButton.setOnClickListener(v -> openBookingDialog());
        }

        private void loadRestaurantDetails() {
            progressBar.setVisibility(View.VISIBLE);

            ApiClient.getApiService().getRestaurantById(restaurantId).enqueue(new Callback<DataModels.Restaurant>() {
                @Override
                public void onResponse(Call<DataModels.Restaurant> call, Response<DataModels.Restaurant> response) {
                    progressBar.setVisibility(View.GONE);
                    if (response.isSuccessful() && response.body() != null) {
                        DataModels.Restaurant r = response.body();
                        restaurantNameTextView.setText(r.name);
                        restaurantAddressTextView.setText(r.address);
                        ratingTextView.setText(String.format(Locale.getDefault(), "★ %.1f", r.rating));
                        cuisineTextView.setText(r.cuisineType);
                        openingHoursTextView.setText("Часы работы: " + (r.openingHours != null ? r.openingHours : "не указаны"));
                    } else {
                        Toast.makeText(RestaurantDetailActivity.this, "Ошибка загрузки ресторана", Toast.LENGTH_SHORT).show();
                    }
                }

                @Override
                public void onFailure(Call<DataModels.Restaurant> call, Throwable t) {
                    progressBar.setVisibility(View.GONE);
                    Toast.makeText(RestaurantDetailActivity.this, "Ошибка сети", Toast.LENGTH_SHORT).show();
                }
            });
        }

        private void loadMenu() {
            ApiClient.getApiService().getRestaurantMenu(restaurantId).enqueue(new Callback<List<DataModels.Dish>>() {
                @Override
                public void onResponse(Call<List<DataModels.Dish>> call, Response<List<DataModels.Dish>> response) {
                    if (response.isSuccessful() && response.body() != null) {
                        List<DataModels.Dish> menu = response.body();
                        Adapters.MenuAdapter adapter = new Adapters.MenuAdapter(menu);
                        menuRecyclerView.setAdapter(adapter);
                    } else {
                        Toast.makeText(RestaurantDetailActivity.this, "Ошибка загрузки меню", Toast.LENGTH_SHORT).show();
                    }
                }

                @Override
                public void onFailure(Call<List<DataModels.Dish>> call, Throwable t) {
                    Toast.makeText(RestaurantDetailActivity.this, "Ошибка сети", Toast.LENGTH_SHORT).show();
                }
            });
        }

        private void openBookingDialog() {
            Calendar calendar = Calendar.getInstance();

            new DatePickerDialog(this, (view, year, month, dayOfMonth) -> {
                new TimePickerDialog(this, (timeView, hourOfDay, minute) -> {
                    calendar.set(year, month, dayOfMonth, hourOfDay, minute);
                    String formattedDate = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.getDefault()).format(calendar.getTime());

                    int userId = getIntent().getIntExtra("userId", 0);
                    if (userId == 0) {
                        Toast.makeText(this, "Ошибка: пользователь не найден", Toast.LENGTH_SHORT).show();
                        return;
                    }

                    ApiClient.getApiService().createBooking(userId, restaurantId, formattedDate, 2)
                            .enqueue(new Callback<Void>() {
                                @Override
                                public void onResponse(Call<Void> call, Response<Void> response) {
                                    if (response.isSuccessful()) {
                                        Toast.makeText(RestaurantDetailActivity.this, "Бронирование успешно", Toast.LENGTH_SHORT).show();
                                    } else {
                                        Toast.makeText(RestaurantDetailActivity.this, "Ошибка бронирования", Toast.LENGTH_SHORT).show();
                                    }
                                }

                                @Override
                                public void onFailure(Call<Void> call, Throwable t) {
                                    Toast.makeText(RestaurantDetailActivity.this, "Ошибка сети", Toast.LENGTH_SHORT).show();
                                }
                            });
                }, calendar.get(Calendar.HOUR_OF_DAY), calendar.get(Calendar.MINUTE), true).show();
            }, calendar.get(Calendar.YEAR), calendar.get(Calendar.MONTH), calendar.get(Calendar.DAY_OF_MONTH)).show();
        }
    }
}