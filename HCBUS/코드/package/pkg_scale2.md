```python
import pymssql

# 창고name, 창고code 얻음
def get_warehouse(whcode=""):
    rows = []
    con = con_open()
    cur=con.cursor()
    sql = ""
    sql += "SELECT CODENAME"
    sql += "      ,MINORCODE"
    sql += "  FROM TB_Standard "
    sql += " WHERE MAJORCODE = 'WHCODE' "
    sql += "   AND MINORCODE <> '$'"
    sql += f"   AND MINORCODE LIKE '%' + '{whcode}' + '%'"
    cur.execute(sql)
    rows = cur.fetchall()
    con.close()
    new_list = []
    for i,j in rows:
        new_list.append(i)
        new_list.append(j)
    return new_list

# 작업자 이름 가져옴
def get_name():
    rows = []
    con = con_open()
    cur=con.cursor()
    sql = ""
    sql += "SELECT WORKERNAME"
    sql += "      ,WORKERID"
    sql += "  FROM TB_WorkerList "
    sql += " ORDER BY WORKERNAME"
    cur.execute(sql)
    rows = cur.fetchall()
    con.close()
    new_list = []
    for i,j in rows:
        new_list.append(i)
        new_list.append(j)
    return new_list

# 현재 창고에 있는 품목정보를 가져옴
def get_current(selected_option):
    rows = []
    con = con_open()
    cur=con.cursor()
    sql = ""
    sql += "   SELECT A.IMGSRC, A.ITEMCODE, B.CUR_STOCKQTY, A.SAFESTOCK, A.ORDERQTY"
    sql += "     FROM TB_ItemMaster A WITH(NOLOCK) JOIN (SELECT SUM(STOCKQTY) AS CUR_STOCKQTY"
    sql += "									             ,ITEMCODE"
    sql += "								    	     FROM TB_StockMM"
    sql += f"								    		WHERE WHCODE = '{selected_option}'"
    sql += "								    	 GROUP BY ITEMCODE) B "
    sql += "								       ON A.ITEMCODE = B.ITEMCODE"
    
    cur.execute(sql)
    rows = cur.fetchall()
    con.close()
    new_list = []
    for i in range(len(rows)):
        new_list.append(rows[i])
    return new_list

# 자동발주 
def auto_enroll(itemcode, autoqty, WorkerID):
    con = con_open()
    cur=con.cursor()

    sql = ""
    sql += " SELECT AORDERFLAG "
    sql += "   FROM TB_ItemMaster "
    sql += f" WHERE ITEMCODE = '{itemcode}' "
    cur.execute(sql)
    rows = cur.fetchone()
    con.close()
    try:
        s = ''.join(rows)
    except:
        s = 'N'
    con = con_open()
    cur = con.cursor()
    sql = ""
    if (s == 'Y'):   # TB_MaterialOrder에 insert 로직 시행
        sql+= " DECLARE @LD_NOWDATE DATETIME "
        sql+= "       ,@LS_NOWDATE VARCHAR(10) "
        sql+= "       ,@LI_SEQ     INT "
        sql+= "       ,@LS_PONO    VARCHAR(20) "
        sql+= "       ,@LS_UNITCODE VARCHAR(10)"
        sql+= "       ,@LS_CUSTCODE VARCHAR(30)"
        sql+= " SET @LD_NOWDATE = GETDATE() "
        sql+= " SET @LS_NOWDATE = CONVERT(VARCHAR,@LD_NOWDATE,23)"
        sql+= ""
        sql+= " SELECT @LI_SEQ = ISNULL(MAX(POSEQ),0) + 1 "
        sql+= "  FROM TB_MaterialOrder WITH (NOLOCK) "
        sql+= " WHERE PLANTCODE = '1000' "
        sql+= "   AND PODATE    = @LS_NOWDATE"
        sql+= ""
        sql+= " SET @LS_PONO = 'PO' + CONVERT(VARCHAR,@LD_NOWDATE,112) + RIGHT('00000' + CONVERT(VARCHAR,@LI_SEQ),4) "
        sql+= ""
        sql+= " SELECT @LS_UNITCODE = BASEUNIT"
        sql+= "       ,@LS_CUSTCODE = MAKECOMPANY"
        sql+= "  FROM TB_ItemMaster"
        sql+= f" WHERE ITEMCODE = '{itemcode}'"
        sql+= ""
        sql+= "  INSERT INTO TB_MaterialOrder (PLANTCODE,  PONO,     ITEMCODE,     PODATE,      POQTY,     UNITCODE,     MAKER,         MAKEDATE,    CUSTCODE,     POSEQ,   AORDERSTATUS, [O/W]) "
        sql+= f"                        VALUES ('1000',    @LS_PONO, '{itemcode}', @LS_NOWDATE, {autoqty}, @LS_UNITCODE, '{WorkerID}',  @LD_NOWDATE, @LS_CUSTCODE, @LI_SEQ, 'Y', 'W') "
    else:   # TB_OrderRequestList에 insert 로직 시행
        sql+= " DECLARE @LD_NOWDATE DATETIME "
        sql+= "        ,@LS_NOWDATE VARCHAR(10) "
        sql+= "        ,@LI_ReqSEQ  INT "
        sql+= "        ,@LS_UNITCODE VARCHAR(10)"
        sql+= "        ,@LS_CUSTCODE VARCHAR(30)"
        sql+= ""
        sql+= " SET @LD_NOWDATE = GETDATE()  "
        sql+= " SET @LS_NOWDATE = CONVERT(VARCHAR,@LD_NOWDATE,23)"
        sql+= ""
        sql+= " SELECT @LI_ReqSEQ = ISNULL(MAX(ReqSEQ), 0) + 1  "
        sql+= "   FROM TB_OrderRequestList WITH(NOLOCK)  "
        sql+= "  WHERE PLANTCODE = '1000'  "
        sql+= "    AND ReqDATE   = @LS_NOWDATE"
        sql+= ""
        sql+= " SELECT @LS_UNITCODE = BASEUNIT"
        sql+= "       ,@LS_CUSTCODE = MAKECOMPANY"
        sql+= "   FROM TB_ItemMaster"
        sql+= f"  WHERE ITEMCODE = '{itemcode}'"
        sql+= ""
        sql+= " INSERT INTO TB_OrderRequestList (PLANTCODE, ReqSEQ,     ReqDATE,     ITEMCODE, ReqQTY,   UNITCODE, CUSTCODE, ApprSTATUS, MAKER,     MAKEDATE, [O/W])  "
        sql+= f"                           VALUES('1000'   , @LI_ReqSEQ, @LS_NOWDATE, '{itemcode}', {autoqty},    @LS_UNITCODE    , @LS_CUSTCODE,  'N'       , '{WorkerID}', @LD_NOWDATE, 'W')"
    cur.execute(sql)
    con.commit()
    con.close()
    return '발주 등록을 완료하였습니다.'

# database open
def con_open():
    con = pymssql.connect(#server="192.168.0.112:1433",
                    #   host='localhost',
                      server = "222.235.141.8:1111",
                      database="KDTB03_1JO",
                      user="KDTB03",
                      password="333538",
                      charset = "EUC-KR") # 글자 꺠짐 방지
    return con
```