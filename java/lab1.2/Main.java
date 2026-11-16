
import java.util.List;
import java.util.Scanner;
import java.util.Comparator;

public class Main {

    private static final Scanner scanner =
        new Scanner(System.in);

    private static final LinkedListContainer<Shop> shops =
        new LinkedListContainer<>();

    private static void addShop() {
        System.out.println("\nADD A SHOP");

        System.out.print("Name: ");
        String name = scanner.nextLine();

        System.out.print("Address: ");
        String address = scanner.nextLine();

        System.out.print("Specialization: ");
        String specialization = scanner.nextLine();

        System.out.print("Working hours: ");
        String hours = scanner.nextLine();

        Shop shop = new Shop(
            name, address, specialization, hours
        );

        System.out.print("Number of phone numbers: ");
        int count = Integer.parseInt(scanner.nextLine());

        for (int i = 0; i < count; i++) {
            System.out.print("Phone number " + (i + 1) + ": ");
            shop.addPhone(scanner.nextLine());
        }

        shops.add(shop);

        System.out.println("Shop added successfully.");
    }

    private static void showShops() {
        System.out.println("\nSHOP LIST");
        ShopUtils.print(shops);
    }

    private static void deleteShop() {
        showShops();

        System.out.print("Enter the shop number to delete: ");
        int index = Integer.parseInt(scanner.nextLine());

        if (shops.remove(index - 1)) {
            System.out.println("Shop deleted.");
        } else {
            System.out.println("Invalid shop number.");
        }
    }

    private static void searchShop() {
        System.out.print("Enter a shop name to search for: ");
        String query = scanner.nextLine().toLowerCase();

        List<Shop> result = ShopUtils.search(
            shops,
            shop -> shop.getName()
                        .toLowerCase()
                        .contains(query)
        );

        System.out.println("\nSEARCH RESULTS");

        if (result.isEmpty()) {
            System.out.println("No shops found.");
        } else {
            for (Shop shop : result) {
                System.out.println(shop);
            }
        }
    }

    private static void sortShops() {
        System.out.println("\nSORT SHOPS");
        System.out.println("1. By name");
        System.out.println("2. By address");
        System.out.println("3. By specialization");

        System.out.print("Your choice: ");
        int choice = Integer.parseInt(scanner.nextLine());

        Comparator<Shop> comparator;

        switch (choice) {
            case 1:
                comparator = Comparator.comparing(
                    Shop::getName,
                    String.CASE_INSENSITIVE_ORDER
                );
                break;

            case 2:
                comparator = Comparator.comparing(
                    Shop::getAddress,
                    String.CASE_INSENSITIVE_ORDER
                );
                break;

            case 3:
                comparator = Comparator.comparing(
                    Shop::getSpecialization,
                    String.CASE_INSENSITIVE_ORDER
                );
                break;

            default:
                System.out.println("Invalid choice.");
                return;
        }

        LinkedListContainer<Shop> sorted =
            ShopUtils.sort(shops, comparator);

        System.out.println("\nSORTED SHOP CATALOG");
        ShopUtils.print(sorted);
    }

    public static void main(String[] args) {

        while (true) {
            System.out.println("\n========== SHOP CATALOG ==========");
            System.out.println("1. Add a shop");
            System.out.println("2. Show all shops");
            System.out.println("3. Delete a shop");
            System.out.println("4. Search for a shop");
            System.out.println("5. Sort the catalog");
            System.out.println("0. Exit");

            System.out.print("Your choice: ");

            try {
                int choice = Integer.parseInt(
                    scanner.nextLine()
                );

                switch (choice) {
                    case 1:
                        addShop();
                        break;

                    case 2:
                        showShops();
                        break;

                    case 3:
                        deleteShop();
                        break;

                    case 4:
                        searchShop();
                        break;

                    case 5:
                        sortShops();
                        break;

                    case 0:
                        System.out.println("Exiting the program.");
                        scanner.close();
                        return;

                    default:
                        System.out.println("Invalid menu option.");
                }

            } catch (NumberFormatException e) {
                System.out.println("Please enter a number.");
            }
        }
    }
}
