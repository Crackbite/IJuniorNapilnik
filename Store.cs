using System;
using System.Collections.Generic;
using System.Linq;

namespace IJuniorNapilnik
{
    internal static class Store
    {
        internal static void UseCase()
        {
            Good iPhone12 = new Good("IPhone 12");
            Good iPhone11 = new Good("IPhone 11");

            Warehouse warehouse = new Warehouse();

            Shop shop = new Shop(warehouse);

            warehouse.Delive(iPhone12, 10);
            warehouse.Delive(iPhone11, 1);

            warehouse.ShowStockBalances(); // Вывод всех товаров на складе с их остатком

            Cart cart = shop.Cart();
            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 3); // При такой ситуации возникает ошибка так, как нет нужного количества товара на складе

            cart.Show(); // Вывод всех товаров в корзине

            Console.WriteLine(cart.Order().Paylink);

            cart.Add(iPhone12, 9); // Ошибка, после заказа со склада убираются заказанные товары
        }
    }

    internal static class StorageManipulatorEx
    {
        public static void Append(this Dictionary<Good, int> goodsStorage, Good good, int amount)
        {
            if (good == null)
            {
                throw new ArgumentNullException(nameof(good));
            }

            if (amount < 1)
            {
                throw new ArgumentException("Количество товаров должно быть больше нуля", nameof(amount));
            }

            if (goodsStorage.TryGetValue(good, out int currentAmount))
            {
                goodsStorage[good] = currentAmount + amount;
            }
            else
            {
                goodsStorage.Add(good, amount);
            }
        }

        public static bool Contains(this Dictionary<Good, int> goodsStorage, Good good, int amount)
        {
            if (good == null)
            {
                throw new ArgumentNullException(nameof(good));
            }

            return goodsStorage.Any(pair => pair.Key == good && pair.Value >= amount && amount > 0);
        }

        public static void Remove(this Dictionary<Good, int> goodsStorage, Good good, int amount)
        {
            if (goodsStorage.Contains(good, amount) == false)
            {
                throw new ArgumentException("Отсутствует указанное количество товара");
            }

            int newAmount = goodsStorage[good] - amount;

            if (newAmount == 0)
            {
                goodsStorage.Remove(good);
            }
            else
            {
                goodsStorage[good] = newAmount;
            }
        }

        public static void Show(this Dictionary<Good, int> goodsStorage)
        {
            if (goodsStorage.Count < 1)
            {
                Console.WriteLine("Товары отсутствуют");
            }

            foreach (KeyValuePair<Good, int> goodAmountPair in goodsStorage)
            {
                Console.WriteLine($"Товар: {goodAmountPair.Key.Name}, количество: {goodAmountPair.Value}");
            }
        }
    }

    internal class Good
    {
        public Good(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
        }

        public string Name { get; }
    }

    internal class Warehouse
    {
        private readonly Dictionary<Good, int> _goodsStorage = new Dictionary<Good, int>();

        public bool Contains(Good good, int amount)
        {
            return _goodsStorage.Contains(good, amount);
        }

        public void Delive(Good good, int amount)
        {
            _goodsStorage.Append(good, amount);
        }

        public void Remove(Good good, int amount)
        {
            _goodsStorage.Remove(good, amount);
        }

        public void ShowStockBalances()
        {
            _goodsStorage.Show();
        }
    }

    internal class Shop
    {
        private readonly Warehouse _warehouse;

        public Shop(Warehouse warehouse)
        {
            _warehouse = warehouse ?? throw new ArgumentNullException(nameof(warehouse));
        }

        public Cart Cart()
        {
            return new Cart(_warehouse);
        }
    }

    internal class Cart
    {
        private readonly Dictionary<Good, int> _goodsStorage = new Dictionary<Good, int>();

        private readonly Warehouse _warehouse;

        public Cart(Warehouse warehouse)
        {
            _warehouse = warehouse ?? throw new ArgumentNullException(nameof(warehouse));
        }

        public int PositionsNumber => _goodsStorage.Count;

        public void Add(Good good, int amount)
        {
            if (good == null)
            {
                throw new ArgumentNullException(nameof(good));
            }

            int checkAmount = amount;

            if (_goodsStorage.TryGetValue(good, out int currentAmount))
            {
                checkAmount = currentAmount + amount;
            }

            if (_warehouse.Contains(good, checkAmount) == false)
            {
                throw new ArgumentException("На складе отсутствует указанное количество товара", nameof(amount));
            }

            _goodsStorage.Append(good, amount);
        }

        public KeyValuePair<Good, int> GetPositionAt(int index)
        {
            return _goodsStorage.ElementAt(index);
        }

        public Order Order()
        {
            return new Order(_warehouse, this);
        }

        public void Remove(Good good, int amount)
        {
            _goodsStorage.Remove(good, amount);
        }

        public void Show()
        {
            _goodsStorage.Show();
        }
    }

    internal class Order
    {
        public Order(Warehouse warehouse, Cart cart)
        {
            if (warehouse == null)
            {
                throw new ArgumentNullException(nameof(warehouse));
            }

            if (cart == null)
            {
                throw new ArgumentNullException(nameof(cart));
            }

            var random = new Random();
            int pseudoHash = random.Next(100000, 999999);

            Paylink = $"https://site.ru/pay?hash={pseudoHash}";

            for (int i = cart.PositionsNumber - 1; i >= 0; i--)
            {
                KeyValuePair<Good, int> goodAmountPair = cart.GetPositionAt(i);

                warehouse.Remove(goodAmountPair.Key, goodAmountPair.Value);
                cart.Remove(goodAmountPair.Key, goodAmountPair.Value);
            }
        }

        public string Paylink { get; }
    }
}