var Chart = (function(){
	
	//Stores all data from loaded files
	var chartData = {};
	
	//Get chart data
	function getChartData(fileName, getUrl, callback){
		
		$.ajax({
			method: "GET",
			url: getUrl
		}).done(function(data){
			var jsonObject = JSON.parse(data);
			chartData[fileName] = jsonObject;
			callback(fileName);
		});
		
	}
	
	//Get keys from json object
	function getKeys(getJsonObject){
		return Object.keys(getJsonObject);
	}
	
	//Load data
	function loadData(fileName, getUrl){
		
		if(fileName in chartData){
			updateLabelInputs(fileName);
		}
		else{
			getChartData(fileName, getUrl, updateLabelInputs);
		}
		
	}
	
	//Update label inputs
	function updateLabelInputs(fileName){
		
		var jsonObject = chartData[fileName];
		var keys = getKeys(jsonObject);
		
		$("#labelX option").remove();
		$("#labelY option").remove();
		$("#labelZ option").remove();
		$("#classAttr option").remove();
		
		$("#labelX").append("<option selected='true' disabled='disabled'>Select column</option>");
		$("#labelY").append("<option selected='true' disabled='disabled'>Select column</option>");
		$("#labelZ").append("<option selected='true' disabled='disabled'>Select column</option>");
		$("#classAttr").append("<option selected='true' disabled='disabled'>Select class attribute</option>");
		
		$("#labelX").append("<option>None</option>");
		$("#labelY").append("<option>None</option>");
		$("#labelZ").append("<option>None</option>");
		$("#classAttr").append("<option>None</option>");
		
		for(var key in keys){
			$("#labelX").append("<option>"+keys[key]+"</option>");
			$("#labelY").append("<option>"+keys[key]+"</option>");
			$("#labelZ").append("<option>"+keys[key]+"</option>");
			$("#classAttr").append("<option>"+keys[key]+"</option>");
		}
		
		$("#nav").css("display", "block");
		$("#info").css("display", "none");
		
	}
	
	//Draw scatter chart
	function drawScatterChart(fileName){
		if(fileName in chartData){
			
			var labelX = $("#labelX :selected").text();
			var labelY = $("#labelY :selected").text();
			var labelZ = $("#labelZ :selected").text();
			
			//Check class attr
			var classAttrOption = $("#classAttr :selected").text();
			var classAttr = null;
			if(classAttrOption != "None"){
				classAttr = classAttrOption;
			}
		
			var trace = {
				x: chartData[fileName][labelX],
				y: chartData[fileName][labelY],
				mode: "markers",
				type: "scatter",
				marker: {size: 12},
				transforms: [{
					type: "groupby",
					groups: chartData[fileName][classAttr]
				}]
			};
			
			var layout = {
				title: "Data from file " + fileName				
			}
			
			var data = [trace];
			Plotly.newPlot('plot', data, layout);
		
		}		
	}
	
	return{
		loadData: loadData,
		drawScatterChart: drawScatterChart
	}
})();

function showGraph(fileName){
	Chart.drawScatterChart(fileName);
}