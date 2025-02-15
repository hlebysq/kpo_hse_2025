using System;
using System.Collections.Generic;
using System.IO;
using Zoopark;

public class ZooUnitTests
{
    public static void Main()
    {
        RunTest(AddAnimal_HealthyAnimal_ShouldBeAdded);
        RunTest(AddAnimal_UnhealthyAnimal_ShouldNotBeAdded);
        RunTest(AddThing_ShouldBeAddedToInventory);
        RunTest(PrintReport_ShouldContainAnimalsAndThings);
        RunTest(AddPredator_ShouldBeAddedIfHealthy);
        RunTest(ContactZoo_ShouldContainOnlyKindHerbivores);
        RunTest(GetAnimals_ShouldReturnAllAnimals);
        RunTest(GetInventory_ShouldReturnAllThings);
    }

    private static void RunTest(Action testMethod)
    {
        try
        {
            testMethod();
            Console.WriteLine($"{testMethod.Method.Name}: PASSED");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{testMethod.Method.Name}: FAILED - {ex.Message}");
        }
    }

    public static void AddAnimal_HealthyAnimal_ShouldBeAdded()
    {
        var vetClinic = new VetClinic();
        var zoo = new Zoo(vetClinic);
        var monkey = new Monkey(1, 90, 8);

        zoo.AddAnimal(monkey);

        if (!zoo.GetAnimals().Contains(monkey))
            throw new Exception("Monkey was not added");
    }

    public static void AddAnimal_UnhealthyAnimal_ShouldNotBeAdded()
    {
        var vetClinic = new VetClinic();
        var zoo = new Zoo(vetClinic);
        var monkey = new Monkey(1, 30, 8);

        zoo.AddAnimal(monkey);

        if (zoo.GetAnimals().Contains(monkey))
            throw new Exception("Unhealthy animal was incorrectly added");
    }

    public static void AddThing_ShouldBeAddedToInventory()
    {
        var vetClinic = new VetClinic();
        var zoo = new Zoo(vetClinic);
        var table = new Table(1);

        zoo.AddThing(table);

        if (!zoo.GetInventory().Contains(table))
            throw new Exception("Table was not added");
    }

    public static void PrintReport_ShouldContainAnimalsAndThings()
    {
        var vetClinic = new VetClinic();
        var zoo = new Zoo(vetClinic);
        var monkey = new Monkey(1, 85, 8);
        var table = new Table(1);

        zoo.AddAnimal(monkey);
        zoo.AddThing(table);

        StringWriter sw = new StringWriter();
        TextWriter originalOut = Console.Out;
        Console.SetOut(sw);

        zoo.PrintReport();
        string output = sw.ToString();
        Console.SetOut(originalOut);

        if (!output.Contains("Monkey") || !output.Contains("Table"))
            throw new Exception("Report does not contain expected entries");
    }

    public static void AddPredator_ShouldBeAddedIfHealthy()
    {
        var vetClinic = new VetClinic();
        var zoo = new Zoo(vetClinic);
        var tiger = new Tiger(1, 40);

        zoo.AddAnimal(tiger);

        if (!zoo.GetAnimals().Contains(tiger))
            throw new Exception("Tiger was not added");
    }

    public static void ContactZoo_ShouldContainOnlyKindHerbivores()
    {
        var vetClinic = new VetClinic();
        var zoo = new Zoo(vetClinic);
        var kindRabbit = new Rabbit(1, 90, 10);
        var aggressiveMonkey = new Monkey(2, 85, 3);

        zoo.AddAnimal(kindRabbit);
        zoo.AddAnimal(aggressiveMonkey);
        
        var contactZooAnimals = zoo.GetContactZooAnimals();

        if (!contactZooAnimals.Contains(kindRabbit))
            throw new Exception("Rabbit should be in contact zoo");
        if (contactZooAnimals.Contains(aggressiveMonkey))
            throw new Exception("Non-herbivores should not be in contact zoo");
    }

    public static void GetAnimals_ShouldReturnAllAnimals()
    {
        var vetClinic = new VetClinic();
        var zoo = new Zoo(vetClinic);
        var monkey = new Monkey(1, 90, 8);
        var tiger = new Tiger(2, 80);

        zoo.AddAnimal(monkey);
        zoo.AddAnimal(tiger);

        var animals = zoo.GetAnimals();
        if (!animals.Contains(monkey) || !animals.Contains(tiger))
            throw new Exception("GetAnimals did not return all added animals");
    }

    public static void GetInventory_ShouldReturnAllThings()
    {
        var vetClinic = new VetClinic();
        var zoo = new Zoo(vetClinic);
        var table = new Table(1);
        var computer = new Computer(2);

        zoo.AddThing(table);
        zoo.AddThing(computer);

        var inventory = zoo.GetInventory();
        if (!inventory.Contains(table) || !inventory.Contains(computer))
            throw new Exception("GetInventory did not return all added things");
    }
}