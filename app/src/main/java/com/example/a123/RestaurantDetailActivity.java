package com.example.a123;

import android.app.DatePickerDialog;
import android.app.TimePickerDialog;
import android.os.Bundle;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.NumberPicker;
import android.widget.ProgressBar;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;



import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.List;
import java.util.Locale;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class RestaurantDetailActivity extends AppCompatActivity {

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
        userId = getSavedUserId();

        if (userId == 0) {
            Toast.makeText(this, "Пользователь не авторизован", Toast.LENGTH_SHORT).show();
            finish();
            return;
        }

        if (restaurantId == 0) {
            Toast.makeText(this, "Ошибка: ресторан не найден", Toast.LENGTH_SHORT).show();
            finish();
            return;
        }

        loadRestaurantDetails();
        loadMenu();
        Spinner guestsSpinner = findViewById(R.id.guestsSpinner);

        Integer[] guestsArray = new Integer[10];
        for (int i = 0; i < 10; i++) {
            guestsArray[i] = i + 1;
        }

        ArrayAdapter<Integer> adapter = new ArrayAdapter<>(
                this,
                android.R.layout.simple_spinner_item, // стандартный вид для выбора
                guestsArray
        );
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);

        guestsSpinner.setAdapter(adapter);

        guestsSpinner.setSelection(1);
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
                    ratingTextView.setText(String.format(Locale.getDefault(), "★ %.1f", 4.5));
                    cuisineTextView.setText(r.tematic);
                    openingHoursTextView.setText("Часы работы: " + r.open_Time + " - " + r.close_Time);
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
        // Находим Spinner
        Spinner guestsSpinner = findViewById(R.id.guestsSpinner);

        // Создаем календарь для даты и времени
        Calendar calendar = Calendar.getInstance();

        // Диалог выбора даты
        new DatePickerDialog(this, (view, year, month, dayOfMonth) -> {
            calendar.set(Calendar.YEAR, year);
            calendar.set(Calendar.MONTH, month);
            calendar.set(Calendar.DAY_OF_MONTH, dayOfMonth);

            // Диалог выбора времени
            new TimePickerDialog(this, (timeView, hourOfDay, minute) -> {
                calendar.set(Calendar.HOUR_OF_DAY, hourOfDay);
                calendar.set(Calendar.MINUTE, minute);

                String formattedDate = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.getDefault())
                        .format(calendar.getTime());

                int numberOfGuests = (int) guestsSpinner.getSelectedItem();

                if (userId == 0) {
                    Toast.makeText(this, "Ошибка: пользователь не найден", Toast.LENGTH_SHORT).show();
                    return;
                }

                // Создаем объект бронирования
                BookingRequest booking = new BookingRequest();
                booking.id = 0;
                booking.user_Id = userId;
                booking.restaurant_Id = restaurantId;
                booking.booking_Date = formattedDate;
                booking.number_Of_Guests = numberOfGuests;
                booking.status = "Ожидание";
                booking.created_At = formattedDate;

                // Отправляем запрос на сервер
                ApiClient.getApiService().createBooking(booking)
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

    private int getSavedUserId() {
        return getSharedPreferences("app_prefs", MODE_PRIVATE)
                .getInt("user_id", 0);
    }
}