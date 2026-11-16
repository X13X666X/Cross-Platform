
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Comparator;
import java.util.List;
import java.util.function.Predicate;

public class ShopUtils {

    public static <T> LinkedListContainer<T> sort(
            LinkedListContainer<T> container,
            Comparator<? super T> comparator) {

        Object[] array = new Object[container.size()];

        int index = 0;

        for (T element : container) {
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

    public static <T> List<T> search(
            LinkedListContainer<T> container,
            Predicate<T> condition) {

        List<T> result = new ArrayList<>();

        for (T element : container) {
            if (condition.test(element)) {
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

        for (T element : container) {
            System.out.println(element);
        }
    }
}
