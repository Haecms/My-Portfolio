플라스크 서버 파일
```python
from flask import Flask, request
from flask import render_template
import time
import RPi.GPIO as GPIO

redPin = 14
greenPin = 15
bluePin = 18

buzzerPin = 23
Frq = [262, 294, 330, 349, 392, 440, 493, 523]

trigPin = 24
echoPin = 25
stop = False

app = Flask(__name__)
GPIO.setmode(GPIO.BCM)
GPIO.setup(redPin, GPIO.OUT, initial = GPIO.LOW)
GPIO.setup(greenPin, GPIO.OUT, initial = GPIO.LOW)
GPIO.setup(bluePin, GPIO.OUT, initial = GPIO.LOW)
GPIO.setup(buzzerPin, GPIO.OUT)
GPIO.setup(trigPin, GPIO.OUT)
GPIO.setup(echoPin, GPIO.IN)
GPIO.setup(trigPin, False)

p=GPIO.PWM(buzzerPin, 100)

# LED 불빛 조절 함수
def led_light(R, l_R, G, l_G, B, l_B):
	GPIO.output(R, l_R)
	GPIO.output(G, l_G)
	GPIO.output(B, l_B)

@app.route("/")
def home():
	return render_template("mProject.html")

# LED RED
@app.route("/led/r")
def led_r():
	try:
		led_light(redPin,1, greenPin,0, bluePin,0)
		return "ok"
	except expression as identifier:
		return "fail"

# LED GREEN
@app.route("/led/g")
def led_g():
	try:
		led_light(redPin,0, greenPin,1, bluePin,0)
		return "ok"
	except expression as identifier:
		return "fail"

# LED BLUE
@app.route("/led/b")
def led_b():
	try:
		led_light(redPin,0, greenPin,0, bluePin,1)
		return "ok"
	except expression as identifier:
		return "fail"

# LED OFF
@app.route("/led/off")
def led_off():
	try:
		led_light(redPin,0, greenPin,0, bluePin,0)
		return "ok"
	except expression as identifier:
		return "fail"

# BUZOR ON
@app.route("/buzzer/on")
def buzzer_on():
	global p
	global Frq
	try:
		p.start(10)
		for fr in Frq:
			p.ChangeFrequency(fr)
			time.sleep(0.5)
		p.stop()
		return "ok"
	except expression as identifier:
		return "fail"

# BUZOR OFF
@app.route("/buzzer/off")
def buzzer_off():
	try:
		p.stop()
		return "ok"
	except expression as identifier:
		return "fail"

# ULTRA SONIC ON
@app.route("/ultra/on")
def ultra_on():
	global stop
	stop = True
	try:
		while stop:
			GPIO.output(trigPin, True)
			time.sleep(0.00001)
			GPIO.output(trigPin, False)

			while GPIO.input(echoPin)==0:
				#print("0")
				start = time.time()
			while GPIO.input(echoPin)==1:
				#print("1")
				end = time.time()

			check_time = end-start
			distance = check_time * 34300/2
			print("Distance: %.1f cm" %distance)
			time.sleep(0.4)
			return "ok"
	except Exception as identifier:
		print("except")
		return "fail"

# ULTRA SONIC OFF
@app.route("/ultra/off")
def ultra_off():
	global stop
	try:
		stop = False
		return "ok"
	except expression as identifier:
		return "fail"

# GPIO RESET
@app.route("/gpio/cleanup")
def gpio_cleanup():
	global p
	global stop
	try:
		led_light(redPin,0, greenPin,0, bluePin,0)
		p.stop()
		stop = False
		GPIO.cleanup
		return "ok"
	except expression as identifier:
		return "fail"

if __name__ == "__main__":
	app.run(host="0.0.0.0", port = "8888")
```
