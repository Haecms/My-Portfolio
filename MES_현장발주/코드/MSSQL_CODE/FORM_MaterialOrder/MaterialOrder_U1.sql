USE [KDTB03_1JO]
GO
/****** Object:  StoredProcedure [dbo].[SP01_MM_MaterialOrder_U1]    Script Date: 2023-09-04 오후 6:21:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		강명서<Author,,Name>
-- Create date: 2023 8 15<Create Date,,>
-- Description:	구매 자재 발주 등록<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP01_MM_MaterialOrder_U1]
	@PLANTCODE VARCHAR(10),  -- 공장
	@PONO      VARCHAR(20),  -- 발주번호
	--@INQTY     FLOAT      ,  -- 입고수량
	--@INWORKER  VARCHAR(30),  -- 입고자
	@ITEMCODE  VARCHAR(10),  -- 품목
	@POQTY     FLOAT      ,  -- 발주수량
	@UNITCODE  VARCHAR(10),  -- 단위
	@CUSTCODE  VARCHAR(10),  -- 거래처
	@MAKER     VARCHAR(30),  -- 등록자

	@LANG      VARCHAR(10),  
	@RS_CODE   VARCHAR(1)   OUTPUT,
	@RS_MSG    VARCHAR(100) OUTPUT

AS
BEGIN
	-- 원자재 입고 등록
	-- 프로시져 공통 변수
	DECLARE @LS_NOWDATE VARCHAR(10),
	        @LD_NOWDATE DATETIME
	
		SET @LD_NOWDATE = GETDATE()
		SET @LS_NOWDATE = CONVERT(VARCHAR,@LD_NOWDATE,23) -- YYYY-MM-DD


	SET @RS_CODE = 'S'

END