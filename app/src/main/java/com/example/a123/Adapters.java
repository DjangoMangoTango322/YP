package com.example.a123;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.*;
import androidx.annotation.NonNull;
import androidx.cardview.widget.CardView;
import androidx.recyclerview.widget.RecyclerView;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;
import java.util.Locale;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;


public class Adapters {  public static class RestaurantAdapter extends RecyclerView.Adapter<RestaurantAdapter.ViewHolder> {
    private List<DataModels.Restaurant> restaurantList;
    private OnRestaurantClickListener listener;

    public interface OnRestaurantClickListener {
        void onRestaurantClick(DataModels.Restaurant restaurant);
    }

    public RestaurantAdapter(List<DataModels.Restaurant> restaurantList, OnRestaurantClickListener listener) {
        this.restaurantList = restaurantList;
        this.listener = listener;
    }

    @NonNull @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_restaurant, parent, false);
        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        DataModels.Restaurant restaurant = restaurantList.get(position);
        holder.nameTextView.setText(restaurant.name);
        holder.addressTextView.setText(restaurant.address);
        holder.ratingTextView.setText(String.format("★ %.1f", restaurant.rating));
        holder.cuisineTextView.setText(restaurant.cuisineType);
        // Установите изображение ресторана (заглушка)
        // holder.restaurantImage.setImageResource(R.drawable.restaurant_placeholder);

        holder.cardView.setOnClickListener(v -> {
            if (listener != null) listener.onRestaurantClick(restaurant);
        });
    }

    @Override
    public int getItemCount() {
        return restaurantList.size();
    }

    public static class ViewHolder extends RecyclerView.ViewHolder {
        CardView cardView;
        ImageView restaurantImage;
        TextView nameTextView, addressTextView, ratingTextView, cuisineTextView;

        public ViewHolder(@NonNull View itemView) {
            super(itemView);
            cardView = itemView.findViewById(R.id.cardView);
            restaurantImage = itemView.findViewById(R.id.restaurantImage);
            nameTextView = itemView.findViewById(R.id.nameTextView);
            addressTextView = itemView.findViewById(R.id.addressTextView);
            ratingTextView = itemView.findViewById(R.id.ratingTextView);
            cuisineTextView = itemView.findViewById(R.id.cuisineTextView);
        }
    }
}

    // Menu Adapter
    public static class MenuAdapter extends RecyclerView.Adapter<MenuAdapter.ViewHolder> {
        private List<DataModels.Dish> menuList;

        public MenuAdapter(List<DataModels.Dish> menuList) {
            this.menuList = menuList;
        }

        @NonNull @Override
        public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_menu, parent, false);
            return new ViewHolder(view);
        }

        @Override
        public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
            DataModels.Dish dish = menuList.get(position);
            holder.dishNameTextView.setText(dish.name);
            holder.dishDescriptionTextView.setText(dish.description);
            holder.dishPriceTextView.setText(String.format(Locale.getDefault(), "$%.2f", dish.price));
            holder.dishCategoryTextView.setText(dish.category);
            holder.vegetarianBadge.setVisibility(dish.isVegetarian ? View.VISIBLE : View.GONE);
        }

        @Override
        public int getItemCount() {
            return menuList != null ? menuList.size() : 0;
        }

        public static class ViewHolder extends RecyclerView.ViewHolder {
            TextView dishNameTextView, dishDescriptionTextView, dishPriceTextView, dishCategoryTextView;
            TextView vegetarianBadge;

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

    // Bookings Adapter
    public static class BookingsAdapter extends RecyclerView.Adapter<BookingsAdapter.ViewHolder> {
        private List<DataModels.Booking> bookingList;

        public BookingsAdapter(List<DataModels.Booking> bookingList) {
            this.bookingList = bookingList;
        }

        @NonNull @Override
        public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_booking, parent, false);
            return new ViewHolder(view);
        }

        @Override
        public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
            DataModels.Booking booking = bookingList.get(position);
            holder.restaurantNameTextView.setText(booking.restaurantName);
            holder.bookingDateTextView.setText(formatDate(booking.bookingDate));
            holder.peopleCountTextView.setText(booking.numberOfPeople + " чел.");
            holder.statusTextView.setText(booking.status);
            setStatusColor(holder.statusTextView, booking.status);

            holder.cancelButton.setOnClickListener(v -> cancelBooking(booking.id, position));
            holder.cancelButton.setVisibility(
                    "completed".equals(booking.status) || "cancelled".equals(booking.status) ? View.GONE : View.VISIBLE);
        }

        @Override
        public int getItemCount() {
            return bookingList != null ? bookingList.size() : 0;
        }

        private String formatDate(Date date) {
            SimpleDateFormat format = new SimpleDateFormat("dd MMMM yyyy, HH:mm", new Locale("ru"));
            return format.format(date);
        }

        private void setStatusColor(TextView statusView, String status) {
            int colorId;
            switch (status.toLowerCase()) {
                case "confirmed":
                    colorId = R.color.PrimaryColor; // Используем существующие цвета
                    break;
                case "pending":
                    colorId = R.color.AccentColor;
                    break;
                case "cancelled":
                    colorId = R.color.PrimaryDarkColor;
                    break;
                default:
                    colorId = R.color.TextSecondaryColor;
            }
            statusView.setTextColor(statusView.getContext().getResources().getColor(colorId));
        }

        private void cancelBooking(String bookingId, int position) {
            ApiClient.getApiService().cancelBooking(bookingId).enqueue(new Callback<ApiClient.ApiResponse<Void>>() {
                @Override
                public void onResponse(Call<ApiClient.ApiResponse<Void>> call, Response<ApiClient.ApiResponse<Void>> response) {
                    if (response.isSuccessful() && response.body() != null && response.body().isSuccess()) {
                        bookingList.get(position).status = "cancelled";
                        notifyItemChanged(position);
                    }
                }

                @Override
                public void onFailure(Call<ApiClient.ApiResponse<Void>> call, Throwable t) {}
            });
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