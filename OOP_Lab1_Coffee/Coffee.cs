using System;

namespace CoffeeApp
{
      public class Coffee
    {
        private string orderCode;

        public string name;            
        public CoffeeType type;        
        public double price;          
        public DateTime roastDate;     
        public bool isDecaf;           
        public int volumeMl;           
        public int temperatureC;      

        public Coffee(string orderCode, string name, CoffeeType type, double price,
                       DateTime roastDate, bool isDecaf, int volumeMl)
        {
            this.orderCode = orderCode;
            this.name = name;
            this.type = type;
            this.price = price;
            this.roastDate = roastDate;
            this.isDecaf = isDecaf;
            this.volumeMl = volumeMl;
            this.temperatureC = 75; 
        }

        public string GetOrderCode()
        {
            return orderCode;
        }

        public void Reheat(int degrees)
        {
            temperatureC += degrees;
            if (temperatureC > 95) temperatureC = 95;
            Console.WriteLine($"  -> {name} підігріто. Поточна температура: {temperatureC}°C");
        }

        public void Cool(int degrees)
        {
            temperatureC -= degrees;
            if (temperatureC < 20) temperatureC = 20;
            Console.WriteLine($"  -> {name} охолоджено. Поточна температура: {temperatureC}°C");
        }

        public void ApplyDiscount(double percent)
        {
            double oldPrice = price;
            price -= price * percent / 100;
            Console.WriteLine($"  -> Знижка {percent}% застосована. Ціна: {oldPrice:F2} -> {price:F2} грн");
        }

        public void AddVolume(int ml)
        {
            volumeMl += ml;
            if (volumeMl > 1000) volumeMl = 1000;
            Console.WriteLine($"  -> Додано {ml} мл. Новий об'єм: {volumeMl} мл");
        }

        public override string ToString()
        {
            return $"{orderCode,-8}{name,-20}{type,-12}{price,-10:F2}{roastDate,-14:dd.MM.yyyy}" +
                   $"{(isDecaf ? "Так" : "Ні"),-7}{volumeMl,-8}{temperatureC}";
        }
    }
}