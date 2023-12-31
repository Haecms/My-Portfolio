USE [KDTB03_MES_1]
GO
/****** Object:  StoredProcedure [dbo].[SP07_QA_CheckRec_S3_HC]    Script Date: 2023-08-31 오전 2:35:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		조해찬
-- Create date: 2023-06-28
-- Description:	그리드 2 클릭시 최종 로트 나오는 화면
-- =============================================
ALTER PROCEDURE [dbo].[SP07_QA_CheckRec_S3_HC]
	@CHKCODE VARCHAR(30),

	@LANG VARCHAR(10) = 'KO',
	@RS_CODE VARCHAR (1)  OUTPUT,
	@RS_MSG  VARCHAR (100) OUTPUT
AS
BEGIN
	SELECT MATLOTNO
	      ,FINALLOTNO
	      ,STOCKQTY
	  FROM P_FINALLOT
	 WHERE CHKCODE = @CHKCODE
END
