package com.example.a123;

import android.app.DatePickerDialog;
import android.app.TimePickerDialog;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;
import retrofit2.Call;
import retrofit2.http.DELETE;
import retrofit2.http.Field;
import retrofit2.http.FormUrlEncoded;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.Path;
import retrofit2.http.Query;
import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.a123.Adapters.MenuAdapter;
import com.example.a123.Adapters.RestaurantAdapter;
import com.example.a123.DataModels.Booking;
import com.example.a123.DataModels.Dish;
import com.example.a123.DataModels.Restaurant;
import com.example.a123.DataModels.User;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.List;
import java.util.Locale;

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
            String enteredLogin = loginEditText.getText().toString().trim();
            String enteredPassword = passwordEditText.getText().toString().trim();

            if (enteredLogin.isEmpty() || enteredPassword.isEmpty()) {
                Toast.makeText(this, "Введите логин и пароль", Toast.LENGTH_SHORT).show();
                return;
            }

            progressBar.setVisibility(View.VISIBLE);

            ApiClient.getApiService().loginUser(enteredLogin, enteredPassword)
                    .enqueue(new Callback<UserIdResponse>() {
                        @Override
                        public void onResponse(Call<UserIdResponse> call, Response<UserIdResponse> response) {
                            progressBar.setVisibility(View.GONE);

                            if (response.isSuccessful() && response.body() != null) {
                                int userId = response.body().UserId;

                                Toast.makeText(LoginActivity.this, "Логин успешный! userId = " + userId, Toast.LENGTH_SHORT).show();

                                Intent intent = new Intent(LoginActivity.this, RestaurantListActivity.class);
                                intent.putExtra("userId", userId);
                                startActivity(intent);
                                finish();
                            } else {
                                Toast.makeText(LoginActivity.this, "Неверные данные", Toast.LENGTH_SHORT).show();
                            }
                        }

                        @Override
                        public void onFailure(Call<UserIdResponse> call, Throwable t) {
                            progressBar.setVisibility(View.GONE);
                            Toast.makeText(LoginActivity.this, "Ошибка сети: " + t.getMessage(), Toast.LENGTH_SHORT).show();
                        }
                    });
        }     }

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
        private int userId;

        @Override
        protected void onCreate(Bundle savedInstanceState) {
            super.onCreate(savedInstanceState);
            setContentView(R.layout.activity_restaurant_list); // твой XML с RecyclerView

            restaurantRecyclerView = findViewById(R.id.restaurantRecyclerView);
            progressBar = findViewById(R.id.progressBar);

            restaurantRecyclerView.setLayoutManager(new LinearLayoutManager(this));

            userId = getIntent().getIntExtra("userId", 0);

            // ImageButton profileButton = findViewById(R.id.profileButton);
          //  profileButton.setOnClickListener(v -> {
         //       Intent intent = new Intent(RestaurantListActivity.this, ProfileActivity.class);
          //      intent.putExtra("userId", userId);
          //      startActivity(intent);
          //  });

            loadRestaurants();
        }

        private void loadRestaurants() {
            progressBar.setVisibility(View.VISIBLE);

            ApiClient.getApiService().getRestaurants().enqueue(new Callback<List<Restaurant>>() {
                @Override
                public void onResponse(Call<List<Restaurant>> call, Response<List<Restaurant>> response) {
                    progressBar.setVisibility(View.GONE);
                    if (response.isSuccessful() && response.body() != null) {
                        List<Restaurant> list = response.body();

                        // подключаем твой адаптер
                        Adapters.RestaurantAdapter adapter = new Adapters.RestaurantAdapter(list, restaurant -> {
                            // открываем страницу деталей ресторана
                            Intent intent = new Intent(RestaurantListActivity.this, RestaurantDetailActivity.class);
                            intent.putExtra("restaurantId", restaurant.Id);
                            intent.putExtra("userId", userId);
                            startActivity(intent);
                        });

                        restaurantRecyclerView.setAdapter(adapter);

                    } else {
                        Toast.makeText(RestaurantListActivity.this, "Ошибка загрузки ресторанов", Toast.LENGTH_SHORT).show();
                    }
                }

                @Override
                public void onFailure(Call<List<Restaurant>> call, Throwable t) {
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
        private int restaurantId, userId;

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
            userId = getIntent().getIntExtra("userId", 0);

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

            ApiClient.getApiService().getRestaurantById(restaurantId).enqueue(new Callback<Restaurant>() {
                @Override
                public void onResponse(Call<Restaurant> call, Response<Restaurant> response) {
                    progressBar.setVisibility(View.GONE);
                    if (response.isSuccessful() && response.body() != null) {
                        Restaurant r = response.body();
                        restaurantNameTextView.setText(r.Name);
                        restaurantAddressTextView.setText(r.Address);
                        ratingTextView.setText(String.format(Locale.getDefault(), "★ %.1f", 4.5));
                        cuisineTextView.setText(r.Tematic);
                        openingHoursTextView.setText("Часы работы: " + r.Open_Time + " - " + r.Close_Time);
                    } else {
                        Toast.makeText(RestaurantDetailActivity.this, "Ошибка загрузки ресторана", Toast.LENGTH_SHORT).show();
                    }
                }

                @Override
                public void onFailure(Call<Restaurant> call, Throwable t) {
                    progressBar.setVisibility(View.GONE);
                    Toast.makeText(RestaurantDetailActivity.this, "Ошибка сети", Toast.LENGTH_SHORT).show();
                }
            });
        }

        private void loadMenu() {
            ApiClient.getApiService().getRestaurantMenu(restaurantId).enqueue(new Callback<List<Dish>>() {
                @Override
                public void onResponse(Call<List<Dish>> call, Response<List<Dish>> response) {
                    if (response.isSuccessful() && response.body() != null) {
                        List<Dish> menu = response.body();
                        Adapters.MenuAdapter adapter = new Adapters.MenuAdapter(menu);
                        menuRecyclerView.setAdapter(adapter);
                    } else {
                        Toast.makeText(RestaurantDetailActivity.this, "Ошибка загрузки меню", Toast.LENGTH_SHORT).show();
                    }
                }

                @Override
                public void onFailure(Call<List<Dish>> call, Throwable t) {
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

                    if (userId == 0) {
                        Toast.makeText(this, "Ошибка: пользователь не найден", Toast.LENGTH_SHORT).show();
                        return;
                    }

                    ApiClient.getApiService().createBooking(userId, restaurantId, formattedDate, 2, "Ожидание", "")
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

    // ---------- ПРОФИЛЬ ----------
    public static class ProfileActivity extends AppCompatActivity {

        private RecyclerView bookingsRecyclerView;
        private ProgressBar progressBar;
        private int userId;

        @Override
        protected void onCreate(Bundle savedInstanceState) {
            super.onCreate(savedInstanceState);
            setContentView(R.layout.activity_profile);

            bookingsRecyclerView = findViewById(R.id.bookingsRecyclerView);
            progressBar = findViewById(R.id.progressBar);

            bookingsRecyclerView.setLayoutManager(new LinearLayoutManager(this));

            userId = getIntent().getIntExtra("userId", 0);

            loadBookings();
        }

        private void loadBookings() {
            progressBar.setVisibility(View.VISIBLE);

            ApiClient.getApiService().getAllBookings().enqueue(new Callback<List<Booking>>() {
                @Override
                public void onResponse(Call<List<Booking>> call, Response<List<Booking>> response) {
                    progressBar.setVisibility(View.GONE);
                    if (response.isSuccessful() && response.body() != null) {
                        List<Booking> allBookings = response.body();
                        List<Booking> userBookings = new ArrayList<>();
                        for (Booking b : allBookings) {
                            if (b.User_Id == userId) {
                                userBookings.add(b);
                                loadRestaurantNameForBooking(b);
                            }
                        }
                        Adapters.BookingsAdapter adapter = new Adapters.BookingsAdapter(userBookings);
                        bookingsRecyclerView.setAdapter(adapter);
                    } else {
                        Toast.makeText(ProfileActivity.this, "Ошибка загрузки бронирований", Toast.LENGTH_SHORT).show();
                    }
                }

                @Override
                public void onFailure(Call<List<Booking>> call, Throwable t) {
                    progressBar.setVisibility(View.GONE);
                    Toast.makeText(ProfileActivity.this, "Ошибка сети: " + t.getMessage(), Toast.LENGTH_SHORT).show();
                }
            });
        }

        private void loadRestaurantNameForBooking(final Booking b) {
            ApiClient.getApiService().getRestaurantById(b.Restaurant_Id).enqueue(new Callback<Restaurant>() {
                @Override
                public void onResponse(Call<Restaurant> call, Response<Restaurant> response) {
                    if (response.isSuccessful() && response.body() != null) {
                        b.restaurantName = response.body().Name;
                        if (bookingsRecyclerView.getAdapter() != null) {
                            bookingsRecyclerView.getAdapter().notifyDataSetChanged();
                        }
                    }
                }

                @Override
                public void onFailure(Call<Restaurant> call, Throwable t) {
                    // Игнорируем
                }
            });
        }
    }
}