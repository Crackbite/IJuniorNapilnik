using System;
using System.Security.Cryptography;
using System.Text;

namespace IJuniorNapilnik
{
    internal static class Program
    {
        public enum Currency
        {
            RUB
        }

        internal interface IPaymentSystem
        {
            public string GetPayingLink(Order order);
        }

        internal static void Main()
        {
            var order = new Order(1, 12000, Currency.RUB);

            IPaymentSystem paymentSystem = new PaymentSystem1();
            string payingLink = paymentSystem.GetPayingLink(order);
            Console.WriteLine($"1) {payingLink}");

            paymentSystem = new PaymentSystem2();
            payingLink = paymentSystem.GetPayingLink(order);
            Console.WriteLine($"2) {payingLink}");

            paymentSystem = new PaymentSystem3("secretKey");
            payingLink = paymentSystem.GetPayingLink(order);
            Console.WriteLine($"3) {payingLink}");

            Console.ReadKey();
        }

        internal static class Cryptography
        {
            public static string ComputeMd5Hash(string text)
            {
                if (string.IsNullOrEmpty(text))
                {
                    throw new ArgumentNullException(text);
                }

                using var md5 = MD5.Create();
                byte[] input = Encoding.ASCII.GetBytes(text);
                byte[] hash = md5.ComputeHash(input);

                return ComputedHashToString(hash);
            }

            public static string ComputeSha1Hash(string text)
            {
                if (string.IsNullOrEmpty(text))
                {
                    throw new ArgumentNullException(text);
                }

                using var sha1 = new SHA1Managed();
                byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(text));

                return ComputedHashToString(hash);
            }

            private static string ComputedHashToString(byte[] hash, string format = "X2")
            {
                var sb = new StringBuilder(hash.Length * 2);

                for (var i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString(format));
                }

                return sb.ToString().ToLower();
            }
        }

        internal class Order
        {
            public readonly int Amount;

            public readonly Currency Currency;

            public readonly int Id;

            public Order(int id, int amount, Currency currency)
            {
                if (id < 1)
                {
                    throw new ArgumentException("Идентификатор заказа должен быть больше 0", nameof(id));
                }

                if (amount <= 0)
                {
                    throw new ArgumentException("Сумма заказа должна быть больше 0", nameof(amount));
                }

                Id = id;
                Amount = amount;
                Currency = currency;
            }
        }

        internal class PaymentSystem1 : IPaymentSystem
        {
            public string GetPayingLink(Order order)
            {
                string hash = Cryptography.ComputeMd5Hash(order.Id.ToString());

                return $"pay.system1.ru/order?amount={order.Amount}{order.Currency}&hash={hash}";
            }
        }

        internal class PaymentSystem2 : IPaymentSystem
        {
            public string GetPayingLink(Order order)
            {
                string hash = Cryptography.ComputeMd5Hash($"{order.Id}{order.Amount}");

                return $"order.system2.ru/pay?hash={hash}";
            }
        }

        internal class PaymentSystem3 : IPaymentSystem
        {
            private readonly string _secretKey;

            public PaymentSystem3(string secretKey)
            {
                if (string.IsNullOrEmpty(secretKey))
                {
                    throw new ArgumentNullException(secretKey);
                }

                _secretKey = secretKey;
            }

            public string GetPayingLink(Order order)
            {
                string hash = Cryptography.ComputeSha1Hash($"{order.Amount}{order.Id}{_secretKey}");

                return $"system3.com/pay?amount={order.Amount}&curency={order.Currency}&hash={hash}";
            }
        }
    }
}