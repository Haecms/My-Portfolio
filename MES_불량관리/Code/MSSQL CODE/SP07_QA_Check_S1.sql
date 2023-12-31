USE [KDTB03_MES_1]
GO
/****** Object:  StoredProcedure [dbo].[SP07_QA_Check_S1]    Script Date: 2023-08-31 오전 2:32:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*---------------------------------------------------------------------------------------------*
  PROEDURE ID    : SP07_QA_Check_S1
  PROCEDURE NAME : 불량 내역 관리 조회
  ALTER DATE     : 2023-05-18
  MADE BY        : 조해찬
  DESCRIPTION    : 
  REMARK         : 
*---------------------------------------------------------------------------------------------*
  ALTER DATE     :
  UPDATE BY      :
  REMARK         :
*---------------------------------------------------------------------------------------------*/
ALTER PROCEDURE [dbo].[SP07_QA_Check_S1]
	@PLANTCODE VARCHAR(10),  -- 공장
	@MATLOTNO  VARCHAR(30),  -- LOT번호
	@ITEMCODE  VARCHAR(30),  -- 품목
--	@STARTDATE VARCHAR(10),  -- 입고 시작일자
--	@ENDDATE   VARCHAR(10),  -- 입고 종료일자

	@LANG      VARCHAR(10),  
	@RS_CODE   VARCHAR(1)   OUTPUT,
	@RS_MSG    VARCHAR(100) OUTPUT

AS
BEGIN
	SELECT 0								AS CHK
		  ,PLANTCODE						AS PLANTCODE
		  ,WORKCENTERCODE					AS WORKCENTERCODE
		  ,MATLOTNO							AS MATLOTNO
		  ,BADWAITINGNO                     AS BADWAITINGNO
		  ,ITEMCODE						    AS ITEMCODE	 -- 품목
		  ,ITEMNAME							AS ITEMNAME
		  ,MAKER							AS MAKER
		  ,MAKEDATE							AS MAKEDATE
		  ,TOTALNO                          AS TOTALNO
	  FROM P_BAD_WAITINGNO WITH(NOLOCK)
	 WHERE PLANTCODE LIKE @PLANTCODE + '%'
	   AND MATLOTNO LIKE '%' + @MATLOTNO + '%'
	   AND ITEMCODE LIKE @ITEMCODE + '%'

END
