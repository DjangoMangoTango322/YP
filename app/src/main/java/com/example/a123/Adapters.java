package com.example.a123;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.cardview.widget.CardView;
import androidx.recyclerview.widget.RecyclerView;

import com.example.a123.DataModels.Booking;
import com.example.a123.DataModels.Dish;
import com.example.a123.DataModels.Restaurant;

import java.util.List;
import java.util.Locale;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class Adapters {

    // RestaurantAdapter
    public static class RestaurantAdapter extends RecyclerView.Adapter<RestaurantAdapter.ViewHolder> {

        public interface OnRestaurantClickListener {
            void onRestaurantClick(Restaurant restaurant);
        }

        private List<Restaurant> restaurantList;
        private OnRestaurantClickListener listener;

        public RestaurantAdapter(List<Restaurant> restaurantList, OnRestaurantClickListener listener) {
            this.restaurantList = restaurantList;
            this.listener = listener;
        }

        @NonNull
        @Override
        public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_restaurant, parent, false);
            return new ViewHolder(view);
        }

        @Override
        public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
            Restaurant r = restaurantList.get(position);
            holder.nameTextView.setText(r.name);
            holder.addressTextView.setText(r.address);
            holder.ratingTextView.setText(String.format(Locale.getDefault(), "★ %.1f", 4.5));
            holder.cuisineTextView.setText(r.tematic != null ? r.tematic : "");
            holder.cardView.setOnClickListener(v -> {
                if (listener != null) listener.onRestaurantClick(r);
            });
        }

        @Override
        public int getItemCount() {
            return restaurantList != null ? restaurantList.size() : 0;
        }

        public static class ViewHolder extends RecyclerView.ViewHolder {
            CardView cardView;
            TextView nameTextView, addressTextView, ratingTextView, cuisineTextView;

            public ViewHolder(@NonNull View itemView) {
                super(itemView);
                cardView = itemView.findViewById(R.id.cardView);
                nameTextView = itemView.findViewById(R.id.restaurantName);
                addressTextView = itemView.findViewById(R.id.addressTextView);
                ratingTextView = itemView.findViewById(R.id.ratingTextView);
                cuisineTextView = itemView.findViewById(R.id.cuisineTextView);
            }
        }
    }

    // MenuAdapter
    public static class MenuAdapter extends RecyclerView.Adapter<MenuAdapter.ViewHolder> {
        private List<Dish> menuList;

        public MenuAdapter(List<Dish> menuList) {
            this.menuList = menuList;
        }

        @NonNull
        @Override
        public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_menu, parent, false);
            return new ViewHolder(view);
        }

        @Override
        public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
            Dish d = menuList.get(position);
            holder.dishNameTextView.setText(d.name);
            holder.dishDescriptionTextView.setText(d.description);
            holder.dishPriceTextView.setText(String.format(Locale.getDefault(), "%.0f ₽", d.price));
            holder.dishCategoryTextView.setText(d.category != null ? d.category : "");
            holder.vegetarianBadge.setVisibility(d.isVegetarian ? View.VISIBLE : View.GONE);
        }

        @Override
        public int getItemCount() {
            return menuList != null ? menuList.size() : 0;
        }

        public static class ViewHolder extends RecyclerView.ViewHolder {
            TextView dishNameTextView, dishDescriptionTextView, dishPriceTextView, dishCategoryTextView, vegetarianBadge;

            public ViewHolder(@NonNull View itemView) {
                super(itemView);
                dishNameTextView = itemView.findViewById(R.id.dishNameTextView);
                dishDescriptionTextView = itemView.findViewById(R.id.dishDescriptionTextView);
                dishPriceTextView = itemView.findViewById(R.id.dishPriceTextView);
                dishCategoryTextView = itemView.findViewById(R.id.dishCategoryTextView);
                vegetarianBadge = itemView.findViewById(R.id.vegetarianBadge);
            }
        }
    }

    // BookingsAdapter
    public static class BookingsAdapter extends RecyclerView.Adapter<BookingsAdapter.ViewHolder> {
        private List<Booking> bookingList;

        public BookingsAdapter(List<Booking> bookingList) {
            this.bookingList = bookingList;
        }

        @NonNull
        @Override
        public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_booking, parent, false);
            return new ViewHolder(view);
        }

        @Override
        public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
            Booking b = bookingList.get(position);
            holder.restaurantNameTextView.setText(b.restaurantName != null ? b.restaurantName : "Загрузка...");
            holder.bookingDateTextView.setText(b.booking_Date != null ? formatBookingDate(b.booking_Date) : "");
            holder.peopleCountTextView.setText(b.number_Of_Guests + " чел.");
            holder.statusTextView.setText(getStatusText(b.status));
            holder.cancelButton.setVisibility(
                    ("completed".equalsIgnoreCase(b.status) || "cancelled".equalsIgnoreCase(b.status))
                            ? View.GONE : View.VISIBLE
            );

            holder.cancelButton.setOnClickListener(v -> {
                int currentPosition = holder.getAdapterPosition();
                if (currentPosition == RecyclerView.NO_POSITION) return;

                Booking booking = bookingList.get(currentPosition);

                ApiClient.getApiService().cancelBooking(booking.user_Id, booking.restaurant_Id)
                        .enqueue(new Callback<Void>() {
                            @Override
                            public void onResponse(Call<Void> call, Response<Void> response) {
                                if (response.isSuccessful()) {
                                    booking.status = "cancelled";
                                    notifyItemChanged(currentPosition);
                                } else {
                                    Toast.makeText(v.getContext(), "Ошибка отмены бронирования", Toast.LENGTH_SHORT).show();
                                }
                            }

                            @Override
                            public void onFailure(Call<Void> call, Throwable t) {
                                Toast.makeText(v.getContext(), "Ошибка соединения", Toast.LENGTH_SHORT).show();
                            }
                        });
            });
        }

        @Override
        public int getItemCount() {
            return bookingList != null ? bookingList.size() : 0;
        }

        private String formatBookingDate(String isoDate) {
            if (isoDate == null) return "";
            // Пример: "2025-12-21T18:00:00" → "21.12.2025 18:00"
            try {
                String[] parts = isoDate.split("T");
                String date = parts[0].replace("-", ".");
                String time = parts[1].substring(0, 5);
                String[] d = date.split("\\.");
                return d[2] + "." + d[1] + "." + d[0] + " " + time;
            } catch (Exception e) {
                return isoDate;
            }
        }

        private String getStatusText(String status) {
            if (status == null) return "Неизвестно";
            switch (status.toLowerCase()) {
                case "ожидание": return "Ожидает подтверждения";
                case "confirmed": return "Подтверждено";
                case "completed": return "Завершено";
                case "cancelled": return "Отменено";
                default: return status;
            }
        }

        public static class ViewHolder extends RecyclerView.ViewHolder {
            TextView restaurantNameTextView, bookingDateTextView, peopleCountTextView, statusTextView;
            Button cancelButton;

            public ViewHolder(@NonNull View itemView) {
                super(itemView);
                restaurantNameTextView = itemView.findViewById(R.id.restaurantNameTextView);
                bookingDateTextView = itemView.findViewById(R.id.bookingDateTextView);
                peopleCountTextView = itemView.findViewById(R.id.peopleCountTextView);
                statusTextView = itemView.findViewById(R.id.statusTextView);
                cancelButton = itemView.findViewById(R.id.cancelButton);
            }
        }
    }
}