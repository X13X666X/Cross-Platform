
import java.util.ArrayList;
import java.util.List;

public class Shop {
    private String name;
    private String address;
    private List<String> phones;
    private String specialization;
    private String workingHours;

    public Shop(String name, String address,
                String specialization, String workingHours) {
        this.name = name;
        this.address = address;
        this.specialization = specialization;
        this.workingHours = workingHours;
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

    public List<String> getPhones() {
        return new ArrayList<>(phones);
    }

    public String getSpecialization() {
        return specialization;
    }

    public String getWorkingHours() {
        return workingHours;
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
