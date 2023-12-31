USE [KDTB03_MES_1]
GO
/****** Object:  StoredProcedure [dbo].[SP07_QA_Analysis_S1]    Script Date: 2023-08-31 오전 2:36:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		김건희
-- Create date: <Create Date,,>
-- Description:	폐기 내역 분석 1번 그리드
-- =============================================
ALTER PROCEDURE [dbo].[SP07_QA_Analysis_S1]

	@CHKCODE      VARCHAR(30),
	@PASSLOTNO    VARCHAR(30),
	@STARTDATE    VARCHAR(30),
	@ENDDATE      VARCHAR(30),  

	@LANG      VARCHAR(10),  
	@RS_CODE   VARCHAR(1)   OUTPUT,
	@RS_MSG    VARCHAR(100) OUTPUT

AS
BEGIN
	

	SELECT  CONVERT(char(10),MAKEDATE,23)                           AS  MAKEDATE    
		   ,CHKCODE   			                                   AS  CHKCODE   
		   ,PASSLOTNO  			                                   AS  PASSLOTNO 
--		   ,ITEMNAME   			                                   AS  ITEMNAME  
		   ,MAKER      			                                   AS  MAKER     
		   ,PASSNO    			                                   AS  PASSNO   
		   ,TOTALNO   			                                   AS  TOTALNO  
		   ,CONVERT(VARCHAR,ROUND(PASSNO * 100 / TOTALNO,2)) +' %' AS  BADRATE  
		   ,PASSREASON 			                                   AS  PASSREASON
	  FROM P_Lot   WITH(NOLOCK)
	 WHERE PASSLOTNO LIKE '%' + @PASSLOTNO + '%'
	   AND MAKEDATE  BETWEEN @STARTDATE + ' 00:00:00' AND @ENDDATE + ' 23:59:59'
	   AND PASSLOTNO LIKE '%' + 'LT_N'  +'%'
	   AND CHKCODE   LIKE @CHKCODE + '%'
	 --AND PLANTCODE LIKE @PLANTCODE + '%'
  ORDER BY BADRATE DESC
END
