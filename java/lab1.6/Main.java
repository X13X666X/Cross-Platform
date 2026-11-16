
import java.io.*;
import java.util.*;
import java.util.regex.Pattern;

public class Main {

    private static final Scanner scanner =
        new Scanner(System.in);

    private static ShopCatalog catalog =
        new ShopCatalog();

    private static final String[] DAYS = {
        "Monday", "Tuesday", "Wednesday",
        "Thursday", "Friday", "Saturday", "Sunday"
    };

    private static Map<String, String> inputSchedule() {

        Map<String, String> schedule =
            new LinkedHashMap<>();

        Pattern pattern = Pattern.compile(
            "^([01]\\d|2[0-3]):[0-5]\\d-" +
            "([01]\\d|2[0-3]):[0-5]\\d$"
        );

        for (String day : DAYS) {
            while (true) {
                System.out.print(day + " (09:00-20:00, or 'Day off'): ");

                String hours = scanner.nextLine().trim();

                if (hours.matches("(?i)day off")) {
                    schedule.put(day, "Day off");
                    break;
                }

                if (pattern.matcher(hours).matches()) {
                    String[] times = hours.split("-");

                    if (times[0].compareTo(times[1]) < 0) {
                        schedule.put(day, hours);
                        break;
                    }
                }

                System.out.println("Invalid working hours. Use HH:MM-HH:MM or 'Day off'.");
            }
        }

        return schedule;
    }

    private static Shop inputShop() {

        System.out.print("Shop name: ");
        String name = scanner.nextLine();

        System.out.print("Address: ");
        String address = scanner.nextLine();

        System.out.print("Specialization: ");
        String specialization = scanner.nextLine();

        Map<String, String> schedule = inputSchedule();

        Shop shop = new Shop(
            name, address, specialization, schedule
        );

        System.out.print("Number of phone numbers: ");
        int count = Integer.parseInt(scanner.nextLine());

        for (int i = 0; i < count; i++) {
            System.out.print("Phone number: ");
            shop.addPhone(scanner.nextLine());
        }

        return shop;
    }

    private static void addShop() {

        Shop shop = inputShop();

        catalog.addShop(shop);

        System.out.println("Shop added.");
    }

    private static void showShops() {

        System.out.println("\nSHOP CATALOG");

        ShopUtils.printShops(catalog.getShops());
    }

    private static void deleteShop() {

        showShops();

        System.out.print("Enter the shop number to delete: ");

        int index = Integer.parseInt(scanner.nextLine());

        if (catalog.removeShop(index - 1)) {
            System.out.println("Shop deleted.");
        } else {
            System.out.println("Invalid shop number.");
        }
    }

    private static void editShop() {

        showShops();

        System.out.print("Enter the shop number to edit: ");

        int index = Integer.parseInt(scanner.nextLine()) - 1;

        Shop oldShop = catalog.getShop(index);

        if (oldShop == null) {
            System.out.println("Shop not found.");
            return;
        }

        System.out.println("Enter the updated shop details:");

        Shop newShop = inputShop();

        catalog.updateShop(index, newShop);

        System.out.println("Shop details updated.");
    }

    private static void searchShop() {

        System.out.print("Enter a shop name to search for: ");

        String query = scanner.nextLine();

        List<Shop> result = ShopUtils.searchByName(
            catalog.getShops(), query
        );

        ShopUtils.printShops(result);
    }

    private static void sortShops() {

        System.out.println("\nSORT SHOPS");
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
                System.out.println("Invalid selection.");
                return;
        }

        List<Shop> result = ShopUtils.sort(
            catalog.getShops(), comparator
        );

