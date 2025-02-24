# Немного контекста
Первое Мини-ДЗ по нашему курсу Конструирование Программного Обеспечения
# Условия задачи
Лежат в pdf-файле КПО-мини-ДЗ_1.pdf
# Структура программы
- Интерфейсы IAlived и IInventory, первый для одушевленных сущностей, второй - для всего имущества зоопарка.
- Абстрактные классы Animal и Thing, для зверей и для неодушевленных предметов соответственно
- Классы Tiger, Wolf, Rabbit и Monkey как примеры животных - по принципу проектирования Open-Closed добавить новых животных очень просто, нужно только пронаследовать их от Animal
- Классы Table, Computer - аналогично предыдущим, примеры неодушевленных предметов инвентаря
- Классы Zoo и VetClinic - классы для зоопарка и ветклиники, связанные композицией: для зоопарка снаружи создается ветклиника и добавляется в конструкторе. VetClinic занимается определением состояния животного при поступлении в зоопарк на основании его параметра Healthy и допуском его в зоопарк. 
- В Main'е при помощи DI контейнера от Microsoft создаются синглтоны VetClinic и Zoo, дальше в зоопарк добавляются стол и компьютер - даже стула нет! Пользователю предлагается наполнить зоопарк животными и инвентарем при помощи простенького консольного интерфейса
# Структура Unit-тестов 
- Метод RunTest, который запускает тест
## Тесты
- AddAnimal_HealthyAnimal_ShouldBeAdded - добавление здорового животного в зоопарк
- AddAnimal_UnhealthyAnimal_ShouldNotBeAdded - добавление больного животного в зоопарк
- AddThing_ShouldBeAddedToInventory - добавление предмета в инвентарь
- PrintReport_ShouldContainAnimalsAndThings - проверка свойства PrintReport класса Zoo, формирующего отчёт
- AddPredator_ShouldBeAddedIfHealthy - добавление здорового хищника
- ContactZoo_ShouldContainOnlyKindHerbivores - проверка свойства GetContactZooAnimals() класса Zoo
- GetAnimals_ShouldReturnAllAnimals - проверка свойства GetAnimals() класса Zoo
- GetInventory_ShouldReturnAllThings - проверка свойства GetInventory() класса Zoo
# Руководство по запуску
Можно запустить в IDE для языка C#, либо через лежащие в {ZooUnitTests/Zoopark}/bin/debug/net9.0/ файл kpo_hw1.exe и ZooUnitTests.exe
# Соблюдение принципов проектирования:
DRY - в Program написаны функции для получения бинарного ответа и для считывания целого числа в диапазоне. 
SOLID:
Single Responsibility - классы и интерфейсы разделены, класс Thing не содержит животных, Animal - компьютеров и столов
Open-closed - все классы открыты для расширения. К примеру, я в один момент захотел добавить поле Healthy для класса Animal. Я добавил его к интерфейсу и просто сделал реализацию и объявление в конструкторе
Liskov - вместо всех Animal'ов можно создавать отдельных животных - в функциональности потерь не будет
Interface segregation - сделаны интерфейсы IAlived и IInventory и их реализация
Dependency Inversion - в основной программе использован DI контейнер от Microsoft для хранения экземпляров классов VetClinic и Zoo, оба в виде синглтонов, второй создается на основе первого с помощью ассоциации
