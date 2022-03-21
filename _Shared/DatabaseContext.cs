using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace _Shared;

public class Post {
    public int PostId { get; set; }
    [MaxLength(256)]
    public string Name { get; set; }

    public DateTime PublishedOn { get; set; }

    [NotMapped]
    [JsonIgnore]
    public string Test { get; set; }


    [JsonIgnore]
    public IgnoreData IgnoreData { get; set; }
}


[NotMapped]
public class IgnoreData {
    public int Test { get; set; }
}

public interface IPostService {
    public List<Post> GetPosts();
}

public class PostService : IPostService {
    private DatabaseContext database;

    public PostService(DatabaseContext database) {
        this.database = database;
    }

    public List<Post> GetPosts() {
        return database.Posts.ToList();
    }
}

internal class PostConfig : IEntityTypeConfiguration<Post> {

    public void Configure(EntityTypeBuilder<Post> builder) {
        ConfigureAsync(builder);
    }

    public async Task ConfigureAsync(EntityTypeBuilder<Post> builder) {
        builder.Property(x => x.PublishedOn).HasColumnType("date");
        builder.HasIndex(x => x.PublishedOn);

    }
}

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



