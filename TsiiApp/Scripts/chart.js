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
			var groupVar = null;
			if($("#classAttrState").is(":checked")){
				var classAttr = $("#classAttr :selected").text();
				groupVar = chartData[fileName][classAttr];
			}
			
			//Chart type
			var chartType = "scatter";
			if($("#labelZ").prop("selectedIndex") > 0){
				chartType = "scatter3d";
			}
		
			var trace = {
				x: chartData[fileName][labelX],
				y: chartData[fileName][labelY],
				z: chartData[fileName][labelZ],
				mode: "markers",
				type: chartType,
				marker: {size: 12},
				transforms: [{
					type: "groupby",
					groups: groupVar
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
	
	var chartType = $("[name=chartType]:checked").attr("value");
	var state = true;
	
	if(chartType == "2d"){
		if($("#labelX").prop("selectedIndex") == 0 
			|| $("#labelY").prop("selectedIndex") == 0){
				
			$("#message").text("You need to specify X column and Y column");
			state = false;
		}
	}
	else{
		if($("#labelX").prop("selectedIndex") == 0 
			|| $("#labelY").prop("selectedIndex") == 0
			|| $("#labelZ").prop("selectedIndex") == 0){
			
			$("#message").text("You need to specify X column, Y column and Z column");
			state = false;
		}
	}
	
	if(state){
		$("#message").css("display", "none");
		Chart.drawScatterChart(fileName);
	}
	else{
		$("#message").css("display", "block");
	}

}

$("[name=chartType]").change(function(){
	var value = ($(this).attr("value"));
	if(value == "3d"){
		$("#labelZ").removeAttr("disabled");
	}
	else{
		$("#labelZ").attr("disabled", "disabled");
	}
});

$("#classAttrState").change(function(){
	if($(this).is(":checked")){
		$("#classAttr").removeAttr("disabled");
	}
	else{
		$("#classAttr").attr("disabled", "disabled");
	}
});