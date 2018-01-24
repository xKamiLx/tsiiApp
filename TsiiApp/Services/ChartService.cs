using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Json;
using TsiiApp.ViewModels;

namespace TsiiApp.Services
{
	public class ChartService
	{
		/**
		 * Get json from chart data
		 */
		public String ChartDataToJson(List<List<String>> data, bool hasFileLabels)
		{
			int pos = 0;
			int nrOfColumns = data[0].Count;
			int nrOfRows = data.Count;

			var jsonObject = new JsonObject();

			//Set labels
			List<String> labels = null;

			if (hasFileLabels)
			{
				labels = data[0];
				pos = 1;
			}
			else
			{
				labels = new List<String>();

				for (int i = 0; i < nrOfColumns; i++)
				{
					labels.Add("Col" + i);
				}
			}

			//Add labels to json object
			foreach (var label in labels)
			{
				jsonObject.Add(label, new JsonArray());
			}


			//Set values
			for (int i = pos; i < nrOfRows; i++)
			{
				List<String> values = data[i];

				for (int j = 0; j < values.Count; j++)
				{
					JsonValue jsonArray = null;

					if (jsonObject.TryGetValue(labels[j], out jsonArray))
					{
						((JsonArray)jsonArray).Add(values[j]);
					}

				}
			}

			return jsonObject.ToString();
		}

		/**
		 * Get chart data from specified file
		 */
		public List<List<String>> GetChartDataFromFile(String userName, String fileName, char getFileSeparator)
		{
			String pathToFile = GetPathToChartFolder(userName) + fileName + ".csv";
			List<List<String>> data = new List<List<String>>();

			try
			{
				using (StreamReader reader = new StreamReader(pathToFile))
				{
					String line;
					while ((line = reader.ReadLine()) != null)
					{
						String[] lineSplit = line.Split(getFileSeparator);
						List<String> rowData = new List<String>();

						foreach (var s in lineSplit)
						{
							rowData.Add(s);
						}

						data.Add(rowData);
					}

				}

			}
			catch (Exception e) { }

			return data;
		}

		/**
		 * Get all chart's file names
		 */
		public HomeViewModel GetChartsFileNames(string username)
		{
			string folderPath = GetPathToChartFolder(username);
			var homeViewModel = new HomeViewModel();

			if (Directory.Exists(folderPath))
			{
				var filenames = Directory.GetFiles(GetPathToChartFolder(username), "*.*").Select(Path.GetFileNameWithoutExtension).ToList();
				homeViewModel.Filenames = filenames;
			}

			return homeViewModel;
		}

		/**
		 * Get path to the folder with charts
		 */
		private String GetPathToChartFolder(string username)
		{
			return $"{AppDomain.CurrentDomain.BaseDirectory}Charts\\{username}\\";
		}
	}
}