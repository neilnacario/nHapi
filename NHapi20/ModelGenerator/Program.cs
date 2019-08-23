namespace ModelGenerator
{
	 public class Program
	 {
			/// <summary>
			/// Example usage: 
			///     ModelGenerator.exe /XmlFilename hl7_25.xml /Version 2.5 /MessageTypeToBuild EventMapping
			/// </summary>
			/// <param name="args"></param>
			public static void Main(string[] args)
			{
				 var command = Args.Configuration.Configure<ModelBuilder>().CreateAndBind(args);
				 command.Execute();
			}
	 }
}