USE [KDTB03_MES_1]
GO
/****** Object:  StoredProcedure [dbo].[SP07_QA_CheckRec_S1_HC]    Script Date: 2023-08-31 오전 2:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		조해찬
-- Create date: 2023-06-27
-- Description:	불량 판정 등록 이력 - 확정 처리 이력 조회
-- =============================================
ALTER PROCEDURE [dbo].[SP07_QA_CheckRec_S1_HC]
	@LOTNO VARCHAR(30),
	@STARTDATE VARCHAR(30),
	@ENDDATE VARCHAR(30),
	@STATE VARCHAR(10),

	@LANG VARCHAR(10) = 'KO',
	@RS_CODE VARCHAR (1)  OUTPUT,
	@RS_MSG  VARCHAR (100) OUTPUT
AS
BEGIN
	DECLARE @LS_STATE VARCHAR(10)
	SET @LS_STATE = ''
IF(@STATE <> '')
BEGIN
	SELECT @LS_STATE= CODENAME
	  FROM TB_Standard
	 WHERE MAJORCODE = 'yesno'
	   AND MINORCODE = @STATE
END

	SELECT A.PASSLOTNO  AS PASSLOTNO
		  ,A.PASSNOREC  AS PASSNOREC
		  ,A.PASSREASON AS PASSREASON
		  ,B.WORKERNAME AS MAKER     
		  ,A.MAKEDATE   AS MAKEDATE
		  ,A.CHKCODE    AS CHKCODE
		  ,A.C_STATE    AS C_STATE
	  FROM P_Lot A WITH(NOLOCK) LEFT JOIN SysManList B WITH(NOLOCK)
									   ON A.MAKER = B.WORKERID
	 WHERE C_STATE LIKE '%' + @LS_STATE
	   AND A.PASSLOTNO LIKE '%' + @LOTNO + '%'
	   OR  A.NOPASSLOTNO LIKE '%' + @LOTNO + '%'
	   AND A.MAKEDATE BETWEEN @STARTDATE AND @ENDDATE
END
