HTML 파일
```html
<!DOCTYPE html>
<html lang="en">
<head>
	<meta charset="UTF-8">
	<meta name="viewport" content="width=device-width, initial-scale=1.0">
	<title>Document</title>
</head>
<body>
	
</body>
</html>
<!DOCTYPE html>
<html>
<head>
	<meta charset="UTF-8">
	<title>HOME NETWORK</title>
	<link rel="stylesheet" href="{{ url_for('static', filename='mProject.css') }}">
</head>
<body>
	<div class="container">
		<h1> HOME IoT</h1>
		<div class="main">
		<div>
			<h2>LED LIGHT</h2>
			<button class="btn_led" onclick="led_r()">LED RED</button>
			<button class="btn_led" onclick="led_g()">LED GREEN</button>
			<button class="btn_led" onclick="led_b()">LED BLUE</button>
			<button class="btn_led" onclick="led_off()">LED OFF</button>
		</div>
		<div>
			<h2>MUSIC BUZZER</h2>
			<button class="btn_ultra" onclick="buzzer_on()">BUZZER ON</button>
			<button class="btn_ultra" onclick="buzzer_off()">BUZZER OFF</button>
		</div>
		<div>
			<h2>ULTRA SONIC</h2>
			<button class="btn_music" onclick="ultra_on()">UltraSensor ON</button>
			<button class="btn_music" onclick="ultra_off()">UltraSensor OFF</button>
			<button class="btn_clean" onclick="gpio_cleanup()">GPIO CleanUp</button>
		</div>
		<div id="result">

		</div>
	</div>
	<script>
		function led_r(){
			fetch("/led/r")
			.then(response=>response.text())
			.then(data=> {
				console.log(data);
				let result = document.querySelector("#result");
				if(data=="ok"){
					result.innerHTML = "<h1>LED RED is running</h1>";
				}else{
					result.innerHTML = "<h1>error</h1>";
				}
			});
		}
		function led_g(){
			fetch("/led/g")
			.then(response=>response.text())
			.then(data=> {
				console.log(data);
				let result = document.querySelector("#result");
				if(data=="ok"){
					result.innerHTML = "<h1>LED GREEN is running</h1>";
				}else{
					result.innerHTML = "<h1>error</h1>";
				}
			});
		}
		function led_b(){
			fetch("/led/b")
			.then(response=>response.text())
			.then(data=> {
				console.log(data);
				let result = document.querySelector("#result");
				if(data=="ok"){
					result.innerHTML = "<h1>LED BLUE is running</h1>";
				}else{
					result.innerHTML = "<h1>error</h1>";
				}
			});
		}
		function led_off(){
			fetch("/led/off")
			.then(response=> response.text())
			.then(data=> {
				console.log(data);
				let result = document.querySelector("#result");
				if(data=="ok"){
					result.innerHTML = "<h1>LED is stopping</h1>";
				}else{
					result.innerHTML = "<h1>error</h1>";
				}
			});
		}
		function buzzer_on(){
			fetch("/buzzer/on")
			.then(response=> response.text())
			.then(data=> {
				console.log(data);
				let result = document.querySelector("#result");
				if(data=="ok"){
					result.innerHTML = "<h1>Music On</h1>";
				}else{
					result.innerHTML = "<h1>error</h1>";
				}
			});
		}
		function buzzer_off(){
			fetch("/buzzer/off")
			.then(response=> response.text())
			.then(data=> {
				console.log(data);
				let result = document.querySelector("#result");
				if(data=="ok"){
					result.innerHTML = "<h1>Music Off</h1>";
				}else{
					result.innerHTML = "<h1>error</h1>";
				}
			});
		}
		function ultra_on(){
			fetch("/ultra/on")
			.then(response=> response.text())
			.then(data=> {
				console.log(data);
				let result = document.querySelector("#result");
				if(data=="ok"){
					result.innerHTML = "<h1>UltraSensor ON</h1>";
				}else{
					result.innerHTML = "<h1>error</h1>";
				}
			});
		}
		function ultra_off(){
			fetch("/ultra/off")
			.then(response=> response.text())
			.then(data=> {
				console.log(data);
				let result = document.querySelector("#result");
				if(data=="ok"){
					result.innerHTML = "<h1>UltraSensor OFF</h1>";
				}else{
					result.innerHTML = "<h1>error</h1>";
				}
			});
		}
		function gpio_cleanup(){
			fetch("/gpio/cleanup")
			.then(response=> response.text())
			.then(data=> {
				console.log(data);
				let result = document.querySelector("#result");
				if(data=="ok"){
					result.innerHTML = "<h1>GPIO CleanUp</h1>";
				}else{
					result.innerHTML = "<h1>error</h1>";
				}
			});
		}
	</script>
</body>
</html>
```
