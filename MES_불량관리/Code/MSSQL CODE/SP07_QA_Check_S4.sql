USE [KDTB03_MES_1]
GO
/****** Object:  StoredProcedure [dbo].[SP07_QA_Check_S4]    Script Date: 2023-08-31 오전 2:33:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	그리드 2번조회
-- =============================================
ALTER PROCEDURE [dbo].[SP07_QA_Check_S4]
	@CHKCODE VARCHAR(30),

	@LANG VARCHAR(10) = 'KO',
	@RS_CODE VARCHAR (1)  OUTPUT,
	@RS_MSG  VARCHAR (100) OUTPUT
AS
BEGIN
	SELECT CHKCODE         AS CHKCODE     
		  ,T_BADWAITINGNO    AS BADWAITINGNO
		  ,MAKER             AS MAKER
		  ,MAKEDATE          AS MAKEDATE
		  ,TOTALNO           AS TOTALNO
	  FROM P_Combine
	 WHERE CHKCODE LIKE '%' + @CHKCODE + '%'
  ORDER BY MAKEDATE


END
