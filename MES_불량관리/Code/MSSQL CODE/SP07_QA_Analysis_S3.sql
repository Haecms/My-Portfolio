USE [KDTB03_MES_1]
GO
/****** Object:  StoredProcedure [dbo].[SP07_QA_Analysis_S3]    Script Date: 2023-08-31 오전 2:36:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		김건희
-- Create date: <Create Date,,>
-- Description:	폐기 내역 분석 3번 그리드
-- =============================================
ALTER PROCEDURE [dbo].[SP07_QA_Analysis_S3]


	@CUSTCODE VARCHAR(30),

	@LANG VARCHAR(10) = 'KO',
	@RS_CODE VARCHAR (1)  OUTPUT,
	@RS_MSG  VARCHAR (100) OUTPUT
AS
BEGIN
	 SELECT  CUSTCODE     AS  CUSTCODE   
			,CUSTNAME	  AS  CUSTNAME	
			,BIZREQNO	  AS  BIZREQNO	
			,ASGNNAME	  AS  ASGNNAME	
			,PHONE  	  AS  PHONE  	
	 
	  FROM TB_CustMaster WITH(NOLOCK) 										
	 WHERE CUSTCODE = @CUSTCODE
END