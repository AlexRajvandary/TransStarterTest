using Domain.Entities;
using Infrastructure.Data;

namespace DataGenerator.Services
{
    public class RandomDataService
    {
        private readonly AppDbContext _context;
        private readonly Random _rnd = new();
        private readonly Dictionary<string, string[]> brandModels = new()
{
    { "Toyota", new[] { "Corolla", "Camry", "RAV4", "Hilux", "Yaris" } },
    { "Volkswagen", new[] { "Golf", "Passat", "Tiguan", "Polo", "Arteon" } },
    { "Ford", new[] { "Focus", "Fiesta", "Mustang", "Explorer", "F-150" } },
    { "Honda", new[] { "Civic", "Accord", "CR-V", "Fit", "HR-V" } },
    { "Nissan", new[] { "Altima", "Maxima", "Leaf", "Rogue", "Sentra" } },
    { "Hyundai", new[] { "Elantra", "Sonata", "Tucson", "Santa Fe", "Kona" } },
    { "Chevrolet", new[] { "Cruze", "Malibu", "Equinox", "Traverse", "Camaro" } },
    { "Kia", new[] { "Rio", "Cerato", "Sportage", "Sorento", "Seltos" } },
    { "Renault", new[] { "Clio", "Megane", "Captur", "Duster", "Koleos" } },
    { "Peugeot", new[] { "208", "308", "3008", "5008", "2008" } },
    { "Mercedes-Benz", new[] { "A-Class", "C-Class", "E-Class", "GLE", "GLC" } },
    { "BMW", new[] { "3 Series", "5 Series", "X3", "X5", "i4" } },
    { "Audi", new[] { "A3", "A4", "A6", "Q3", "Q5" } },
    { "Volvo", new[] { "S60", "S90", "XC40", "XC60", "XC90" } },
    { "Suzuki", new[] { "Swift", "Vitara", "Jimny", "SX4", "Celerio" } },
    { "Fiat", new[] { "500", "Panda", "Tipo", "500X", "Punto" } },
    { "Skoda", new[] { "Fabia", "Octavia", "Karoq", "Kodiaq", "Rapid" } },
    { "Mitsubishi", new[] { "Lancer", "Outlander", "ASX", "Eclipse Cross", "Pajero" } },
    { "Mazda", new[] { "Mazda2", "Mazda3", "CX-3", "CX-5", "CX-30" } },
    { "Opel", new[] { "Corsa", "Astra", "Grandland", "Mokka", "Insignia" } },
    { "Subaru", new[] { "Impreza", "Forester", "Outback", "XV", "BRZ" } },
    { "Citroen", new[] { "C3", "C4", "C5 Aircross", "C4 Cactus", "Berlingo" } },
    { "Land Rover", new[] { "Range Rover", "Discovery", "Defender", "Velar" } },
    { "Jaguar", new[] { "XE", "XF", "F-Pace", "E-Pace" } },
    { "Jeep", new[] { "Renegade", "Compass", "Cherokee", "Wrangler" } },
    { "Dodge", new[] { "Charger", "Challenger", "Durango" } },
    { "Chrysler", new[] { "300", "Pacifica" } },
    { "Tesla", new[] { "Model S", "Model 3", "Model X", "Model Y" } },
    { "Acura", new[] { "ILX", "TLX", "RDX", "MDX" } },
    { "Infiniti", new[] { "Q50", "Q60", "QX50", "QX60" } },
    { "Lexus", new[] { "IS", "ES", "RX", "NX", "UX" } },
    { "Mini", new[] { "Cooper", "Clubman", "Countryman", "Convertible" } },
    { "Alfa Romeo", new[] { "Giulia", "Stelvio", "Tonale" } },
    { "Seat", new[] { "Ibiza", "Leon", "Ateca", "Arona" } },
    { "Smart", new[] { "Fortwo", "Forfour" } },
    { "Lincoln", new[] { "MKZ", "Corsair", "Nautilus", "Aviator" } },
    { "Cadillac", new[] { "CT4", "CT5", "XT4", "XT5", "Escalade" } },
    { "Buick", new[] { "Regal", "Enclave", "Encore" } },
    { "GMC", new[] { "Sierra", "Terrain", "Acadia" } },
    { "Ram", new[] { "1500", "2500", "3500" } },
    { "Holden", new[] { "Commodore", "Colorado", "Acadia" } },
    { "Isuzu", new[] { "D-Max", "MU-X" } },
    { "Mahindra", new[] { "XUV500", "Thar", "Bolero" } },
    { "Tata", new[] { "Nexon", "Harrier", "Tiago" } },
    { "Proton", new[] { "Saga", "Persona", "X70" } },
    { "Great Wall", new[] { "Haval H6", "Haval H9", "Wingle 7" } },
    { "Chery", new[] { "Tiggo 3", "Tiggo 5", "Arrizo 5" } },
    { "BYD", new[] { "Tang", "Han", "Yuan" } },
    { "Geely", new[] { "Coolray", "Atlas", "Emgrand" } },
    { "Haval", new[] { "H6", "H9", "F7" } },
    { "MG", new[] { "ZS", "HS", "5" } },
    { "NIO", new[] { "ES8", "ES6", "EC6" } },
    { "XPeng", new[] { "P7", "G3", "P5" } },
    { "Li Auto", new[] { "Li One", "Li L9" } },
    { "Ora", new[] { "Good Cat", "Black Cat" } },
    { "Wey", new[] { "VV7", "VV6" } },
    { "Lynk & Co", new[] { "01", "02", "03" } },
    { "Dongfeng", new[] { "AX7", "Fengon 580" } },
    { "FAW", new[] { "Bestune T77", "Bestune T99" } },
    { "BAIC", new[] { "X55", "BJ40" } },
    { "JAC", new[] { "S3", "S5", "iEV7S" } }
    };

