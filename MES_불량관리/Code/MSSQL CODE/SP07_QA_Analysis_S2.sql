USE [KDTB03_MES_1]
GO
/****** Object:  StoredProcedure [dbo].[SP07_QA_Analysis_S2]    Script Date: 2023-08-31 오전 2:36:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		김건희
-- Create date: <Create Date,,>
-- Description:	폐기 내역 분석 2번 그리드
-- =============================================
ALTER PROCEDURE [dbo].[SP07_QA_Analysis_S2]

	@CHKCODE VARCHAR(30),

	@LANG VARCHAR(10) = 'KO',
	@RS_CODE VARCHAR (1)  OUTPUT,
	@RS_MSG  VARCHAR (100) OUTPUT
AS
BEGIN
	 SELECT A.PLANTCODE                   AS  PLANTCODE       
	 	   ,A.CHKCODE   				  AS  CHKCODE   		
	 	   ,A.WORKCENTERCODE			  AS  WORKCENTERCODE
	 	   ,A.MATLOTNO  				  AS  MATLOTNO  		
	 	   ,A.ITEMCODE			          AS  ITEMCODE		
	 	   ,A.ITEMNAME  				  AS  ITEMNAME  		
	 	   ,A.MAKER    				      AS  MAKER    		
	 	   ,A.MAKEDATE                    AS  MAKEDATE        
	 	   ,B.CUSTCODE                    AS  CUSTCODE        
	 
	  FROM P_Combine_B A WITH(NOLOCK) LEFT JOIN TB_MaterialOrder B WITH(NOLOCK)
										     ON A.MATLOTNO = B.LOTNO										
	 WHERE A.CHKCODE = @CHKCODE
END