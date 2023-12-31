USE [KDTB03_1JO]
GO
/****** Object:  StoredProcedure [dbo].[SP01_MM_MaterialOrder_I1]    Script Date: 2023-09-04 오후 6:20:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		강명서
-- Create date: 2023 8 15
-- Description:	구매 자재 발주 추가 저장
-- =============================================
ALTER PROCEDURE [dbo].[SP01_MM_MaterialOrder_I1]
	@PLANTCODE VARCHAR(10),  -- 공장
	@ITEMCODE  VARCHAR(10),  -- 품목
	@PODATE    VARCHAR(30),  -- 발주일자
	@POQTY     FLOAT      ,  -- 발주수량
	@UNITCODE  VARCHAR(10),  -- 단위
	@CUSTCODE  VARCHAR(10),  -- 거래처
	@MAKER     VARCHAR(30),  -- 등록자
	--@PODATE VARCHAR(30), -- 발주일 
	--6개
	@MAKEDATE VARCHAR(30),
	@LANG      VARCHAR(10),
	@RS_CODE   VARCHAR(1)   OUTPUT,
	@RS_MSG    VARCHAR(100) OUTPUT

AS
BEGIN
	-- 프로시져 공통 변수
	DECLARE @LS_NOWDATE VARCHAR(10),
	        @LD_NOWDATE DATETIME

		SET @LD_NOWDATE = GETDATE()
		SET @LS_NOWDATE = CONVERT(VARCHAR,@LD_NOWDATE,23) -- YYYY-MM-DD

	-- 구매자재 발주 등록 PS
	IF (@POQTY =0)
	BEGIN
		SET @RS_CODE = 'X'
		SET @RS_MSG  = '발주 수량은 0일 수 없습니다.'
	END

	-- 발주 번호 채번
	DECLARE @LI_SEQ INT

	SELECT @LI_SEQ = ISNULL(MAX(POSEQ) , 0) + 1
	  FROM TB_MaterialOrder WITH(NOLOCK)
	 WHERE PLANTCODE = @PLANTCODE -- 공장
	   AND PODATE    = @LS_NOWDATE -- 발주일

	DECLARE @PONO VARCHAR(30)
	    SET @PONO = 'PO' + REPLACE(@LS_NOWDATE,'-','') + RIGHT(('0000' + CONVERT(VARCHAR,@LI_SEQ)),4)

	 INSERT INTO TB_MaterialOrder (PLANTCODE,  PONO,  ITEMCODE,  PODATE,      POQTY,  UNITCODE,  MAKEDATE,    CUSTCODE, POSEQ, AORDERSTATUS, MAKER, [O/W])
						   VALUES (@PLANTCODE, @PONO, @ITEMCODE, @LS_NOWDATE, @POQTY, @UNITCODE, @LD_NOWDATE,  @CUSTCODE, @LI_SEQ,'N', @MAKER, 'O')
-- 공장, 발주 번호, 품목 코드, 발주일, 발주 수량, 단위, 등록일, 거래처 코드, 발주 순번, 자동 발주 여부, OFFICE/WORKCENTER
	SET @RS_CODE = 'S'
END