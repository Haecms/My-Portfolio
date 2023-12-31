USE [KDTB03_1JO]
GO
/****** Object:  StoredProcedure [dbo].[_1JO_MM_MatOrderApproval_D1]    Script Date: 2023-09-04 오후 6:24:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		권문규
-- Create date: 2023-08-11
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[_1JO_MM_MatOrderApproval_D1] 
	@DATE DATETIME,

	@LANG    VARCHAR(10),
	@RS_CODE VARCHAR(1) OUTPUT,
	@RS_MSG  VARCHAR(100) OUTPUT
AS
BEGIN

	IF (SELECT ApprSTATUS 
	      FROM TB_OrderRequestList WITH(NOLOCK)
		 WHERE MAKEDATE = @DATE) = 'Y'
	BEGIN
		SET @RS_CODE = 'X'
		SET @RS_MSG  = '이미 발주 승인 된 품목은 삭제할 수 없습니다. 확인 후 다시 시도해주세요.'
	END
		
	ELSE
	BEGIN
		DELETE FROM TB_OrderRequestList
		 WHERE MAKEDATE = @DATE

		SET @RS_CODE = 'S'
		SET @RS_MSG  = '정상적으로 삭제되었습니다.'
	END

END
