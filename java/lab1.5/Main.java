
import java.util.Scanner;
import java.util.regex.Pattern;
import java.util.Comparator;

public class Main {

    private static final Scanner scanner =
        new Scanner(System.in);

    private static LinkedListContainer<Shop> shops =
        new LinkedListContainer<>();

    // Validate the shop name
    private static final Pattern NAME_PATTERN =
        Pattern.compile("^[\\p{L}\\p{N}][\\p{L}\\p{N} .'-]{1,49}$");

    // Validate the address
    private static final Pattern ADDRESS_PATTERN =
        Pattern.compile("^[\\p{L}\\p{N} .,'/-]{3,100}$");

    // Validate the specialization
    private static final Pattern SPECIALIZATION_PATTERN =
        Pattern.compile("^[\\p{L}\\p{N} .,'-]{2,50}$");

    // Validate Ukrainian mobile phone numbers
    private static final Pattern PHONE_PATTERN =
        Pattern.compile(
            "^(?:\\+380|380|0)(?:39|50|63|66|67|68|" +
            "73|91|92|93|94|95|96|97|98|99)\\d{7}$"
        );

    // Validate working hours
    private static final Pattern HOURS_PATTERN =
        Pattern.compile(
            "^([01]\\d|2[0-3]):[0-5]\\d-" +
            "([01]\\d|2[0-3]):[0-5]\\d$"
        );

    private static String readValidated(
            String message, Pattern pattern) {

        while (true) {
            System.out.print(message);

            String value = scanner.nextLine().trim();

            if (pattern.matcher(value).matches()) {
                return value;
            }

            System.out.println(
                "Invalid input. Please try again."
            );
        }
    }

    private static String readPhone() {
        while (true) {
            System.out.print("Phone number: ");

            String phone = scanner.nextLine()
                .replaceAll("[\\s()\\-]", "");

            if (PHONE_PATTERN.matcher(phone).matches()) {
                return phone;
            }

            System.out.println(
                "Invalid Ukrainian mobile phone number."
            );
        }
    }

    private static String readHours() {
        while (true) {
            System.out.print("Working hours (09:00-20:00): ");

            String hours = scanner.nextLine().trim();

            if (HOURS_PATTERN.matcher(hours).matches()) {
                String[] times = hours.split("-");

                if (times[0].compareTo(times[1]) < 0) {
                    return hours;
                }
            }

            System.out.println("Invalid working hours.");
        }
    }

    private static void addShop() {

        System.out.println("\nADD A SHOP");

        String name = readValidated(
            "Shop name (e.g., City Books): ", NAME_PATTERN
        );

        String address = readValidated(
            "Address (e.g., 10 Main Street, Kyiv): ", ADDRESS_PATTERN
        );

        String specialization = readValidated(
            "Specialization (e.g., books, clothing): ",
            SPECIALIZATION_PATTERN
        );

        String[] days = {
            "Monday", "Tuesday", "Wednesday", "Thursday",
            "Friday", "Saturday", "Sunday"
        };

        String[] hours = new String[7];

        for (int i = 0; i < 7; i++) {
            System.out.print(days[i] + " ");

            System.out.println(
                "(enter Day off or working hours, e.g., 09:00-18:00)"
            );

            String input = scanner.nextLine().trim();

            if (input.equalsIgnoreCase("Day off")) {
                hours[i] = "Day off";
            } else if (HOURS_PATTERN.matcher(input).matches()) {
                String[] times = input.split("-");

                if (times[0].compareTo(times[1]) >= 0) {
                    System.out.println("Invalid time interval.");
                    i--;
                    continue;
                }

                hours[i] = input;
            } else {
                System.out.println("Invalid working hours.");
                i--;
                continue;
            }
        }

        Shop shop = new Shop(
            name, address, specialization, hours
        );

        System.out.print("Number of phone numbers: ");
        int count = Integer.parseInt(scanner.nextLine());

        for (int i = 0; i < count; i++) {
            shop.addPhone(readPhone());
        }

        shops.add(shop);

        System.out.println("Shop added.");
    }

    private static void showShops() {
        ShopUtils.print(shops);
    }

    private static void deleteShop() {
        showShops();

        System.out.print("Enter the number of the shop to remove: ");
        int index = Integer.parseInt(scanner.nextLine());

        if (shops.remove(index - 1)) {
            System.out.println("Shop removed.");
        } else {
            System.out.println("Invalid shop number.");
        }
    }

    private static boolean isNonFood(Shop shop) {
        String specialization =
            shop.getSpecialization().toLowerCase();

        return !specialization.contains("продукт")
            && !specialization.contains("продоволь")
            && !specialization.contains("food")
            && !specialization.contains("grocery")
            && !specialization.contains("supermarket");
    }

    private static boolean worksWithoutDaysOff(
            Shop shop, int requiredDays) {

        String[] hours = shop.getWorkingHours();

        if (hours.length != 7) {
            return false;
        }

        int workingDays = 0;

        for (String day : hours) {
            if (day != null &&
                HOURS_PATTERN.matcher(day).matches()) {
                workingDays++;
            }
        }

        return workingDays >= requiredDays;
    }

