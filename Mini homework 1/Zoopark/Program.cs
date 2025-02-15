using System;
using System.Xml;
using Microsoft.Extensions.DependencyInjection;

namespace Zoopark;

class Program
{
    static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<VetClinic>();
        serviceCollection.AddSingleton<Zoo>();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var random = new Random();
        var zoo = serviceProvider.GetRequiredService<Zoo>();
        var counters = new Dictionary<string, int>();
        counters["Monkey"] = 0;
        counters["Rabbit"] = 0;
        counters["Wolf"] = 0;
        counters["Tiger"] = 0;
        counters["Table"] = 1;
        counters["Computer"] = 1;
        zoo.AddThing(new Table(1));
        zoo.AddThing(new Computer(1));
        Console.Clear();
        Console.WriteLine("В зоопарке пустовато! Только стол и компьютер - надо бы его заполнить!");
        Console.WriteLine("Нажмите любую клавишу чтобы начать работу");
        Console.ReadKey();
        do
        {
            Console.Clear();
            Console.WriteLine("Вы хотите поселить в зоопарк животное или пополнить инвентарь зоопарка предметом?");
            Console.WriteLine("1. Поселить животное");
            Console.WriteLine("2. Пополнить инвентарь");
            if (ReadBoolAnswer())
            {
                Console.Clear();
                Console.WriteLine("Вы хотите заселить травоядное животное или хищное?");
                Console.WriteLine("1. Травоядное");
                Console.WriteLine("2. Хищное");
                if (ReadBoolAnswer())
                {
                    Console.Clear();
                    Console.WriteLine("Вы хотите заселить обезьяну или кролика?");
                    Console.WriteLine("1. Обезьяну");
                    Console.WriteLine("2. Кролика");
                    if (ReadBoolAnswer())
                    {
                        Console.Clear();
                        ReadInt("Оцените здоровье вашего животного от 1 до 100", out int healthy, 1, 100);
                        zoo.AddAnimal(new Monkey(++counters["Monkey"], healthy, random.Next(1, 11)));
                    }
                    else
                    {
                        Console.Clear();
                        ReadInt("Оцените здоровье вашего животного от 1 до 100", out int healthy, 1, 100);
                        zoo.AddAnimal(new Rabbit(++counters["Rabbit"], healthy, random.Next(1, 11)));
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Вы хотите заселить тигра или волка?");
                    Console.WriteLine("1. Тигра");
                    Console.WriteLine("2. Волка");
                    if (ReadBoolAnswer())
                    {
                        Console.Clear();
                        ReadInt("Оцените здоровье вашего животного от 1 до 100", out int healthy, 1, 100);
                        zoo.AddAnimal(new Tiger(++counters["Tiger"], healthy));
                    }
                    else
                    {
                        Console.Clear();
                        ReadInt("Оцените здоровье вашего животного от 1 до 100", out int healthy, 1, 100);
                        zoo.AddAnimal(new Wolf(++counters["Wolf"], healthy));
                    }
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Вы хотите добавить в инвентарь компьютер или стол?");
                Console.WriteLine("1. Компьютер");
                Console.WriteLine("2. Стол");
                if (ReadBoolAnswer())
                {
                    zoo.AddThing(new Computer(++counters["Computer"]));
                }
                else
                {
                    zoo.AddThing(new Table(++counters["Table"]));
                }
            }
            Console.WriteLine("Вы хотите вывести текущий отчет по зоопарку?");
            Console.WriteLine("1. Да");
            Console.WriteLine("2. Нет");
            if (ReadBoolAnswer())
            {
                zoo.PrintReport();
            }
            Console.WriteLine("Нажмите любую клавишу чтобы продолжить либо Escape чтобы завершить.");

        } while (Console.ReadKey().Key != ConsoleKey.Escape);
    }
    private static bool ReadBoolAnswer()
    {
        while (true)
        {
            var key = Console.ReadKey().Key;
            switch (key)
            {
                case ConsoleKey.D1:
                    return true;
                case ConsoleKey.D2:
                    return false;
            }
        }
    }

    private static void ReadInt(string text, out int value, int left, int right)
    {
        Console.Write(text);
        int ans = -1;
        while (!int.TryParse(Console.ReadLine(), out ans) && !(left <= ans && ans <= right))
        {
            Console.WriteLine("Вы вводите не то! Попробуйте еще раз.");
        }
        value = ans;

    }
}


public interface IAlive
{ 
    int Food{get;set;}
    int Healthy{get;set;}
}

public interface IInventory
{
    int Number{get;set;}
}


public abstract class Animal : IAlive, IInventory
{
    public int Food{get;set;}
    public int Number{get;set;}
    public string Name{get;set;}
    public int Healthy{get;set;}

