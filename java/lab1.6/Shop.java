
import java.io.Serializable;
import java.util.LinkedHashSet;
import java.util.LinkedHashMap;
import java.util.Map;
import java.util.Set;
import java.util.regex.Pattern;

public class Shop implements Serializable {

    private static final long serialVersionUID = 1L;

    private String name;
    private String address;
    private Set<String> phones;
    private String specialization;
    private Map<String, String> workingHours;

    private static final String[] DAYS = {
        "Monday", "Tuesday", "Wednesday",
        "Thursday", "Friday", "Saturday", "Sunday"
    };

    private static final Pattern PHONE_PATTERN =
        Pattern.compile(
            "^(?:\\+380|380|0)(?:39|50|63|66|67|68|" +
            "73|91|92|93|94|95|96|97|98|99)\\d{7}$"
        );

    public Shop(String name, String address,
                String specialization,
                Map<String, String> workingHours) {

        this.name = name;
        this.address = address;
        this.specialization = specialization;

        this.phones = new LinkedHashSet<>();

        this.workingHours =
            new LinkedHashMap<>(workingHours);
    }

    public void addPhone(String phone) {
        String normalized =
            phone.replaceAll("[\\s()\\-]", "");

        if (PHONE_PATTERN.matcher(normalized).matches()) {
            phones.add(normalized);
        } else {
            System.out.println(
                "Invalid phone number: " + phone
            );
        }
    }

    public boolean removePhone(String phone) {
        return phones.remove(
            phone.replaceAll("[\\s()\\-]", "")
        );
    }

    public String getName() {
        return name;
    }

    public String getAddress() {
        return address;
    }

    public String getSpecialization() {
        return specialization;
    }

    public Set<String> getPhones() {
        return new LinkedHashSet<>(phones);
    }

    public Map<String, String> getWorkingHours() {
        return new LinkedHashMap<>(workingHours);
    }

    public boolean isNonFood() {
        String value = specialization.toLowerCase();

        if (value.contains("непродоволь")
                || value.contains("непродукт")
                || value.contains("non-food")) {
            return true;
        }

        return !value.contains("продоволь")
            && !value.contains("продукт")
            && !value.contains("food")
            && !value.contains("grocery");
    }

    public boolean worksEveryDayWithoutLunchBreak() {

        if (workingHours.size() != 7) {
            return false;
        }

        Pattern hoursPattern = Pattern.compile(
            "^([01]\\d|2[0-3]):[0-5]\\d-" +
            "([01]\\d|2[0-3]):[0-5]\\d$"
        );

        for (String day : DAYS) {
            String hours = workingHours.get(day);

            if (hours == null ||
                !hoursPattern.matcher(hours).matches()) {
                return false;
            }
        }

        return true;
    }

    public boolean hasMobilePhone() {
        return !phones.isEmpty();
    }

    public boolean matchesVariant19() {
        return isNonFood()
            && worksEveryDayWithoutLunchBreak()
            && hasMobilePhone();
    }

    @Override
    public String toString() {

        return "\nName: " + name +
               "\nAddress: " + address +
               "\nPhone numbers: " + phones +
               "\nSpecialization: " + specialization +
               "\nWorking hours: " + workingHours +
               "\n-----------------------------";
    }
}
