
import java.io.Serializable;
import java.util.Iterator;
import java.util.NoSuchElementException;

public class ShopContainer
        implements Iterable<Shop>, Serializable {

    private static final long serialVersionUID = 1L;

    private Shop[] shops;
    private int size;

    public ShopContainer() {
        shops = new Shop[10];
        size = 0;
    }

    public void add(Shop shop) {
        if (size == shops.length) {
            Shop[] newArray = new Shop[shops.length * 2];

            System.arraycopy(
                shops, 0, newArray, 0, size
            );

            shops = newArray;
        }

        shops[size++] = shop;
    }

    public boolean remove(int index) {
        if (index < 0 || index >= size) {
            return false;
        }

        System.arraycopy(
            shops, index + 1,
            shops, index,
            size - index - 1
        );

        shops[--size] = null;

        return true;
    }

    public boolean update(int index, Shop shop) {
        if (index < 0 || index >= size) {
            return false;
        }

        shops[index] = shop;
        return true;
    }

    public Shop get(int index) {
        if (index < 0 || index >= size) {
            throw new IndexOutOfBoundsException();
        }

        return shops[index];
    }

    public int size() {
        return size;
    }

    public boolean isEmpty() {
        return size == 0;
    }

    @Override
    public Iterator<Shop> iterator() {
        return new Iterator<Shop>() {

            private int index = 0;

            @Override
            public boolean hasNext() {
                return index < size;
            }

            @Override
            public Shop next() {
                if (!hasNext()) {
                    throw new NoSuchElementException();
                }

                return shops[index++];
            }
        };
    }
}
