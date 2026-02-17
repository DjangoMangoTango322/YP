package com.example.a123;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;
import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.example.a123.DataModels.Dish; // Добавлен этот импорт
import java.util.List;

public class AiDishAdapter extends RecyclerView.Adapter<AiDishAdapter.ViewHolder> {

    private List<Dish> dishList;
    private OnDishClickListener listener;

    public interface OnDishClickListener {
        void onDishClick(Dish dish);
    }

    public AiDishAdapter(List<Dish> dishList, OnDishClickListener listener) {
        this.dishList = dishList;
        this.listener = listener;
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.item_ai_dish, parent, false);
        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        Dish dish = dishList.get(position);
        holder.nameView.setText(dish.name); // Используем поле name из DataModels.Dish

        holder.itemView.setOnClickListener(v -> listener.onDishClick(dish));
    }

    @Override
    public int getItemCount() {
        return dishList.size();
    }

    public static class ViewHolder extends RecyclerView.ViewHolder {
        TextView nameView;

        public ViewHolder(@NonNull View itemView) {
            super(itemView);
            nameView = itemView.findViewById(R.id.tvDishName);
        }
    }
}