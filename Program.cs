namespace AdventOfCode2022 {

public class Elf {
    public Elf(List<int> foods) {
        Food = new List<int>(foods);
    }
    
    public List<int> Food {get;}

    public int TotalCalories() {
        return Food.Sum();
    }
    
    public int HighestCaloricFood() {
        return Food.Aggregate((x, y) => x > y ? x : y);
    }
    
    public int FoodCount() {
        return Food.Count;
    }
}

public class DayOne {
    public static void Main() {
        var elves = new List<Elf>();
        var foods = new List<int>();

        // grab our input file contents
        string CD = AppDomain.CurrentDomain.BaseDirectory;
        string inputFile = System.IO.Path.Combine(CD, @"..\..\..\inputs\01.txt");
        string[] lines = System.IO.File.ReadAllLines(inputFile);
        

        // parse input file into our data structures
        foreach (var line in lines) {
            if (line.CompareTo("") == 0) {
                elves.Add(new Elf(foods));
                foods.Clear();
            }
            else {
                foods.Add(Int32.Parse(line));
            }
        }
        
        Elf largestElf = elves.Aggregate((x, y) => x.TotalCalories() > y.TotalCalories() ? x : y);
        Console.WriteLine(largestElf.TotalCalories());
    }
}

}