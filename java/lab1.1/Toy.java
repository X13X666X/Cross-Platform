
public class Toy {
    private String name;
    private String type;
    private int priceKop;
    private int minAge;
    private int maxAge;
    private int extra;

    public Toy(String name, String type, int priceKop,
               int minAge, int maxAge, int extra) {
        this.name = name;
        this.type = type;
        this.priceKop = priceKop;
        this.minAge = minAge;
        this.maxAge = maxAge;
        this.extra = extra;
    }

    public String getName() {
        return name;
    }

    public String getType() {
        return type;
    }

    public int getPriceKop() {
        return priceKop;
    }

    public int getMinAge() {
        return minAge;
    }

    public int getMaxAge() {
        return maxAge;
    }

    public int getExtra() {
        return extra;
    }

    public boolean isSuitableForAge(int age) {
        return age >= minAge && age <= maxAge;
    }

    public String getExtraInfo() {
        if (type.equalsIgnoreCase("blocks")) {
            return "Number of blocks: " + extra;
        }

        if (type.equalsIgnoreCase("doll")) {
            return "Size: " + extra + " cm";
        }

        if (type.equalsIgnoreCase("ball")) {
            return "Weight: " + extra + " g";
        }

        if (type.equalsIgnoreCase("construction set")) {
            return "Number of pieces: " + extra;
        }

        return "";
    }

    @Override
    public String toString() {
        return String.format(
            "%-20s | Price: UAH %d.%02d | %s",
            name,
            priceKop / 100,
            priceKop % 100,
            getExtraInfo()
        );
    }
}
