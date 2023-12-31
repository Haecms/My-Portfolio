USE [KDTB03_1JO]
GO
/****** Object:  StoredProcedure [dbo].[SP07_MM_MaterialInput_S1]    Script Date: 2023-09-04 오후 6:27:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		조해찬
-- Create date: 2023-08-15
-- Description:	자재 입고 조회
-- =============================================
ALTER PROCEDURE [dbo].[SP07_MM_MaterialInput_S1]
	@STARTDATE VARCHAR(20),
	@ENDDATE   VARCHAR(20),  
	@INPUTLOT  VARCHAR(20), 
	@PLANTCODE VARCHAR(20),
	@ITEMCODE  VARCHAR(20), 
	@PONO      VARCHAR(20),
	
	@LANG      VARCHAR(10),  
	@RS_CODE   VARCHAR(1)   OUTPUT,
	@RS_MSG    VARCHAR(100) OUTPUT
AS
BEGIN
	SELECT A.PLANTCODE						AS PLANTCODE
		  ,A.INPUTSEQ						AS INPUTSEQ 
		  ,A.INPUTLOT						AS INPUTLOT 
		  ,B.ITEMCODE						AS ITEMCODE
		  ,C.ITEMNAME						AS ITEMNAME
		  ,D.CUSTNAME						AS CUSTNAME
		  ,CASE ISNULL(B.[O/W], '0') WHEN '0' THEN E.[O/W]
		                           ELSE B.[O/W]END AS WO
		  ,B.PODATE                         AS ORDERDATE
		  ,A.INPUTQTY						AS INPUTQTY 
		  ,A.INPUTDATE						AS INPUTDATE
		  ,A.PONO							AS PONO    
		  ,DBO.FN_GET_USERNAME(A.MAKER)     AS MAKER    
		  ,A.MAKEDATE						AS MAKEDATE
	 FROM TB_MaterialInput A WITH(NOLOCK) LEFT JOIN TB_MaterialOrder B WITH(NOLOCK)
	                                             ON A.PONO = B.PONO
										  LEFT JOIN TB_ItemMaster C WITH(NOLOCK)
										         ON B.ITEMCODE = C.ITEMCODE
										  LEFT JOIN TB_CustMaster D WITH(NOLOCK)
										         ON C.MAKECOMPANY = D.CUSTCODE
										  LEFT JOIN TB_OrderRequestList E WITH(NOLOCK)
										         ON A.PONO = E.PONO
	WHERE A.INPUTLOT LIKE '%' + @INPUTLOT + '%'
	  AND A.PLANTCODE LIKE '%' + @PLANTCODE + '%'
	  AND B.ITEMCODE LIKE '%' + @ITEMCODE + '%'
	  AND A.PONO LIKE '%' + @PONO + '%'
	  AND INPUTDATE BETWEEN @STARTDATE AND @ENDDATE




END
