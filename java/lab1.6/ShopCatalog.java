
import java.io.Serializable;
import java.util.ArrayList;
import java.util.List;

public class ShopCatalog implements Serializable {

    private static final long serialVersionUID = 1L;

    private ArrayList<Shop> shops;

    public ShopCatalog() {
        shops = new ArrayList<>();
    }

    public void addShop(Shop shop) {
        shops.add(shop);
    }

    public boolean removeShop(int index) {
        if (index < 0 || index >= shops.size()) {
            return false;
        }

        shops.remove(index);
        return true;
    }

    public boolean updateShop(int index, Shop shop) {
        if (index < 0 || index >= shops.size()) {
            return false;
        }

        shops.set(index, shop);
        return true;
    }

    public Shop getShop(int index) {
        if (index < 0 || index >= shops.size()) {
            return null;
        }

        return shops.get(index);
    }

    public List<Shop> getShops() {
        return new ArrayList<>(shops);
    }

    public int size() {
        return shops.size();
    }

    public boolean isEmpty() {
        return shops.isEmpty();
    }

    public void clear() {
        shops.clear();
    }
}
