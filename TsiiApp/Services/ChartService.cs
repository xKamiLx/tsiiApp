using System;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using System.Json;

namespace TsiiApp.Services
{
	public class ChartService
	{

		/**
		 * Get json from chart data
		 */
		public async String ChartDataToJson(List<List<String>> data, boolean hasFileLabels){
			
			int pos = 0;
			int nrOfColumns = data[0].Count;
			int nrOfRows = data.Count;
			
			var jsonObject = new JsonObject();
			
			//Set labels
			List<String> labels = null;
			
			if(hasFileLabels){
				labels = data[0];
				pos = 1;
			}
			else{
				labels = new List<String>();
				
				for(int i=0; i<nrOfColumns; i++){
					labels.Add("Col" + i);
				}
			}
			
			//Add labels to json object
			foreach(var label in labels){
				jsonObject.Add(label, new JsonArray());
			}
			
			
			//Set values
			for(int i = pos; i<nrOfRows; i++){
				List<String> values = data[i];
				
				for(int j = 0; j<values.Count; j++){
					JsonValue jsonArray = null;
					
					if(jsonObject.TryGetValue(labels[j], out jsonArray)){
						((JsonArray)jsonArray).Add(values[j]);
					}
					
				}
				
			}
			
			return jsonObject.ToString();
			
		}
		
		/**
		 * Get chart data from specified file
		 */
		public async List<List<String>> GetChartDataFromFile(String fileName, char getFileSeparator){
			
			String data = null;
			String pathToFile = GetPathToChartFolder() + fileName;
			List<List<String>> data = new List<List<String>>();
			
			try{
				
				using(StreamReader reader = new StreamReader(pathToFile)){
					
					String line = await reader.ReadToEndAsync();
					String[] lineSplit = line.Split(getFileSeparator);
					
					List<String> rowData = new List<List<String>>();
					foreach(var s in lineSplit){
						rowData.Add(s);
					}
					
					data.add(rowData);
					
				}
				
			}
			catch(Exception e){}
			
			return data;
			
		}
		
		/**
		 * Get all chart's file names
		 */
		public HomeViewModel GetChartsFileNames(){
			
			var filenames = Directory.GetFiles(GetPathToChartFolder(), "*.*").Select(Path.GetFileNameWithoutExtension).ToList();
			var homeViewModel = new HomeViewModel
			{
				Filenames = filenames
			};
			
			return homeViewModel;
			
		}
		
		/**
		 * Get path to the folder with charts
		 */
		private String GetPathToChartFolder(){
			return AppDomain.CurrentDomain.BaseDirectory+"Charts\\";
		}
	}
}