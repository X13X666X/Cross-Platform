
import java.io.*;
import java.util.Comparator;
import java.util.Scanner;

public class Main {

    private static final Scanner scanner =
        new Scanner(System.in);

    private static LinkedListContainer<Shop> shops =
        new LinkedListContainer<>();

    private static void addShop() {

        System.out.print("Name: ");
        String name = scanner.nextLine();

        System.out.print("Address: ");
        String address = scanner.nextLine();

        System.out.print("Specialization: ");
        String specialization = scanner.nextLine();

        String[] days = {
            "Monday", "Tuesday", "Wednesday", "Thursday",
            "Friday", "Saturday", "Sunday"
        };

        String[] hours = new String[7];

        for (int i = 0; i < 7; i++) {
            System.out.print(days[i] + ": ");
            hours[i] = scanner.nextLine();
        }

        Shop shop = new Shop(
            name, address, specialization, hours
        );

        System.out.print("Number of phone numbers: ");
        int count = Integer.parseInt(scanner.nextLine());

        for (int i = 0; i < count; i++) {
            System.out.print("Phone: ");
            shop.addPhone(scanner.nextLine());
        }

        shops.add(shop);

        System.out.println("Shop added.");
    }

    private static void showShops() {
        ShopUtils.print(shops);
    }

    private static void deleteShop() {
        showShops();

        System.out.print("Enter the number to delete: ");
        int index = Integer.parseInt(scanner.nextLine());

        if (shops.remove(index - 1)) {
            System.out.println("Shop deleted.");
        } else {
            System.out.println("Invalid number.");
        }
    }

    private static void sortShops() {

        System.out.println("1. By name");
        System.out.println("2. By address");
        System.out.println("3. By specialization");

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

        ShopUtils.print(sorted);
    }

    private static void searchShop() {

        System.out.print("Enter a name to search: ");
        String query = scanner.nextLine();

        LinkedListContainer<Shop> result =
            ShopUtils.search(
                shops,
                shop -> shop.getName()
                    .toLowerCase()
                    .contains(query.toLowerCase())
            );

        ShopUtils.print(result);
    }

    private static void save() {

        try (ObjectOutputStream output =
                 new ObjectOutputStream(
                     new FileOutputStream("shops.dat"))) {

            output.writeObject(shops);

            System.out.println("Data saved.");

        } catch (IOException e) {
            System.out.println(
                "Unable to save data to shops.dat ("
                    + e.getClass().getSimpleName() + ")."
            );
        }
    }

    @SuppressWarnings("unchecked")
    private static void load() {

        File file = new File("shops.dat");
        if (!file.isFile()) {
            System.out.println("No saved data found (shops.dat).");
            return;
        }

        try (ObjectInputStream input =
                 new ObjectInputStream(
                     new FileInputStream(file))) {

            Object object = input.readObject();

            if (object instanceof LinkedListContainer<?>) {
                shops = (LinkedListContainer<Shop>) object;
                System.out.println("Data loaded.");
            } else {
                System.out.println("The file does not contain a valid shop catalog.");
            }

        } catch (IOException | ClassNotFoundException e) {
            System.out.println(
                "Unable to load data from shops.dat ("
                    + e.getClass().getSimpleName() + ")."
            );
        }
    }

    private static void automaticMode() {

        shops = new LinkedListContainer<>();

        shops.add(new Shop(
            "Grocery Store",
            "Kyiv, Khreshchatyk Street",
            "Groceries",
            new String[] {
                "08:00-22:00", "08:00-22:00",
                "08:00-22:00", "08:00-22:00",
                "08:00-22:00", "08:00-22:00",
                "08:00-22:00"
            }
        ));

        Shop shop1 = new Shop(
            "Home Improvement Store",
            "Kyiv, Berkovetska Street",
            "Building supplies",
            new String[] {
                "09:00-21:00", "09:00-21:00",
                "09:00-21:00", "09:00-21:00",
                "09:00-21:00", "09:00-21:00",
                "09:00-21:00"
            }
        );

        shop1.addPhone("+380671234567");
        shops.add(shop1);

        Shop shop2 = new Shop(
            "Book World",
            "Lviv, Shevchenko Street",
            "Books",
            new String[] {
                "09:00-18:00", "09:00-18:00",
                "09:00-18:00", "09:00-18:00",
                "09:00-18:00", "Closed",
                "Closed"
            }
        );

        shop2.addPhone("+380931234567");
        shops.add(shop2);

        System.out.println("AUTOMATIC MODE");
        System.out.println("Sample data created.");

        System.out.println("\nInitial catalog:");
        showShops();

        System.out.println("\nSorting by name:");

        ShopUtils.print(
            ShopUtils.sort(
                shops,
                Comparator.comparing(
                    Shop::getName,
                    String.CASE_INSENSITIVE_ORDER
                )
            )
        );

        System.out.println("\nSaving data:");
        save();

        System.out.println("\nLoading data:");
        load();

        System.out.println("\nAutomatic mode finished.");
    }

    public static void main(String[] args) {

        if (args.length > 0 && args[0].equals("-auto")) {
            automaticMode();
            return;
        }

        while (true) {

            System.out.println("\n========== SHOP CATALOG ==========");
            System.out.println("1. Add a shop");
            System.out.println("2. Show catalog");
            System.out.println("3. Delete a shop");
            System.out.println("4. Sort");
            System.out.println("5. Search");
            System.out.println("6. Save");
            System.out.println("7. Load");
            System.out.println("0. Exit");

            System.out.print("Your choice: ");

            try {
                int choice = Integer.parseInt(scanner.nextLine());

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
                        sortShops();
                        break;
                    case 5:
                        searchShop();
                        break;
                    case 6:
                        save();
                        break;
                    case 7:
                        load();
                        break;
                    case 0:
                        scanner.close();
                        return;
                    default:
                        System.out.println("Invalid choice.");
                }

            } catch (NumberFormatException e) {
                System.out.println("Please enter a number.");
            }
        }
    }
}
