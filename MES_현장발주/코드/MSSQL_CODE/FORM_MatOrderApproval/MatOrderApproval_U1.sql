USE [KDTB03_1JO]
GO
/****** Object:  StoredProcedure [dbo].[_1JO_MM_MatOrderApproval_U1]    Script Date: 2023-09-04 오후 6:25:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		권문규
-- Create date: 2023-08-14
-- Description: 발주승인 창에서 Modified 된 항목들 중 체크박스 체크가 안되어있는 항목(수정사항이 없거나, 발주수량이 변경된 경우)들 업데이트
-- =============================================
ALTER PROCEDURE [dbo].[_1JO_MM_MatOrderApproval_U1]
	@DATE     DATETIME,		-- 요청일시
	@REQQTY   INT,            -- 요청수량
	@EDITOR   VARCHAR(10),
	@CUSTNAME VARCHAR(15),

	@LANG    VARCHAR(10),
	@RS_CODE VARCHAR(1) OUTPUT,
	@RS_MSG  VARCHAR(100) OUTPUT
AS
BEGIN
	IF (SELECT ApprSTATUS
		  FROM TB_OrderRequestList
		 WHERE MAKEDATE = @DATE) = 'Y'
	BEGIN
		SET @RS_CODE = 'X'
		SET @RS_MSG  = '이미 발주 완료된 품목은 수량 변경을 할 수 없습니다! 확인 후 다시 시도해주세요.'
	END
	ELSE
	BEGIN
		DECLARE @LD_NOWDATE DATETIME
		       ,@LS_NOWDATE VARCHAR(10)
		SET @LD_NOWDATE = GETDATE()
		SET @LS_NOWDATE = CONVERT(VARCHAR, @LD_NOWDATE, 23)

		UPDATE TB_OrderRequestList
		   SET ReqQTY = @REQQTY
		      ,EDITOR = @EDITOR
		      ,EDITDATE = @LD_NOWDATE
			  ,CUSTCODE = @CUSTNAME
		 WHERE MAKEDATE = @DATE

		SET @RS_CODE = 'S'
	END
END
