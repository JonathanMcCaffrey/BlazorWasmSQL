# Setup

I couldn't find a complete tutorial showing how to use a SQLite database with Blazor Wasm. So here is an example.

You need 4 projects. One to hold the context, one to generate the sql database, one to convert the database to json, and finally one to convert that json back into an sql database.  


---

NOT POSSIBLE CURRENTLY. DEPLOYING THE WEB CODE WILL GET SQL ERRORS.

https://github.com/dotnet/aspnetcore/issues/39528


---

## _Shared

Project that holds the context. 

```
public class DatabaseContext : DbContext {

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {
        // Must be included, given our database will never properly load on creation in Wasm
        Database.EnsureCreated();
    }

    public DbSet<Post> Posts => Set<Post>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfiguration(new PostConfig());
    }
}
```

## _Generate

Blazor Server project that exists only to generate and use the sql database file.

You must have .NET WebAssembly build tools installed to build this project.

You must set _Shared to be the selected project when running ef commands or they won't work.

If still getting problems, I was able to reinstall Visual Studio to resolve some cryptic errors.


```
/**
 * EntityFramework Commands
 * 
 * ## Create
 * dotnet ef migrations add InitialCreate
 *
 * ## Update
 * dotnet ef database update
 */
// Local Database Connection
builder.Services.AddDbContext<DatabaseContext>(options => {
    options.UseSqlite("Data Source=./Database.db",
        b => b.MigrationsAssembly("_Generate"));
});

builder.Services.AddTransient<IPostService, PostService>();
```

## _Console

Console project that converts the database from the Blazor Server project to one that can be used in Wasm.

```
string projectPath = $"{Environment.CurrentDirectory}/../../../..";
string webPath = $"{projectPath}/BlazorWasmSQL/wwwroot/json";

DbContextOptionsBuilder<DatabaseContext> options = new DbContextOptionsBuilder<DatabaseContext>();
options.UseSqlite($"Filename={projectPath}/_Generate/Database.db");

// Load our database
using (var db = new DatabaseContext(options.Options)) {
    // And save data in format Blazor Wasm can use
    File.WriteAllTextAsync($"{webPath}/PostModels.json", JsonSerializer.Serialize(db.Posts, new JsonSerializerOptions {
        WriteIndented = true
    }));
}
```

## BlazorWasmSQL

Blazor Wasm project, that finally loads the json data generate in the _Console project, and converts it back into a local database.

```
@foreach (var post in postService.GetPosts()) {
    <div>@post.Name</div>
}

@code {
    protected override async Task OnInitializedAsync() {
        if (database.Posts.Count() == 0) {
            // Now we reload our database
            Post[] posts = await Http.GetFromJsonAsync<Post[]>("json/PostModels.json");
            if (posts.Count() != 0) {
                // And convert it back into SQL
                database.Posts.AddRange(posts);
                database.SaveChanges();
                StateHasChanged();
            }
        }
    }
}
```