    private static boolean hasMobilePhone(
            Shop shop, String operatorPattern) {

        for (String phone : shop.getPhones()) {
            String normalized = phone.replaceAll(
                "[\\s()\\-]", ""
            );

            if (Pattern.matches(
                    operatorPattern, normalized)) {
                return true;
            }
        }

        return false;
    }

    private static String getOperatorPattern(int choice) {

        String prefix;

        switch (choice) {
            case 1:
                prefix = "67|68|96|97|98";
                break;

            case 2:
                prefix = "50|66|95|99";
                break;

            case 3:
                prefix = "63|73|93";
                break;

            default:
                prefix =
                    "39|50|63|66|67|68|73|" +
                    "91|92|93|94|95|96|97|98|99";
        }

        return "^(?:\\+380|380|0)(?:" +
            prefix + ")\\d{7}$";
    }

    private static void searchVariant19() {

        System.out.println("\nSEARCH CRITERIA");

        System.out.println("1. Open all 7 days");
        System.out.println("2. Open at least 6 days");

        System.out.print("Choose an option: ");
        int requiredDays = Integer.parseInt(
            scanner.nextLine()
        );

        requiredDays = requiredDays == 2 ? 6 : 7;

        System.out.println("\nSelect a mobile carrier:");
        System.out.println("0. Any Ukrainian carrier");
        System.out.println("1. Kyivstar");
        System.out.println("2. Vodafone");
        System.out.println("3. lifecell");

        System.out.print("Choose an option: ");
        int operator = Integer.parseInt(
            scanner.nextLine()
        );

        if (operator < 0 || operator > 3) {
            System.out.println("Invalid selection.");
            return;
        }

        String pattern = getOperatorPattern(operator);

        System.out.println("\nSEARCH RESULTS");

        boolean found = false;

        for (Shop shop : shops) {

            boolean nonFood = isNonFood(shop);

            boolean workingDays =
                worksWithoutDaysOff(shop, requiredDays);

            boolean mobile =
                hasMobilePhone(shop, pattern);

            if (nonFood && workingDays && mobile) {
                System.out.println(shop);
                found = true;
            }
        }

        if (!found) {
            System.out.println("No shops found.");
        }
    }

    private static void save() {
        try (java.io.ObjectOutputStream output =
                 new java.io.ObjectOutputStream(
                     new java.io.FileOutputStream("shops.dat"))) {

            output.writeObject(shops);
            System.out.println("Data saved.");

        } catch (java.io.IOException e) {
            System.out.println("Unable to save data: " + e.getMessage());
        }
    }

    @SuppressWarnings("unchecked")
    private static void load() {
        try (java.io.ObjectInputStream input =
                 new java.io.ObjectInputStream(
                     new java.io.FileInputStream("shops.dat"))) {

            Object object = input.readObject();

            if (object instanceof LinkedListContainer<?>) {
                shops = (LinkedListContainer<Shop>) object;
                System.out.println("Data loaded.");
            }

        } catch (java.io.IOException |
                 ClassNotFoundException e) {
            System.out.println("Unable to load data: " + e.getMessage());
        }
    }

    private static void automaticMode() {

        shops = new LinkedListContainer<>();

        Shop first = new Shop(
            "Home and Build",
            "10 Main Street, Kyiv",
            "Home improvement",
            new String[] {
                "09:00-21:00", "09:00-21:00",
                "09:00-21:00", "09:00-21:00",
                "09:00-21:00", "09:00-21:00",
                "09:00-21:00"
            }
        );

        first.addPhone("+380671234567");
        shops.add(first);

        Shop second = new Shop(
            "City Books",
            "25 Shevchenko Street, Lviv",
            "Books",
            new String[] {
                "09:00-18:00", "09:00-18:00",
                "09:00-18:00", "09:00-18:00",
                "09:00-18:00", "Day off",
                "Day off"
            }
        );

        second.addPhone("+380931234567");
        shops.add(second);

        Shop third = new Shop(
            "Fresh Market",
            "5 Central Avenue, Kyiv",
            "Grocery store",
            new String[] {
                "08:00-22:00", "08:00-22:00",
                "08:00-22:00", "08:00-22:00",
                "08:00-22:00", "08:00-22:00",
                "08:00-22:00"
            }
        );

        third.addPhone("+380501234567");
        shops.add(third);

        System.out.println("AUTOMATIC MODE");

        System.out.println("\nFull catalog:");
        showShops();

        System.out.println("\nSearch results (open all 7 days, any carrier):");

        for (Shop shop : shops) {
            if (isNonFood(shop)
                && worksWithoutDaysOff(shop, 7)
                && hasMobilePhone(
                    shop, getOperatorPattern(0))) {

                System.out.println(shop);
            }
        }

        System.out.println("\nSave and load:");
        save();
        load();
    }

    public static void main(String[] args) {

        if (args.length > 0 && args[0].equals("-auto")) {
            automaticMode();
            return;
        }

        while (true) {

            System.out.println("\n========== SHOP CATALOG ==========");
            System.out.println("1. Add a shop");
            System.out.println("2. View the catalog");
            System.out.println("3. Remove a shop");
            System.out.println("4. Search by criteria");
            System.out.println("5. Save");
            System.out.println("6. Load");
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
                        searchVariant19();
                        break;
                    case 5:
                        save();
                        break;
                    case 6:
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
