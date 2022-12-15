namespace AdventOfCode2022 {

public class Elf {
    public Elf(List<int> foods) {
        Food = new List<int>(foods);
        TotalCalories = Food.Sum();
    }
    
    public List<int> Food {get;}

    public int TotalCalories {get;}
    
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
        
        Elf largestElf = elves.Aggregate((x, y) => x.TotalCalories > y.TotalCalories ? x : y);

        Console.WriteLine("Top largest elf: " + largestElf.TotalCalories);        
        
        var topElves = new PriorityQueue<Elf, int>(4); // default PQ is a minimum PQ which is what we want
        var iter = elves.GetEnumerator();

        for (int i = 0; i < 3; i++) {
            iter.MoveNext();
            topElves.Enqueue(iter.Current, iter.Current.TotalCalories);
        }
        
        while (iter.MoveNext()) {
            if (iter.Current.TotalCalories > topElves.Peek().TotalCalories) {
                topElves.EnqueueDequeue(iter.Current, iter.Current.TotalCalories);
            }
        } 
        
        int total = 0;
        for (int i = 0; i < 3; i++) {
            total += topElves.Dequeue().TotalCalories;
        }
        Console.WriteLine("Top 3 Largest Elves: " + total);
    }
}

}