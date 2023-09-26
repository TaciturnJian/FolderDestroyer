
if (args.Length == 0) {
    Console.WriteLine("usage:\nprogram <path>");    
}

var path = args[0];

int total = 0;
int current = 0;

if (File.Exists(path))
{
    Console.WriteLine("Found path, but it's file, delete or not?(y/n)");
    Console.Write(">>> ");

    var input = Console.ReadLine();
    if ((input?.ToLower() ?? "") != "y")
    {
        Console.WriteLine("User refused, exiting");
        return;
    }

    Console.WriteLine($"Executing delete file({path})");
    try
    {
        if (!File.Exists(path))
        {
            return;
        }
        File.Delete(path);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception in file deleting: {ex.ToString}");
    }

    return;
}

if (Directory.Exists(path)) {
    Console.WriteLine("Found directory path, listing subdirectory below: ");
    var sub_directories = Directory.GetDirectories(path);
    var files = Directory.GetFiles(path);

    var threads = new List<Thread>();

    Console.WriteLine("Subdirectories: ");
    foreach (var dir in sub_directories) { 
        Console.WriteLine($"\t-{dir}");
        threads.Add(new Thread(()=>{try {Directory.Delete(dir, true);} catch(Exception ex) { Console.WriteLine(ex); }; Counter(); }));
        total++;
    }

    Console.WriteLine("Files: ");
    foreach (var file in files) { 
        Console.WriteLine($"\t-{file}");
        threads.Add(new Thread(()=>{ try {File.Delete(file);} catch(Exception ex) { Console.WriteLine(ex); }; Counter(); }));
        total++;
    }

    Console.WriteLine($"Ready to delete {total} items, delete or not?(y/n)");
    Console.Write(">>> ");

    var input = Console.ReadLine();
    if ((input?.ToLower() ?? "") != "y")
    {
        Console.WriteLine("User refused, exiting");
        return;
    }

    foreach (var thread in threads) {
        thread.Start();
    }

    while (current < total) {
        Thread.Sleep(100);
    }
    return;
}

Console.WriteLine($"Cannot access the path({path}), exiting.");

void Counter() {
    Interlocked.Add(ref current, 1);
    Console.WriteLine($"{current}/{total}");
}
