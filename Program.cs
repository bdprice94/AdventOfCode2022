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

public class Utils {
    public static string[] GetInput(String s) {
        string CD = AppDomain.CurrentDomain.BaseDirectory;
        string inputFile = System.IO.Path.Combine(CD, @"..\..\..\inputs\" + s);
        return System.IO.File.ReadAllLines(inputFile);
    }
}

public class DayOne {
    public static void Main() {

        // grab our input file contents
        string[] lines = Utils.GetInput("01.txt");
        var elves = new List<Elf>();
        var foods = new List<int>();

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

public class DayTwo {
        public const int LOST = 0;
        public const int DRAW = 3;
        public const int WON = 6;
        public const int ROCK = 1;
        public const int PAPER = 2;
        public const int SCISSORS = 3;
        public static readonly Dictionary<char,int> decoder = new Dictionary<char, int> {
            {'A', -1}, // They Rock
            {'B', 0},  // They Paper
            {'C', 1},  // They Scissors
            {'X', 1},  // You Lose
            {'Y', 4},  // You Draw
            {'Z', 7}   // You Win
        };
        
        public static readonly Dictionary<char,int> points = new Dictionary<char,int> {
            {'X', 0},
            {'Y', 3},
            {'Z', 6}
        };
        
        public static readonly int[] results = {DRAW, LOST, WON, WON, DRAW, LOST, LOST, WON, DRAW};
        public static readonly int[] yourPlay = {SCISSORS, ROCK, PAPER, ROCK, PAPER, SCISSORS, PAPER, SCISSORS, ROCK};
    
    public static int CalcPoints(char them, char you) {
        return points[you] + yourPlay[decoder[them] + decoder[you]];
    }
    
    public static void Main() {

        var instructions = new List<Tuple<char, char>>();
        var inputs = Utils.GetInput("02.txt");
        
        foreach (var line in inputs) {
            instructions.Add(Tuple.Create(line[0], line[2]));            
        }

        var score = instructions.Aggregate(0, (int x, Tuple<char,char> y) => x + CalcPoints(y.Item1, y.Item2), (int x) => x);
        
        Console.Write(score);
    }
}

}