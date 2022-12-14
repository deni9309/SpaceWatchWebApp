namespace SpaceWatch.Core.Extensions.Interfaces
{
	/// <summary>
	/// Used to extend classes with properties that SelectListItem type needs.
	/// </summary>
	public interface IPrimaryProperties
	{
		int Id { get; set; }
		string Title { get; set; }
	}
}
