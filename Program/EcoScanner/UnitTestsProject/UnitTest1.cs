using EcoScanner;

namespace UnitTestsProject
{
	public class UnitTest1
	{
		[Fact]
		public void Test1()
		{
			EcoScanner.Services.History.addToHistory();
		}
	}
}