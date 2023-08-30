USE [KDTB03_MES_1]
GO
/****** Object:  StoredProcedure [dbo].[SP07_QA_Check_S3]    Script Date: 2023-08-31 오전 2:33:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		김건희
-- Create date: <Create Date,,>
-- Description:	그리드 3번 조회
-- =============================================
ALTER PROCEDURE [dbo].[SP07_QA_Check_S3]

	@CHKCODE   VARCHAR(30) ,

	@LANG      VARCHAR(10),  
	@RS_CODE   VARCHAR(1)   OUTPUT,
	@RS_MSG    VARCHAR(100) OUTPUT

AS
BEGIN

	SELECT CHKCODE              AS CHKCODE
		  ,	PASSLOTNO			AS PASSLOTNO	 
		  ,	PASSNO				AS PASSNO			 
--		  , NOPASSLOTNO         AS NOPASSLOTNO 
--		  , NOPASSNO			AS NOPASSNO
		  , PASSREASON          AS PASSREASON
		  , C_STATE             AS C_STATE
--		  , NOPASSREASON        AS NOPASSREASON
	  FROM P_Lot
	 WHERE CHKCODE = @CHKCODE




END