    protected Animal(string name, int food, int number, int healthy)
    {
        Name = name;
        Food = food;
        Number = number;
        Healthy = healthy;
    }
}

public abstract class Herbo : Animal
{
    public int Kindness{get;set;}

    protected Herbo(string name, int food, int number, int healthy, int kindness) : base(name, food, number, healthy)
    {
        Kindness = kindness;
    }
}
public abstract class Predator(string name, int food, int number, int healthy) : Animal(name, food, number, healthy);

public class Monkey : Herbo
{
    public Monkey(int number, int healthy, int kindness) : base("Monkey", 5, number, healthy, kindness) { }
}

public class Rabbit : Herbo
{
    public Rabbit(int number, int healthy, int kindness) : base("Rabbit", 2, number, healthy, kindness) { }

}

public class Tiger : Predator
{
    public Tiger(int number, int healthy) : base("Tiger", 10, number, healthy) {}
}

public class Wolf : Predator
{
    public Wolf(int number, int healthy) : base("Wolf", 7, number, healthy) {}
}

public class Thing : IInventory
{
    public int Number{get;set;}
    public string Name { get; private set; }
    
    protected Thing(string name, int number) {
        Name = name;
        Number = number;
    }
}

public class Table : Thing
{
    public Table(int number) : base("Table", number) {}
}

public class Computer : Thing
{
    public Computer(int number) : base("Computer", number) {}
}

public class VetClinic
{
    public bool CheckHealth(Animal animal) {
        return animal.Healthy >= 35; 
    }
}

public class Zoo
{
    private readonly List<Animal> _animals = new();
    private readonly List<Thing> _inventory = new();
    private readonly VetClinic _clinic;

    public Zoo(VetClinic clinic)
    {
        _clinic = clinic;
    }
    
    public void AddAnimal(Animal animal) {
        
        if (_clinic.CheckHealth(animal)) {
            _animals.Add(animal);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Животное {animal.GetType()} с именем {animal.Name}{animal.Number} полностью здорово и принято в зоопарк!");
        } else {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Ветклиника с прискорбием сообщает, что животное {animal.GetType()} с именем {animal.Name}{animal.Number} не подходит по состоянию здоровье и не может быть принято в зоопарк, извините...");
        }
        Console.ResetColor();
    }

    public List<Animal> GetAnimals()
    {
        return _animals;
    }

    public List<Thing> GetInventory()
    {
        return _inventory;
    }
    public List<Herbo> GetContactZooAnimals()
    {
        return _animals.OfType<Herbo>().Where(a => a.Kindness > 5).ToList();
    }

    public void AddThing(Thing thing) {
        _inventory.Add(thing);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{thing.Name} под номером {thing.Number} успешно добавлен в инвентарь!");
        Console.ResetColor();
    }
    
    public void PrintReport() {
        Console.WriteLine("Отчет о животных в зоопарке:");
        Console.WriteLine("----------------------------------");
        foreach (var animal in _animals) {
            Console.WriteLine($"Животное {animal.GetType()} с именем {animal.Name}{animal.Number} ест {animal.Food} кг еды в день");
        }
        Console.WriteLine("----------------------------------");
        Console.WriteLine("Отчет об инвентаре зоопарка:");
        foreach (var thing in _inventory) {
            Console.WriteLine($"{thing.Name} под номером {thing.Number}");
        }
        Console.WriteLine("----------------------------------");
        Console.WriteLine("Животные для контактного зоопарка:");
        foreach (var animal in GetContactZooAnimals()) {
            Console.WriteLine($"{animal.Name} (#{animal.Number})");
        }
        Console.WriteLine("----------------------------------");
    }
}