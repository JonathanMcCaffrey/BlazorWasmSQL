// See https://aka.ms/new-console-template for more information
using _Shared;

using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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