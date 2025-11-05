package com.example.a123;

import android.content.Context;
import android.content.SharedPreferences;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.widget.Toast;

public class Utils {// Network Utilities
    public static class Network {
        public static boolean isNetworkAvailable(Context context) {
            ConnectivityManager cm = (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);
            NetworkInfo activeNetwork = cm.getActiveNetworkInfo();
            return activeNetwork != null && activeNetwork.isConnected();
        }
    }

    // SharedPreferences Helper
    public static class Prefs {
        private static final String PREFS_NAME = "RestaurantAppPrefs";
        private static final String KEY_USER_ID = "user_id", KEY_USER_NAME = "user_name";
        private static final String KEY_USER_EMAIL = "user_email", KEY_IS_LOGGED_IN = "is_logged_in";
        private static final String KEY_AUTH_TOKEN = "auth_token";

        private SharedPreferences prefs;

        public Prefs(Context context) {
            prefs = context.getSharedPreferences(PREFS_NAME, Context.MODE_PRIVATE);
        }

        public void saveUserData(String userId, String name, String email, String token) {
            prefs.edit().putString(KEY_USER_ID, userId).putString(KEY_USER_NAME, name)
                    .putString(KEY_USER_EMAIL, email).putString(KEY_AUTH_TOKEN, token)
                    .putBoolean(KEY_IS_LOGGED_IN, true).apply();
        }

        public void clearUserData() {
            prefs.edit().remove(KEY_USER_ID).remove(KEY_USER_NAME).remove(KEY_USER_EMAIL)
                    .remove(KEY_AUTH_TOKEN).putBoolean(KEY_IS_LOGGED_IN, false).apply();
        }

        public boolean isLoggedIn() { return prefs.getBoolean(KEY_IS_LOGGED_IN, false); }
        public String getUserId() { return prefs.getString(KEY_USER_ID, null); }
        public String getUserName() { return prefs.getString(KEY_USER_NAME, null); }
        public String getUserEmail() { return prefs.getString(KEY_USER_EMAIL, null); }
        public String getAuthToken() { return prefs.getString(KEY_AUTH_TOKEN, null); }
    }

    // UI Utilities
    public static class UI {
        public static void showToast(Context context, String message) {
            Toast.makeText(context, message, Toast.LENGTH_SHORT).show();
        }

        public static void showLongToast(Context context, String message) {
            Toast.makeText(context, message, Toast.LENGTH_LONG).show();
        }
    }

    // Date Utilities
    public static class DateUtils {
        public static String formatBookingDate(Date date) {
            return new SimpleDateFormat("dd MMMM yyyy 'в' HH:mm", new Locale("ru")).format(date);
        }

        public static String formatDateTime(Date date) {
            return new SimpleDateFormat("dd/MM/yyyy HH:mm", Locale.getDefault()).format(date);
        }

        public static String getCurrentDate() {
            return new SimpleDateFormat("dd/MM/yyyy", Locale.getDefault()).format(new Date());
        }

        public static String getCurrentTime() {
            return new SimpleDateFormat("HH:mm", Locale.getDefault()).format(new Date());
        }
    }

    // Booking Utilities
    public static class BookingHelper {
        public static String getStatusText(String status) {
            return switch (status.toLowerCase()) {
                case "confirmed" -> "Подтверждено";
                case "pending" -> "Ожидание подтверждения";
                case "completed" -> "Завершено";
                case "cancelled" -> "Отменено";
                default -> status;
            };
        }

        public static boolean isValidPeopleCount(int count) {
            return count > 0 && count <= 20;
        }
    }
}
