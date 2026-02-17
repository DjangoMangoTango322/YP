package com.example.a123;

import android.app.ProgressDialog;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;
import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.a123.DataModels.Dish; // Добавлен этот импорт
import com.google.gson.JsonObject;
import java.util.List;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class AiDishListActivity extends AppCompatActivity {

    private RecyclerView recyclerView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_ai_dishes);

        recyclerView = findViewById(R.id.rvAiDishes);
        recyclerView.setLayoutManager(new LinearLayoutManager(this));

        loadDishes();
    }

    private void loadDishes() {
        ApiClient.getApiService().getAllDishes().enqueue(new Callback<List<Dish>>() {
            @Override
            public void onResponse(Call<List<Dish>> call, Response<List<Dish>> response) {
                if (response.isSuccessful() && response.body() != null) {
                    AiDishAdapter adapter = new AiDishAdapter(response.body(), dish -> {
                        askAiAboutDish(dish.name); // Используем поле name
                    });
                    recyclerView.setAdapter(adapter);
                } else {
                    Toast.makeText(AiDishListActivity.this, "Не удалось загрузить меню", Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<List<Dish>> call, Throwable t) {
                Toast.makeText(AiDishListActivity.this, "Ошибка соединения", Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void askAiAboutDish(String dishName) {
        ProgressDialog loading = new ProgressDialog(this);
        loading.setMessage("GigaChat пишет историю про " + dishName + "...");
        loading.setCancelable(false);
        loading.show();

        ApiClient.getApiService().getAiDescription(dishName).enqueue(new Callback<JsonObject>() {
            @Override
            public void onResponse(Call<JsonObject> call, Response<JsonObject> response) {
                loading.dismiss();
                if (response.isSuccessful() && response.body() != null) {
                    String aiText = response.body().get("description").getAsString();
                    showResultDialog(dishName, aiText);
                } else {
                    Toast.makeText(AiDishListActivity.this, "AI не ответил", Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<JsonObject> call, Throwable t) {
                loading.dismiss();
                Toast.makeText(AiDishListActivity.this, "Ошибка сети", Toast.LENGTH_SHORT).show();
            }
        });
    }

    // Вставьте это в AiDishListActivity.java вместо старого showResultDialog
    private void showResultDialog(String title, String message) {
        androidx.appcompat.app.AlertDialog.Builder builder = new androidx.appcompat.app.AlertDialog.Builder(this);

        // Инфлейтим (раздуваем) наш красивый макет
        android.view.View layout = getLayoutInflater().inflate(R.layout.dialog_ai_info, null);
        builder.setView(layout);

        // Привязываем данные к элементам макета
        android.widget.TextView tvTitle = layout.findViewById(R.id.dialogTitle);
        android.widget.TextView tvMessage = layout.findViewById(R.id.dialogMessage);
        android.widget.Button btnClose = layout.findViewById(R.id.btnDialogClose);

        tvTitle.setText(title);
        tvMessage.setText(message); // Здесь будет история от GigaChat

        androidx.appcompat.app.AlertDialog dialog = builder.create();

        // Делаем углы диалога скругленными (убираем стандартный белый квадрат фона)
        if (dialog.getWindow() != null) {
            dialog.getWindow().setBackgroundDrawableResource(android.R.color.transparent);
        }

        btnClose.setOnClickListener(v -> dialog.dismiss());
        dialog.show();
    }
}