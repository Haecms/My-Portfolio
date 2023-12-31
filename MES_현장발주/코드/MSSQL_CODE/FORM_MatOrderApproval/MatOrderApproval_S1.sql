USE [KDTB03_1JO]
GO
/****** Object:  StoredProcedure [dbo].[_1JO_MM_MatOrderApproval_S1]    Script Date: 2023-09-04 오후 6:25:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		권문규
-- Create date: 2023-08-08
-- Description:	현장발주내역 승인 폼에서 조회하는 프로시저.
-- =============================================
ALTER PROCEDURE [dbo].[_1JO_MM_MatOrderApproval_S1]
	@PLANTCODE VARCHAR(10),            -- 공장
	@CUSTCODE  VARCHAR(10),            -- 거래처
	@ITEMCODE  VARCHAR(30),            -- 품목
	@STARTDATE VARCHAR(10),            -- 발주요청시작
	@ENDDATE   VARCHAR(10),            -- 발주요청종료
	@APPRSTAT  VARCHAR(10),			   -- 승인여부

	@LANG    VARCHAR(10),
	@RS_CODE VARCHAR(1) OUTPUT,
	@RS_MSG  VARCHAR(100) OUTPUT
AS
BEGIN
	SELECT 0							  AS CHK
	      ,A.PLANTCODE					  AS PLANTCODE
		  ,A.ReqDATE					  AS REQDATE
		  ,A.ITEMCODE					  AS ITEMCODE
		  ,A.ReqQTY						  AS REQQTY
		  ,A.UNITCODE					  AS UNITCODE
		  ,ISNULL(C.CUSTNAME, '')		  AS CUSTNAME
		  ,A.ApprSTATUS					  AS APPRSTATUS
		  ,DBO.FN_GET_USERNAME(A.MAKER)	  AS MAKER
		  ,A.MAKEDATE					  AS MAKEDATE
		  ,DBO.FN_GET_USERNAME(A.EDITOR)  AS EDITOR
		  ,A.EDITDATE					  AS EDITDATE
		  ,A.[O/W]                        AS [O/W]
	  FROM TB_OrderRequestList A WITH(NOLOCK) LEFT JOIN TB_CustMaster C WITH(NOLOCK)
											    ON A.PLANTCODE = C.PLANTCODE
											   AND A.CUSTCODE  = C.CUSTCODE
	 WHERE A.PLANTCODE LIKE '%' + @PLANTCODE + '%'
	   AND ISNULL(A.CUSTCODE, '')  LIKE '%' + @CUSTCODE  + '%'
	   AND ISNULL(A.ITEMCODE, '')  LIKE '%' + @ITEMCODE  + '%'
	   AND ((@APPRSTAT = 'Y') AND A.ApprSTATUS = 'Y') OR ((@APPRSTAT = 'N') AND A.ApprSTATUS = 'N') OR ((@APPRSTAT = '') AND A.ApprSTATUS = A.ApprSTATUS)
	   AND A.ReqDATE   BETWEEN @STARTDATE AND @ENDDATE
  ORDER BY A.ReqDATE, A.MAKEDATE

END