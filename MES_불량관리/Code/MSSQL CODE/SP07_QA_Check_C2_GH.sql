USE [KDTB03_MES_1]
GO
/****** Object:  StoredProcedure [dbo].[SP07_QA_Check_C2_GH]    Script Date: 2023-08-31 오전 2:34:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP07_QA_Check_C2_GH]
	@PASSNO INT,  
	@NOPASSNO INT,
	@CHKCODE VARCHAR(30),
	@PASSREASON VARCHAR(30),
	@NOPASSREASON VARCHAR(30),
	@TOTALNO INT,
	@MAKER VARCHAR(30),

	@LANG VARCHAR(10) = 'KO',
	@RS_CODE VARCHAR (1)  OUTPUT,
	@RS_MSG  VARCHAR (100) OUTPUT

AS
BEGIN
	DECLARE @LS_CHKCODE  VARCHAR(30)
		   ,@PASSLOT  VARCHAR(30)
		   ,@NOPASSLOT  VARCHAR(30)
		   ,@LS_CODENAME VARCHAR(30)
		   ,@LD_NOWDATE DATETIME
		   ,@LS_STATE VARCHAR(10)

	SELECT @LS_CODENAME = CODENAME
	  FROM TB_Standard
	 WHERE MINORCODE = @NOPASSREASON
	   AND MAJORCODE = 'ERRORTYPE'

	SET @LD_NOWDATE = GETDATE()

	SET @PASSLOT   = 'LT_P' + REPLACE(REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR,GETDATE(),121),'-',''),':',''),' ',''),'.','')
	SET @NOPASSLOT = 'LT_N' + REPLACE(REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR,GETDATE(),121),'-',''),':',''),' ',''),'.','')




	-- 밸리데이션 누군가 이미 데이터를 판정 확정처리하였을 때
	IF((SELECT T_BADWAITINGNO - @PASSNO - @NOPASSNO
	     FROM P_Combine
		WHERE CHKCODE = @CHKCODE)<0)
	BEGIN
		SET @RS_CODE = 'E'
		SET @RS_MSG = '이미 판정 확정하였습니다. 재조회후 다시 진행해주시기 바랍니다.'
	END

	-- 로직 시작
	IF(@PASSNO>0)
	BEGIN 
		SET @LS_STATE = '합' 
	END
			INSERT INTO P_LOT (CHKCODE,  PASSLOTNO, PASSNO,  PASSREASON,  NOPASSLOTNO, NOPASSNO,  NOPASSREASON, TOTALNO, MAKER, MAKEDATE,C_STATE, PASSNOREC)
			           VALUES (@CHKCODE, @PASSLOT,  @PASSNO, @PASSREASON, '',          '',        '',           @TOTALNO,@MAKER, @LD_NOWDATE,@LS_STATE, @PASSNO)

			UPDATE P_Combine
			   SET T_BADWAITINGNO = T_BADWAITINGNO - @PASSNO
			 WHERE CHKCODE = @CHKCODE
		SET @LS_STATE = ''
	IF(@NOPASSNO>0)
	BEGIN 
		SET @LS_STATE = '불' 
	END
			INSERT INTO P_LOT (CHKCODE,  PASSLOTNO, PASSNO,     PASSREASON,                              NOPASSLOTNO, NOPASSNO,  NOPASSREASON,		TOTALNO, MAKER, MAKEDATE,C_STATE)
		               VALUES (@CHKCODE, @NOPASSLOT,@NOPASSNO,  @NOPASSREASON +' [' + @LS_CODENAME + ']','',          '',        '',                @TOTALNO, @MAKER, @LD_NOWDATE,@LS_STATE)

			UPDATE P_Combine
			   SET T_BADWAITINGNO = T_BADWAITINGNO - @NOPASSNO
			 WHERE CHKCODE = @CHKCODE

		--IF((SELECT T_BADWAITINGNO
		--     FROM P_Combine
		--	WHERE CHKCODE = @CHKCODE) = 0)
		--BEGIN
		--	DELETE P_Combine
		--	 WHERE CHKCODE = @CHKCODE
		--END
	SET @RS_CODE = 'S'
END
