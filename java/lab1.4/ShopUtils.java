
import java.util.Arrays;
import java.util.Comparator;

public class ShopUtils {

    public static <T> LinkedListContainer<T> sort(
            LinkedListContainer<T> source,
            Comparator<T> comparator) {

        Object[] array = new Object[source.size()];

        int index = 0;

        for (T element : source) {
            array[index++] = element;
        }

        Arrays.sort(array, (a, b) ->
            comparator.compare((T) a, (T) b)
        );

        LinkedListContainer<T> result =
            new LinkedListContainer<>();

        for (Object element : array) {
            result.add((T) element);
        }

        return result;
    }

    public static <T> LinkedListContainer<T> search(
            LinkedListContainer<T> source,
            Condition<T> condition) {

        LinkedListContainer<T> result =
            new LinkedListContainer<>();

        for (T element : source) {
            if (condition.check(element)) {
                result.add(element);
            }
        }

        return result;
    }

    public static <T> void print(
            LinkedListContainer<T> container) {

        if (container.isEmpty()) {
            System.out.println("The catalog is empty.");
            return;
        }

        int index = 1;

        for (T element : container) {
            System.out.println("\n" + index++);
            System.out.println(element);
        }
    }

    @FunctionalInterface
    public interface Condition<T> {
        boolean check(T element);
    }
}
