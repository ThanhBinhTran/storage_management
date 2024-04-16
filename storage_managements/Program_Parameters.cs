using System;
using System.IO;

namespace storage_managements
{
	class Program_Parameters
	{
		public const string dataPath = @"data\\";
		public const string backupPath = dataPath + @"backup\\";
		public const string filePath_goods = dataPath +  @"goods.json";
		public const string filePath_storage = dataPath + @"storage.json";

		/* create path for program if nonexist*/
		public static void create_paths()
        {
			Create_Path(Program_Parameters.dataPath);
			Create_Path(Program_Parameters.backupPath);
			//Create_file(Program_Parameters.filePath_storage);
			//Create_file(Program_Parameters.filePath_goods);
		}



		private static void Create_file(string filepath)
        {
			if (!File.Exists(filepath))
			{
				File.Create(filepath).Close();
			}
		}
		private static void Create_Path(string folderPath)
		{
			try
			{
				if (!Directory.Exists(folderPath))
				{
					Directory.CreateDirectory(folderPath);
					Console.WriteLine($"Directory '{folderPath}' created successfully.");
				}
				else
				{
					Console.WriteLine($"Directory '{folderPath}' already exists.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error creating directory: {ex.Message}");
			}
		}
	}
}
