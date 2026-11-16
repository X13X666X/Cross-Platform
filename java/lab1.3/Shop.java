
import java.io.Serializable;
import java.util.ArrayList;
import java.util.List;

public class Shop implements Serializable {

    private static final long serialVersionUID = 1L;

    private String name;
    private String address;
    private List<String> phones;
    private String specialization;
    private String[] workingHours;

    private static final String[] DAYS = {
        "Monday", "Tuesday", "Wednesday",
        "Thursday", "Friday", "Saturday", "Sunday"
    };

    public Shop(String name, String address,
                String specialization, String[] workingHours) {

        this.name = name;
        this.address = address;
        this.specialization = specialization;
        this.workingHours = workingHours.clone();
        this.phones = new ArrayList<>();
    }

    public void addPhone(String phone) {
        if (phone != null && !phone.trim().isEmpty()) {
            phones.add(phone.trim());
        }
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

    public List<String> getPhones() {
        return new ArrayList<>(phones);
    }

    public void setName(String name) {
        this.name = name;
    }

    public void setAddress(String address) {
        this.address = address;
    }

    public void setSpecialization(String specialization) {
        this.specialization = specialization;
    }

    public void setWorkingHours(String[] hours) {
        if (hours.length != 7) {
            throw new IllegalArgumentException(
                "You must specify a schedule for 7 days."
            );
        }

        this.workingHours = hours.clone();
    }

    public boolean isNonFood() {
        return !specialization.toLowerCase()
                .contains("product");
    }

    public boolean worksEveryDayWithoutLunchBreak() {
        if (workingHours.length != 7) {
            return false;
        }

        for (String hours : workingHours) {
            if (hours == null ||
                !hours.matches(
                    "(?i)^\\s*\\d{2}:\\d{2}\\s*-\\s*\\d{2}:\\d{2}\\s*$"
                )) {
                return false;
            }
        }

        return true;
    }

    public boolean hasUkrainianMobilePhone() {
        String pattern =
            "^(?:\\+380|380|0)(?:39|50|63|66|67|68|" +
            "73|91|92|93|94|95|96|97|98|99)\\d{7}$";

        for (String phone : phones) {
            String normalized = phone.replaceAll("[\\s()\\-]", "");

            if (normalized.matches(pattern)) {
                return true;
            }
        }

        return false;
    }

    public boolean matchesVariant19() {
        return isNonFood()
            && worksEveryDayWithoutLunchBreak()
            && hasUkrainianMobilePhone();
    }

    @Override
    public String toString() {
        StringBuilder result = new StringBuilder();

        result.append("\nName: ").append(name);
        result.append("\nAddress: ").append(address);
        result.append("\nPhones: ").append(phones);
        result.append("\nSpecialization: ").append(specialization);
        result.append("\nWorking hours:\n");

        for (int i = 0; i < DAYS.length; i++) {
            result.append(DAYS[i])
                  .append(": ")
                  .append(workingHours[i])
                  .append("\n");
        }

        return result.toString();
    }
}
