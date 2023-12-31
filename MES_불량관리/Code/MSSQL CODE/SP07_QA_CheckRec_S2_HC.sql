USE [KDTB03_MES_1]
GO
/****** Object:  StoredProcedure [dbo].[SP07_QA_CheckRec_S2_HC]    Script Date: 2023-08-31 오전 2:35:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		조해찬
-- Create date: 2023-06-27
-- Description:	불량 판정 등록 이력 - 상세 내역 조회
-- =============================================
ALTER PROCEDURE [dbo].[SP07_QA_CheckRec_S2_HC]
	@CHKCODE VARCHAR(30),

	@LANG VARCHAR(10) = 'KO',
	@RS_CODE VARCHAR (1)  OUTPUT,
	@RS_MSG  VARCHAR (100) OUTPUT
AS
BEGIN
	SELECT DISTINCT A.PLANTCODE         AS PLANTCODE     
	               ,A.WORKCENTERCODE    AS WORKCENTERCODE
	           	   ,A.MATLOTNO		    AS LOTNO		 
	           	   ,A.ITEMCODE		    AS ITEMCODE		 
	           	   ,A.ITEMNAME		    AS ITEMNAME		 
	           	   ,A.BADWAITINGNO	    AS BADWAITINGNO	 
	           	   ,B.UNITCODE		    AS UNITCODE		 
	           	   ,A.MAKER		        AS MAKER		 
	           	   ,A.MAKEDATE		    AS MAKEDATE
	           	   ,A.CHKCODE           AS CHKCODE
	          FROM P_Combine_B A WITH(NOLOCK) LEFT JOIN TB_StockMMrec B WITH(NOLOCK)
	        									     ON A.MATLOTNO = B.MATLOTNO
	        										AND B.INOUTFLAG = 'O'
	         WHERE A.CHKCODE = @CHKCODE
END
