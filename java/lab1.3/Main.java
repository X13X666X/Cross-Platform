
import java.io.*;
import java.util.Scanner;
import java.io.File;

public class Main {

    private static final Scanner scanner =
        new Scanner(System.in);

    private static ShopContainer container =
        new ShopContainer();

    private static void addShop() {
        System.out.println("\nADD SHOP");

        System.out.print("Name: ");
        String name = scanner.nextLine();

        System.out.print("Address: ");
        String address = scanner.nextLine();

        System.out.print("Specialization: ");
        String specialization = scanner.nextLine();

        String[] hours = new String[7];

        String[] days = {
            "Monday", "Tuesday", "Wednesday",
            "Thursday", "Friday", "Saturday", "Sunday"
        };

        for (int i = 0; i < 7; i++) {
            System.out.print(
                days[i] + " (for example 09:00-20:00): "
            );

            hours[i] = scanner.nextLine();
        }

        Shop shop = new Shop(
            name, address, specialization, hours
        );

        System.out.print("Number of phones: ");
        int count = Integer.parseInt(scanner.nextLine());

        for (int i = 0; i < count; i++) {
            System.out.print("Phone " + (i + 1) + ": ");
            shop.addPhone(scanner.nextLine());
        }

        container.add(shop);

        System.out.println("Shop added.");
    }

    private static void showShops() {
        System.out.println("\nSHOP CATALOG");

        if (container.isEmpty()) {
            System.out.println("The catalog is empty.");
            return;
        }

        int index = 1;

        for (Shop shop : container) {
            System.out.println("\nNumber: " + index++);
            System.out.println(shop);
        }
    }

    private static void deleteShop() {
        showShops();

        System.out.print("Shop number to delete: ");
        int index = Integer.parseInt(scanner.nextLine());

        if (container.remove(index - 1)) {
            System.out.println("Shop deleted.");
        } else {
            System.out.println("Invalid number.");
        }
    }

    private static void editShop() {
        showShops();

        System.out.print("Shop number to edit: ");
        int index = Integer.parseInt(scanner.nextLine()) - 1;

        if (index < 0 || index >= container.size()) {
            System.out.println("Invalid number.");
            return;
        }

        Shop oldShop = container.get(index);

        System.out.print("New name: ");
        String name = scanner.nextLine();

        System.out.print("New address: ");
        String address = scanner.nextLine();

        System.out.print("New specialization: ");
        String specialization = scanner.nextLine();

        System.out.print("New number of phones: ");
        int count = Integer.parseInt(scanner.nextLine());

        Shop newShop = new Shop(
            name.isEmpty() ? oldShop.getName() : name,
            address.isEmpty() ? oldShop.getAddress() : address,
            specialization.isEmpty()
                ? oldShop.getSpecialization() : specialization,
            new String[] {
                "09:00-18:00", "09:00-18:00",
                "09:00-18:00", "09:00-18:00",
                "09:00-18:00", "10:00-16:00",
                "Day off"
            }
        );

        if (count == 0) {
            for (String phone : oldShop.getPhones()) {
                newShop.addPhone(phone);
            }
        } else {
            for (int i = 0; i < count; i++) {
                System.out.print("Phone " + (i + 1) + ": ");
                newShop.addPhone(scanner.nextLine());
            }
        }

        container.update(index, newShop);

        System.out.println("Shop data updated.");
        System.out.println(
            "Note: working hours were reset to the default schedule."
        );
    }

    private static void searchVariant19() {
        System.out.println(
            "\nSHOPS MATCHING VARIANT 19"
        );

        boolean found = false;

        for (Shop shop : container) {
            if (shop.matchesVariant19()) {
                System.out.println(shop);
                found = true;
            }
        }

        if (!found) {
            System.out.println(
                "No matching shops were found."
            );
        }
    }

    private static File navigateDirectory() {
        File current = new File(
            System.getProperty("user.dir")
        );

        while (true) {
            System.out.println("\nCURRENT DIRECTORY:");
            System.out.println(current.getAbsolutePath());

            File[] files = current.listFiles();

            if (files != null) {
                for (int i = 0; i < files.length; i++) {
                    String type = files[i].isDirectory()
                        ? "[DIRECTORY]" : "[FILE]";

                    System.out.println(
                        i + ". " + type + " " + files[i].getName()
                    );
                }
            }

            System.out.println("\n-1. Select this directory");
            System.out.println("-2. Go back");
            System.out.println("-3. Enter a different path");

            System.out.print("Your choice: ");

            try {
                int choice = Integer.parseInt(scanner.nextLine());

                if (choice == -1) {
                    return current;
                }

                if (choice == -2) {
                    current = current.getParentFile();

                    if (current == null) {
                        current = new File(
                            System.getProperty("user.dir")
                        );
                    }

                    continue;
                }

                if (choice == -3) {
                    System.out.print("Path: ");
                    File selected =
                        new File(scanner.nextLine());

                    if (selected.isDirectory()) {
                        current = selected;
                    } else {
                        System.out.println(
                            "This directory does not exist."
                        );
                    }

                    continue;
                }

                if (files != null &&
                    choice >= 0 && choice < files.length) {

                    if (files[choice].isDirectory()) {
                        current = files[choice];
                    } else {
                        System.out.println(
                            "Select a directory."
                        );
                    }
                }

            } catch (NumberFormatException e) {
                System.out.println("Enter a number.");
            }
        }
    }

    private static void saveData() {
        File directory = navigateDirectory();

        System.out.print("File name (for example shops.dat): ");
        String fileName = scanner.nextLine();

        File file = new File(directory, fileName);

        try (ObjectOutputStream output =
                 new ObjectOutputStream(
                     new FileOutputStream(file))) {

            output.writeObject(container);

            System.out.println(
                "Data saved: " + file.getAbsolutePath()
            );

        } catch (IOException e) {
            System.out.println(
                "Save error: " + e.getMessage()
            );
        }
    }

    private static void loadData() {
        File directory = navigateDirectory();

        System.out.print("File name to load: ");
        String fileName = scanner.nextLine();

        File file = new File(directory, fileName);

        if (!file.isFile()) {
            System.out.println("File not found.");
            return;
        }

        try (ObjectInputStream input =
                 new ObjectInputStream(
                     new FileInputStream(file))) {

            Object object = input.readObject();

            if (object instanceof ShopContainer) {
                container = (ShopContainer) object;

                System.out.println(
                    "Data restored. Number of shops: "
                    + container.size()
                );
            } else {
                System.out.println("Invalid file format.");
            }

        } catch (IOException | ClassNotFoundException e) {
            System.out.println(
                "Restore error: " + e.getMessage()
            );
        }
    }

    public static void main(String[] args) {

        while (true) {
            System.out.println(
                "\n========== SHOP CATALOG =========="
            );

            System.out.println("1. Add shop");
            System.out.println("2. View catalog");
            System.out.println("3. Delete shop");
            System.out.println("4. Edit shop");
            System.out.println("5. Search by variant 19");
            System.out.println("6. Save data");
            System.out.println("7. Restore data");
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
                        editShop();
                        break;
                    case 5:
                        searchVariant19();
                        break;
                    case 6:
                        saveData();
                        break;
                    case 7:
                        loadData();
                        break;
                    case 0:
                        scanner.close();
                        return;
                    default:
                        System.out.println("Invalid choice.");
                }

            } catch (NumberFormatException e) {
                System.out.println("Enter a valid number.");
            }
        }
    }
}
