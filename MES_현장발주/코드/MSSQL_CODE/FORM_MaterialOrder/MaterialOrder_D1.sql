USE [KDTB03_1JO]
GO
/****** Object:  StoredProcedure [dbo].[SP07_MM_MaterialOrderIn_D1]    Script Date: 2023-09-04 오후 6:22:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		조해찬
-- Create date: 2023-05-17
-- Description:	구매자재 발주 등록 내역 취소. (삭제)
-- =============================================
ALTER PROCEDURE [dbo].[SP07_MM_MaterialOrderIn_D1]
	@PLANTCODE VARCHAR(10),  -- 공장
	@PONO	   VARCHAR(30),  -- 발주번호

	@LANG      VARCHAR(10),  
	@RS_CODE   VARCHAR(1)   OUTPUT,
	@RS_MSG    VARCHAR(100) OUTPUT


AS
BEGIN
	-- 발주등록 내역 취소 SQL
	-- 1. 발주 취소 할 수 없는 상황 체크
	IF(ISNULL((SELECT INFLAG
	     FROM TB_MaterialOrder WITH(NOLOCK)
		WHERE PONO = @PONO
		  AND PLANTCODE = @PLANTCODE), 'N') <> 'N')
	BEGIN 
		SET @RS_CODE = 'E'
		SET @RS_MSG  = '이미 입고 내역이 존재하는 발주 입니다. 취소할 수 없습니다.'
		RETURN
	END

	-- 2. 발주 취소
	DELETE TB_MaterialOrder
	 WHERE PLANTCODE = @PLANTCODE
	   AND PONO      = @PONO

	SET @RS_CODE = 'S'

END
