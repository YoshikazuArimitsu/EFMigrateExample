using System.Diagnostics;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EFMigrateExample.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Console.WriteLine("Hello");
    }

    public static IEnumerable<object[]> DummyTestData()
    {
        for (int i = 0; i < 1000000 ; i++)
        {
            yield return new object[] { i };
            }
    }

    private static int _counter = 0;
    Process proc = Process.GetCurrentProcess();

    [Theory]
    [MemberData(nameof(DummyTestData))]
    public void SomeTest(int dummyParam)     // dummyParam is unused
    {
        if(_counter % 100 == 0) {
            proc.Refresh();
            Console.WriteLine($"RepeatTest {_counter} {proc.PrivateMemorySize64}");
        }

#if true
        var _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // These options will be used by the context instances in this test suite, including the connection opened above.
        var options = new DbContextOptionsBuilder<BloggingContext>()
            .UseSqlite(_connection)
            .Options;
#else
        var options = new DbContextOptionsBuilder<BloggingContext>()
                            .UseInMemoryDatabase(Guid.NewGuid().ToString())
                            .Options;
#endif
        using (var db = new BloggingContext(options))
        {
            db.Database.EnsureCreated();
            db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
            db.SaveChanges();
            db.ChangeTracker.Clear();
        }

        // GC.Collect();
        _counter++;
    }

}