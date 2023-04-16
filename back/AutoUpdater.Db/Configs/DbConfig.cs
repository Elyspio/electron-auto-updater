namespace AutoUpdater.Db.Configs;

public class DbConfig
{
	public const string Section = "Database";
	public required string Username { get; set; }
	public long Port { get; set; }
	public required string Database { get; set; }
	public required string Password { get; set; }
	public required string Host { get; set; }
}