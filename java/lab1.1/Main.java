
import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;
import java.util.Locale;
import java.util.Scanner;

public class Main {

    private static List<Toy> readToys(String fileName) {
        List<Toy> toys = new ArrayList<>();

        try (BufferedReader reader =
                 new BufferedReader(new FileReader(fileName))) {

            String line;

            while ((line = reader.readLine()) != null) {
                if (line.trim().isEmpty()) {
                    continue;
                }

                String[] parts = line.split(";");

                if (parts.length != 6) {
                    System.out.println(
                        "Invalid row: " + line
                    );
                    continue;
                }

                try {
                    String name = parts[0].trim();
                    String type = parts[1].trim();
                    int priceKop = Integer.parseInt(parts[2].trim());
                    int minAge = Integer.parseInt(parts[3].trim());
                    int maxAge = Integer.parseInt(parts[4].trim());
                    int extra = Integer.parseInt(parts[5].trim());

                    toys.add(new Toy(
                        name, type, priceKop,
                        minAge, maxAge, extra
                    ));

                } catch (NumberFormatException e) {
                    System.out.println(
                        "Invalid numeric data: " + line
                    );
                }
            }

        } catch (IOException e) {
            System.out.println(
                "Error reading file: " + e.getMessage()
            );
        }

        return toys;
    }

    private static List<Toy> findSuitableToys(
            List<Toy> toys, int maxPriceKop) {

        List<Toy> result = new ArrayList<>();

        for (Toy toy : toys) {
            if (toy.getType().equalsIgnoreCase("blocks")
                    && toy.isSuitableForAge(2)
                    && toy.getPriceKop() >= 2000
                    && toy.getPriceKop() <= maxPriceKop) {

                result.add(toy);
            }
        }

        result.sort(
            Comparator.comparingInt(Toy::getPriceKop)
                      .reversed()
        );

        return result;
    }

    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);

        List<Toy> toys = readToys("toys.txt");

        System.out.println("TOY CATALOG");
        System.out.println("Toys loaded: " + toys.size());

        System.out.print(
            "Enter the maximum price in UAH: "
        );

        scanner.useLocale(Locale.US);
        double maxPrice = scanner.nextDouble();

        if (!Double.isFinite(maxPrice) || maxPrice < 0
                || maxPrice > Integer.MAX_VALUE / 100.0) {
            System.out.println("Invalid amount.");
            scanner.close();
            return;
        }

        int maxPriceKop = (int) Math.round(maxPrice * 100);

        List<Toy> result = findSuitableToys(
            toys, maxPriceKop
        );

        System.out.println(
            "\nBLOCKS FOR 2-YEAR-OLD CHILDREN"
        );
        System.out.println(
            "Price: from UAH 20 to "
            + String.format(Locale.US, "%.2f", maxPrice)
            + " UAH"
        );
        System.out.println(
            "Sorted by descending price\n"
        );

        if (result.isEmpty()) {
            System.out.println(
                "No toys match the specified criteria."
            );
        } else {
            for (Toy toy : result) {
                System.out.println(toy);
            }
        }

        scanner.close();
    }
}