        ShopUtils.printShops(result);
    }

    private static void searchVariant19() {

        System.out.println(
            "\nSEARCH RESULTS FOR VARIANT 19"
        );

        List<Shop> result =
            ShopUtils.searchVariant19(catalog.getShops());

        ShopUtils.printShops(result);
    }

    private static void saveData() {

        try (ObjectOutputStream output =
                 new ObjectOutputStream(
                     new FileOutputStream("shops_collection.dat"))) {

            output.writeObject(catalog);

            System.out.println("Catalog saved.");

        } catch (IOException e) {
            System.out.println(
                "Error saving catalog: " + e.getMessage()
            );
        }
    }

    private static void loadData() {

        File file = new File("shops_collection.dat");

        if (!file.exists()) {
            System.out.println("Save file not found.");
            return;
        }

        try (ObjectInputStream input =
                 new ObjectInputStream(
                     new FileInputStream(file))) {

            Object object = input.readObject();

            if (object instanceof ShopCatalog) {
                catalog = (ShopCatalog) object;

                System.out.println(
                    "Catalog loaded. Number of shops: "
                    + catalog.size()
                );
            }

        } catch (IOException | ClassNotFoundException e) {
            System.out.println(
                "Error loading catalog: " + e.getMessage()
            );
        }
    }

    private static void automaticMode() {

        catalog = new ShopCatalog();

        Map<String, String> allWeek =
            new LinkedHashMap<>();

        for (String day : DAYS) {
            allWeek.put(day, "09:00-21:00");
        }

        Shop first = new Shop(
            "BuildRight",
            "Kyiv, Berkovetska Street",
            "Building supplies",
            allWeek
        );

        first.addPhone("+380671234567");
        first.addPhone("+380931234567");

        catalog.addShop(first);

        Shop second = new Shop(
            "Book World",
            "Lviv, Shevchenko Street",
            "Books",
            allWeek
        );

        second.addPhone("+380501234567");

        catalog.addShop(second);

        Map<String, String> daysOff =
            new LinkedHashMap<>(allWeek);

        daysOff.put("Sunday", "Day off");

        Shop third = new Shop(
            "Fresh Market",
            "Kyiv, Central Street",
            "Grocery store",
            allWeek
        );

        third.addPhone("+380631234567");

        catalog.addShop(third);

        Shop fourth = new Shop(
            "Clothing Store",
            "Lviv, Svobody Avenue",
            "Clothing",
            daysOff
        );

        fourth.addPhone("+380931234567");

        catalog.addShop(fourth);

        System.out.println("\nAUTOMATIC MODE");

        System.out.println("\nInitial catalog:");
        showShops();

        System.out.println("\nSorted by name:");

        ShopUtils.printShops(
            ShopUtils.sort(
                catalog.getShops(),
                Comparator.comparing(
                    Shop::getName,
                    String.CASE_INSENSITIVE_ORDER
                )
            )
        );

        System.out.println("\nSearch for variant 19:");

        searchVariant19();

        System.out.println("\nStatistics:");
        ShopUtils.showStatistics(catalog.getShops());

        System.out.println("\nSaving:");
        saveData();

        System.out.println("\nLoading:");
        loadData();

        System.out.println("\nAutomatic mode complete.");
    }

    public static void main(String[] args) {

        if (args.length > 0 && args[0].equals("-auto")) {
            automaticMode();
            return;
        }

        while (true) {

            System.out.println(
                "\n========== SHOP CATALOG =========="
            );

            System.out.println("1. Add a shop");
            System.out.println("2. Show catalog");
            System.out.println("3. Delete a shop");
            System.out.println("4. Edit a shop");
            System.out.println("5. Search by name");
            System.out.println("6. Sort shops");
            System.out.println("7. Search using variant 19");
            System.out.println("8. Catalog statistics");
            System.out.println("9. Save catalog");
            System.out.println("10. Load catalog");
            System.out.println("0. Exit");

            System.out.print("Your selection: ");

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
                        editShop();
                        break;

                    case 5:
                        searchShop();
                        break;

                    case 6:
                        sortShops();
                        break;

                    case 7:
                        searchVariant19();
                        break;

                    case 8:
                        ShopUtils.showStatistics(
                            catalog.getShops()
                        );
                        break;

                    case 9:
                        saveData();
                        break;

                    case 10:
                        loadData();
                        break;

                    case 0:
                        scanner.close();
                        return;

                    default:
                        System.out.println("Invalid selection.");
                }

            } catch (NumberFormatException e) {
                System.out.println("Please enter a valid number.");
            }
        }
    }
}