        private readonly string[] carColors = new[]
{
    "White", "Black", "Silver", "Gray", "Blue", "Red", "Green", "Yellow", "Brown", "Beige",
    "Orange", "Purple", "Gold", "Maroon", "Pink", "Cyan", "Turquoise", "Dark Blue", "Dark Green", "Champagne"
};

        private readonly string[] configurations = { "Standard", "Premium", "Sport" };
        private readonly string[] menFirstNames =
 {
    "Александр", "Максим", "Иван", "Дмитрий", "Никита",
    "Михаил", "Андрей", "Сергей", "Егор", "Алексей",
};

        private readonly string[] middleNames =
        {
    "Александрович", "Максимович", "Иванович", "Дмитриевич", "Никитович",
    "Михайлович", "Андреевич", "Сергеевич", "Егорович", "Алексеевич"
};

        private readonly string[] lastNames =
        {
    "Иванов", "Петров", "Сидоров", "Кузнецов", "Смирнов",
    "Попов", "Васильев", "Морозов", "Волков", "Соколов"
};

        private List<Brand> brands = new();
        private List<Model> models = new();
        private List<Configuration> configs = new();
        private List<Car> cars = new();

        public RandomDataService(AppDbContext context)
        {
            _context = context;
        }

        public void Generate(int salesCount,  int years = 5)
        {
            brands = GenerateBrands();
            _context.SaveChanges();

            models = GenerateModels();
            _context.SaveChanges();

            configs = GenerateConfigurations();
            _context.SaveChanges();

            cars = GenerateCars(100); // или нужное количество
            _context.SaveChanges();

            var customers = GenerateCustomers(salesCount / 5);
            _context.SaveChanges();

            GenerateSales(salesCount, years, customers, cars);
            _context.SaveChanges();
        }

        private List<Brand> GenerateBrands()
        {
            var list = brandModels.Keys.Select(name => new Brand { Name = name }).ToList();
            _context.Brands.AddRange(list);
            return list;
        }

        private List<Model> GenerateModels()
        {
            var list = new List<Model>();
            foreach (var brand in brands)
            {
                foreach (var modelName in brandModels[brand.Name])
                {
                    list.Add(new Model
                    {
                        Name = modelName,
                        BrandId = brand.Id
                    });
                }
            }
            _context.Models.AddRange(list);
            return list;
        }

        private List<Configuration> GenerateConfigurations()
        {
            var list = configurations.Select(c => new Configuration { Name = c }).ToList();
            _context.Configurations.AddRange(list);
            return list;
        }

        private List<Car> GenerateCars(int carsCount)
        {
            var list = new List<Car>();

            for (int i = 0; i < carsCount; i++)
            {
                var brand = brands[_rnd.Next(brands.Count)];
                var brandModelsList = models.Where(m => m.BrandId == brand.Id).ToList();
                var model = brandModelsList[_rnd.Next(brandModelsList.Count)];
                var config = configs[_rnd.Next(configs.Count)];
                var color = carColors[_rnd.Next(carColors.Length)];

                list.Add(new Car
                {
                    BrandId = brand.Id,
                    ModelId = model.Id,
                    ConfigurationId = config.Id,
                    Color = color,
                    Price = GenerateCarPrice()
                });
            }

            _context.Cars.AddRange(list);
            return list;
        }

        public decimal GenerateCarPrice(decimal minPrice = 1_000_000, decimal maxPrice = 20_000_000)
        {
            decimal price = (decimal)(_rnd.NextDouble() * (double)(maxPrice - minPrice) + (double)minPrice);
            
            return Math.Round(price / 1000) * 1000;
        }

        private List<Customer> GenerateCustomers(int count)
        {
            var list = new List<Customer>();
            for (int i = 0; i < count; i++)
            {
                var firstName = menFirstNames[_rnd.Next(menFirstNames.Length)];
                var lastName = lastNames[_rnd.Next(lastNames.Length)];
                var middleName = middleNames[_rnd.Next(middleNames.Length)];

                list.Add(new Customer
                {
                    FirstName = firstName,
                    LastName = lastName,
                    MiddleName = middleName,
                    Email = GenerateEmail(firstName, lastName, middleName),
                    Phone = $"+7(9{_rnd.Next(10, 99)}){_rnd.Next(100, 999)}{_rnd.Next(1000, 9999)}"
                });
            }
            _context.Customers.AddRange(list);
            return list;
        }

        private string GenerateEmail(string firstName, string lastName, string middleName)
        {
            var initials = $"{firstName[0]}{middleName[0]}".ToLower();
            var last = lastName.ToLower();
            return $"{last}.{initials}@example.com";
        }

        private void GenerateSales(int salesCount, int years, List<Customer> customers, List<Car> cars)
        {
            for (int i = 0; i < salesCount; i++)
            {
                var customer = customers[_rnd.Next(customers.Count)];
                var sale = new Sale
                {
                    CustomerId = customer.Id,
                    Date = DateTime.Now.AddYears(-_rnd.Next(years)).AddDays(-_rnd.Next(365)),
                    TotalPrice = 0
                };
                _context.Sales.Add(sale);
                _context.SaveChanges();

                int itemsCount = _rnd.Next(1, 2);
                decimal totalPrice = 0;
                for (int j = 0; j < itemsCount; j++)
                {
                    var car = cars[_rnd.Next(cars.Count)];
                    totalPrice += car.Price;

                    _context.SaleItems.Add(new SaleItem
                    {
                        SaleId = sale.Id,
                        CarId = car.Id,
                        Price = car.Price
                    });
                }

                sale.TotalPrice = totalPrice;
                _context.SaveChanges();
            }
        }
    }
}
