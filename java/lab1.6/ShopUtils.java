
import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.List;

public class ShopUtils {

    // Sort shops
    public static List<Shop> sort(
            List<Shop> shops,
            Comparator<Shop> comparator) {

        List<Shop> result = new ArrayList<>(shops);

        Collections.sort(result, comparator);

        return result;
    }

    // Search by name
    public static List<Shop> searchByName(
            List<Shop> shops, String query) {

        List<Shop> result = new ArrayList<>();

        for (Shop shop : shops) {
            if (shop.getName().toLowerCase()
                    .contains(query.toLowerCase())) {

                result.add(shop);
            }
        }

        return result;
    }

    // Search using variant 19
    public static List<Shop> searchVariant19(
            List<Shop> shops) {

        List<Shop> result = new ArrayList<>();

        for (Shop shop : shops) {
            if (shop.matchesVariant19()) {
                result.add(shop);
            }
        }

        return result;
    }

    // Display catalog statistics
    public static void showStatistics(
            List<Shop> shops) {

        if (shops.isEmpty()) {
            System.out.println("Catalog is empty.");
            return;
        }

        int totalPhones = 0;
        int nonFoodShops = 0;
        int suitableShops = 0;

        for (Shop shop : shops) {
            totalPhones += shop.getPhones().size();

            if (shop.isNonFood()) {
                nonFoodShops++;
            }

            if (shop.matchesVariant19()) {
                suitableShops++;
            }
        }

        System.out.println("\nCATALOG STATISTICS");

        System.out.println(
            "Number of shops: " + shops.size()
        );

        System.out.println(
            "Non-food shops: " + nonFoodShops
        );

        System.out.println(
            "Total phone numbers: " + totalPhones
        );

        System.out.println(
            "Shops matching variant 19: "
            + suitableShops
        );

        System.out.println(
            "Average phone numbers per shop: "
            + (double) totalPhones / shops.size()
        );
    }

    // Print catalog
    public static void printShops(List<Shop> shops) {

        if (shops.isEmpty()) {
            System.out.println("No shops found.");
            return;
        }

        for (int i = 0; i < shops.size(); i++) {
            System.out.println("\nShop #" + (i + 1));
            System.out.println(shops.get(i));
        }
    }
}
