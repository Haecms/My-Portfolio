USE [KDTB03_1JO]
GO
/****** Object:  StoredProcedure [dbo].[SP07_POP_MaterialInput_U1]    Script Date: 2023-09-04 오후 6:15:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		조해찬
-- Create date: 2023-05-17
-- Description:	구매자재 발주 내역으로 입고 등록
-- =============================================
ALTER PROCEDURE [dbo].[SP07_POP_MaterialInput_U1]
	@PLANTCODE       VARCHAR(10),  -- 공장
	@PONO            VARCHAR(20),  -- 발주번호
	@POSSIBLEQTY     FLOAT      ,  -- 입고수량
	@INWORKER		 VARCHAR(30),  -- 입고자
	@POSEQ           VARCHAR(5),
	@PODATE          VARCHAR(10),

	@LANG      VARCHAR(10),  
	@RS_CODE   VARCHAR(1)   OUTPUT,
	@RS_MSG    VARCHAR(100) OUTPUT

AS
BEGIN
	-- 원자재 입고 등록
	-- 프로시져 공통 변수
	DECLARE @LS_NOWDATE VARCHAR(10),
	        @LD_NOWDATE DATETIME,
			@LI_SEQQ INT, -- TB_MaterialInput에서 SEQ 찾기
			@LI_INPUTSEQ VARCHAR(10)
	
		SET @LD_NOWDATE = GETDATE()
		SET @LS_NOWDATE = CONVERT(VARCHAR,@LD_NOWDATE,23) -- YYYY-MM-DD

	-- LOT NO 채번
	DECLARE @LS_MATLOTNO VARCHAR(30)	-- ****** 원자재 LOT NO
	SET @LS_MATLOTNO = DBO.FN_LOTNO('LT_R')

	-- 처음 데이터가 들어오면 LI_SEQQ에 아무것도 등록이 안되기에 NULL이 들어감
	-- INPUTSEQ
	SELECT @LI_SEQQ = ISNULL(CONVERT(INT, REPLACE(INPUTSEQ,'R','')),0) + 1
	  FROM TB_MaterialInput
	 WHERE PLANTCODE = @PLANTCODE
	   AND PONO = @PONO

	SET @LI_INPUTSEQ = 'R' + RIGHT(('000' + CONVERT(VARCHAR, ISNULL(@LI_SEQQ,1))),3)
	
	 --밸리데이션
	 --1. 남아있는 양보다 입고양이 많을 때
	IF((SELECT A.POQTY - B.POQTY
		  FROM TB_MaterialOrder A WITH(NOLOCK)LEFT JOIN (SELECT PONO          AS PONO,
	                                                            SUM(INPUTQTY) AS POQTY
												           FROM TB_MaterialInput
												       GROUP BY PONO) B
												     ON A.PONO = B.PONO
		 WHERE A.PONO = @PONO) < @POSSIBLEQTY)
		 
	BEGIN
		SET @RS_CODE = 'F'
		SET @RS_MSG = '남아있는 양보다 많이 입력하였습니다'
		RETURN
	END



	-- 입고 등록 테이블에 데이터 등록
	INSERT INTO TB_MaterialInput(PLANTCODE,  INPUTSEQ,     INPUTQTY,     INPUTLOT,    
								 PONO,       INPUTDATE,    MAKER,        MAKEDATE,    POSEQ,  PODATE)
	                      VALUES(@PLANTCODE, @LI_INPUTSEQ, @POSSIBLEQTY, @LS_MATLOTNO,
							     @PONO,      @LS_NOWDATE,  @INWORKER,    @LD_NOWDATE, @POSEQ, @PODATE)

	-- 2. 원자재 창고 테이블에 실물 데이터 등록
	INSERT INTO TB_StockMM(PLANTCODE,  MATLOTNO,      WHCODE,    STORAGELOCCODE,
						   ITEMCODE,   STOCKQTY,      UNITCODE,  INDATE,
						   CUSTCODE,   MAKEDATE,      MAKER)
				    SELECT @PLANTCODE, @LS_MATLOTNO, 'WH001',   'A-03',
							ITEMCODE,  @POSSIBLEQTY,  UNITCODE,  @LS_NOWDATE,
							CUSTCODE,  @LD_NOWDATE,   @INWORKER
					  FROM TB_MaterialOrder WITH(NOLOCK)
					 WHERE PLANTCODE = @PLANTCODE
					   AND PONO      = @PONO
					   AND POSEQ     = @POSEQ
					   AND PODATE    = @PODATE

	-- 3. 원자재 입고 이력
	-- 지금 이력을 등록해야할 일자 별 입출 순번 찾기. (일자별 최대 입출순번 +1)
   DECLARE @LI_SEQ INT -- 일자별 입출 순번 +1
	SELECT @LI_SEQ = ISNULL(MAX(INOUTSEQ),0) + 1
	  FROM TB_StockMMrec WITH(NOLOCK)
	 WHERE PLANTCODE = @PLANTCODE
	   AND INOUTDATE = @LS_NOWDATE

	INSERT INTO TB_StockMMrec(PLANTCODE,  INOUTDATE,   INOUTSEQ,     ITEMCODE, MATLOTNO,     WHCODE,      PONO,
							  INOUTCODE,  INOUTFLAG,   INOUTQTY,     UNITCODE, INOUTWORKER,  MAKEDATE,    MAKER)
					   SELECT @PLANTCODE, @LS_NOWDATE, @LI_SEQ,      ITEMCODE, @LS_MATLOTNO, 'WH001',     @PONO,
							  '10',		  'I',         @POSSIBLEQTY, UNITCODE, @INWORKER,    @LD_NOWDATE, @INWORKER
					     FROM TB_MaterialOrder WITH(NOLOCK)
						WHERE PLANTCODE = @PLANTCODE
						  AND PONO      = @PONO
						  AND POSEQ     = @POSEQ
						  AND PODATE    = @PODATE

	-- 4. 원자재 
	--UPDATE TB_MaterialOrder
	--   SET Confirm = 'Y'
	-- WHERE PONO = @PONO

	SET @RS_CODE = 'S'

END
