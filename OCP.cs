using System;
using System.Collections.Generic;
using System.Linq;

namespace IMJunior
{
    internal static class Program
    {
        private static void Main()
        {
            var paymentSystems = new List<PaymentSystem>
            {
                new Qiwi(),
                new WebMoney(),
                new Card()
            };

            var orderForm = new OrderForm(paymentSystems);

            orderForm.Show();
            bool paymentSystemSelected = orderForm.TrySelectPaymentSystem(out PaymentSystem paymentSystem);

            if (paymentSystemSelected == false)
            {
                Console.WriteLine("Выбранной системы не существует!");
                Console.ReadKey();
                return;
            }

            paymentSystem.MakePayment();
            paymentSystem.ShowPaymentResult();

            Console.ReadKey();
        }
    }

    internal abstract class PaymentSystem
    {
        public abstract string Id { get; }

        public abstract void MakePayment();

        public void ShowPaymentResult()
        {
            Console.WriteLine($"Вы оплатили с помощью {Id}");
            Console.WriteLine($"Проверка платежа через {Id}...");
            Console.WriteLine("Оплата прошла успешно!");
        }

        public override string ToString()
        {
            return Id;
        }
    }

    internal sealed class Qiwi : PaymentSystem
    {
        public override string Id => "QIWI";

        public override void MakePayment()
        {
            Console.WriteLine($"Перевод на страницу {Id}...");
        }
    }

    internal sealed class WebMoney : PaymentSystem
    {
        public override string Id => "WebMoney";

        public override void MakePayment()
        {
            Console.WriteLine($"Вызов API {Id}...");
        }
    }

    internal sealed class Card : PaymentSystem
    {
        public override string Id => "Card";

        public override void MakePayment()
        {
            Console.WriteLine($"Вызов API банка эмитера карты {Id}...");
        }
    }

    internal sealed class OrderForm
    {
        private readonly List<PaymentSystem> _paymentSystems;

        public OrderForm(List<PaymentSystem> paymentSystems)
        {
            if (paymentSystems == null)
            {
                throw new ArgumentNullException(nameof(paymentSystems));
            }

            if (paymentSystems.Count < 1)
            {
                throw new ArgumentException(nameof(paymentSystems));
            }

            _paymentSystems = paymentSystems;
        }

        public void Show()
        {
            Console.Write("Мы принимаем: ");

            for (var i = 0; i < _paymentSystems.Count; i++)
            {
                Console.Write(_paymentSystems[i]);

                if (i != _paymentSystems.Count - 1)
                {
                    Console.Write(", ");
                }
            }

            Console.WriteLine(string.Empty);
        }

        public bool TrySelectPaymentSystem(out PaymentSystem paymentSystem)
        {
            // Симуляция веб интерфейса
            Console.Write("Какое системой вы хотите совершить оплату: ");
            string systemId = Console.ReadLine();

            paymentSystem = _paymentSystems.FirstOrDefault(system => system.Id == systemId);
            return paymentSystem != null;
        }
    }
}