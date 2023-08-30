```html
<html>
    <head>
        <meta charset="UTF-8">
        <!-- <link rel="stylesheet" type="text/css" href="auto_scale2.css" /> -->
        <style>
            body{
                margin: 0;
                padding: 0;
                background-size :900px;
                background-position-x: center;
            }
            img{
               width: 300px;
               height: 200px; 
            }
            #top{
                text-align: center;
                font-size: 50px;
                margin-top: 20px;
            }
            .head{
                text-align: center;
                padding: 40px;
                margin-top: 20px;
                border: 1px solid black;
                background-color: azure;
            }
            #itemname{
                width: 100px;
                text-align: center;
            }
            #WorkerID{
                margin-left: 20px;
            }
            .center{
                width: 100%;
                margin-top: 30px;
            }
            .one_line{
                display: flex;
                margin-left: 50px;
                flex-wrap: wrap;
            }
            .one_line_items{
                width: 25%;
                margin-top: 20px;
            }
        </style>
    </head>
    <body>
        <div id="top">{{plantcode}}</div>
        <div class="head">
            <form action="/" method="post">
                <span>창고: </span>
                <select name="selected_option" id="select_value" onchange= "get_itemcode()">
                    <option value="none">---선택---</option>
                    {% for i in range(0, items|length, 2) %}
                    <option id="{{ items[i+1] }}" value="{{items[i+1]}}">{{items[i]}}</option> 
                    {% endfor %}
                </select>
                <span>창고코드: </span>
                <input id="itemname" type="text" disabled></input>
                <button type="submit">조회</button>

                <span id="WorkerID">작업자: </span>
                <select name="selected_name" id="name_value">
                    <option value="none">---선택---</option>
                    {% for i in range(0, items2|length, 2) %}
                    <option id="{{ items2[i+1] }}" value="{{items2[i+1]}}">{{items2[i]}}</option>
                    {% endfor %}
                </select>
            </form>

        </div>
        <div class= "center">
            <div class="one_line">
                {% for i in range(0, itemcode1|length) %}
                <div class=" one_line_items">
                    <img src="data:image/jpg;base64,{{ itemcode1[i][0] }}" alt="Image">
                    <div id = itemcode>품목 코드: {{ itemcode1[i][1] }}</div>
                    <div>현재 재고: {{itemcode1[i][2]}}</div>
                    <div>안전 재고: {{itemcode1[i][3]}}</div>
                    <div>자동 발주 수량: {{itemcode1[i][4]}}</div>
                    <button id="{{itemcode1[i][1]}}" value="{{itemcode1[i][4]}}" onclick="auto_send()">자동 발주</button>
                </div>
                {% endfor %}
                
            </div>
        </div>
        <script>
            // 콤보박스 선택시 이벤트, itemcode를 클릭하면 itemname 등장
            function get_itemcode(){
                let selectElement = document.getElementById("select_value");
                let selectedOption = selectElement.options[selectElement.selectedIndex]; // select - option 인덱스 추출
                let selectedOptionId = selectedOption.getAttribute("id"); // 인덱스의 id를 추출

                document.getElementById("itemname").value = selectedOptionId // 위에서 가져온 id의 value를 표 
            }
            async function auto_send(){
                const clickedButton = event.target;
                const itemcode = clickedButton.id; //itemcode
                const autoqty = parseInt((clickedButton.value).replace(/,/g, "")); // 자동발주수량 갯수
                let selectElement = document.getElementById("name_value");
                let selectedOption = selectElement.options[selectElement.selectedIndex];
                let WorkerID = selectedOption.getAttribute("value"); // WorkerID 
                if(WorkerID == "none"){
                    alert("작업자를 선택해주십시오")
                    return
                }
                let response = await fetch(`/fetch_data_insert?itemcode=${itemcode}&autoqty=${autoqty}&WorkerID=${WorkerID}`); 
                let data = await response.json();
                alert(data.message)
            }
        </script>
    </body>
</html>
```