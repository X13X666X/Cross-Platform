
import java.io.Serializable;

public class Shop implements Serializable {

    private static final long serialVersionUID = 1L;

    private String name;
    private String address;
    private String specialization;
    private String[] phones;
    private String[] workingHours;

    public Shop(String name, String address,
                String specialization, String[] workingHours) {

        this.name = name;
        this.address = address;
        this.specialization = specialization;
        this.workingHours = workingHours.clone();
        this.phones = new String[0];
    }

    public void addPhone(String phone) {
        String[] newPhones = new String[phones.length + 1];

        System.arraycopy(
            phones, 0, newPhones, 0, phones.length
        );

        newPhones[phones.length] = phone;
        phones = newPhones;
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

    public String[] getPhones() {
        return phones.clone();
    }

    public String[] getWorkingHours() {
        return workingHours.clone();
    }

    @Override
    public String toString() {
        StringBuilder result = new StringBuilder();

        result.append("\nName: ").append(name);
        result.append("\nAddress: ").append(address);
        result.append("\nSpecialization: ").append(specialization);
        result.append("\nPhone numbers: ");

        for (String phone : phones) {
            result.append(phone).append(" ");
        }

        result.append("\nWorking hours:\n");

        String[] days = {
            "Monday", "Tuesday", "Wednesday", "Thursday",
            "Friday", "Saturday", "Sunday"
        };

        for (int i = 0; i < workingHours.length; i++) {
            result.append(days[i])
                  .append(": ")
                  .append(workingHours[i])
                  .append("\n");
        }

        return result.toString();
    }
}
